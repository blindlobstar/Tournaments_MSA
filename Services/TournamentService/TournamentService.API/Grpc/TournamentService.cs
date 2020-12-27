using AutoMapper;
using Grpc.Core;
using GrpcTournamentService;
using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using static GrpcTournamentService.TournamentService;

namespace TournamentService.API.Grpc
{
    public class TournamentService : TournamentServiceBase
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentsUsersRepository _tournamentsUsersRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository tournamentRepository,
            ITournamentsUsersRepository tournamentsUsersRepository,
            IExerciseRepository exerciseRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _tournamentsUsersRepository = tournamentsUsersRepository;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public override async Task<Tournament> Get(GetRequest request, ServerCallContext context)
        {
            var tournament = await _tournamentRepository.Get(request.Id);
            return _mapper.Map<Tournament>(tournament);
        }

        public override async Task<GetAvaliableResponse> GetAvaliable(GetAvaliableRequest request, ServerCallContext context)
        {
            var response = new GetAvaliableResponse();
            var tournaments = await _tournamentRepository.GetAvailable(request.Date.ToDateTime());

            response.Tournaments.AddRange(_mapper.Map<IEnumerable<Tournament>>(tournaments));

            return response; 
        }

        public override async Task<GetExercisesResponse> GetExercises(GetExercisesRequest request, ServerCallContext context)
        {
            var response = new GetExercisesResponse();
            var exercises = await _exerciseRepository.GetForTournament(request.TournamentId);

            response.Exercises.AddRange(_mapper.Map<IEnumerable<Exercise>>(exercises));
            return response;
        }
    }
}
