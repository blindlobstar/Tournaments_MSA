using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TournamentService.API.Logic;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
using System.Linq;

namespace TournamentService.Test.Unit.Locgic
{
    public class TestResultCalucation
    {
        private CalculateTournamentResult _calculateTournamentResult;
        private Mock<ITournamentsUsersRepository> _tournamentsUsersRepository;
        private Mock<IExercisesUsersRepository> _exercisesUsersRepository;
        private List<TournamentsUsers> _tournamentsUsers;
        private List<ExercisesUsers> _exercisesUsers;
        private string _firstUser;
        private string _secondUser;
        private string _thirdUser;


        [SetUp]
        public void SetUp()
        {
            _firstUser = Guid.NewGuid().ToString();
            _secondUser = Guid.NewGuid().ToString();
            _thirdUser = Guid.NewGuid().ToString();

            var tournament = new TournamentDto()
            {
                Id = 1,
                Caption = "New Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };


            var exercises = new ExerciseDto[]
            {
                new ExerciseDto()
                {
                    Id = 1,
                    Text = "1+1",
                    Answer = "2",
                    OrderNumber = 1,
                    Tournament = tournament
                },
                new ExerciseDto()
                {
                    Id = 2,
                    Text = "3+5",
                    Answer = "8",
                    OrderNumber = 2,
                    Tournament = tournament
                },
                new ExerciseDto()
                {
                    Id = 3,
                    Text = "First three letters of alphabet",
                    Answer = "a,b,c",
                    OrderNumber = 3,
                    Tournament = tournament
                }
            };


            _tournamentsUsers = new List<TournamentsUsers>()
            {
                new TournamentsUsers()
                {
                    Tournament = tournament,
                    TournamentId = tournament.Id,
                    UserId = _firstUser,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddMinutes(20)
                },
                new TournamentsUsers()
                {
                    Tournament = tournament,
                    TournamentId = tournament.Id,
                    UserId = _secondUser,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddMinutes(10)
                },
                new TournamentsUsers()
                {
                    Tournament = tournament,
                    TournamentId = tournament.Id,
                    UserId = _thirdUser,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddMinutes(20)
                }
            };

            _exercisesUsers = new List<ExercisesUsers>()
            {
                new ExercisesUsers()
                {
                    Exercise = exercises[0],
                    UserAnswer = "2",
                    ExerciseId = exercises[0].Id,
                    UserId = _firstUser,
                    IsCorrect = true
                },
                new ExercisesUsers()
                {
                    Exercise = exercises[0],
                    UserAnswer = "2",
                    ExerciseId = exercises[0].Id,
                    UserId = _secondUser,
                    IsCorrect = true
                },
                new ExercisesUsers()
                {
                    Exercise = exercises[0],
                    UserAnswer = "2",
                    ExerciseId = exercises[0].Id,
                    UserId = _thirdUser,
                    IsCorrect = true
                },
                new ExercisesUsers()
                {
                    Exercise = exercises[1],
                    UserAnswer = "8",
                    ExerciseId = exercises[1].Id,
                    UserId = _firstUser,
                    IsCorrect = true
                },
                new ExercisesUsers()
                {
                    Exercise = exercises[1],
                    UserAnswer = "8",
                    ExerciseId = exercises[1].Id,
                    UserId = _secondUser,
                    IsCorrect = true
                },
                new ExercisesUsers()
                {
                    Exercise = exercises[1],
                    UserAnswer = "9",
                    ExerciseId = exercises[1].Id,
                    UserId = _thirdUser,
                    IsCorrect = false
                },
            };

            _tournamentsUsersRepository = new Mock<ITournamentsUsersRepository>();
            _exercisesUsersRepository = new Mock<IExercisesUsersRepository>();
        }


        [Test]
        public void IsAnswerCorrect_number_true()
        {
            //Arrange
            _calculateTournamentResult = new CalculateTournamentResult(_exercisesUsersRepository.Object, _tournamentsUsersRepository.Object);
            
            //Act
            var isCorrect = _calculateTournamentResult.IsAnswerCorrect("2", "2");

            //Assert
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void IsAnswerCorrect_number_false()
        {
            //Arrange
            _calculateTournamentResult = new CalculateTournamentResult(_exercisesUsersRepository.Object, _tournamentsUsersRepository.Object);
            
            //Act
            var isCorrect = _calculateTournamentResult.IsAnswerCorrect("3", "2");

            //Assert
            Assert.IsFalse(isCorrect);
        }

        [Test]
        public void GetPlaces_SecondUser_FirstPlace()
        {
            //Arrange
            _exercisesUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_exercisesUsers);
            _tournamentsUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_tournamentsUsers);
            _calculateTournamentResult = new CalculateTournamentResult(_exercisesUsersRepository.Object, _tournamentsUsersRepository.Object);

            //Act
            var userList = _calculateTournamentResult.GetPlaces(_tournamentsUsers, _exercisesUsers);

            //Assert
            Assert.AreEqual(userList.First().Place, 1);
            Assert.AreEqual(userList.First().UserId, _secondUser);
        }

        [Test]
        public void GetPlaces_FirstUser_SecondPlace()
        {
            //Arrange
            _exercisesUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_exercisesUsers);
            _tournamentsUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_tournamentsUsers);
            _calculateTournamentResult = new CalculateTournamentResult(_exercisesUsersRepository.Object, _tournamentsUsersRepository.Object);

            //Act
            var userList = _calculateTournamentResult.GetPlaces(_tournamentsUsers, _exercisesUsers);

            //Assert
            Assert.AreEqual(userList.First(u => u.UserId == _firstUser).Place, 2);
        }

        [Test]
        public void GetPlaces_ThirdUser_LastPlace()
        {
            //Arrange
            _exercisesUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_exercisesUsers);
            _tournamentsUsersRepository.Setup(x => x.GetByTournamentId(1)).ReturnsAsync(_tournamentsUsers);
            _calculateTournamentResult = new CalculateTournamentResult(_exercisesUsersRepository.Object, _tournamentsUsersRepository.Object);

            //Act
            var userList = _calculateTournamentResult.GetPlaces(_tournamentsUsers, _exercisesUsers);

            //Assert
            Assert.AreEqual(userList.Last().Place, 3);
            Assert.AreEqual(userList.First(u => u.UserId == _thirdUser).Place, 3);
        }



    }
}