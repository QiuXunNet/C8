
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common
{
    /// <summary>
    /// 配置文件辅助类
    /// </summary>
    public class ConfigureHelper
    {
        /// <summary>
        /// 获取数据库连接信息
        /// </summary>
        /// <param name="name">连接字符串的名称</param>
        /// <returns>值</returns>
        public static string GetConnStr(string name)
        {
            var connStr = ConfigurationManager.ConnectionStrings[name];
            return connStr != null ? connStr.ConnectionString : null;
        }

        /// <summary>
        /// 获取AppSetting信息
        /// </summary>
        /// <param name="name">键</param>
        /// <returns>值</returns>
        public static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// 获取组件的key
        /// </summary>
        /// <param name="configKey">注册组件的关键字</param>
        /// <returns>当前配置的组件类型</returns>
        public static string GetComponentKey(string configKey)
        {
            return ConfigurationManager.AppSettings[configKey];
        }
    }
}
