using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.NUnit;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Models;
using ExerciseFlow.API.Services;
using Moq;
using NUnit.Framework;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public class TournamentManagerActorTest : TestKit
    {
        private readonly List<Exercise> _exercises;

        public TournamentManagerActorTest()
        {
            _exercises = new List<Exercise>()
            {
                new() {Id = 1, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 2, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 3, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 4, Answer = string.Empty, TournamentId = 1, Text = string.Empty}
            };
        }

        [Test]
        public void GetExercises_TournamentId_1_Count_4()
        {
            //Arrange
            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(It.IsAny<int>())).Returns(() => _exercises);
            var actor = Sys.ActorOf(TournamentManagerActor.Props(clientMock.Object));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new TournamentManagerActor.GetTournamentExercise(1), probe);

            //Assert
            probe.ExpectMsg<List<Exercise>>(msg => msg.Count == 4);
        }

        [Test]
        public void GetExercise_TournamentId_2_Count_3()
        {
            //Arrange
            var exercises = new List<Exercise>()
            {
                new() {Id = 1, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 2, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 3, Answer = string.Empty, TournamentId = 1, Text = string.Empty}
            };

            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(1)).Returns(() => _exercises);
            clientMock.Setup(x => x.GetExercises(2)).Returns(() => exercises);
            var actor = Sys.ActorOf(TournamentManagerActor.Props(clientMock.Object));
            var probe = CreateTestProbe();

            //Act
            actor.Ask(new TournamentManagerActor.GetTournamentExercise(1));
            actor.Tell(new TournamentManagerActor.GetTournamentExercise(2), probe);

            //Assert
            probe.ExpectMsg<List<Exercise>>(msg => msg.Count == 3);
        }
    }
}