using System.Text;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Web.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();

            return Services;
        }
        public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>((option) =>
            {
                option.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorsResponse;
            });
            return Services;
        }
        public static IServiceCollection AddJwtServices(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddAuthentication((config) =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:Audience"],

                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],

                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]))
                };
            });
            return Services;
        }
    }
}
