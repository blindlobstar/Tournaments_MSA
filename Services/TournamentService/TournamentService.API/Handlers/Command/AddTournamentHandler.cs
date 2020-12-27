using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using TournamentService.API.Logic;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.API.Handlers.Command
{
    public class AddTournamentHandler : ICommandHandler<AddTournament>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly CalculateTournamentResult _calculateTournamentResult;
        public AddTournamentHandler(ITournamentRepository tournamentRepository,
            CalculateTournamentResult calculateTournamentResult)
        {
            _tournamentRepository = tournamentRepository;
            _calculateTournamentResult = calculateTournamentResult;
        }

        public async Task HandleAsync(AddTournament command)
        {
            var newTournament = new TournamentDto()
            {
                Caption = command.Caption,
                Description = command.Description,
                EndDate = command.EndDate,
                StartDate = command.StartDate
            };
            await _tournamentRepository.Add(newTournament);
            await _tournamentRepository.SaveChanges();
        }
    }
}
