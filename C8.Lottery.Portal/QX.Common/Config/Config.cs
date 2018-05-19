using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common.Config
{
    /// <summary>
    /// 配置文件读取
    /// </summary>
    public class Config
    {
        
        /// <summary>
        /// 获取管理后台库链接
        /// </summary>
        public static string ConnectionString_PlatformManager
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_PlatformManager");
            }
        }
        /// <summary>
        /// 获取用户资料库
        /// </summary>
        public static string ConnectionString_Accounts
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_Accounts");
            }
        }
        /// <summary>
        /// 获取用户财富库
        /// </summary>
        public static string ConnectionString_Treasure
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_Treasure");
            }
        }
        /// <summary>
        /// 获取游戏平台配置库
        /// </summary>
        public static string ConnectionString_Platform
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_Platform");
            }
        }
        /// <summary>
        /// 获取日志库
        /// </summary>
        public static string ConnectionString_Record
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_Record");
            }
        }
        /// <summary>
        /// 获取游戏记录库
        /// </summary>
        public static string ConnectionString_GameRecord
        {
            get
            {
                return ConfigureHelper.GetConnStr("ConnectionString_GameRecord");
            }
        }

        /// <summary>
        /// 纯真IP数据库文件路径
        /// </summary>
        public static string IPDbFilePath
        {
            get
            {
                return @"\File\ipdata.config";
            }
        }

    }
}
