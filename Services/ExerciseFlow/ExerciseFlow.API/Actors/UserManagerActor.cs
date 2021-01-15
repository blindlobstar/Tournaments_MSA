using System.Collections.Generic;
using System.Linq;
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
            Become(WaitForExercises(new Dictionary<string, IActorRef>(), new List<(string, int)>()));
        }

        protected override void OnReceive(object message)
        {
            
        }

        public UntypedReceive WaitForExercises(Dictionary<string, IActorRef> waiters, 
            List<(string, int)> tournamentToUser) => message =>
        {
            switch (message)
            {
                case GetQuestionForUser request:
                    //If user actor exist, just forward a message from sender
                    if (_idToActor.TryGetValue(request.UserId, out var userRef))
                    {
                        userRef.Forward(UserActor.GetNextQuestion.Instance);
                        break;
                    }

                    //Otherwise add user for a waiter list and become to wait for exercise state
                    waiters.Add(request.UserId, Sender);
                    tournamentToUser.Add((request.UserId, request.TournamentId));
                    Become(WaitForExercises(waiters, tournamentToUser));

                    _tournamentManagerActor.Tell(
                        new TournamentManagerActor.GetTournamentExercise(request.TournamentId));
                    break;
                case List<Exercise> exercises:
                    //Get exercises TournamentId 
                    var tournamentId = exercises.First().TournamentId;
                    
                    //Find user and sender, that wait for exercises, from this tournament
                    var userTournament = tournamentToUser.First(x => x.Item2 == tournamentId);
                    var waiter = waiters[userTournament.Item1];
                    
                    //Create user actor, and ask him for a next question
                    var newUserActor = Context.ActorOf(UserActor.Props(userTournament.Item1, exercises, _busPublisher));
                        _idToActor.Add(userTournament.Item1, newUserActor);
                    newUserActor.Tell(UserActor.GetNextQuestion.Instance, waiter);
                    
                    //Remove user for waiter list, and become to next state
                    tournamentToUser.Remove(userTournament);
                    waiters.Remove(userTournament.Item1);
                    Become(WaitForExercises(waiters, tournamentToUser));
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
