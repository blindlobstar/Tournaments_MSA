using System;
using Akka.TestKit.NUnit;
using ExerciseFlow.API.Models;
using ExerciseFlow.API.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Common.Core.DataExchange.EventBus;
using ExerciseFlow.API.Actors;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public sealed class UserManagerActorTest : TestKit
    {
        private readonly List<Exercise> _exercises;

        public UserManagerActorTest()
        {
            _exercises = new List<Exercise>()
            {
                new() {Id = 1, Answer = string.Empty, TournamentId = 1, Text = "1"},
                new() {Id = 2, Answer = string.Empty, TournamentId = 1, Text = "2"},
                new() {Id = 3, Answer = string.Empty, TournamentId = 2, Text = "3"},
                new() {Id = 4, Answer = string.Empty, TournamentId = 2, Text = "4"}
            };
        }

        [Test]
        public void GetQuestion_UserId_1_return_Exercise()
        {
            //Arrange
            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(It.IsAny<int>())).Returns(() => _exercises);
            var busPublisherMock = new Mock<IBusPublisher>();
            var actor = Sys.ActorOf(UserManagerActor.Props(busPublisherMock.Object, clientMock.Object));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new UserManagerActor.GetQuestionForUser("1", 1), probe);

            //Assert
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>(msg => msg.Question == "1");
        }

        [Test]
        public async Task GetQuestion_UserId_1_return_Exercise_Text_1_UserId_2_Exercise_Text_3_user_2_is_faster()
        {
            //Arrange
            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(1))
                .Callback(() => Thread.Sleep(2))
                .Returns(() => _exercises.Take(2).ToList());
            clientMock.Setup(x => x.GetExercises(2))
                .Returns(() => _exercises.Skip(2).Take(2).ToList());
            var busPublisherMock = new Mock<IBusPublisher>();
            var actor = Sys.ActorOf(UserManagerActor.Props(busPublisherMock.Object, clientMock.Object));
            var probe = CreateTestProbe();

            //Act
            var task1 = actor.Ask(new UserManagerActor.GetQuestionForUser("1", 1));
            var task2 = actor.Ask(new UserManagerActor.GetQuestionForUser("2", 2));

            var fastestTask = Task.WaitAny(task1, task2);

            var ex1 = (await task1) as ExerciseActor.GetExerciseResponse;
            var ex2 = (await task2) as ExerciseActor.GetExerciseResponse;
            
            //Assert
            Assert.AreEqual(task2.Id, fastestTask);
            Assert.AreEqual("1", ex1.Question);
            Assert.AreEqual("3", ex2.Question);
        }
    }
}