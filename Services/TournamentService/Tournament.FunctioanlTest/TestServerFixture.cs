using System;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using TournamentService.API;
using TournamentService.Data;
using TournamentService.Data.Seeds;

namespace Tournament.FunctionalTest
{
    public sealed class TestServerFixture
    {
        public GrpcChannel GrpcChannel { get; private set; }

        public TestServerFixture()
        {
            //Creating test server
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration(c => c.AddJsonFile("appsettings.json"))
                .UseStartup<Startup>();
            var testServer = new TestServer(host);

            using var scope = testServer.Host.Services.CreateScope();
            var retryPolicy = Policy.Handle<SqlException>()
                .WaitAndRetry(15, retry => TimeSpan.FromSeconds(retry * 2));
            //Seed data, if test
            var context = scope.ServiceProvider.GetService<TournamentContext>();
            retryPolicy.Execute(() => context.SeedData());

            var client = testServer.CreateClient();
            
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions()
            {
                HttpClient = client
            });
        }
    }
}
