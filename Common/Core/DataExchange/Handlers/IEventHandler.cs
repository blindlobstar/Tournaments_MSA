using Common.Core.DataExchange.Messages;
using System.Threading.Tasks;

namespace Common.Core.DataExchange.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
