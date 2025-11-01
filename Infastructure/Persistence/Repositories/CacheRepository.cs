using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    internal class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string cacheKey)
        {
            var CacheValue = await _database.StringGetAsync(cacheKey);
            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(cacheKey, cacheValue, TimeToLive);
        }
    }
}
