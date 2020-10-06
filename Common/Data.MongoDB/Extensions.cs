using Common.Data.MongoDB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Data.MongoDB
{
    public static class Extensions
    {
        public static void AddMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseSettings, DatabaseSettings>(opt =>
            {
                var configuration = (IConfiguration) opt.GetService(typeof(IConfiguration));
                var options = new DatabaseSettings();
                configuration.GetSection("MongoDb").Bind(options);
                return options;
            });
        }
    }
}