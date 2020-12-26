using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using Common.EventBus.RabbitMq;
using Common.Logic.Auth;
using IdentityService.API.Data;
using IdentityService.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            services.AddJwtAuth();
            services.AddRabbitMq();
            services.AddMongoDb();

            services.AddScoped<UserContext>();

            services.AddScoped<IUserRepository, UserRepository>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}