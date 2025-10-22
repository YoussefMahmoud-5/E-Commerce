using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Persistence
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services , IConfiguration Configuration)
        {
            Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnection"));
            });
            return Services;
        }
    }
}
