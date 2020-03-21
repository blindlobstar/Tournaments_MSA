using Microsoft.EntityFrameworkCore;

namespace TournamentService.Data
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> dbContextOptions) : base(dbContextOptions) { }


    }
}