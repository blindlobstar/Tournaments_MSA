using Common.Core.DataExchange.EventBus;
using Common.EventBus.RabbitMq;
using Common.Logic.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleApiGateway
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
            services.AddRabbitMq();

            services.AddJwtAuth();
            var jwtOptions = new JwtOptions();
            Configuration.GetSection("JwtOptions").Bind(jwtOptions);
            services.ConfigureJwtAuth(jwtOptions);
            
            //DI
            services.AddTransient<IBusPublisher, BusPublisher>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitMq();
        }
    }
}
