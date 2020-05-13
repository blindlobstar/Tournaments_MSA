using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using TournamentService.Core.Data;
using TournamentService.Core.Models;

namespace TournamentService.API.Handlers.Command
{
    public class AddTournamentHandler : ICommandHandler<AddTournament>
    {
        private readonly ITournamentRepository _tournamentRepository;
        public AddTournamentHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Task HandleAsync(AddTournament command)
        {
            var newTournament = new Tournament()
            {
                Caption = command.Caption,
                Description = command.Description,
                EndDate = command.EndDate,
                StartDate = command.StartDate
            };
            _tournamentRepository.Add(newTournament);
            return Task.Run( () => _tournamentRepository.SaveChanges());
        }
    }
}
