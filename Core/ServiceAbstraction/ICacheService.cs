using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICacheService
    {
        // Get 
        Task<string?> GetAsync(string cacheKey);

        // Set
        Task SetAsync(string cacheKey, object cacheValue , TimeSpan timeToLive);
    }
}
