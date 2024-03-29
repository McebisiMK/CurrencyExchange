using CurrencyExchange.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CurrencyExchange.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache  _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<TData> GetCachedData<TData>(string cacheKey)
        {
            var stringData = await _distributedCache.GetStringAsync(cacheKey);

            if (IsEmpty(stringData)) return default(TData);

            return JsonConvert.DeserializeObject<TData>(stringData);
        }

        public async Task SetCacheData<TData>(string cacheKey, TData data, TimeSpan cacheDuration)
        {
            var options = GetCacheEntryOptions(cacheDuration);
            var stringData = JsonConvert.SerializeObject(data);

            await _distributedCache.SetStringAsync(cacheKey, stringData, options);
        }

        private static DistributedCacheEntryOptions GetCacheEntryOptions(TimeSpan cacheDuration)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };
        }

        private static bool IsEmpty(string? stringData)
        {
            return string.IsNullOrWhiteSpace(stringData);
        }
    }
}
