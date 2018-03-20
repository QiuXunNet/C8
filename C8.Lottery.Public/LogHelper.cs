using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace C8.Lottery.Public
{
    public class LogHelper
    {
        public static ILog Loger
        {
            get
            {
                return LogerFactory.GetCurrentLoger();
            }
        }


        public static void WriteLog(string message)
        {
            Loger.Error(message);
        }
    }
}
