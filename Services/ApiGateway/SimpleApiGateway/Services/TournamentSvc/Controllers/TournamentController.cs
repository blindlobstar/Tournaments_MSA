using System;
using System.Threading.Tasks;
using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.EventBus;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleApiGateway.Utils;
using static GrpcTournamentService.TournamentService;

namespace SimpleApiGateway.Services.TournamentSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;
        private readonly TournamentServiceClient _tournamentServiceClient;

        public TournamentController(IBusPublisher busPublisher, 
            TournamentServiceClient tournamentServiceClient)
        {
            _busPublisher = busPublisher;
            _tournamentServiceClient = tournamentServiceClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) =>
            Ok(await _tournamentServiceClient
                .GetAsync(new GrpcTournamentService.GetRequest() { Id = id }));

        [HttpGet("date/{date}")]
        public async Task<IActionResult> Get(DateTime date) =>
            Ok(await _tournamentServiceClient
                .GetAvaliableAsync(new GrpcTournamentService.GetAvaliableRequest() { Date = Timestamp.FromDateTime(date) }));

        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercises(int id) =>
            Ok(await _tournamentServiceClient
                .GetExercisesAsync(new GrpcTournamentService.GetExercisesRequest() { TournamentId = id }));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(AddTournament tournament)
        {
            await _busPublisher.Send(tournament);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(UpdateTournament tournament)
        {
            await _busPublisher.Send(tournament);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("registrate")]
        public async Task<IActionResult> Registrate(int tournamentId)
        {
            var userId = HttpContext.GetUserId();

            var user = new RegisterUser()
            {
                TournamentId = tournamentId,
                UserId = userId
            };

            await _busPublisher.Send(user);
            return Ok();
        }
    }
}