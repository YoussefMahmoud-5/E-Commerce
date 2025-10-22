using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransfereObeject.BasketModule;

namespace Service
{
    internal class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var basketModel = _mapper.Map<BasketDto , Basket>(basket);
            var createdOrUpdate = await _basketRepository.CreateOrUpdateBasketAsync(basketModel);
            if(createdOrUpdate is not null)
                return await GetBasketAsync(basket.Id);
            throw new Exception("Basket Can't Created Or Update Right Now, Please Try Agian Later");
            
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _basketRepository.DeleteBasketAsync(key);
        }

        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var basket = await _basketRepository.GetBasketAsync(key);
            if(basket is not  null)
                return _mapper.Map<Basket, BasketDto>(basket);
            throw new BasketNotFoundException(key);
        }
    }
}
