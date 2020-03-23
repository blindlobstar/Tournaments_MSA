using Common.Core.Data;
using Common.Data.EFCore.Repositories;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface ITournamentsUsersRepository : IEFRepository<TournamentsUsers,int>
    {
        
    }
}