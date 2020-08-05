using Common.Core.DataExchange.Messages;

namespace Common.Contracts.TournamentService.Events
{
    public class TournamentEnded : IEvent
    {
        public int TournamentId { get; set; }
        public string WinnerUserId { get; set; }
    }
}
