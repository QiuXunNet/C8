using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memcached.ClientLibrary;

namespace C8.Lottery.Public
{
    public class CacheHelper
    {
        public static MemcachedClient memcachedClient
        {
            get { return MemClientFactory.GetCurrentMemClient(); }
        }

        public static void AddCache(string key, object value)
        {
            memcachedClient.Add(key, value);
        }

        public static void AddCache(string key, object value, DateTime expDate)
        {
            memcachedClient.Add(key, value, expDate);
        }



        public static object GetCache(string key)
        {
            return memcachedClient.Get(key);
        }


        public static void SetCache(string key, object value)
        {
            memcachedClient.Set(key, value);
        }

        public static void SetCache(string key, object value, DateTime expDate)
        {
            memcachedClient.Set(key, value, expDate);
        }

        public static void DeleteCache(string key)
        {
            memcachedClient.Delete(key);
        }


        public static bool IsExistCache(string key)
        {
            return memcachedClient.KeyExists(key);
        }


    }
}
