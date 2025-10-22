﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.BasketModule;

namespace DomainLayer.Contracts
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(string key);
        Task<Basket?> CreateOrUpdateBasketAsync(Basket basket , TimeSpan? timeToReturn = null);
        Task<bool> DeleteBasketAsync(string key);
    }
}
