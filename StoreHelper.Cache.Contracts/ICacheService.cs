using System;
using System.Collections.Generic;

namespace StoreHelper.Cache.Contracts
{
    public interface ICacheService
    {
        /// <summary>
        /// Get cached value
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheKey">cache key</param>
        /// <returns>cached value</returns>
        T Get<T>(string cacheKey) where T : class;

        /// <summary>
        /// Get or Set in one operation
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheKey">cache key</param>
        /// <param name="getItemCallback">get callback</param>
        /// <param name="policy">Timed cache palicy by default store for 3 hours</param>
        /// <returns>cached value</returns>
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, CacheItemTimePolicy policy = null) where T : class;

        /// <summary>
        /// Set cached value
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="value">New cache item (value)</param>
        /// <param name="policy">Timed cache palicy by default store for 3 hours</param>
        void Set<T>(string cacheKey, T value, CacheItemTimePolicy policy = null) where T : class;

        /// <summary>
        /// Invalidate cache item by key
        /// </summary>
        /// <param name="cacheKey">Cache item key</param>
        /// <returns>true if success</returns>
        bool InvalidateByKey(string cacheKey);

        /// <summary>
        /// Invalidate cache
        /// </summary>
        /// <param name="keys">keys to invalidate</param>
        void Invalidate(IEnumerable<string> keys);
    }
}
