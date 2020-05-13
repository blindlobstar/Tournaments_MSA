using Common.Core.DataExchange.EventBus;
using Common.Core.DataExchange.Messages;
using Microsoft.AspNetCore.Builder;
using EasyNetQ;
using Common.Core.DataExchange.Handlers;
using Microsoft.Extensions.Logging;
using System.Reflection;
using IMessage = Common.Core.DataExchange.Messages.IMessage;

namespace Common.EventBus.RabbitMq
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IApplicationBuilder _app;
        private readonly IBus _busClient;
        private readonly ILogger<IMessage> _logger;

        public BusSubscriber(IApplicationBuilder app, IBus busClient,
            ILogger<IMessage> logger)
        {
            _busClient = busClient;
            _app = app;
            _logger = logger;
        }

        public IBusSubscriber SubscribeCommand<TCommand>() where TCommand : class, ICommand
        {
            _logger.LogInformation($"Service: {Assembly.GetCallingAssembly().GetName().Name} has subscribed to command: {typeof(TCommand).FullName}");
            _busClient.SubscribeAsync<TCommand>(string.Empty, async (command) =>
            {
                var handler = (ICommandHandler<TCommand>)_app.ApplicationServices.GetService(typeof(ICommandHandler<TCommand>));
                await handler.HandleAsync(command);
            });
            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>() where TEvent : class, IEvent
        {
            _logger.LogInformation($"Service: {Assembly.GetCallingAssembly().GetName().Name} has subscribed to event: {typeof(TEvent).FullName}");
            _busClient.SubscribeAsync<TEvent>(string.Empty, async (@event) =>
            {
                var handler = (IEventHandler<TEvent>)_app.ApplicationServices.GetService(typeof(IEventHandler<TEvent>));
                await handler.HandleAsync(@event);
            });
            return this;
        }
    }
}
