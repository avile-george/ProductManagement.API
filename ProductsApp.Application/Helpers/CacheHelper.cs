using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ProductsApp.Application.Helpers
{
    public class CacheHelper(IMemoryCache cache) : ICacheHelper
    {
        public const int DefaultCacheDurationMinutes = 5; // In a real world app, this would be configurable

        private readonly IMemoryCache _cache = cache;

        public T Get<T>(string cacheKey)
        {
            return _cache.Get<T>(cacheKey) ?? default;
        }

        public void Set<T>(string cacheKey, T data)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(DefaultCacheDurationMinutes))
                .SetPriority(CacheItemPriority.High);

            _cache.Set(cacheKey, data, options);
        }

        public void Clear(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }
}
