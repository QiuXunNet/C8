using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QX.Common.Cache.WebCache
{
    /// <summary>
    /// 站点缓存
    /// </summary>
    public class ObjectCache: ICacheManager
    {
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
        /// <summary>
        /// 获取指定缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            System.Web.Caching.Cache objectCache = HttpRuntime.Cache;
            return (T)objectCache[key];
        }

        public T Get<T>(string key, Func<T> fun, int cacheTime = 20)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            System.Web.Caching.Cache obj = HttpRuntime.Cache;
            obj.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 写一个缓存,默认为20
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expire"></param>
        public void Set(string key, object data,int expire = 20)
        {
            System.Web.Caching.Cache obj = HttpRuntime.Cache;
            obj.Insert(key, data,null,DateTime.Now.AddMinutes(expire), TimeSpan.Zero);
        }

        public void Set(string key, object data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 批量移除
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveList(IEnumerable<string> keys)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            keys.ForEach(p=> {
                _cache.Remove(p);
            });
        }
    }
}
