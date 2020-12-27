using Common.Data.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface ITournamentRepository : IEFRepository<TournamentDto, int>
    {
        Task<List<TournamentDto>> GetForUser(string userId);
        Task<List<TournamentDto>> GetAvailable(DateTime dateTime);
    }
}