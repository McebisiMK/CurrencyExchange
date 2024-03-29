namespace CurrencyExchange.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<TData> GetCachedData<TData>(string cacheKey);
        Task SetCacheData<TData>(string cacheKey, TData data, TimeSpan cacheDuration);
    }
}
