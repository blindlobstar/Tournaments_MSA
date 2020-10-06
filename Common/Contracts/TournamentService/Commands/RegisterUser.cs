using Common.Core.DataExchange.Messages;

namespace Common.Contracts.TournamentService.Commands
{
    public class RegisterUser : ICommand
    {
        public int TournamentId { get; set; }
        public string UserId { get; set; }
    }
}
