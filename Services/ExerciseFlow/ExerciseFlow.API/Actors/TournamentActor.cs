using System.Collections.Generic;
using Akka.Actor;
using ExerciseFlow.API.Services;
using Exercise = ExerciseFlow.API.Models.Exercise;

namespace ExerciseFlow.API.Actors
{
    public sealed class TournamentActor : UntypedActor
    {
        public sealed class GetTournamentExercise
        {
            public static readonly GetTournamentExercise Instance = new GetTournamentExercise();
        }

        public TournamentActor(ITournamentService client, int tournamentId)
        {
            Client = client;
            TournamentId = tournamentId;
            Exercises = new List<Exercise>();
        }

        private List<Exercise> Exercises { get; }
        private ITournamentService Client { get; }
        private int TournamentId { get; }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetTournamentExercise request:
                    if (Exercises.Count == 0)
                    {
                        var exercises =
                            Client.GetExercises(TournamentId);
                        Exercises.AddRange(exercises);
                    }

                    Sender.Tell(Exercises);
                    break;
            }
        }

        public static Props Props(ITournamentService client, int tournamentId) =>
            Akka.Actor.Props.Create(() => new TournamentActor(client, tournamentId));
    }
}
