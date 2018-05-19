using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using log4net;
using log4net.Config;
using log4net.Appender;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QX.Common.Logger
{
    /// <summary>  
    /// 日志管理类  
    /// </summary>
    public partial class Logger
    {
       
        private static ILog _defaultLogger = LogManager.GetLogger(typeof(Logger));

        /// <summary>
        /// Gets the default logger.(full Log4netAPI)
        /// </summary>
        /// <value>
        /// The default logger.
        /// </value>
        public static ILog Default { get { return _defaultLogger; } }



        /// <summary>
        /// 写入一个INFO级别的日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(Exception ex)
        {
            _defaultLogger.Info(ex);
        }

        public static void Info(string message, Exception ex = null)
        {
            _defaultLogger.Info(message, ex);
        }

        /// <summary>
        /// 写入一个Error级别的日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null)
        {
            _defaultLogger.Error(message, exception);
        }

        /// <summary>
        /// 写入一个Error级别的日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(Exception ex)
        {
            _defaultLogger.Error(ex);
        }

        public static void Warn(Exception ex)
        {
            _defaultLogger.Warn(ex);
        }

        public static void Warn(string message, Exception ex=null)
        {
            _defaultLogger.Warn(message, ex);
        }

        public static void Fatal(string message, Exception ex=null)
        {
            _defaultLogger.Fatal(message, ex);
        }
        public static void Fatal(string message)
        {
            _defaultLogger.Fatal(message);
        }
        public static void Debug(string message)
        {
            _defaultLogger.Debug(message);
        }
        public static void Debug(string message, Exception ex=null)
        {
            _defaultLogger.Debug(message, ex);
        }
    }
}
