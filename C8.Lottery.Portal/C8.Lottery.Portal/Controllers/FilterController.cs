using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C8.Lottery.Portal.Controllers
{
    public class FilterController : Controller
    {
        //
        // GET: /Filter/
        /// <summary>
        /// 全局登录验证 KCP
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["UserInfo"] == null)
            {
                //filterContext.HttpContext.Response.Redirect("/User/Login");
                filterContext.Result = Redirect("/Home/Login");
            }
        }

    }
}
