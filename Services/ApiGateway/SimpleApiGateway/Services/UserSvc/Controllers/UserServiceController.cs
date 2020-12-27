using System.Threading.Tasks;
using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleApiGateway.Requests;
using SimpleApiGateway.Utils;

namespace SimpleApiGateway.Services.UserSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;

        public UserController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(UppdateUserInformation request)
        {
            var id = HttpContext.GetUserId();

            var command = new UpdateUser()
            {
                Id = id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Phone = request.Phone
            };
            await _busPublisher.Send(command);
            return Ok();
        }
    }
}