using Common.Core.DataExchange.Messages;

namespace Common.Contracts.TournamentService.Commands
{
    public class AddAnswer : ICommand
    {
        public int ExerciseId { get; set; }
        public string UserId { get; set; }
        public string UserAnswer { get; set; }
    }
}
