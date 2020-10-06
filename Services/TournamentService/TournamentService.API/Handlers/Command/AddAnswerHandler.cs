using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using TournamentService.Core.Data;

namespace TournamentService.API.Handlers.Command
{
    public class AddAnswerHandler : ICommandHandler<AddAnswer>
    {
        private readonly IExercisesUsersRepository _exercisesUsersRepository;

        public AddAnswerHandler(IExercisesUsersRepository exerciseRepository)
        {
            _exercisesUsersRepository = exerciseRepository;
        }

        public async Task HandleAsync(AddAnswer command)
        {
            await _exercisesUsersRepository.AddAnswer(command.ExerciseId, command.UserId, command.UserAnswer);
            await _exercisesUsersRepository.SaveChanges();
        }
    }
}
