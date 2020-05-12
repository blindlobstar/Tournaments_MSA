using Common.Core.DataExchange.EventBus;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.EventBus.RabbitMq
{
    public static class Extensions
    {
        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
            => new BusSubscriber(app);

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton(typeof(RabbitMqOptions), service =>
            {
                var configuration = (IConfiguration)service.GetService(typeof(IConfiguration));
                var options = new RabbitMqOptions();
                configuration.GetSection("RabbitMq").Bind(options);
                return options;
            });

            services.AddTransient(typeof(IBus), service =>
            {
                var options = (RabbitMqOptions)service.GetService(typeof(RabbitMqOptions));
                return RabbitHutch.CreateBus(options.ConnectionString);
            });
        }
    }
}
