using Core.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Core.Implements.Cache
{
    public class CacheService : ICacheService
    {
        public void SetCacheObject(IMemoryCache cache, string key, object obj)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            cache.Set<object>(key, obj, options);
        }

        public void SetCacheObject(IMemoryCache cache, string key, object obj, int minutes)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(minutes),
                SlidingExpiration = TimeSpan.FromMinutes(minutes)
            };
            cache.Set<object>(key, obj, options);
        }
    }
}
