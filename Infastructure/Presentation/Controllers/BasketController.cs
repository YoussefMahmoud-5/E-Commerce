using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransfereObeject.BasketModule;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get Basket 
        // GET : baseUrl/api/Basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
           var basket = await _serviceManager.basketService.GetBasketAsync(key);
            return Ok(basket);
        }

        // Create Or Update Basket 
        [HttpPost]
        // POST : BaseUrl/api/Basket
        public async Task<ActionResult<BasketDto>> CreateOrDeleteBasket(BasketDto basketDto)
        {
            var basket = await _serviceManager.basketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(basket);
        }
        // Delete Basket 
        [HttpDelete("{key}")] // DELETE : BaseUrl/api/Basket/basket01
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var isDelete = await _serviceManager.basketService.DeleteBasketAsync(key);
            return isDelete;
        }
    }
}
