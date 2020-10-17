using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Common.DelayedPR
{
    public static class Extensions
    {
        public static void AddDelayedPR(this IServiceCollection services, DelayedPROptions options)
        {
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = options.RedisConnectionString;
                option.InstanceName = options.InstanseName;
            });

            services.AddSingleton(ConnectionMultiplexer.Connect(options.RedisConnectionString));
        }
    }
}
