using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcTournamentService;
using NUnit.Framework;

namespace Tournament.FunctionalTest
{
    [TestFixture]
    [Category("Functional")]
    public class TournamentServiceTest
    {
        private GrpcTournamentService.TournamentService.TournamentServiceClient _client;

        [SetUp]
        public void SetUp()
        {
            var testServer = new TestServerFixture();
            _client = new GrpcTournamentService.TournamentService.TournamentServiceClient(testServer.GrpcChannel);
        }

        [Test]
        public async Task GetTournament_1_Returns_NotNull_NewTournament()
        {
            //Act
            var tournament = await _client.GetAsync(new GetRequest() {Id = 1});

            //Assert
            Assert.NotNull(tournament);
            Assert.AreEqual("New Tournament", tournament.Caption);
            Assert.AreEqual("First added tournament", tournament.Description);
            Assert.AreEqual(20, tournament.TournamentTime);
        }

        [Test]
        public async Task GetAvaliable_Now_Count_2()
        {
            //Act
            var tournaments = await _client.GetAvaliableAsync(new GetAvaliableRequest()
                {Date = Timestamp.FromDateTime(DateTime.UtcNow)});

            //Assert
            Assert.NotNull(tournaments);
            Assert.NotNull(tournaments.Tournaments);
            Assert.AreEqual(2, tournaments.Tournaments.Count);
        }

        [Test]
        public async Task GetAvailable_AddDays_5_None()
        {
            //Act
            var tournaments = await _client.GetAvaliableAsync(new GetAvaliableRequest()
                {Date = Timestamp.FromDateTime(DateTime.Now.AddDays(5).ToUniversalTime())});

            //Assert
            Assert.AreEqual(0,tournaments.Tournaments.Count);
        }

        [Test]
        public async Task GetExercises_1_NotNull_Count_GreaterThan_0()
        {
            //Act
            var exercises = await _client.GetExercisesAsync(new GetExercisesRequest() {TournamentId = 1});

            //Assert
            Assert.NotNull(exercises);
            Assert.NotNull(exercises.Exercises);
            Assert.Greater(exercises.Exercises.Count, 0);
        }

    }
}