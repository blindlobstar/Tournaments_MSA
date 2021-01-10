using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TournamentService.Core.Data;
using TournamentService.Data.Repositories;

namespace TournamentService.Test.Unit.Repositories
{
    [TestFixture]
    public sealed class TournamentsUsersRepositoryTest : RepositoryTestBase
    {
        private ITournamentsUsersRepository _tournamentsUsersRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            _tournamentsUsersRepository = new TournamentsUsersRepository(Context);
        }

        [Test]
        public async Task GetByTournamentId_1_Count_is_1_UserId_is_5e7398bde6ab1940182c5cfd()
        {
            //Act
            var testedTournament = await _tournamentsUsersRepository.GetByTournamentId(1);

            //Assert
            Assert.AreEqual(1, testedTournament.Count);
            Assert.AreEqual("5e7398bde6ab1940182c5cfd", testedTournament.First().UserId);
        }

        [Test]
        public async Task GetByTournamentId_2_Count_is_0()
        {
            //Act
            var tournaments = await _tournamentsUsersRepository.GetByTournamentId(2);

            //Assert
            Assert.AreEqual(0, tournaments.Count);
        }
    }
}
