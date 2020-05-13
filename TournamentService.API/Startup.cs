using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using Common.EventBus.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TournamentService.API.Handlers.Command;
using TournamentService.Core.Data;
using TournamentService.Data;
using TournamentService.Data.Repositories;
using TournamentService.Data.Seeds;

namespace TournamentService.API
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

            services.AddRabbitMq();

            //Inject repositories
            services.AddTransient<ITournamentRepository, TournamentRepository>(implementationFactory =>
            {
                var options = new DbContextOptionsBuilder<TournamentContext>()
                    .UseSqlServer(Configuration.GetConnectionString("Default"))
                    .Options;
                var context = new TournamentContext(options);
                context.EnsureSeed();
                return new TournamentRepository(context);
            });

            //Inject handlers
            services.AddTransient<ICommandHandler<AddTournament>, AddTournamentHandler>();

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
                .SubscribeCommand<AddTournament>();
        }
    }
}
