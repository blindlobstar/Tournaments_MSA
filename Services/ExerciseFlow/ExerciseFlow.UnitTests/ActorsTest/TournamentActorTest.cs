using System.Collections.Generic;
using System.Linq;
using Akka.TestKit.NUnit;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Models;
using ExerciseFlow.API.Services;
using Moq;
using NUnit.Framework;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public sealed class TournamentActorTest : TestKit
    {
        private readonly List<Exercise> _exercises;

        public TournamentActorTest()
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
        public void GetTournamentExercise_Count_4()
        {
            //Arrange
            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(It.IsAny<int>())).Returns(() => _exercises);
            var client = clientMock.Object; 
            var actor = Sys.ActorOf(TournamentActor.Props(client, 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);

            //Assert
            probe.ExpectMsg<IEnumerable<API.Models.Exercise>>(message => message.Count() == 4);
            clientMock.Verify(x => x.GetExercises(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetTournamentExercise_Count_4_Do_Not_Call_Grpc()
        {
            //Arrange
            var clientMock = new Mock<ITournamentService>();
            clientMock.Setup(x => x.GetExercises(It.IsAny<int>())).Returns(() => _exercises);
            var actor = Sys.ActorOf(TournamentActor.Props(clientMock.Object, 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);
            //Assert
            probe.ExpectMsg<IEnumerable<Exercise>>(message => message.Count() == 4);
            probe.ExpectMsg<IEnumerable<Exercise>>(message => message.Count() == 4);
            clientMock.Verify(x => x.GetExercises(It.IsAny<int>()), Times.Once);
        }
    }
}
