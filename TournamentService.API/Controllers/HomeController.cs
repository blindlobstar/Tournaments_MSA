﻿using Microsoft.AspNetCore.Mvc;

namespace TournamentService.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
            => "TournamentService is working";
    }
}