using Common.Core.DataExchange.EventBus;
using Common.Core.DataExchange.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.RabbitMq
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBus _busClient;
        public BusPublisher(IApplicationBuilder app)
        {
            _busClient = (IBus)app.ApplicationServices.GetService(typeof(IBus));

        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : class, IEvent
            => _busClient.PublishAsync(@event);

        public Task Send<TCommand>(TCommand command) where TCommand : class, ICommand
            => _busClient.PublishAsync(command);
    }
}
