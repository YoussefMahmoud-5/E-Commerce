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

        // Get All Delivery Methods 
        Task<IEnumerable<DeliveryMehtodDto>> GetDeliveryMehtodAsync();

        // Get All Orders 
        Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email);

        // Get Order by Id
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid id);
    }
}
