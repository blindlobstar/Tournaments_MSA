using System.Threading.Tasks;
using Common.Core.Auth;
using IdentityService.API.Domain;
using IdentityService.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityService.API.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public IdentityController(IJwtOptions jwtOptions, IJwtService jwtService, 
            ILogger logger, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("/signUp/")]
        public async Task<ActionResult> SignUp(User user)
        {
            var existingUser = await _userRepository.GetByLogin(user.Login);
            if (existingUser != null)
            {
                return BadRequest();
            }
            
            var newUser = _userRepository.Add(user);
            _logger.LogInformation($"New user has sign up with id: {newUser.Id}");
            return Ok();
        }

        [HttpPost]
        [Route("/SignIn/")]
        public async Task<ActionResult<IJwtToken>> SignIn(User user)
        {
            var authUser = await _userRepository.Authenticate(user.Login, user.Password);

            if (authUser == null)
            {
                return NotFound();
            }

            var token = _jwtService.CreateToken(authUser.Id, authUser.Role);

            return Ok(token);
        }
    }
}