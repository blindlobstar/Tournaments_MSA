using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.API.Handlers.Command
{
    public class RegisterUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly ITournamentsUsersRepository _tournamentsUsersRepository;

        public RegisterUserHandler(ITournamentsUsersRepository tournamentsUsersRepository)
        {
            _tournamentsUsersRepository = tournamentsUsersRepository;
        }

        public async Task HandleAsync(RegisterUser command)
        {
            var tournamentUserRow = new TournamentsUsers()
            {
                TournamentId = command.TournamentId,
                UserId = command.UserId
            };

            await _tournamentsUsersRepository.Add(tournamentUserRow);
            await _tournamentsUsersRepository.SaveChanges();
        }
    }
}
