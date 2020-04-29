using System;
using Liquid.Interfaces;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Liquid.Runtime
{
    

    /// <summary>
    /// Include support of Cache, that processing data included on Configuration file.
    /// </summary>
    [Obsolete("This class wi'll be removed in the next version.")] 
    public abstract class LightCache : ILightCache
    {
        /// <summary>
        /// Initialize support of Cache
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Get Key on the server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public abstract T Get<T>(string key);
        /// <summary>
        /// Get Key Async on the server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public abstract Task<T> GetAsync<T>(string key);
        /// <summary>
        /// Set Key  and value on the server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>object</returns>
        public abstract void Set<T>(string key, T value);
        /// <summary>
        /// Set Key and value Async on the server cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object</param>
        /// <returns>Task with object</returns>
        public abstract Task SetAsync<T>(string key, T value);

        /// <summary>
        /// Refresh key get on the server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public abstract void Refresh(string key);
        /// <summary>
        /// Refresh async key get on the server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public abstract Task RefreshAsync(string key);
        /// <summary>
        ///  Remove key on the server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        public abstract void Remove(string key);
        /// <summary>
        ///  Remove async key on the server cache
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <returns>Task</returns>
        public abstract Task RemoveAsync(string key);

       
    }
}
