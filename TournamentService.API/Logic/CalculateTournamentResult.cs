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
    }

    public class CalculateTournamentResult
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IExercisesUsersRepository _exercisesUsersRepository;
        private readonly ITournamentsUsersRepository _tournamentsUsersRepository;
        private readonly IBusPublisher _busPublisher;

        public CalculateTournamentResult(ITournamentRepository tournamentRepository,
            IExercisesUsersRepository exercisesUsersRepository,
            IExerciseRepository exerciseRepository,
            IBusPublisher busPublisher)
        {
            _tournamentRepository = tournamentRepository;
            _exerciseRepository = exerciseRepository;
            _exercisesUsersRepository = exercisesUsersRepository;
            _busPublisher = busPublisher;
        }

        public async Task Calculate(int tournamentId)
        {
            var exercises = await _exerciseRepository.GetForTournament(tournamentId);
            var userAnswers = await _exercisesUsersRepository.GetForExercises(exercises.Select(e => e.Id));
            Parallel.ForEach(userAnswers, (currentAnswer) =>
            {
                var answer = exercises.Find(e => e.Id == currentAnswer.ExerciseId)?.Answer;
                currentAnswer.IsCorrect = IsAnswerCorrect(answer, currentAnswer.UserAnswer);
                _exercisesUsersRepository.Update(currentAnswer);
            });

            var tournament = await _tournamentRepository.Get(tournamentId);
            int lastExercise = exercises.OrderByDescending(e => e.OrderNumber.Value).FirstOrDefault().Id;
            int firstExercise = exercises.OrderBy(e => e.OrderNumber.Value).FirstOrDefault().Id;
            var users = await _tournamentsUsersRepository.GetForTournament(tournamentId);
            var winner = GetFirstPlace(users, userAnswers, firstExercise, lastExercise);
            var tournamentUserWinner = users.Find(u => u.UserId == winner);
            tournamentUserWinner.Place = 1;
            _tournamentsUsersRepository.Update(tournamentUserWinner);

            //Заменить это на UOW
            await _exercisesUsersRepository.SaveChanges();
            await _tournamentsUsersRepository.SaveChanges();
            
            await _busPublisher.Publish(new TournamentEnded()
            {
                TournamentId = tournamentUserWinner.TournamentId,
                WinnerUserId = tournamentUserWinner.UserId
            });

        }

        public bool IsAnswerCorrect(string correctAnswer, string userAnswer)
        {
            var sb = new StringBuilder(userAnswer);
            sb = sb.Replace(',', '.')
                .Replace("?", string.Empty)
                .Replace(";", string.Empty);
            return string.Compare(correctAnswer, sb.ToString(), true) == 0;
        }

        public string GetFirstPlace(List<TournamentsUsers> tournamentsUsers, List<ExercisesUsers> userAnswers,
            int firstExercise, int lastExercise)
        {
            var userList = new List<UserWhoAnswered>();

            foreach (var registeredUser in tournamentsUsers)
            {
                var startTime = userAnswers.Find(a => a.UserId == registeredUser.UserId && a.ExerciseId == firstExercise).Created;
                var lastTime = userAnswers.Find(a => a.UserId == registeredUser.UserId && a.ExerciseId == lastExercise).Created;
                userList.Add(new UserWhoAnswered()
                {
                    CorrectCount = userAnswers.Where(a => a.UserId == registeredUser.UserId && a.IsCorrect).Count(),
                    TimeDiff = lastTime.Subtract(startTime).TotalSeconds,
                    UserId = registeredUser.UserId
                });
            }

            return userList.OrderByDescending(u => u.CorrectCount)
                .ThenBy(u => u.TimeDiff).FirstOrDefault().UserId;
        }
    }
}
