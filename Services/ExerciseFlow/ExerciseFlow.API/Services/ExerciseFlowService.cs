using System.Threading.Tasks;
using Akka.Actor;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Actors.Providers;
using Grpc.Core;

namespace ExerciseFlow.API.Services
{
    public class ExerciseFlowService : global::ExerciseFlowService.ExerciseFlowServiceBase
    {
        private readonly IActorRef _actor;

        public ExerciseFlowService(UserManagerProvider provider)
        {
            _actor = provider();
        }

        public override async Task<GetQuestionResponse> GetQuestion(GetQuestionRequest request, ServerCallContext context)
        {
            var response = new GetQuestionResponse();
            var question = await _actor.Ask(new UserManagerActor.GetQuestionForUser(request.UserId, request.TournamentId));
            
            if (question is not ExerciseActor.GetExerciseResponse getExerciseResponse)
                return response;
            
            response.Text = getExerciseResponse.Question;
            response.Time = getExerciseResponse.Time;

            return response;
        }
    }
}