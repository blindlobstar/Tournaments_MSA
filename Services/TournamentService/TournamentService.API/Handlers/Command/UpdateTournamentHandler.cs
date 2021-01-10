using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using System;
using System.Threading.Tasks;
using TournamentService.Core.Data;

namespace TournamentService.API.Handlers.Command
{
    public class UpdateTournamentHandler : ICommandHandler<UpdateTournament>
    {
        private readonly ITournamentRepository _tournamentRepository;

        public UpdateTournamentHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task HandleAsync(UpdateTournament command)
        {
            var tournament = await _tournamentRepository.Get(command.Id);

            tournament.Caption = string.IsNullOrWhiteSpace(command.Caption) ? tournament.Caption : command.Caption;
            tournament.Description = string.IsNullOrWhiteSpace(command.Description) ? tournament.Description : command.Description;
            tournament.EndDate = command.EndDate.HasValue ? tournament.EndDate : command.EndDate.Value;
            tournament.StartDate = command.StartDate.HasValue ? tournament.StartDate : command.StartDate.Value;
            tournament.TournamentTime = command.TournamentTime ?? tournament.TournamentTime;

            await _tournamentRepository.SaveChanges();
        }
    }
}
