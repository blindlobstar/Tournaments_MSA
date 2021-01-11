using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TournamentService.API;

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
            
            var client = testServer.CreateClient();
            
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions()
            {
                HttpClient = client
            });
        }
    }
}
