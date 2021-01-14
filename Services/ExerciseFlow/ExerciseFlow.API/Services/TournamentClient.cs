using System;
using System.Collections.Generic;
using System.Linq;
using Grpc.Net.Client;
using GrpcTournamentService;
using GrpcUtils;
using Exercise = ExerciseFlow.API.Models.Exercise;

namespace ExerciseFlow.API.Services
{
    public class TournamentClient : ITournamentService
    {
        private readonly GrpcCaller<ITournamentService> _grpcCaller;

        public TournamentClient(GrpcCaller<ITournamentService> grpcCaller)
        {
            _grpcCaller = grpcCaller;
        }

        public List<Exercise> GetExercises(int tournamentId)
        {
            var response = _grpcCaller.Call(channel =>
            {
                var client = new TournamentService.TournamentServiceClient(channel);
                return client.GetExercises(new GetExercisesRequest() {TournamentId = tournamentId});
            });

            var exercises = from ex in response.Exercises
                select new Exercise()
                {
                    TournamentId = ex.TournamentId,
                    Answer = ex.Answer,
                    Id = ex.Id,
                    Text = ex.Text,
                    OrderNumber = ex.OrderNumber
                };

            return exercises.ToList();
        }
    }
}
