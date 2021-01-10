using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface IExercisesUsersRepository : IEFRepository<ExercisesUsers, int>
    {
        Task AddAnswer(int ExerciseId, string userId, string Answer);
        Task<List<ExercisesUsers>> GetByTournamentId(int tournamentId);
    }
}
