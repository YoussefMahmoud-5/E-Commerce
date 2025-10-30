using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.IdentityModule;
using Shared.DataTransfereObeject.IdentityModule;

namespace Service.MappingProfiles
{
    internal class IdentityProfiles : Profile
    {
        public IdentityProfiles()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
