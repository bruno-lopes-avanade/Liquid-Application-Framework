using Liquid.Runtime;
using Liquid.Runtime.Configuration.Base;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System;
using System.Threading.Tasks;
using Liquid.Interfaces;
using Liquid.Runtime.Miscellaneous;

namespace Liquid.OnAzure
{
    /// <summary>
    ///  Include support of AzureRedis, that processing data included on Configuration file.
    /// </summary>
    [Obsolete("Prefer to use the standard Microsoft.Extensions.Caching.Redis.RedisCache over Liquid.OnAzure.AzureRedis. \n This class wi'll be removed in the next version.")]
    public class AzureRedis : ILightCache
    {
        private AzureRedisConfiguration _config;
        private IDistributedCache _redisClient = null;
        private DistributedCacheEntryOptions _options = null;

        public AzureRedis(AzureRedisConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Initialize(configuration);
            _config = configuration;
        }
        public AzureRedis()
        {
        }
        /// <summary>
        /// Initialize support of Cache and read file config
        /// </summary>
        public void Initialize()
        {
            _config = LightConfigurator.Config<AzureRedisConfiguration>("AzureRedis");
            Initialize(_config);
        }

        /// <summary>
        /// Initializes the class based on the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuiration for this class.</param>
        private void Initialize(AzureRedisConfiguration configuration)
        {
            _redisClient = new RedisCache(new RedisCacheOptions()
            {
                Configuration = configuration.Configuration,
                InstanceName = configuration.InstanceName
            });

            _options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(configuration.SlidingExpirationSeconds),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.AbsoluteExpirationRelativeToNowSeconds)
            };
        }

        /// <summary>
        /// Get Key on the Azure Redis server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public T Get<T>(string key)
        {
            var data = _redisClient.Get(key);
            return Utils.FromByteArray<T>(data);
        }
        /// <summary>
        /// Get Key Async on the Azure Redis server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var data = await _redisClient.GetAsync(key);
            return Utils.FromByteArray<T>(data);
        }
        /// <summary>
        /// Refresh key get on the Azure Redis server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public void Refresh(string key)
        {
            _redisClient.Refresh(key);
        }
        /// <summary>
        /// Refresh async key get on the Azure Redis server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public async Task RefreshAsync(string key)
        {
            await _redisClient.RefreshAsync(key);
        }
        /// <summary>
        ///  Remove key on the Azure Redis server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public void Remove(string key)
        {
            _redisClient.Remove(key);
        }
        /// <summary>
        ///  Remove async key on the Azure Redis server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public Task RemoveAsync(string key)
        {
            return _redisClient.RemoveAsync(key);
        }
        /// <summary>
        /// Set Key  and value on the Azure Redis server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public void Set<T>(string key, T value)
        {
            _redisClient.Set(key, Utils.ToByteArray(value), _options);
        }
        /// <summary>
        /// Set Key and value Async on the Azure Redis server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public async Task SetAsync<T>(string key, T value)
        {
            await _redisClient.SetAsync(key, Utils.ToByteArray(value), _options);
        }

    }
}
