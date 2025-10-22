﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models.BasketModule;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    internal class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<Basket?> CreateOrUpdateBasketAsync(Basket basket, TimeSpan? timeToReturn = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket,timeToReturn ?? TimeSpan.FromDays(3));
            if (isCreatedOrUpdated)
                return await GetBasketAsync(basket.Id);
            return null;
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<Basket?> GetBasketAsync(string key)
        {
            var basket = await _database.StringGetAsync(key);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonSerializer.Deserialize<Basket>(basket!);
        }
    }
}
