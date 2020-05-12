using Common.Core.DataExchange.EventBus;
using Common.Core.DataExchange.Messages;
using Microsoft.AspNetCore.Builder;
using EasyNetQ;
using Common.Core.DataExchange.Handlers;
using System;

namespace Common.EventBus.RabbitMq
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IApplicationBuilder _app;
        private readonly IBus _busClient;

        public BusSubscriber(IApplicationBuilder app)
        {
            _busClient = (IBus)app.ApplicationServices.GetService(typeof(IBus));
            _app = app;
        }

        public IBusSubscriber SubscribeCommand<TCommand>() where TCommand : class, ICommand
        {
            var subscriptionId = Guid.NewGuid().ToString();
            _busClient.SubscribeAsync<TCommand>(string.Empty, async (command) =>
            {
                var handler = (ICommandHandler<TCommand>)_app.ApplicationServices.GetService(typeof(ICommandHandler<TCommand>));
                await handler.HandleAsync(command);
            });
            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>() where TEvent : class, IEvent
        {
            var subscriptionId = Guid.NewGuid().ToString();
            _busClient.SubscribeAsync<TEvent>(string.Empty, async (@event) =>
            {
                var handler = (IEventHandler<TEvent>)_app.ApplicationServices.GetService(typeof(IEventHandler<TEvent>));
                await handler.HandleAsync(@event);
            });
            return this;
        }
    }
}
