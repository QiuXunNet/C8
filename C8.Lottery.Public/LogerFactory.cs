using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace C8.Lottery.Public
{
    public class LogerFactory
    {
        public static ILog GetCurrentLoger()
        {
            ILog loger = CallContext.GetData("loger") as ILog;
            if (loger == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                loger = LogManager.GetLogger("RollingLogFileAppender");
                CallContext.SetData("loger", loger);
            }
            return loger;
        }
    }
}
