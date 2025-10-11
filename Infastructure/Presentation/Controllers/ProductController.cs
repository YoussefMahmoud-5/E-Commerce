using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransfereObeject;

namespace Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController(IServiceManager _serviceManager) : ControllerBase
    {
        //Get All Products
        [HttpGet]
        // GET: BaseUrl/Product/Get
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var Products = await _serviceManager.productService.GetAllProductsAsync();
            return Ok(Products);
        }
        //Get Product By Id 
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
