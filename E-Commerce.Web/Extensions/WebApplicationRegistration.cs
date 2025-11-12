using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task DataSeedAsync(this WebApplication app)
        {
            var Scope = app.Services.CreateScope();

            var seed = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await seed.DataSeedAsync();
            await seed.IdentityDataSeedAsync();
        }
        public static IApplicationBuilder UseCustomExcptionMiddelWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddelWares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option => 
            {
                option.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };
                option.DocumentTitle = "Talabat Api";
                option.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                option.DocExpansion(DocExpansion.None);
                option.EnableFilter();
            });
            return app;
        }
    }
}
