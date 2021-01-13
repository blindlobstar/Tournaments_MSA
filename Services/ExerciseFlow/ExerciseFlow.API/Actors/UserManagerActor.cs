using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Common.Core.DataExchange.EventBus;
using GrpcTournamentService;
using Exercise = ExerciseFlow.API.Models.Exercise;

namespace ExerciseFlow.API.Actors
{
    public class UserManagerActor : UntypedActor
    {
        public sealed class GetQuestionForUser
        {
            public GetQuestionForUser(string userId, int tournamentId)
            {
                UserId = userId;
                TournamentId = tournamentId;
            }

            public string UserId { get; }
            public int TournamentId { get; }
        }

        public sealed class TakeUserAnswer
        {
            public TakeUserAnswer(string userId, string answer)
            {
                UserId = userId;
                Answer = answer;
            }

            public string UserId { get; }
            public string Answer { get; }
        }

        public sealed class UserDoNotWaitForAnAnswer
        {
            public static readonly UserDoNotWaitForAnAnswer Instance = new UserDoNotWaitForAnAnswer();
        }

        private readonly IBusPublisher _busPublisher;
        private readonly TournamentService.TournamentServiceClient _client;
        private readonly Dictionary<string, IActorRef> _idToActor;

        public UserManagerActor(IBusPublisher busPublisher, 
            TournamentService.TournamentServiceClient client)
        {
            _busPublisher = busPublisher;
            _client = client;
            _idToActor = new Dictionary<string, IActorRef>();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetQuestionForUser request:
                    if (_idToActor.TryGetValue(request.UserId, out var userRef))
                    {
                        userRef.Forward(UserActor.GetNextQuestion.Instance);
                        break;
                    }
                    //Get exercise from tournamentService.API
                    var response =
                        _client.GetExercises(new GetExercisesRequest() {TournamentId = request.TournamentId});

                    var exercises = from ex in response.Exercises
                        select new Exercise()
                        {
                            Answer = ex.Answer, 
                            Id = ex.Id, 
                            Text = ex.Text, 
                            OrderNumber = ex.OrderNumber,
                            TournamentId = ex.TournamentId
                        };

                    var newUserActor = Context.ActorOf(UserActor.Props(request.UserId, exercises, _busPublisher));
                    _idToActor.Add(request.UserId, newUserActor);
                    newUserActor.Forward(UserActor.GetNextQuestion.Instance);
                    break;
                case TakeUserAnswer request:
                    if (_idToActor.TryGetValue(request.UserId, out var userThatAnswered))
                    {
                        userThatAnswered.Tell(new ExerciseActor.TakeAnswerRequest(request.Answer));
                        break;
                    }
                    Sender.Tell(UserDoNotWaitForAnAnswer.Instance);
                    break;
            }
        }
    }
}
