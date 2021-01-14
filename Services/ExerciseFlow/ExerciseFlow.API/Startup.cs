using System;
using Akka.Actor;
using Common.Core.DataExchange.EventBus;
using Common.EventBus.RabbitMq;
using ExerciseFlow.API.Actors;
using ExerciseFlow.API.Actors.Providers;
using ExerciseFlow.API.Services;
using GrpcUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace ExerciseFlow.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            //Add TournamentService GRPC client
            services.AddSingleton(sp =>
                new GrpcCaller<ITournamentService>(Environment.GetEnvironmentVariable("TOURNAMENT_URI")));
            services.AddScoped<ITournamentService, TournamentClient>();

            //Add RabbitMq
            services.AddRabbitMq(Environment.GetEnvironmentVariable("GRPC_CONNECTION"));

            //Register actor system and actors
            services.AddSingleton(sp =>
                ActorSystem.Create("ExerciseFlowSystem"));
            services.AddSingleton<UserManagerProvider>(sp =>
            {
                var system = sp.GetRequiredService<ActorSystem>();
                var publisher = sp.GetRequiredService<IBusPublisher>();
                var client = sp.GetRequiredService<ITournamentService>();
                return () => system.ActorOf(UserManagerActor.Props(publisher, client));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            //Creating Actor System
            lifetime.ApplicationStarted.Register(() => app.ApplicationServices.GetService<ActorSystem>());
            
            //Terminating Actor System
            lifetime.ApplicationStopped.Register(() =>
                app.ApplicationServices.GetRequiredService<ActorSystem>().Terminate().Wait());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEndpoints(endpoints =>
            {
                
            });
        }
    }
}
