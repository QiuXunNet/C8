using QX.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common.Cache.Redis
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class RedisConfigs: IConfigInfo
    {
        private static readonly System.Timers.Timer redisConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static RedisConfigInfo _configinfo;

        static RedisConfigs()
        {
            _configinfo = RedisConfigFileManager.LoadConfig();
            redisConfigTimer.AutoReset = true;
            redisConfigTimer.Enabled = true;
            redisConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            redisConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            _configinfo = RedisConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static RedisConfigInfo GetConfig()
        {
            return _configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(RedisConfigInfo configinfo)
        {
            RedisConfigFileManager rcfm = new RedisConfigFileManager();
            RedisConfigFileManager.ConfigInfo = configinfo;
            return rcfm.SaveConfig();
        }
    }
}
