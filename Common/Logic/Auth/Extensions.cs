using System.Text;
using Common.Core.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Common.Logic.Auth
{
    public static class Extensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services.AddSingleton<IJwtOptions>(opt =>
            {
                var configuration = (IConfiguration)opt.GetService(typeof(IConfiguration));
                var options = new JwtOptions();
                configuration.GetSection("JwtOptions").Bind(options);
                return options;
            });

            services.AddTransient<IJwtService>(opt =>
            {
                var options = (IJwtOptions) opt.GetService(typeof(IJwtOptions));
                return new JwtService(options);
            });
            
            return services;
        }
        
        public static void ConfigureJwtAuth(this IServiceCollection services, IJwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidateIssuerSigningKey = true
                    });
        }
    }
}