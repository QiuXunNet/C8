using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace C8.Lottery.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //        name: "hk6",
            //        url: "hk6/202.html",
            //        defaults: new { controller = "News", action = "TypeList", id = 5 }
            //    );

            routes.MapRoute(
                    name: "In",
                    url: "In/{id}",
                    defaults: new { controller = "In", action = "Index" }
                );
            routes.MapRoute(
                     name: "Go",
                     url: "Go/{id}",
                     defaults: new { controller = "Go", action = "Index" }
                 );

            routes.MapRoute(
                    name: "article",
                    url: "News/Index/{id}/{ntype}",
                    defaults: new { controller = "News", action = "Index" }
                );

            routes.MapRoute(
                    name: "newarticle",
                    url: "News/NewIndex/{id}/{ntype}",
                    defaults: new { controller = "News", action = "NewIndex" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "GetCode",
               url: "{controller}/{action}/{mobile}",
               defaults: new { controller = "Home", action = "GetCode", mobile = UrlParameter.Optional }
           );
        }
    }
}