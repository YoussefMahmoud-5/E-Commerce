using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public void DataSeed()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var productBrandsData = File.ReadAllText(@"..\Infastructure\Persistence\Data\DataSeed\brands.json");
                    var productBrand = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandsData);
                    if (productBrand is not null && productBrand.Any())
                    {
                        _dbContext.ProductBrands.AddRange(productBrand);
                    }
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypeData = File.ReadAllText(@"..\Infastructure\Persistence\Data\DataSeed\types.json");
                    var productType = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                    if (productType is not null && productType.Any())
                    {
                        _dbContext.ProductTypes.AddRange(productType);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var productData = File.ReadAllText(@"..\Infastructure\Persistence\Data\DataSeed\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);
                    if (products is not null && products.Any())
                    {
                        _dbContext.Products.AddRange(products);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //TO Do
            }
            
        }
    }
}
