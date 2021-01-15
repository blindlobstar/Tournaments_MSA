using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Common.Core.DataExchange.EventBus;
using Common.Core.DataExchange.Messages;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Models;
using Moq;
using NUnit.Framework;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public sealed class UserActorTest : TestKit
    {
        private readonly Mock<IBusPublisher> _busPublisherMock;
        private readonly List<Exercise> _exercises;

        public UserActorTest()
        {
            _busPublisherMock = new Mock<IBusPublisher>(MockBehavior.Loose);
            _busPublisherMock.Setup(b => b.Send(It.IsAny<ICommand>())).Returns(Task.CompletedTask);
            _exercises = new List<Exercise>()
            {
                new() {Id = 1, Text = "1+1", Answer = "2", TournamentId = 1},
                new() {Id = 2, Text = "1+2", Answer = "3", TournamentId = 1},
                new() {Id = 3, Text = "1+3", Answer = "4", TournamentId = 1}
            };
        }

        [Test]
        public async Task GetNext_Returns_FirstExercise()
        {
            //Arrange
            var userActor = Sys.ActorOf(UserActor.Props("1", _exercises, _busPublisherMock.Object));

            //Act
            var exercise = await userActor.Ask<ExerciseActor.GetExerciseResponse>(UserActor.GetNextQuestion.Instance);

            //Assert
            Assert.AreEqual(_exercises.First().Text, exercise.Question);
        }

        [Test]
        public async Task GetNext_Firs_2_times_without_answer()
        {
            //Arrange
            var userActor = Sys.ActorOf(UserActor.Props("1", _exercises, _busPublisherMock.Object));
            var probe = CreateTestProbe();

            //Act
            userActor.Tell(UserActor.GetNextQuestion.Instance, probe);
            userActor.Tell(UserActor.GetNextQuestion.Instance, probe);

            //Assert
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>(msg => msg.Question == "1+1");
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>(msg => msg.Question == "1+1");
        }

        [Test]
        public async Task GetNext_2_times_with_answer()
        {
            //Arrange
            var userActor = Sys.ActorOf(UserActor.Props("1", _exercises, _busPublisherMock.Object));
            var probe = CreateTestProbe();

            //Act
            userActor.Tell(UserActor.GetNextQuestion.Instance);
            userActor.Tell(new ExerciseActor.TakeAnswerRequest(string.Empty));
            userActor.Tell(UserActor.GetNextQuestion.Instance, probe);

            //Assert
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>(msg => msg.Question == "1+2", TimeSpan.FromMilliseconds(10000));

        }

        [Test]
        public void GetNext_NoMoreQuestion()
        {
            //Arrange
            var userActor = Sys.ActorOf(UserActor.Props("1", _exercises, _busPublisherMock.Object));
            var probe = CreateTestProbe();

            //Act
            userActor.Tell(UserActor.GetNextQuestion.Instance);
            userActor.Tell(new ExerciseActor.TakeAnswerRequest(string.Empty));
            userActor.Tell(UserActor.GetNextQuestion.Instance);
            userActor.Tell(new ExerciseActor.TakeAnswerRequest(string.Empty));
            userActor.Tell(UserActor.GetNextQuestion.Instance);
            userActor.Tell(new ExerciseActor.TakeAnswerRequest(string.Empty));
            userActor.Tell(UserActor.GetNextQuestion.Instance, probe);

            //Assert
            probe.ExpectMsg<UserActor.NoMoreQuestions>();
        }

        [Test]
        public void TakeAnswer_Publish()
        {
            //Arrange
            var userActor = Sys.ActorOf(UserActor.Props("1", _exercises, _busPublisherMock.Object));
            var probe = CreateTestProbe();

            //Act
            userActor.Tell(UserActor.GetNextQuestion.Instance, probe);
            userActor.Tell(new ExerciseActor.TakeAnswerRequest("2"), probe);

            //Assert
            probe.AwaitAssert(() => _busPublisherMock.Verify(e => e.Send(It.IsAny<ICommand>()), Times.AtLeastOnce));
        }
    }
}
