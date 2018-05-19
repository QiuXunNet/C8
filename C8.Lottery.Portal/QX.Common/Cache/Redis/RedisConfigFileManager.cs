using QX.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QX.Common.Cache.Redis
{
    /// <summary>
    /// 配置管理类
    /// </summary>
   public class RedisConfigFileManager: DefaultConfigFileManager
    {
        private static RedisConfigInfo _configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime _fileoldchange;

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        private static string _filename = null;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static RedisConfigFileManager()
        {
            if (SystemHelper.FileExists(ConfigFilePath))
            {
                _fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                _configinfo =
                    (RedisConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(RedisConfigInfo));
            }
        }

        /// <summary>
        /// 当前的配置类实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return _configinfo; }
            set { _configinfo = (RedisConfigInfo)value; }
        }




        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (_filename == null)
                {
                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        _filename = context.Server.MapPath("~\\config\\Redis.config");
                        if (!File.Exists(_filename))
                        {
                            _filename = context.Server.MapPath("config\\Redis.config");
                        }
                    }
                    else
                    {
                        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        if (baseDir.Contains(@"\bin\"))
                        {
                            baseDir = baseDir.Split(new[] { @"\bin\" }, StringSplitOptions.None).First();
                        }
                        _filename = Path.Combine(baseDir, "config\\Redis.config");
                    }

                    if (!File.Exists(_filename))
                    {
                        throw new Exception("发生错误: 虚拟目录或网站根目录下没有正确的redis.config文件");
                    }
                }


                return _filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static RedisConfigInfo LoadConfig()
        {
            if (SystemHelper.FileExists(ConfigFilePath))
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref _fileoldchange, ConfigFilePath, ConfigInfo);
                return ConfigInfo as RedisConfigInfo;
            }
            else
                return null;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
