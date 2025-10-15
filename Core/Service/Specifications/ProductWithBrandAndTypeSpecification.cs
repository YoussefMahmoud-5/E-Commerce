using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using Shared;

namespace Service.Specifications
{
    class ProductWithBrandAndTypeSpecification : BaseSpecification<Product,int>
    {
        //Get All Product
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams)
            :base(P => (!queryParams.brandId.HasValue || P.BrandId == queryParams.brandId) 
                    && (!queryParams.typeId.HasValue || P.TypeId == queryParams.typeId)
                    && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
            switch (queryParams.sortingOption)
            {
                case ProductSortingOption.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOption.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    break;



            }
        }
        // Get Product By Id 
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id) 
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }

    }

}
