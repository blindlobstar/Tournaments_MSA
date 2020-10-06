using Common.Contracts.IdentityService.Events;
using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.Handlers;
using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using Common.EventBus.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.API.Handlers;
using UserService.Core.Data;
using UserService.Data;
using UserService.Data.Repositories;

namespace UserService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            services.AddMongoDb();
            services.AddRabbitMq();

            services.AddTransient<IUserRepository, UserRepository>(implementationFactory =>
            {
                var options = implementationFactory.GetRequiredService<IDatabaseSettings>();
                var context = new UserContext(options);
                return new UserRepository(context);
            });

            //Inject handlers
            services.AddTransient<IEventHandler<UserAdded>, UserAddedHandler>();
            services.AddTransient<ICommandHandler<UpdateUser>, UpdateUserHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitMq()
                .SubscribeEvent<UserAdded>()
                .SubscribeCommand<UpdateUser>();
        }
    }
}
