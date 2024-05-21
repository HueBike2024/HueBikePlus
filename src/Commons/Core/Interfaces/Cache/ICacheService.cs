using Microsoft.Extensions.Caching.Memory;

namespace Core.Interfaces.Cache
{
    public interface ICacheService
    {
        public void SetCacheObject(IMemoryCache cache, string key, object obj);
        public void SetCacheObject(IMemoryCache cache, string key, object obj, int minutes);
    }
}
