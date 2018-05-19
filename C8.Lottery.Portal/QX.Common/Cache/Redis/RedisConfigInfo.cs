using QX.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common.Cache.Redis
{
    /// <summary>
    /// 获取配置文件
    /// </summary>
    public class RedisConfigInfo : IConfigInfo
    {
        /// <summary>
        /// 是否应用Redis
        /// </summary>
        public bool ApplyRedis { get; set; }

        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public string WriteServerList { get; set; }

        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        public string ReadServerList { get; set; }

        private int _maxWritePoolSize;
        /// <summary>
        /// 最大写链接数
        /// </summary>
        public int MaxWritePoolSize
        {
            get
            {
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            }
            set
            {
                _maxWritePoolSize = value;
            }
        }

        private int _maxReadPoolSize;
        private int _localCacheTime = 3000;
        private bool _recordeLog = false;
        private int _defaultDbIndex = 0;

        /// <summary>
        /// 最大读链接数
        /// </summary>
        public int MaxReadPoolSize
        {
            get
            {
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            }
            set
            {
                _maxReadPoolSize = value;
            }
        }

        /// <summary>
        /// 自动重启
        /// </summary>
        public bool AutoStart { get; set; }

        public string RedisPassword { get; set; }


        /// <summary>
        /// 本地缓存到期时间，该设置会与memcached搭配使用，单位:秒
        /// </summary>
        public int LocalCacheTime
        {
            get { return _localCacheTime; }
            set { _localCacheTime = value; }
        }

        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        public bool RecordeLog
        {
            get { return _recordeLog; }
            set { _recordeLog = value; }
        }

        /// <summary>
        /// 默认的数据库索引
        /// </summary>
        public int DefaultDbIndex
        {
            get { return _defaultDbIndex; }
            set { _defaultDbIndex = value; }
        }

        /// <summary>
        /// 最大链接超时时间，单位为
        /// </summary>
        public int ConnectTimeout { get; set; }
    }
}
