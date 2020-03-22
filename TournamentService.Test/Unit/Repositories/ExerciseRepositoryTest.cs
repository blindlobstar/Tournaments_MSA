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
    public class ExerciseRepositoryTest
    {
        private IExerciseRepository _exerciseRepository;
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

            _exerciseRepository = new ExerciseRepository(tournamentContext);
        }

        [Test]
        public async Task GetForTournament_1_Exerciseid1()
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
            var tournamentExercises = await _exerciseRepository.GetForTournament(1);

            //Assert
            Assert.NotNull(tournamentExercises);
            Assert.Contains(exercise.Text, tournamentExercises.Select(e => e.Text).ToList());
        }
    }
}