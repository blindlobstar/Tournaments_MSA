using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Models;

namespace TournamentService.Core.Data
{
    public interface IExerciseRepository : IEFRepository<ExerciseDto, int>
    {
        Task<List<ExerciseDto>> GetForTournament(int tournamentId);
    }
}