using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;

namespace Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper,
                                IBasketRepository _basketRepository,
                                UserManager<ApplicationUser> _userManager,
                                IConfiguration _configuration) : IServiceManager
    {

        private readonly Lazy<IProductService> _lazyProductService = new Lazy<IProductService>(() => new  ProductService(_unitOfWork,_mapper));
        private readonly Lazy<IBasketService> _lazyBasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _configuration, _mapper));
        private readonly Lazy<IOrderService> _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(_mapper, _basketRepository, _unitOfWork));
        public IProductService productService => _lazyProductService.Value;

        public IBasketService basketService => _lazyBasketService.Value;

        public IAuthenticationService authenticationService => _lazyAuthenticationService.Value;

        public IOrderService orderService => _lazyOrderService.Value;
    }
}
