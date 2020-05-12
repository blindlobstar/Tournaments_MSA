using System.Threading.Tasks;
using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.EventBus;
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

        public string Get() =>
            "TournamentService is working";

        [HttpPost]
        public async Task<IActionResult> Post(AddTournament tournament)
        {
            await _busPublisher.Send(tournament);
            return Ok();
        }
    }
}