using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using ServiceAbstraction;

namespace Service
{
    internal class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacheRepository.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
        {
            var Value = JsonSerializer.Serialize(cacheValue);
            await _cacheRepository.SetAsync(cacheKey, Value, timeToLive);
        }
    }
}
