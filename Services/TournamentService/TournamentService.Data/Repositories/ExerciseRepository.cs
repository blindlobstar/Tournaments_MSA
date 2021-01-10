using Common.Data.EFCore.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class ExerciseRepository : BaseRepository<ExerciseDto, int>, IExerciseRepository
    {
        public ExerciseRepository(TournamentContext context) : base(context) { }
        public Task<List<ExerciseDto>> GetForTournament(int tournamentId) =>
            DbSet.Where(e => e.TournamentId.Equals(tournamentId)).ToListAsync();
    }
}