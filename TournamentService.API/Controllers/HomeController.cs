using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TournamentService.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }   
        [HttpGet]
        public string Get()
            => "TournamentService is working";
    }
}