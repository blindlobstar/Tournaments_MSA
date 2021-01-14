using System.Collections.Generic;
using Akka.Actor;
using Akka.Actor.Dsl;
using ExerciseFlow.API.Services;

namespace ExerciseFlow.API.Actors
{
    public class TournamentManagerActor : UntypedActor
    {
        public sealed class GetTournamentExercise
        {
            public GetTournamentExercise(int tournamentId)
            {
                TournamentId = tournamentId;
            }

            public int TournamentId { get; }
        }

        private readonly Dictionary<int, IActorRef> _idToActor;
        private readonly ITournamentService _client;

        public TournamentManagerActor(ITournamentService client)
        {
            _client = client;
            _idToActor = new Dictionary<int, IActorRef>();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetTournamentExercise req:
                    if (_idToActor.TryGetValue(req.TournamentId, out var tournamentActor))
                    {
                        tournamentActor.Forward(TournamentActor.GetTournamentExercise.Instance);
                        break;
                    }

                    var actor = Context.ActorOf(TournamentActor.Props(_client, req.TournamentId));
                    _idToActor.Add(req.TournamentId, tournamentActor);
                    actor.Forward(TournamentActor.GetTournamentExercise.Instance);
                    break;
            }
        }

        public static Props Props(ITournamentService client) =>
            Akka.Actor.Props.Create(() => new TournamentManagerActor(client));
    }
}
