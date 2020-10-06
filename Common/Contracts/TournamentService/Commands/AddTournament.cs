using Common.Core.DataExchange.Messages;
using System;

namespace Common.Contracts.TournamentService.Commands
{
    public class AddTournament : ICommand
    {
        //[JsonConstructor]
        //public AddTournament(string caption, string description, 
        //    DateTime startDate, DateTime endDate, int? tournamentTime = null)
        //{
        //    Caption = caption;
        //    Description = description;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //    TournamentTime = tournamentTime;
        //}

        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TournamentTime { get; set; }
    }
}
