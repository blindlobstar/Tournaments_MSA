using Common.Core.DataExchange.Messages;

namespace Common.Contracts.ExerciseFlow.Commands
{
    public class AddUserAnswer : ICommand
    {
        public string UserId { get; set; }
        public string Answer { get; set; }
    }
}