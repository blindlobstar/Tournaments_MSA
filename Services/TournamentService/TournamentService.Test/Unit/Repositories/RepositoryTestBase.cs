using Microsoft.EntityFrameworkCore;
using TournamentService.Data;
using TournamentService.Data.Seeds;

namespace TournamentService.Test.Unit.Repositories
{
    public abstract class RepositoryTestBase
    {
        private protected TournamentContext Context { get; private set; }

        protected RepositoryTestBase()
        {
            var options = new DbContextOptionsBuilder<TournamentContext>()
                .UseInMemoryDatabase(databaseName: "TournamentRepositoryTest")
                .Options;
            Context = new TournamentContext(options);
            Context.EnsureSeed();
        }
    }
}
