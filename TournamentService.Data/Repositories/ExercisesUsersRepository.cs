using Common.Data.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.Data.Repositories
{
    public class ExercisesUsersRepository : BaseRepository<ExercisesUsers, int>, IExercisesUsersRepository
    {
        public ExercisesUsersRepository(TournamentContext dataContext) : base(dataContext) { }

        public async Task AddAnswer(int exerciseId, string userId, string answer)
        {
            await DbSet.AddAsync(new ExercisesUsers()
            {
                ExerciseId = exerciseId,
                UserId = userId,
                UserAnswer = answer
            });
        }

        public Task<List<ExercisesUsers>> GetForExercises(IEnumerable<int> exercisesIds)
        {
            return (from userAnswer in DbSet
                    where exercisesIds.Contains(userAnswer.ExerciseId)
                    select userAnswer).ToListAsync();

        }
    }
}
