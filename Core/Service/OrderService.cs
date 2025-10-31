using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using DomainLayer.Models.OrderModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransfereObeject.IdentityModule;
using Shared.DataTransfereObeject.OrdeModule;

namespace Service
{
    internal class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map AddressDto To Order Address
            var address = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(orderDto.BasketId);
            // Create Order Items 
            List<OrderItem> OrderItems = new List<OrderItem>();
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await ProductRepo.GetByIdAsync(item.Id);
                if(product is null)
                    throw new ProductNotFoundException(item.Id);
                var OrderItem = new OrderItem()
                {
                    Product = new ProductItemOrderd()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    },
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                OrderItems.Add(OrderItem);
            }

            // Get Delivery Method
            var delivery = await _unitOfWork.GetRepository<DeliveryMethod,int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (delivery is null)
                throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);
            // Calculate  Sub Total
            var subTotal = OrderItems.Sum(OI => OI.Price *  OI.Quantity);

            var order = new Order(email, address, delivery, OrderItems, subTotal);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Order,OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMehtodDto>> GetDeliveryMehtodAsync()
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMehtodDto>>(deliveryMethod);
        }
        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }
        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }
    }
}
