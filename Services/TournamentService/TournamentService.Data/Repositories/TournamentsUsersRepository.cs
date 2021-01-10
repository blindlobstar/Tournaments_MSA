using Common.Data.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class TournamentsUsersRepository : BaseRepository<TournamentsUsers, int>, ITournamentsUsersRepository
    {
        public TournamentsUsersRepository(TournamentContext context) : base(context) { }

        public Task<List<TournamentsUsers>> GetByTournamentId(int tournamentId) =>
            (from data in DbSet
             where data.TournamentId == tournamentId
             select data)
            .ToListAsync();
    }
}