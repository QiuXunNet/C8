using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Public;
using C8.Lottery.Model;
namespace C8.Lottery.Portal.Controllers
{
    public class FilterController : BaseController
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


            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

            if (controllerName == "personal" &&
                (actionName == "taskrule" || actionName == "rewardrules" || actionName == "commissionrules" ||
                 actionName == "voucherrules"))
            {
                //跳过登录验证
            }
            else
            {

                var httpCookie = Request.Cookies["UserId"];
                string sessionId = "";
                if (httpCookie != null)
                {
                    sessionId = httpCookie.Value;
                }
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
                        Response.Redirect("/Home/Login");
                    }

                }
                else
                {
                   // object userId = CacheHelper.GetCache(sessionId);
                    object userId = CacheManager.GetObject<int>(sessionId);
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
                            Response.Redirect("/Home/Login");
                        }
                    }

                    //MemClientFactory.WriteCache(sessionId, user, 30);
                }
            }

        }

    }
}
