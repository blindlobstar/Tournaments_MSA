using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<Tournament> Get(int id)
        {
            var tournament = await _tournamentRepository.Get(id);

            if (tournament == null)
            {
                throw new ArgumentException("Can't find tournament with given id");
            }

            return tournament;
        }

        [Route("")]
        [HttpGet]
        public async Task<List<Tournament>> GetAll()
        {
            var tournaments = await _tournamentRepository.GetAll();

            if (tournaments == null || tournaments.Count == 0)
            {
                throw new ArgumentException("Can't find tournaments");
            }

            return tournaments;
        }

        [Route("/Available/{dateTime}")]
        [HttpGet]
        public async Task<List<Tournament>> GetAvailable(DateTime dateTime)
        {
            var tournaments = await _tournamentRepository.GetAvailable(dateTime);

            if (tournaments == null || tournaments.Count == 0)
            {
                throw new ArgumentException("Can't find available tournaments with given date");
            }

            return tournaments;
        }

        [Route("/{id}/Exercises")]
        [HttpGet]
        public async Task<List<Exercise>> GetExercisesForTournament(int id)
        {
            var exercises = await _exerciseRepository.GetForTournament(id);

            if (exercises == null || exercises.Count == 0)
            {
                throw new ArgumentException("Can't find exercises for given tournament");
            }

            return exercises;
        }

        [Route("/Add")]
        [HttpPost]
        public void Add([FromBody] Tournament tournament)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                    from errors in item.Errors
                    select errors.ErrorMessage).ToList();

                throw new ArgumentException(JsonConvert.SerializeObject(errorList));
            }

            var addedTournament = _tournamentRepository.Add(tournament);

            if (addedTournament == null)
            {
                throw new ArgumentException("Can't add new tournament");
            }

            _tournamentRepository.SaveChanges();
        }
        

        [Route("{tournamentId}/Registration/{userId}")]
        [HttpPost]
        public async Task Registration(int tournamentId, string userId)
        {
            var tournament = await _tournamentRepository.Get(tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException("Can't find tournament with given id");
            }

            var tournamentsUsers = new TournamentsUsers()
            {
                TournamentId = tournament.Id,
                UserId = userId
            };

            var registration = _tournamentsUsersRepository.Add(tournamentsUsers);

            if (registration == null)
            {
                throw new ApplicationException("Failed attempt of registration");
            }

            _tournamentRepository.SaveChanges();
        }

        [Route("")]
        [HttpPut]
        public async Task Update([FromBody] Tournament tournament)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                    from errors in item.Errors
                    select errors.ErrorMessage).ToList();

                throw new ArgumentException(JsonConvert.SerializeObject(errorList));
            }

            var tournamentForUpdate = await _tournamentRepository.Get(tournament.Id);

            if (tournamentForUpdate == null)
            {
                throw new ArgumentException("Can't find tournament with given id");
            }

            _tournamentRepository.Update(tournament);
            _tournamentRepository.SaveChanges();
        }

        [Route("/{id}")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            var tournamentForDelete = await _tournamentRepository.Get(id);

            if (tournamentForDelete == null)
            {
                throw new ArgumentException("Can't find tournament with given id");
            }

            _tournamentRepository.Delete(id);
            _tournamentRepository.SaveChanges();
        }
    }
}