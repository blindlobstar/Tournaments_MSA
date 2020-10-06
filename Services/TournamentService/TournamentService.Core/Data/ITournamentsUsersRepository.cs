using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface ITournamentsUsersRepository : IEFRepository<TournamentsUsers,int>
    {
        Task<List<TournamentsUsers>> GetForTournament(int tournamentId);
    }
}