using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise, int>, IExerciseRepository
    {
        public ExerciseRepository(TournamentContext context) : base(context) { }
        public Task<List<Exercise>> GetForTournament(int tournamentId)
        {
            throw new System.NotImplementedException();
        }
    }
}