using Microsoft.AspNetCore.Mvc;

namespace SimpleApiGateway.Controllers
{
    [Route("Home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get() =>
            "Service is working";
    }
}