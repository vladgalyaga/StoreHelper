using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using StoreHelper.Cache.Contracts;

namespace StoreHelper.ODA.Cache
{

    /// <summary>
    /// This class is initialized as Singletone in the Unity Container.
    /// </summary>
    public sealed class CacheService : ICacheService
    {
        private readonly MemoryCache cache;

        public CacheService()
        {
            cache = new MemoryCache(typeof(CacheService).ToString());
        }

        /// <summary>
        /// Get cached value
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="cacheKey">Cache key</param>
        /// <returns>cached value</returns>
        public T Get<T>(string cacheKey) where T : class
        {
            if (String.IsNullOrEmpty(cacheKey))
                return default(T);

            T cacheItem = this.cache.Get(cacheKey) as T;
            return cacheItem;
        }

        /// <summary>
        /// Set cached value
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="value">New cache item (value)</param>
        /// <param name="policy">Timed cache palicy by default store for 3 hours</param>
        public void Set<T>(string cacheKey, T value, CacheItemTimePolicy timePolicy = null) where T : class
        {
            if (String.IsNullOrEmpty(cacheKey))
                return;

            timePolicy = timePolicy ?? CacheItemTimePolicy.Default; //By default expiration timeout is set to 4 hours
            TimeSpan expiration = (timePolicy.ValidTill - DateTime.UtcNow);

            var cachePolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(expiration.TotalHours) };
            this.cache.Set(new CacheItem(cacheKey, value), cachePolicy);
        }

        /// Get or Set in one operation
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheKey">cache key</param>
        /// <param name="getItemCallback">get callback</param>
        /// <returns>cached value</returns>
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, CacheItemTimePolicy timePolicy = null) where T : class
        {
            if (String.IsNullOrEmpty(cacheKey))
                return default(T);

            T value = Get<T>(cacheKey);
            if (value != null)
                return value;

            value = getItemCallback();
            Set(cacheKey, value, timePolicy);

            return value;
        }

        /// <summary>
        /// Invalidate cache item by key
        /// </summary>
        /// <param name="cacheKey">Cache item key</param>
        /// <returns>true if success</returns>
        public bool InvalidateByKey(string cacheKey)
        {
            if (!String.IsNullOrEmpty(cacheKey) && this.cache.Contains(cacheKey))
            {
                object item = cache.Remove(cacheKey);
                return (item != null);
            }

            return false;
        }

        /// <summary>
        /// Invalidate cache by keys
        /// </summary>
        /// <param name="keys">Keys to invalidate</param>
        public void Invalidate(IEnumerable<string> keys)
        {
            foreach (string cacheKey in keys)
            {
                if (cache.Contains(cacheKey))
                    this.cache.Remove(cacheKey);
            }
        }
    }
}
