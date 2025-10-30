using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransfereObeject.OrdeModule;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        // Create Order 
        // Take OrderDto , Email
        // Return OrderToReturn --Detials About Order--
        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email);
    }
}
