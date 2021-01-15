using System.Threading.Tasks;
using Akka.Actor;
using Common.Contracts.ExerciseFlow.Commands;
using Common.Core.DataExchange.Handlers;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Actors.Providers;

namespace ExerciseFlow.API.Handlers
{
    public class AddUserAnswerHandler : ICommandHandler<AddUserAnswer>
    {
        private readonly IActorRef _actor;

        public AddUserAnswerHandler(UserManagerProvider provider)
        {
            _actor = provider();
        }

        public async Task HandleAsync(AddUserAnswer command)
        {
            await Task.Yield();
            _actor.Tell(new UserManagerActor.TakeUserAnswer(command.UserId, command.Answer));
        }
    }
}