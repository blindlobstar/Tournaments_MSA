using Microsoft.AspNetCore.Mvc;

namespace SimpleApiGateway.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public string Get() =>
            "Service is working";
    }
}