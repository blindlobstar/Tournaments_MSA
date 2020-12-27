using AutoMapper;
using Common.Contracts.TournamentService.Commands;
using Common.Core.DataExchange.Handlers;
using Common.EventBus.RabbitMq;
using Google.Protobuf.WellKnownTypes;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TournamentService.API.Handlers.Command;
using TournamentService.API.Logic;
using TournamentService.Core.Data;
using TournamentService.Core.Models;
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
            services.AddGrpc();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            services.AddRabbitMq();

            services.AddDbContext<TournamentContext>(option => option
                .UseSqlServer(Configuration.GetConnectionString("Default"),
                        x => x.MigrationsAssembly("TournamentService.Data")),
                    optionsLifetime: ServiceLifetime.Singleton,
                    contextLifetime: ServiceLifetime.Scoped);

            //// Add Hangfire services.
            //services.AddHangfire(configuration => configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            //    {
            //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //        QueuePollInterval = TimeSpan.Zero,
            //        UseRecommendedIsolationLevel = true,
            //        DisableGlobalLocks = true
            //    }));

            //// Add the processing server as IHostedService
            //services.AddHangfireServer();

            //Inject repositories
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITournamentsUsersRepository, TournamentsUsersRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IExercisesUsersRepository, ExercisesUsersRepository>();

            //Inject handlers
            services.AddScoped<ICommandHandler<AddTournament>, AddTournamentHandler>();
            services.AddScoped<ICommandHandler<UpdateTournament>, UpdateTournamentHandler>();
            services.AddScoped<ICommandHandler<RegisterUser>, RegisterUserHandler>();

            services.AddTransient<CalculateTournamentResult>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<DateTime, Timestamp>()
                    .ConvertUsing(s => Timestamp.FromDateTime(s.ToUniversalTime()));
                cfg.CreateMap<TournamentDto, GrpcTournamentService.Tournament>();
                cfg.CreateMap<ExerciseDto, GrpcTournamentService.Exercise>();
            });
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Grpc.TournamentService>();
            });
            //Ensure that database created
            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.GetService<TournamentContext>().Database.EnsureCreated();

            app.UseRabbitMq()
                .SubscribeCommand<AddTournament>()
                .SubscribeCommand<UpdateTournament>()
                .SubscribeCommand<RegisterUser>();
        }
    }
}
