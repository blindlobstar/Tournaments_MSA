using System.Threading.Tasks;
using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Mvc;

namespace SimpleApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserServiceController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;

        public UserServiceController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddUser command)
        {
            await _busPublisher.Send(command);
            return Ok();
        }
    }
}