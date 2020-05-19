using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    public class HomeController : ControllerBase
    {
        public string Get()
            => "Identity service is working";
    }
}