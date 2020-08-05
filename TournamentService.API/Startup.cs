using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using Common.Data.EFCore;
using Common.EventBus.RabbitMq;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TournamentService.API.Handlers.Command;
using TournamentService.API.Logic;
using TournamentService.Core.Data;
using TournamentService.Data;
using TournamentService.Data.Repositories;

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
            services.AddEfCore();
            services.AddDbContext<TournamentContext>();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //Inject repositories
            services.AddTransient<ITournamentRepository, TournamentRepository>();
            services.AddTransient<ITournamentsUsersRepository, TournamentsUsersRepository>();
            services.AddTransient<IExerciseRepository, ExerciseRepository>();
            services.AddTransient<IExercisesUsersRepository, ExercisesUsersRepository>();

            //Inject handlers
            services.AddTransient<ICommandHandler<AddTournament>, AddTournamentHandler>();
            services.AddTransient<ICommandHandler<UpdateTournament>, UpdateTournamentHandler>();
            services.AddTransient<ICommandHandler<RegisterUser>, RegisterUserHandler>();
            
            services.AddTransient<CalculateTournamentResult>();
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
                .SubscribeCommand<AddTournament>()
                .SubscribeCommand<UpdateTournament>()
                .SubscribeCommand<RegisterUser>();
        }
    }
}
