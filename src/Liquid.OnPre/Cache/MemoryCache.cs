using Liquid.Runtime;
using System;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Liquid.Runtime.Configuration.Base;
using Liquid.Interfaces;

namespace Liquid.OnWindowsClient
{
    /// <summary>
    ///  Include support of MemoryCache, that processing data included on Configuration file.
    /// </summary>
    [Obsolete("Prefer to use the standard IDistributedCache over ILightCache. \n This class wi'll be removed in the next version.")]
    public class MemoryCache : ILightCache
    {
        /// <summary>
        /// Cache of memory
        /// </summary>
        private static System.Runtime.Caching.MemoryCache Cache = System.Runtime.Caching.MemoryCache.Default;

        private MemoryCacheConfiguration config;
        private CacheItemPolicy options = null;

        /// <summary>
        /// Initialize support of Cache and read file config
        /// </summary>
        public  void Initialize()
        {
            config = LightConfigurator.Config<MemoryCacheConfiguration>("MemoryCache");
            options = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromSeconds(config.SlidingExpirationSeconds),
                AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(config.AbsoluteExpirationRelativeToNowSeconds)
            };
        }
        /// <summary>
        /// Get Key on the MemoryCache server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public T Get<T>(string key)
        {
            var data = (T)Cache.Get(key);
            return data;
        }
        /// <summary>
        /// Get Key Async on the MemoryCache server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public  async Task<T> GetAsync<T>(string key)
        {
            var data = (T)Cache.Get(key);
            return await Task.FromResult<T>(data);
        }
        /// <summary>
        /// Refresh key get on the MemoryCache server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public  void Refresh(string key)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Refresh async key get on the MemoryCache server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public  async Task RefreshAsync(string key)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  Remove key on the MemoryCache server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public  void Remove(string key)
        {
            Cache.Remove(key);
        }
        /// <summary>
        ///  Remove async key on the MemoryCache server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public  Task RemoveAsync(string key)
        {
            Cache.Remove(key);
            return Task.FromResult(true);
        }
        /// <summary>
        /// Set Key  and value on the MemoryCache server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public  void Set<T>(string key, T value)
        {
            Cache.Add(key, value, options);
        }
        /// <summary>
        /// Set Key and value Async on the MemoryCache server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public  async Task SetAsync<T>(string key, T value)
        {
            Cache.Add(key, value, options);
        }
    }
}
