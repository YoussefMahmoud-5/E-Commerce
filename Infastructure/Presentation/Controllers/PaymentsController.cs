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
    public class PaymentsController(IServiceManager serviceManger) :ApiBaseController
    {
        [HttpPost("{basketId}")] // POST : baseUrl/api/Payments/{basketId}
        public async Task<ActionResult<BasketDto>> CreateOrUpdate(string basketId)
        {
            var basket = await serviceManger.paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(basket);
        }

        [HttpPost("WebHook")] // POST : baseUrl/api/Payments/WebHook
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await serviceManger.paymentService.UpdateOrderPaymentStatusAsync(json, Request.Headers["Stripe-Signature"]!);
            return new EmptyResult();
        }
    }
}
