using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QX.Common.Cache.Redis
{
    /// <summary>
    /// 缓存封装
    /// </summary>
    public class RedisCache : ICacheManager
    {
        /// <summary>
        /// 默认过期时间
        /// </summary>
        protected int _timeOut = 20;
        public int TimeOut
        {
            set { _timeOut = value > 0 ? value : 20; }
            get { return _timeOut > 0 ? _timeOut : 20; }
        }


        public IRedisClient RedisClient
        {
            get
            {
                return RedisClientFactory.Client;
            }
        }
        /// <summary>
        /// 根据给定的key，获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            using (var client = RedisClient)
            {
                return client.Get<T>(key);
            }
        }
        /// <summary>
        /// 根据一组KEY获取dic集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, T> Get<T>(List<string> keys)
        {
            using (var client = RedisClient)
            {
                var result = client.GetAll<T>(keys);

                return result;
            }
        }
        /// <summary>
        ///  根据给定的key，取指定的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public T Get<T>(string key, Func<T> fun, int cacheTime = 20)
        {
            if (IsSet(key))
            {
                return Get<T>(key);
            }
            else
            {
                T obj = fun();
                Set(key, obj, cacheTime);
                return obj;
            }

        }
        /// <summary>
        /// 写一个缓存对象 默认为20
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set(string key, object data)
        {

            if (key == null || key.Length == 0 || data == null)
            {
                return;
            }

            Set(key, data, TimeOut);

        }
        /// <summary>
        /// 写一个缓存，自定义时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object data, int cacheTime)
        {
            using (var client = RedisClient)
            {
                //if (key == null || key.Length == 0 || data == null)
                //{
                //    return;
                //}

                //if (IsSet(key))
                //{
                //    client.Replace(key, data, DateTime.Now.AddMinutes(cacheTime));
                //}
                //else
                //{
                //    client.Set(key, data, DateTime.Now.AddMinutes(cacheTime));
                //}

                client.Set(key, data);
                client.ExpireEntryAt(key, DateTime.Now.AddMinutes(cacheTime));

            }
        }
        /// <summary>
        /// 检查是否存在缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            using (var client = RedisClient)
            {
                return client.ContainsKey(key);
            }

        }
        /// <summary>
        /// 移除一个缓存对象
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            using (var client = RedisClient)
            {
                if (key == null || key.Length == 0)
                {
                    return;
                }
                client.Remove(key);
            }
        }
        /// <summary>
        /// 根据正则表达式匹配所有缓存，移除
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveByPattern(string pattern)
        {
            using (var client = RedisClient)
            {
                var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var keysToRemove = new List<String>();

                keysToRemove = (from key in client.GetAllKeys()
                                where regex.IsMatch(key)
                                select key).ToList();

                client.RemoveAll(keysToRemove);
               
            }
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            using (var client = RedisClient)
            {
                client.RemoveAll(client.GetAllKeys());
            }
        }
        /// <summary>
        /// 批量移除
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveList(IEnumerable<string> keys)
        {
            using (var client = RedisClient)
            {
                client.RemoveAll(keys);
            }
        }
    }
}
