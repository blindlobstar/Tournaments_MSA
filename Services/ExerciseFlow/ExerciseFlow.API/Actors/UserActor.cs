using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.EventBus;
using ExerciseFlow.API.Models;

namespace ExerciseFlow.API.Actors
{
    public sealed class UserActor : UntypedActor
    {
        public sealed class GetNextQuestion
        {
            public static readonly GetNextQuestion Instance = new GetNextQuestion();
        }

        public sealed class NoMoreQuestions
        {
            public static readonly NoMoreQuestions Instance = new NoMoreQuestions();
        }


        private readonly string _userId;
        private readonly Queue<IActorRef> _unansweredExercises;
        private readonly IBusPublisher _busPublisher;
        private readonly Dictionary<int, IActorRef> _exerciseIdToActor;

        public UserActor(string userId, IEnumerable<Exercise> exercises, IBusPublisher busPublisher)
        {
            _userId = userId;
            _busPublisher = busPublisher;
            _exerciseIdToActor = new Dictionary<int, IActorRef>();

            exercises
                .Select(e => new { 
                    actor = Context.ActorOf(ExerciseActor.Props(e.Id, e.Text, e.Answer, 15)),
                    ex = e.Id
                })
                .ForEach(x => _exerciseIdToActor.Add(x.ex, x.actor));

            _unansweredExercises = new Queue<IActorRef>(_exerciseIdToActor.Values);
            
        }

        private IActorRef CurrentEx { get; set; }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetNextQuestion _:
                    if (!_unansweredExercises.TryPeek(out var exerciseActorRef))
                    {
                        Sender.Tell(NoMoreQuestions.Instance);
                        Context.Stop(Self);
                    }

                    CurrentEx = exerciseActorRef;
                    exerciseActorRef.Forward(new ExerciseActor.GetExerciseRequest(1));
                    break;
                case ExerciseActor.TakeAnswerRequest request:
                    CurrentEx.Tell(request);
                    CurrentEx = _unansweredExercises.Dequeue();
                    break;
                case ExerciseActor.TakeAnswerResponse request:
                    _busPublisher.Send(new AddAnswer()
                        {ExerciseId = request.ExerciseId, UserId = _userId, UserAnswer = request.Answer});
                    break;
                case Terminated _:

                    break;
            }
        }

        public static Props Props(string userId, IEnumerable<Exercise> exercises, IBusPublisher busPublisher) =>
            Akka.Actor.Props.Create(() => new UserActor(userId, exercises, busPublisher));
    }
}
