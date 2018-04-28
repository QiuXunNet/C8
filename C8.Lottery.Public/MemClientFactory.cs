using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Memcached.ClientLibrary;

namespace C8.Lottery.Public
{
    public class MemClientFactory
    {
        private static MemcachedClient client = null;

        public static MemcachedClient GetCurrentMemClient()
        {
            MemcachedClient client = CallContext.GetData("client") as MemcachedClient;
            if (client == null)
            {
                string strAppMemcachedServer = System.Configuration.ConfigurationManager.AppSettings["MemcachedServerList"];
                string[] servers = strAppMemcachedServer.Split(',');
                //初始化池
                SockIOPool pool = SockIOPool.GetInstance();
                pool.SetServers(servers);
                pool.InitConnections = 3;
                pool.MinConnections = 3;
                pool.MaxConnections = 500;
                pool.SocketConnectTimeout = 1000;
                pool.SocketTimeout = 3000;
                pool.MaintenanceSleep = 30;
                pool.Failover = true;
                pool.Nagle = false;
                pool.Initialize();
                //客户端实例
                client = new Memcached.ClientLibrary.MemcachedClient();
                client.EnableCompression = false;
                CallContext.SetData("client", client);
            }
            return client;
        }



        public static MemcachedClient GetCurrentMemClient2()
        {
            if (client == null)
            {
                string strAppMemcachedServer = System.Configuration.ConfigurationManager.AppSettings["MemcachedServerList"];
                string[] servers = strAppMemcachedServer.Split(',');
                //初始化池
                SockIOPool pool = SockIOPool.GetInstance();
                pool.SetServers(servers);
                pool.InitConnections = 3;
                pool.MinConnections = 3;
                pool.MaxConnections = 500;
                pool.SocketConnectTimeout = 1000;
                pool.SocketTimeout = 3000;
                pool.MaintenanceSleep = 30;
                pool.Failover = true;
                pool.Nagle = false;
                pool.Initialize();
                //客户端实例
                client = new Memcached.ClientLibrary.MemcachedClient();
                client.EnableCompression = false;
            }

            return client;
        }


        public static void WriteCache<T>(string key, T value, int minutes = 10) where T : class
        {
            if (value == null) return;

            if (client == null)
                client = GetCurrentMemClient();

            string json = value.ToJsonString();

            if (client.KeyExists(key))
            {
                client.Replace(key, json, DateTime.Now.AddMinutes(minutes));
            }
            else
            {
                client.Set(key, json, DateTime.Now.AddMinutes(minutes));
            }
        }

        public static T GetCache<T>(string key) where T : class
        {
            if (client == null)
                client = GetCurrentMemClient();
            try
            {

                string json = client.Get(key).ToString();
                return json.FromJsonString<T>();
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteCache(string key)
        {
            if (client == null)
                client = GetCurrentMemClient();
            try
            {
                return client.Delete(key);
            }
            catch
            {
                return false;
            }
        }


    }
}
