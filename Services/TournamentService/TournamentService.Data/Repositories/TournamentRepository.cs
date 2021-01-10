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
    public class TournamentRepository : BaseRepository<TournamentDto, int>, ITournamentRepository
    {
        public TournamentRepository(TournamentContext context) : base(context) { }
        
        public Task<List<TournamentDto>> GetAvailable(DateTime dateTime) =>
            DbSet.Where(t => t.EndDate > dateTime)
                .ToListAsync();

        public Task<List<TournamentDto>> GetForUser(string userId) =>
            DbSet.Where(t => t.TournamentsUsers
                    .Any(tu => tu.UserId.Equals(userId)))
                .ToListAsync();
    }
}