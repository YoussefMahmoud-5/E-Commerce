using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.BasketModule;
using Shared.DataTransfereObeject.BasketModule;

namespace Service.MappingProfiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<Basket,BasketDto>().ReverseMap();
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
        }
    }
}
