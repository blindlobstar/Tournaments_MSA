using System.Collections.Generic;
using System.Linq;
using Akka.TestKit.NUnit;
using ExerciseFlow.API.Actors;
using GrpcTournamentService;
using Moq;
using NUnit.Framework;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public sealed class TournamentActorTest : TestKit
    {
        private readonly Mock<TournamentService.TournamentServiceClient> _clientMock;

        public TournamentActorTest()
        {
            var exercises = new List<Exercise>()
            {
                new() {Id = 1, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 2, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 3, Answer = string.Empty, TournamentId = 1, Text = string.Empty},
                new() {Id = 4, Answer = string.Empty, TournamentId = 1, Text = string.Empty}
            };
            var response = new GetExercisesResponse();
            response.Exercises.AddRange(exercises);

            _clientMock = new Mock<TournamentService.TournamentServiceClient>();
            _clientMock.Setup(x => x.GetExercises(It.IsAny<GetExercisesRequest>(), 
                null, null, default)).Returns(() => response);
        }

        [Test]
        public void GetTournamentExercise_Count_4()
        {
            //Arrange
            var actor = Sys.ActorOf(TournamentActor.Props(_clientMock.Object, 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);

            //Assert
            probe.ExpectMsg<IEnumerable<API.Models.Exercise>>(message => message.Count() == 4);
            _clientMock.Verify(x => x.GetExercises(It.IsAny<GetExercisesRequest>(),
                null, null, default), Times.Once);
        }

        [Test]
        public void GetTournamentExercise_Count_4_Do_Not_Call_Grpc()
        {
            //Arrange
            var actor = Sys.ActorOf(TournamentActor.Props(_clientMock.Object, 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);
            actor.Tell(TournamentActor.GetTournamentExercise.Instance, probe);
            //Assert
            probe.ExpectMsg<IEnumerable<API.Models.Exercise>>(message => message.Count() == 4);
            probe.ExpectMsg<IEnumerable<API.Models.Exercise>>(message => message.Count() == 4);
            _clientMock.Verify(x => x.GetExercises(It.IsAny<GetExercisesRequest>(),
                null, null, default), Times.Once);
        }
    }
}
