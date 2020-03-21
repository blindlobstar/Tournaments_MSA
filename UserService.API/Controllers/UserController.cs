using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<User>> Get() =>
            await _userRepository.GetAll();

        public async Task<User> Get(string id) =>
            await _userRepository.Get(id);

        public async Task<User> Authenticate(string login, string password) =>
            await _userRepository.Authenticate(login, password);

        public User Register(string login, string password)
        {
            var newUser = new User()
            {
                Login = login,
                Password = password,
                Role = Roles.User.ToString()
            };

            return _userRepository.Add(newUser);
        }
    }
}