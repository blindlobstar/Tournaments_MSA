using Common.Core.DataExchange.EventBus;
using Common.Core.DataExchange.Messages;
using EasyNetQ;
using Common.Core.DataExchange.Handlers;
using Microsoft.Extensions.Logging;
using System.Reflection;
using IMessage = Common.Core.DataExchange.Messages.IMessage;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EventBus.RabbitMq
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBus _busClient;
        private readonly ILogger<IMessage> _logger;

        public BusSubscriber(IBus busClient,
            ILogger<IMessage> logger, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _busClient = busClient;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IBusSubscriber SubscribeCommand<TCommand>() where TCommand : class, ICommand
        {
            _logger.LogInformation($"Service: {Assembly.GetCallingAssembly().GetName().Name} has subscribed to command: {typeof(TCommand).FullName}");
            _busClient.SubscribeAsync<TCommand>(string.Empty, async (command) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var handler = (ICommandHandler<TCommand>)scope.ServiceProvider.GetService(typeof(ICommandHandler<TCommand>));
                await handler.HandleAsync(command);
            });
            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>() where TEvent : class, IEvent
        {
            _logger.LogInformation($"Service: {Assembly.GetCallingAssembly().GetName().Name} has subscribed to event: {typeof(TEvent).FullName}");
            _busClient.SubscribeAsync<TEvent>(string.Empty, async (@event) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var handler = (IEventHandler<TEvent>)scope.ServiceProvider.GetService(typeof(IEventHandler<TEvent>));
                await handler.HandleAsync(@event);
            });
            return this;
        }
    }
}
