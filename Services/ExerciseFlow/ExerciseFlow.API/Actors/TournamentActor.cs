using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using GrpcTournamentService;
using Exercise = ExerciseFlow.API.Models.Exercise;

namespace ExerciseFlow.API.Actors
{
    public sealed class TournamentActor : UntypedActor
    {
        public sealed class GetTournamentExercise
        {
            public static readonly GetTournamentExercise Instance = new GetTournamentExercise();
        }

        public TournamentActor(TournamentService.TournamentServiceClient client, int tournamentId)
        {
            Client = client;
            TournamentId = tournamentId;
            Exercises = new List<Exercise>();
        }

        private List<Exercise> Exercises { get; }
        private TournamentService.TournamentServiceClient Client { get; }
        private int TournamentId { get; }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetTournamentExercise request:
                    if (Exercises.Count == 0)
                    {
                        var response =
                            Client.GetExercises(new GetExercisesRequest() { TournamentId = TournamentId });

                        Exercises.AddRange(from ex in response.Exercises
                            select new Exercise()
                            {
                                Answer = ex.Answer,
                                Id = ex.Id,
                                Text = ex.Text,
                                OrderNumber = ex.OrderNumber,
                                TournamentId = ex.TournamentId
                            });
                    }

                    Sender.Tell(Exercises);
                    break;
            }
        }

        public static Props Props(TournamentService.TournamentServiceClient client, int tournamentId) =>
            Akka.Actor.Props.Create(() => new TournamentActor(client, tournamentId));
    }
}
