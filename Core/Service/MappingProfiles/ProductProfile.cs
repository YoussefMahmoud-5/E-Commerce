using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using DomainLayer.Models.ProductModule;
using Shared.DataTransfereObeject.ProductModule;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dist => dist.BrandName, option => option.MapFrom(scr => scr.ProductBrand))
                .ForMember(dist => dist.TypeName, option => option.MapFrom(scr => scr.ProductType))
                .ForMember(dist => dist.PictureUrl, option => option.MapFrom<PictureUrlResolver>());
            
            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductType, TypeDto>();

        }
    }
}
