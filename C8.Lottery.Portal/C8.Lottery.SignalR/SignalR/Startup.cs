using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(C8.Lottery.SignalR.Startup))]

namespace C8.Lottery.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr",
                map =>
                {
                    //允许跨域
                    map.UseCors(CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration {
                        //EnableJSONP = true;
                    };
                    map.RunSignalR(hubConfiguration);
                }
            );

            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            //注册管道,使用默认的虚拟地址,根目录下的"/signalr",当然你也可以自己定义
           // app.MapSignalR();
        }
    }
}