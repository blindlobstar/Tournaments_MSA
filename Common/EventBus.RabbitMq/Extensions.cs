using Common.Core.DataExchange.EventBus;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EventBus.RabbitMq
{
    public static class Extensions
    {
        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IBusSubscriber>();

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton(typeof(RabbitMqOptions), service =>
            {
                var configuration = service.GetRequiredService<IConfiguration>();
                var options = new RabbitMqOptions();
                configuration.GetSection("RabbitMq").Bind(options);
                return options;
            });

            services.AddTransient(typeof(IBus), service =>
            {
                var options = service.GetRequiredService<RabbitMqOptions>();
                return RabbitHutch.CreateBus(options.ConnectionString);
            });

            services.AddSingleton<IBusPublisher, BusPublisher>();
        }
    }
}
