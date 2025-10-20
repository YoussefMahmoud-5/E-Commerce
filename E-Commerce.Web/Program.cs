
using System.Threading.Tasks;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            // .Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            #endregion

            var app = builder.Build();

            await app.DataSeedAsync();

            #region Configure the HTTP request pipeline
            // Configure the HTTP request pipeline.

            ///app.Use(async (RequestContext, NextMiddleWare) =>
            ///{
            ///    Console.WriteLine("Request Under Processing...");
            ///    await NextMiddleWare.Invoke();
            ///    Console.WriteLine("Wanting Response...");
            ///    Console.WriteLine(RequestContext.Response.Body);
            ///});
            app.UseCustomExcptionMiddelWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddelWares();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
