using Common.Data.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class TournamentRepository : BaseRepository<Tournament, int>, ITournamentRepository
    {
        public TournamentRepository(TournamentContext context) : base(context) { }
        public async Task<List<Tournament>> GetAvailable(DateTime dateTime) =>
            await DbSet.Where(t => t.EndDate > dateTime)
                .ToListAsync();

        public async Task<List<Tournament>> GetForUser(string userId) =>
            await DbSet.Where(t => t.TournamentsUsers
                    .Any(tu => tu.UserId.Equals(userId)))
                .ToListAsync();
    }
}