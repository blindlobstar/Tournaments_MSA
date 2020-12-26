using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentServiceController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;

        public TournamentServiceController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        [HttpGet]
        public string Get() =>
            "TournamentService is working";

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
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var userId = claim.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

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