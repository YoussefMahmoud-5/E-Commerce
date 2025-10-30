using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DataTransfereObeject.IdentityModule;
using Shared.DataTransfereObeject.OrdeModule;

namespace Service.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dist => dist.DeliveryMethod, option => option.MapFrom(scr => scr.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodId, opt => opt.MapFrom(src => src.DeliveryMethodId));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dist => dist.ProductName, option => option.MapFrom(scr => scr.Product.ProductName))
                .ForMember(dist => dist.PictureUrl, option => option.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
