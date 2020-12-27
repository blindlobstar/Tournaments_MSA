using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<List<UserDto>> Get() =>
            await _userRepository.GetAll();

        [HttpGet("/{id}")]
        public async Task<UserDto> Get(string id) =>
            await _userRepository.Get(id);
    }
}