using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransfereObeject.OrdeModule;

namespace Presentation.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        [Authorize]
        [HttpPost] // POST : baseUrl/api/Order
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var order = await _serviceManager.orderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return Ok(order);   
        }
    }
}
