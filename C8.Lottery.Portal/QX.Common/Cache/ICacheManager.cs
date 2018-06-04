using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common.Cache
{
    /// <summary>
    ///缓存策略接口，提供统一的缓存
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 根据给定的key，获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">Key</param>
        /// <returns>根据key返回的对象</returns>
        T Get<T>(string key);

        /// <summary>
        /// 根据给定的key，取指定的缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="fun">如果为空，将该回调返回</param>
        /// <returns></returns> 
        T Get<T>(string key, Func<T> fun, int cacheTime = 20);

        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存对象</param>
        void Set(string key, object data);

        /// <summary>
        /// 根据给定的key和对象，添加到缓存中
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="expire">缓存时间</param>
        void Set(string key, object data, int expire);

        /// <summary>
        /// 根据key，判断是否已经完成
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool IsSet(string key);

        /// <summary>
        /// 根据给定的key,移除缓存
        /// </summary>
        /// <param name="key">/key</param>
        void Remove(string key);

        /// <summary>
        /// 根据key 的匹配模式，移除是有缓存
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="keys"></param>
        void RemoveList(IEnumerable<string> keys);

        /// <summary>
        /// 正则匹配Keys
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        List<string> GetKeysByPattern(string pattern);
    }
}
