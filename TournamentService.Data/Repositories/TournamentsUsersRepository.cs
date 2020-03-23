using Common.Data.EFCore.Repositories;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class TournamentsUsersRepository : BaseRepository<TournamentsUsers, int>, ITournamentsUsersRepository
    {
        public TournamentsUsersRepository(TournamentContext context) : base(context) { }
    }
}