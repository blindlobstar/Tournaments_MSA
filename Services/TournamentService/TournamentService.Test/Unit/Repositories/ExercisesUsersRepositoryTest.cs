using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
using TournamentService.Data.Repositories;

namespace TournamentService.Test.Unit.Repositories
{
    [TestFixture]
    public sealed class ExercisesUsersRepositoryTest : RepositoryTestBase
    {
        private IExercisesUsersRepository _exercisesUsersRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            _exercisesUsersRepository = new ExercisesUsersRepository(Context);
        }

        [Test]
        public async Task AddAnswer_Get_UserId_abc_Answer_abc_ExerciseId_4()
        {
            //Act
            await _exercisesUsersRepository.AddAnswer(4, "abc", "abc");
            await _exercisesUsersRepository.SaveChanges();
            var row = (await _exercisesUsersRepository.GetAll()).Last();

            //Assert
            Assert.AreEqual("abc", row.UserId);
            Assert.AreEqual("abc", row.UserAnswer);
            Assert.AreEqual(4, row.ExerciseId);
        }

        [Test]
        public async Task GetByTournamentId_1_Returns_One_UserId_5e7398bde6ab1940182c5cfd_ExerciseId_1_UserAnswer_2_IsCorrect_True()
        {
            //Act
            var rows = await _exercisesUsersRepository.GetByTournamentId(1);

            //Arrange
            Assert.AreEqual(1,rows.Count);
            Assert.AreEqual("2", rows.First().UserAnswer);
            Assert.IsTrue(rows.First().IsCorrect);
            Assert.AreEqual("5e7398bde6ab1940182c5cfd", rows.First().UserId);
        }
    }
}