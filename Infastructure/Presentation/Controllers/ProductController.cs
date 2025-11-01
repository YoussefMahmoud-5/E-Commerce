using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Attributes;
using ServiceAbstraction;
using Shared;
using Shared.DataTransfereObeject.ProductModule;
using Shared.ErrorModels;

namespace Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController(/*[FromKeyedServices("Lazy")]*/IServiceManager _serviceManager) : ControllerBase
    {
        //Get All Products
        //[Authorize(Roles ="Admin")]
        [HttpGet]

        // GET: BaseUrl/Product/Get
        [Cache(500)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var Products = await _serviceManager.productService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }
        //Get Product By Id 
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorToReturn), StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        // GET: BaseUrl/Product/Get/id
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _serviceManager.productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        //Get All Brands
        [HttpGet("brands")]
        // GET: BaseUrl/Product/Get/brands

        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.productService.GetAllBrandsAsync();
            return Ok(brands);
        }
        //Get All Types
        [HttpGet("types")]
        // GET: BaseUrl/Product/Get/types

        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.productService.GetAllTypesAsync();
            return Ok(types);
        }

    }
}
