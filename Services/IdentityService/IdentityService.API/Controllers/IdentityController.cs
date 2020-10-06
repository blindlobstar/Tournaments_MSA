using System;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.IdentityService.Events;
using Common.Core.Auth;
using Common.Core.DataExchange.EventBus;
using IdentityService.API.Domain;
using IdentityService.API.Repositories;
using IdentityService.API.Requests;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly byte[] _salt;

        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IBusPublisher _busPublisher;

        public IdentityController(IJwtService jwtService, IUserRepository userRepository,
            IBusPublisher busPublisher)
        {
            _salt = Encoding.ASCII.GetBytes("SaltSaltSalt");
            _jwtService = jwtService;
            _userRepository = userRepository;
            _busPublisher = busPublisher;
        }

        [HttpPost]
        [Route("signUp/")]
        public async Task<ActionResult> SignUp(SignUser user)
        {   
            if (await _userRepository.GetByLogin(user.Login) != null)
            {
                return BadRequest();
            }
            
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
            var newUser = new User()
            {
                Login = user.Login,
                Password = hashed,
                Role = Roles.User.ToString()
            };
            await _userRepository.Add(newUser);

            await _busPublisher.Publish(new UserAdded() { Id = newUser.Id, Login = newUser.Login });
            
            return Ok();
        }

        [HttpPost]
        [Route("SignIn/")]
        public async Task<ActionResult<IJwtToken>> SignIn(SignUser user)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
            var authUser = await _userRepository.Authenticate(user.Login, hashed);

            if (authUser == null)
            {
                return NotFound();
            }

            var token = _jwtService.CreateToken(authUser.Id, authUser.Role);

            return Ok(token);
        }
    }
}