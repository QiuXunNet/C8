using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
//using Newtonsoft.Json;
namespace C8.Lottery.Portal.Controllers
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string sessionId = filterContext.HttpContext.Request["UserId"];
            string sheader = filterContext.HttpContext.Request.Headers["X-Requested-With"];
            bool isAjaxRequest = (sheader != null && sheader == "XMLHttpRequest") ? true : false;

            if (string.IsNullOrEmpty(sessionId))
            {
                if (isAjaxRequest)
                {
                    JsonResult json = new JsonResult();
                    json.Data = new { Code = 401, Message = "未经授权" };
                    filterContext.Result = json;
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/Home/Login");
                }
            }
            else
            {
                object userId = CacheHelper.GetCache(sessionId);

                if (userId == null)
                {
                    if (isAjaxRequest)
                    {
                        JsonResult json = new JsonResult();
                        json.Data = new { Code = 401, Message = "未经授权" };
                        filterContext.Result = json;
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Redirect("/Home/Login");
                    }
                }

               
            }

            base.OnActionExecuting(filterContext);
        }


    }
}