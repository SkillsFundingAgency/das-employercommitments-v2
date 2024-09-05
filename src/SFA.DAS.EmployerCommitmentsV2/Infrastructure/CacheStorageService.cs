using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;

namespace SFA.DAS.EmployerCommitmentsV2.Infrastructure
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheStorageService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task SaveToCache<T>(string key, T item, TimeSpan timeSpan)
        {
            var json = JsonConvert.SerializeObject(item);

            await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            });
        }

        public async Task SaveToCache<T>(string key, T item, int expirationInHours)
        {
            await SaveToCache(key, item, TimeSpan.FromHours(expirationInHours));
        }

        public Task SaveToCache<T>(Guid key, T item, int expirationInHours)
        {
            return SaveToCache(key.ToString(), item, expirationInHours);
        }

        public Task SaveToCache<T>(T item, int expirationInHours) where T : ICacheModel
        {
            return SaveToCache(item.CacheKey.ToString(), item, expirationInHours);
        }

        /// <summary>
        /// Returns NULL instead of throwing exception if cached item not found.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> SafeRetrieveFromCache<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync(key);

            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> RetrieveFromCache<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync(key);

            if (json == null)
            {
                throw new CacheItemNotFoundException<T>($"Cache item {key} of type {typeof(T).Name} not found");
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public Task<T> RetrieveFromCache<T>(Guid key)
        {
            return RetrieveFromCache<T>(key.ToString());
        }

        public async Task DeleteFromCache(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}