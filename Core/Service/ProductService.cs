using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransfereObeject.ProductModule;

namespace Service
{
    internal class ProductService(IUnitOfWork _unitOfWork,
                                  IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductBrand,int>();
            var brands = await repo.GetAllAsync();
            var brandsDto = _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDto>>(brands);
            return brandsDto;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Product,int>();
            var specification = new ProductWithBrandAndTypeSpecification(queryParams);
            var products = await repo.GetAllAsync(specification);
            var productsDto = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(products);
            var ProductCount = productsDto.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var TotalCount = await repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(ProductCount , queryParams.PageIndex , TotalCount , productsDto);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Product,int>();
            var specification = new ProductWithBrandAndTypeSpecification(id);
            var product = await repo.GetByIdAsync(specification);
            if (product is null)
                throw new ProductNotFoundException(id);
            return  _mapper.Map<Product, ProductDto>(product);
        }
        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var types = await repo.GetAllAsync();
            var typesDto = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
            return typesDto;
        }

    }
}
