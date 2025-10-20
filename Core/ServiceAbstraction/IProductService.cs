﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DataTransfereObeject;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        //Get All Product 
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);
        //Get Product By Id 
        Task<ProductDto> GetProductByIdAsync(int id);
        //Get All Brands 
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        //Get All Types 
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
