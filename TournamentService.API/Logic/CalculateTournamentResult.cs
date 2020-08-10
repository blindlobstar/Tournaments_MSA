using Common.Contracts.TournamentService.Events;
using Common.Core.DataExchange.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.API.Logic
{
    public class UserWhoAnswered
    {
        public string UserId { get; set; }
        public int CorrectCount { get; set; }
        public double TimeDiff { get; set; }
        public TournamentsUsers TournamentsUsers { get; set; }
    }

    public class CalculateTournamentResult
    {
        private readonly IExercisesUsersRepository _exercisesUsersRepository;
        private readonly ITournamentsUsersRepository _tournamentsUsersRepository;

        public CalculateTournamentResult(IExercisesUsersRepository exercisesUsersRepository,
            ITournamentsUsersRepository tournamentsUsersRepository)
        {
            _exercisesUsersRepository = exercisesUsersRepository;
            _tournamentsUsersRepository = tournamentsUsersRepository;
        }

        public async Task Calculate(int tournamentId)
        {
            var usersExercises = await _exercisesUsersRepository.GetForCalculating(tournamentId);
            var usersTournaments = await _tournamentsUsersRepository.GetForTournament(tournamentId);
            
            Parallel.ForEach(usersExercises, (current) =>
            {
                current.IsCorrect = IsAnswerCorrect(current.Exercise.Answer, current.UserAnswer);
            });

            GetPlaces(usersTournaments, usersExercises);

            await _tournamentsUsersRepository.SaveChanges();
            await _exercisesUsersRepository.SaveChanges();
        }

        public bool IsAnswerCorrect(string correctAnswer, string userAnswer)
        {
            var sb = new StringBuilder(userAnswer);
            sb = sb.Replace(',', '.')
                .Replace("?", string.Empty)
                .Replace(";", string.Empty);
            return string.Compare(correctAnswer, sb.ToString(), true) == 0;
        }

        public IEnumerable<TournamentsUsers> GetPlaces(List<TournamentsUsers> tournamentsUsers, List<ExercisesUsers> userAnswers)
         => (from userScore in userAnswers
                         where userScore.IsCorrect
                         group userScore by userScore.UserId into groupUserScore
                         join ut in tournamentsUsers
                         on groupUserScore.Key equals ut.UserId
                         select new UserWhoAnswered()
                         {
                             CorrectCount = groupUserScore.Count(),
                             UserId = groupUserScore.Key,
                             TimeDiff = (ut.EndDate - ut.StartDate).TotalMinutes,
                             TournamentsUsers = ut
                         }).OrderBy(x => x.CorrectCount)
                         .ThenByDescending(x => x.TimeDiff)
                         .Select((a, i) =>
                         {
                             a.TournamentsUsers.Place = (uint)i + 1;
                             return a.TournamentsUsers;
                         });
    }
}
