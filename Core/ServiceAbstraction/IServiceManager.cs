using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IServiceManager
    {
        public IProductService productService { get;}
        public IBasketService basketService { get;}
        public IAuthenticationService authenticationService { get; }
        public IOrderService orderService { get; }
    }
}
