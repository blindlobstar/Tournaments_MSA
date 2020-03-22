using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise, int>, IExerciseRepository
    {
        public ExerciseRepository(TournamentContext context) : base(context) { }
        public  async Task<List<Exercise>> GetForTournament(int tournamentId)
        {
            return await DbSet.Where(e => e.TournamentId.Equals(tournamentId)).ToListAsync();
        }
    }
}