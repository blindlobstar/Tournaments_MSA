using Common.Data.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface ITournamentRepository : IEFRepository<Tournament, int>
    {
        Task<List<Tournament>> GetForUser(string userId);
        Task<List<Tournament>> GetAvailable(DateTime dateTime);
    }
}