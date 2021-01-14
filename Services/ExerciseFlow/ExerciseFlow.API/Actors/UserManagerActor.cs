using System.Collections.Generic;
using Akka.Actor;
using Common.Core.DataExchange.EventBus;
using ExerciseFlow.API.Services;
using ExerciseFlow.API.Models;

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
        private readonly ITournamentService _client;
        private readonly Dictionary<string, IActorRef> _idToActor;
        private readonly IActorRef _tournamentManagerActor;

        public UserManagerActor(IBusPublisher busPublisher, 
            ITournamentService client)
        {
            _busPublisher = busPublisher;
            _client = client;
            _tournamentManagerActor = Context.ActorOf(TournamentManagerActor.Props(_client));
            _idToActor = new Dictionary<string, IActorRef>();
            Become(WaitForExercises(null, null));
        }

        protected override void OnReceive(object message)
        {
            
        }

        public UntypedReceive WaitForExercises(IActorRef waiter, string userId) => message =>
        {
            switch (message)
            {
                case GetQuestionForUser request:
                    if (_idToActor.TryGetValue(request.UserId, out var userRef))
                    {
                        userRef.Forward(UserActor.GetNextQuestion.Instance);
                        break;
                    }

                    Become(WaitForExercises(Sender, request.UserId));
                    _tournamentManagerActor.Tell(
                        new TournamentManagerActor.GetTournamentExercise(request.TournamentId));
                    break;
                case List<Exercise> exercises:
                    var newUserActor = Context.ActorOf(UserActor.Props(userId, exercises, _busPublisher));
                    _idToActor.Add(userId, newUserActor);
                    newUserActor.Tell(UserActor.GetNextQuestion.Instance, waiter);
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
        };

        public static Props Props(IBusPublisher busPublisher, ITournamentService tournamentService) =>
            Akka.Actor.Props.Create(() => new UserManagerActor(busPublisher, tournamentService));
    }
}
