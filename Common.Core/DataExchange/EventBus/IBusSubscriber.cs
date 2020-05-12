
using Common.Core.DataExchange.Messages;

namespace Common.Core.DataExchange.EventBus
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>() where TCommand : class, ICommand;
        IBusSubscriber SubscribeEvent<TEvent>() where TEvent : class, IEvent;
    }
}
