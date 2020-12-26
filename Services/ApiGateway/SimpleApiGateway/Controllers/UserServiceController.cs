using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleApiGateway.Requests;

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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(UppdateUserInformation request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var id = claim.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

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