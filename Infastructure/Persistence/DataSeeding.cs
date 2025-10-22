using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
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
    }
}
