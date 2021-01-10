using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
using TournamentService.Data.Repositories;

namespace TournamentService.Test.Unit.Repositories
{
    [TestFixture]
    public sealed class ExerciseRepositoryTest : RepositoryTestBase
    {
        private IExerciseRepository _exerciseRepository;

        [SetUp]
        public void Setup()
        {
            _exerciseRepository = new ExerciseRepository(Context);
        }

        [Test]
        public async Task GetForTournament_1_ExerciseId_1()
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
            var tournamentExercises = await _exerciseRepository.GetForTournament(1);

            //Assert
            Assert.NotNull(tournamentExercises);
            Assert.Contains(exercise.Text, tournamentExercises.Select(e => e.Text).ToList());
        }
    }
}