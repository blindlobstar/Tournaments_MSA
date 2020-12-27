using System.Threading.Tasks;
using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleApiGateway.Requests;
using SimpleApiGateway.Utils;
using static GrpcUserService.UserService;

namespace SimpleApiGateway.Services.UserSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;
        private readonly UserServiceClient _userServiceClient;

        public UserController(IBusPublisher busPublisher, 
            UserServiceClient userServiceClient)
        {
            _busPublisher = busPublisher;
            _userServiceClient = userServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _userServiceClient.GetAllAsync(new Google.Protobuf.WellKnownTypes.Empty()));

        [HttpGet("current")]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetUserId();
            var response = await _userServiceClient.GetAsync(new GrpcUserService.GetRequest() { Id = userId });

            return Ok(response);
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