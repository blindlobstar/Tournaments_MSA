using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
using TournamentService.Data;
using TournamentService.Data.Repositories;
using TournamentService.Data.Seeds;

namespace TournamentService.Test.Unit.Repositories
{
    public class TournamentRepositoryTest
    {
        private ITournamentRepository _tournamentRepository;
        private DbContextOptions<TournamentContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<TournamentContext>()
                .UseInMemoryDatabase(databaseName: "TournamentRepositoryTest")
                .Options;
            using var context = new TournamentContext(_options);
            context.EnsureSeed();
            var tournamentContext = new TournamentContext(_options);
            
            _tournamentRepository = new TournamentRepository(tournamentContext);
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
            var exercise = new Exercise()
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
        public void Add_ABCTournament()
        {
            //Arrange
            var tournament = new Tournament()
            {
                Caption = "ABCTournament",
                EndDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now
            };

            //Act
            var newTournament = _tournamentRepository.Add(tournament);

            //Assert
            Assert.NotNull(newTournament);
            Assert.Greater(newTournament.Id, 1);
            Assert.AreEqual("ABCTournament", newTournament.Caption);
        }

        [Test]
        public async Task Update_NewTournament_ABCTournament()
        {
            //Arrange
            var tournament = await _tournamentRepository.Get(1);
            tournament.Caption = "ABCTournament";

            //Act
            _tournamentRepository.Update(tournament);
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
        public async Task Delete_NewTournament_null()
        {
            //Arrange
            var tournament = await _tournamentRepository.Get(1);

            //Act
            _tournamentRepository.Delete(tournament);
            _tournamentRepository.SaveChanges();
            var deletedTournament = await _tournamentRepository.Get(1);

            //Assert
            Assert.Null(deletedTournament);
        }

        [Test]
        public async Task Delete_1_null()
        {
            //Act
            _tournamentRepository.Delete(1);
            _tournamentRepository.SaveChanges();
            var deletedTournament = await _tournamentRepository.Get(1);

            //Assert
            Assert.Null(deletedTournament);
        }

    }
}