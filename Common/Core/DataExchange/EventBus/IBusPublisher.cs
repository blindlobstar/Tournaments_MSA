using System.Threading.Tasks;
using Common.Core.DataExchange.Messages;

namespace Common.Core.DataExchange.EventBus 
{
    public interface IBusPublisher
    {
        Task Send<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}