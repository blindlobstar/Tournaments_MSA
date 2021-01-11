using Common.Core.DataExchange.Messages;

namespace Common.Contracts.TournamentService.Commands
{
    public sealed class AddExercise : ICommand
    {
        public int TournamentId { get; set; }
        public int? OrderNumber { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
    }
}