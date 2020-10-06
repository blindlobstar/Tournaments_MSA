using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers
{
    [Route("/")]
    public class HomeController : ControllerBase
    {
        public string Get()
            => "UserService is working";
    }
}