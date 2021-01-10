using Common.Core.DataExchange.Messages;
using System;

namespace Common.Contracts.TournamentService.Commands
{
    public class UpdateTournament : ICommand
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TournamentTime { get; set; }
    }
}
