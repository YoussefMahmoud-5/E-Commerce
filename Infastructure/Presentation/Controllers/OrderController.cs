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
    [Authorize]
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        //[Authorize]
        [HttpPost] // POST : baseUrl/api/Order
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var order = await _serviceManager.orderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return Ok(order);   
        }

        // Get All Delivery Methods 
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")] // GET : baseUrl/api/Order/DeliveryMehtods
        public async Task<ActionResult<IEnumerable<DeliveryMehtodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.orderService.GetDeliveryMehtodAsync();
            return Ok(deliveryMethods);
        }

        // Get All Orders 
        //[Authorize]
        [HttpGet] // GET : baseUrl/api/Order
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var orders = await _serviceManager.orderService.GetAllOrdersAsync(email!);
            var orders = await _serviceManager.orderService.GetAllOrdersAsync(GetEmailFromToken());
            return Ok(orders);
        }

        // Get Order By Id 
        //[Authorize]
        [HttpGet("{id:guid}")] // GET : baseUrl/api/Order/{id}
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        


    }
}
