﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TournamentService.API.Controllers;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Test.Unit.Controllers
{
    public class TournamentControllerTest
    {
        private TournamentController _tournamentController;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IExerciseRepository> _exerciseRepositoryMock;
        private Mock<ITournamentsUsersRepository> _tournamentsUsersRepositoryMock;
        private TournamentDto _tournament;
        private List<TournamentDto> _tournaments;
        private List<ExerciseDto> _exercises;

        [SetUp]
        public void SetUp()
        {
            _tournament = new TournamentDto()
            {
                Id = 1,
                Caption = "New Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };

            _tournaments = new List<TournamentDto>()
            {
                _tournament,
                new TournamentDto()
                {
                    Id = 2,
                    Caption = "Absolute New Tournament",
                    Description = "Second added tournament",
                    StartDate = DateTime.Now.AddDays(-3),
                    EndDate = DateTime.Now.AddDays(-1),
                    TournamentTime = 20
                }
            };

            _exercises = new List<ExerciseDto>()
            {
                new ExerciseDto()
                {
                    Id = 1,
                    Text = "1+1",
                    Answer = "2",
                    OrderNumber = 1,
                    Tournament = _tournament
                },
                new ExerciseDto()
                {
                    Id = 2,
                    Text = "3+5",
                    Answer = "8",
                    OrderNumber = 2,
                    Tournament = _tournament
                },
                new ExerciseDto()
                {
                    Id = 3,
                    Text = "First three letters of alphabet",
                    Answer = "a,b,c",
                    OrderNumber = 3,
                    Tournament = _tournament
                }
            };

            //Initialize repositories
            _tournamentRepositoryMock = new Mock<ITournamentRepository>();
            _exerciseRepositoryMock = new Mock<IExerciseRepository>();
            _tournamentsUsersRepositoryMock = new Mock<ITournamentsUsersRepository>();
        }

        // [Test]
        // public async Task Get_1_NewTournament()
        // {
        //     //Arrange
        //     _tournamentRepositoryMock.Setup(r => r.Get(1)).ReturnsAsync(_tournament);
        //     _tournamentController = new TournamentController(_tournamentRepositoryMock.Object, _tournamentsUsersRepositoryMock.Object, _exerciseRepositoryMock.Object);

        //     //Act
        //     var tournament = await _tournamentController.Get(1);

        //     //Assert
        //     Assert.NotNull(tournament.Value);
        //     Assert.AreEqual("New Tournament", tournament.Value.Caption);
        // }

        // [Test]
        // public void Get_10_ArgumentException()
        // {
        //     //Arrange
        //     _tournamentRepositoryMock.Setup(r => r.Get(1)).ReturnsAsync(_tournament);
        //     _tournamentController = new TournamentController(_tournamentRepositoryMock.Object, _tournamentsUsersRepositoryMock.Object, _exerciseRepositoryMock.Object);

        //     //Assert
        //     Assert.ThrowsAsync<ArgumentException>(() => _tournamentController.Get(10), "Can't find tournament with given id");
        // }

        // [Test]
        // public async Task GetAll_NewTournament()
        // {
        //     //Arrange
        //     _tournamentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(_tournaments);
        //     _tournamentController = new TournamentController(_tournamentRepositoryMock.Object, _tournamentsUsersRepositoryMock.Object, _exerciseRepositoryMock.Object);

        //     //Act
        //     var tournaments = await _tournamentController.GetAll();

        //     //Assert
        //     Assert.NotNull(tournaments);
        //     Assert.Contains("New Tournament", tournaments.Value.Select(t => t.Caption).ToList());
        // }

        // [Test]
        // public async Task GetAvailable_datetimeNow_NewTournament()
        // {
        //     //Arrange
        //     var date = DateTime.Now;
        //     _tournamentRepositoryMock.Setup(r => r.GetAvailable(date))
        //         .ReturnsAsync(_tournaments.Where(t => DateTime.Compare(t.EndDate, date) > 0).ToList());
        //     _tournamentController = new TournamentController(_tournamentRepositoryMock.Object, _tournamentsUsersRepositoryMock.Object, _exerciseRepositoryMock.Object);

        //     //Act
        //     var tournaments = await _tournamentController.GetAvailable(date);

        //     //Assert
        //     Assert.NotNull(tournaments);
        //     Assert.Contains("New Tournament", tournaments.Value.Select(t => t.Caption).ToList());
        //     Assert.Null(tournaments.Value.FirstOrDefault(t => t.Caption == "Absolute New Tournament"));
        // }

        // [Test]
        // public async Task GetAvailable_datetimeNow_NotFound()
        // {
        //     //Arrange
        //     var date = DateTime.Now;
        //     _tournamentRepositoryMock.Setup(r => r.GetAvailable(date));
        //     _tournamentController = new TournamentController(_tournamentRepositoryMock.Object, _tournamentsUsersRepositoryMock.Object, _exerciseRepositoryMock.Object);

        //     //Act
        //     var response = await _tournamentController.GetAvailable(date);

        //     //Assert
        //     Assert.IsNotNull(response);
        //     Assert.AreEqual(response.Result.GetType(), typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        // }
    }
}