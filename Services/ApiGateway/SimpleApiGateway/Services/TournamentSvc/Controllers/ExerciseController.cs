using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleApiGateway.Utils;
using System.Threading.Tasks;

namespace SimpleApiGateway.Services.TournamentSvc.Controllers
{
    public class ExerciseController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;

        public ExerciseController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        [HttpPost("{id}/answer")]
        public async Task<IActionResult> Post([FromQuery]int id, 
            [FromBody]string answer)
        {
            var userId = HttpContext.GetUserId();
            
            await _busPublisher.Send(new AddAnswer() 
            { 
                ExerciseId = id, 
                UserId = userId, 
                UserAnswer = answer 
            });

            return Ok();
        }
    }
}
