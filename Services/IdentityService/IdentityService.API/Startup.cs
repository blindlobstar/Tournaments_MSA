using Common.Contracts.IdentityService.Events;
using Common.Core.DataExchange.EventBus;
using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using Common.EventBus.RabbitMq;
using Common.Logic.Auth;
using IdentityService.API.Data;
using IdentityService.API.Domain;
using IdentityService.API.Repositories;
using IdentityService.API.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

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

            services.AddScoped<IBaseContext<User>, UserContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSwaggerGen();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Seed admin data
                using var scope = app.ApplicationServices.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var admin = userRepository.GetByLogin("Admin1").Result;
                if (admin == null)
                {
                    var salt = Encoding.ASCII.GetBytes("SaltSaltSalt");
                    var user = new User()
                    {
                        Login = "Admin1",
                        Password = "123".CreateHash(salt),
                        Role = "Admin"
                    };
                    userRepository.Add(user);
                    var publisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();
                    publisher.Publish(new UserAdded() { Id = user.Id, Login = user.Login }).Wait();
                }

            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}