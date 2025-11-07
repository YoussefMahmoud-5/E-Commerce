using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransfereObeject.BasketModule;
using Stripe;

namespace Service
{
    internal class PaymentService(IConfiguration _configuration,
                                  IBasketRepository _basketRepository,
                                  IUnitOfWork _unitOfWork,
                                  IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }

            var ProductRepo = _unitOfWork.GetRepository<DomainLayer.Models.Product, int>();
            foreach (var item in basket.Items)
            {
                var productItem = await ProductRepo.GetByIdAsync(item.Id);
                if (productItem is null)
                {
                    throw new ProductNotFoundException(item.Id);
                }
                item.Price = productItem.Price;
            }
            if (basket.DeliveryMehtodId is null)
            {
                throw new ArgumentException();
            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                                  .GetByIdAsync(basket.DeliveryMehtodId.Value);
            if (deliveryMethod is null)
            {
                throw new DeliveryMethodNotFoundException(basket.DeliveryMehtodId.Value);
            }
            basket.ShippingPrice = deliveryMethod.Price;
            var amount = (long)((basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingPrice) * 100);

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<Basket, BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var endPointSecret = _configuration.GetSection("Stripe")["WebHook"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endPointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFaildAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentRecivedAsync(paymentIntent.Id);
                    break;
                default:
                    Console.WriteLine($"UnHandel Stripe Event Type : {stripeEvent.Type}");
                    break;
            }
        }
        private async Task UpdatePaymentRecivedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order,Guid>()
                                   .GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));
            order.OrderStatus = OrderStatus.PaymentReceived;
            _unitOfWork.GetRepository<Order,Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task UpdatePaymentFaildAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                   .GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));
            order.OrderStatus = OrderStatus.PaymentFailed;
            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
