using Common.Core.DataExchange.Messages;
using System.Threading.Tasks;

namespace Common.Core.DataExchange.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
