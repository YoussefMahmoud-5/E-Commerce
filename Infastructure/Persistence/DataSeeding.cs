using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,
                             UserManager<ApplicationUser> _userManager,
                             RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if (( await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var productBrandsData = File.OpenRead(@"..\Infastructure\Persistence\Data\DataSeed\brands.json");
                    var productBrand = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandsData);
                    if (productBrand is not null && productBrand.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(productBrand);
                    }
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypeData = File.OpenRead(@"..\Infastructure\Persistence\Data\DataSeed\types.json");
                    var productType = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypeData);
                    if (productType is not null && productType.Any())
                    {
                         await _dbContext.ProductTypes.AddRangeAsync(productType);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var productData = File.OpenRead(@"..\Infastructure\Persistence\Data\DataSeed\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(productData);
                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TO Do
            }
            
        }

        public async Task IdentityDataSeedAsync()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!_userManager.Users.Any())
            {
                var user01 = new ApplicationUser()
                {
                    Email = "Mohamed@gamil.com",
                    DisplayName = "Mohamed Aly",
                    UserName = "MohamedAly",
                    PhoneNumber = "01272313488"
                };
                var user02 = new ApplicationUser()
                {
                    Email = "Salma@gamil.com",
                    DisplayName = "Salma Mohamed",
                    UserName = "SalmaMohamed",
                    PhoneNumber = "01553646443"
                };
                await _userManager.CreateAsync(user01, "P@ssw0rd");
                await _userManager.CreateAsync(user02, "P@ssw0rd");

                await _userManager.AddToRoleAsync(user01, "Admin");
                await _userManager.AddToRoleAsync(user02, "SuperAdmin");
            }
        }
    }
}
