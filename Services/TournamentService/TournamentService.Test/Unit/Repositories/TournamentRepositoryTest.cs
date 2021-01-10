using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
using TournamentService.Data.Repositories;

namespace TournamentService.Test.Unit.Repositories
{
    [TestFixture]
    public class TournamentRepositoryTest : RepositoryTestBase
    {
        private ITournamentRepository _tournamentRepository;
        private TournamentDto _baseTournament;

        [SetUp]
        public void Setup()
        {
            _tournamentRepository = new TournamentRepository(Context);
            _baseTournament = new TournamentDto()
            {
                Id = 1,
                Caption = "New Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };
        }

        [Test]
        public async Task Get_id1_NewTournament()
        {
            //Act
            var tournament = await _tournamentRepository.Get(1);

            //Assert
            Assert.NotNull(tournament);
            Assert.AreEqual("New Tournament", tournament.Caption);
        }

        [Test]
        public async Task Get_id1includeExercise_NewTournamentWithExercise()
        {
            //Arrange
            var exercise = new ExerciseDto()
            {
                Id = 1,
                Text = "1+1",
                Answer = "2",
                OrderNumber = 1,
                TournamentId = 1
            };

            //Act
            var tournament = await _tournamentRepository.Get(1, "Exercises");

            //Assert
            Assert.NotNull(tournament);
            Assert.NotNull(tournament.Exercises);
            Assert.AreEqual("New Tournament", tournament.Caption);
            Assert.Greater(tournament.Exercises.Count, 0);
            Assert.Contains(exercise.Text, tournament.Exercises.Select(e => e.Text).ToList());
        }

        [Test]
        public async Task Get_id1includesExercise_NewTournamentWithExercise()
        {
            //Arrange
            var includes = new List<string>()
            {
                "Exercises"
            };

            //Act
            var tournament = await _tournamentRepository.Get(1, includes);


            //Assert
            Assert.NotNull(tournament);
            Assert.NotNull(tournament.Exercises);
            Assert.AreEqual("New Tournament", tournament.Caption);
            Assert.Greater(tournament.Exercises.Count, 0);
            Assert.Contains("1+1", tournament.Exercises.Select(e => e.Text).ToList());
        }

        [Test]
        public async Task GetAll_NewTournament()
        {
            //Act
            var tournaments = await _tournamentRepository.GetAll();

            //Assert
            Assert.NotNull(tournaments);
            Assert.Greater(tournaments.Count, 0);
            Assert.Contains("New Tournament", tournaments.Select(t => t.Caption).ToList());
        }

        [Test]
        public async Task GetAll_Exercise_NewTournamentWithExercise()
        {
            //Act
            var tournaments = await _tournamentRepository.GetAll("Exercises");

            //Assert
            Assert.NotNull(tournaments);
            Assert.Greater(tournaments.Count, 0);
            Assert.Contains("New Tournament", tournaments.Select(t => t.Caption).ToList());
            Assert.NotNull(tournaments[0].Exercises);
            Assert.Greater(tournaments[0].Exercises.Count, 0);
            Assert.Contains("1+1", tournaments[0].Exercises.Select(e => e.Text).ToList());
        }

        [Test]
        public async Task GetAll_ListExercise_NewTournamentWithExercise()
        {
            //Arrange
            var includes = new List<string>()
            {
                "Exercises"
            };

            //Act
            var tournaments = await _tournamentRepository.GetAll(includes);

            //Assert
            Assert.NotNull(tournaments);
            Assert.Greater(tournaments.Count, 0);
            Assert.Contains("New Tournament", tournaments.Select(t => t.Caption).ToList());
            Assert.NotNull(tournaments[0].Exercises);
            Assert.Greater(tournaments[0].Exercises.Count, 0);
            Assert.Contains("1+1", tournaments[0].Exercises.Select(e => e.Text).ToList());
        }

        [Test]
        public async Task Add_ABCTournament()
        {
            //Arrange
            var tournament = new TournamentDto()
            {
                Caption = "ABCTournament",
                EndDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now
            };

            //Act
            await _tournamentRepository.Add(tournament);
            await _tournamentRepository.SaveChanges();

            //Assert
            Assert.NotNull(tournament);
            Assert.Greater(tournament.Id, 1);
            Assert.AreEqual("ABCTournament", tournament.Caption);
        }

        [Test]
        public async Task Update_NewTournament_ABCTournament()
        {
            //Arrange
            var tournament = await _tournamentRepository.Get(1);
            tournament.Caption = "ABCTournament";

            //Act
            _tournamentRepository.Update(tournament);
            await _tournamentRepository.SaveChanges();
            var updatedTournament = await _tournamentRepository.Get(1);

            //Assert
            Assert.NotNull(updatedTournament);
            Assert.AreEqual("ABCTournament", updatedTournament.Caption);
        }

        [Test]
        public async Task GetForUser_5e7398bde6ab1940182c5cfd_NewTournament()
        {
            //Act
            var tournaments = await _tournamentRepository.GetForUser("5e7398bde6ab1940182c5cfd");

            //Assert
            Assert.NotNull(tournaments);
            Assert.Greater(tournaments.Count, 0);
            Assert.Contains("New Tournament", tournaments.Select(t => t.Caption).ToList());
        }

        [Test]
        public async Task GetAvailable_DateTimeNow_NewTournament()
        {
            //Act
            var tournaments = await _tournamentRepository.GetAvailable(DateTime.Now);

            //Assert
            Assert.NotNull(tournaments);
            Assert.Greater(tournaments.Count, 0);
            Assert.Contains("New Tournament", tournaments.Select(t => t.Caption).ToList());
        }

        [Test]
        public async Task Delete_OldTournament_null()
        {
            //Arrange
            var tournament = await _tournamentRepository.Get(2);

            //Act
            _tournamentRepository.Delete(tournament);
            await _tournamentRepository.SaveChanges();
            var deletedTournament = await _tournamentRepository.Get(2);

            //Assert
            Assert.Null(deletedTournament);
            await _tournamentRepository.Add(tournament);
            await _tournamentRepository.SaveChanges();
        }

        [Test]
        public async Task Delete_3_IsExisted_true_AfterDeleted_null()
        {
            //Arrange
            var beforeDeleted = await _tournamentRepository.Get(3);
            var isExisted = beforeDeleted is not null;

            //Act
            await _tournamentRepository.Delete(3);
            await _tournamentRepository.SaveChanges();
            var afterDeleted = await _tournamentRepository.Get(3);

            //Assert
            Assert.IsTrue(isExisted);
            Assert.Null(afterDeleted);
        }

    }
}