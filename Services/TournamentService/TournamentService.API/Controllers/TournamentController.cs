using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentsUsersRepository _tournamentsUsersRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public TournamentController(ITournamentRepository tournamentRepository,
            ITournamentsUsersRepository tournamentsUsersRepository,
            IExerciseRepository exerciseRepository)
        {
            _tournamentRepository = tournamentRepository;
            _tournamentsUsersRepository = tournamentsUsersRepository;
            _exerciseRepository = exerciseRepository;
        }

        [Route("/{id}")]
        [HttpGet]
        public async Task<ActionResult<Tournament>> Get(int id)
        {
            var tournament = await _tournamentRepository.Get(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament);
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<List<Tournament>>> GetAll()
        {
            var tournaments = await _tournamentRepository.GetAll();

            if (tournaments == null || tournaments.Count == 0)
            {
                return NotFound();
            }

            return Ok(tournaments);
        }

        [Route("/Available/{dateTime}")]
        [HttpGet]
        public async Task<ActionResult<List<Tournament>>> GetAvailable(DateTime dateTime)
        {
            var tournaments = await _tournamentRepository.GetAvailable(dateTime);

            if (tournaments == null || tournaments.Count == 0)
            {
                return NotFound();
            }

            return Ok(tournaments);
        }

        [Route("/{id}/Exercises")]
        [HttpGet]
        public async Task<ActionResult<List<Exercise>>> GetExercisesForTournament(int id)
        {
            var exercises = await _exerciseRepository.GetForTournament(id);

            if (exercises == null || exercises.Count == 0)
            {
                return NotFound();
            }

            return Ok(exercises);
        }
    }
}