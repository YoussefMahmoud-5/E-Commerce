using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceAbstraction;

namespace Service
{
    public class ServiceManagerWithFactoryDelegate(Func<IProductService> ProductFactory,
                                                     Func<IBasketService> BasketFactory,
                                                     Func<IAuthenticationService> AuthenticationFactory,
                                                     Func<IOrderService> OrderFactory) 
    {
        public IProductService productService => ProductFactory.Invoke();

        public IBasketService basketService => BasketFactory.Invoke();

        public IAuthenticationService authenticationService => AuthenticationFactory.Invoke();

        public IOrderService orderService => OrderFactory.Invoke();
    }
}
