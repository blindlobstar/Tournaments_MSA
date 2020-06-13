using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace Common.Data.EFCore
{
    public static class Extensions
    {
        public static void AddEfCore(this IServiceCollection services)
        {
            var contextType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .FirstOrDefault(t => t.IsSubclassOf(typeof(DbContext)));

            var type = typeof(DbContextOptionsBuilder<>).MakeGenericType(contextType);
            var dbContextOptionsBuilder = (DbContextOptionsBuilder)Activator.CreateInstance(type);

            services.AddTransient(contextType, (opt) =>
            {
                var configuration = (IConfiguration)opt.GetService(typeof(IConfiguration));
                var options = dbContextOptionsBuilder
                    .UseSqlServer(configuration.GetConnectionString("Default"))
                    .Options;
                var context = Activator.CreateInstance(contextType, options);
                return context;
            });
        }
    }
}
