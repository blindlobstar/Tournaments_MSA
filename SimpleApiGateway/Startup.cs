using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Core.DataExchange.EventBus;
using Common.EventBus.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleApiGateway
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRabbitMq();
            services.AddTransient<IBusPublisher, BusPublisher>();
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

            app.UseRabbitMq();
        }
    }
}
