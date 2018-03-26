using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Public;
using C8.Lottery.Model;
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


       

            string sessionId = Request["sessionId"];
            if (string.IsNullOrEmpty(sessionId))
            {

                Response.Redirect("/Home/Login");
            }
            else
            {
                UserInfo user= MemClientFactory.GetCache<UserInfo>(sessionId);


                if (user == null)
                {
                    Response.Redirect("/Home/Login");
                }

                MemClientFactory.WriteCache(sessionId, user, 30);
            }
           
        }

        /// <summary>
        /// 获取缓存用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUser()
        {
            string sessionId = Request["sessionId"];
            UserInfo user = MemClientFactory.GetCache<UserInfo>(sessionId);
            return user;
             
        }
        /// <summary>
        /// 更新缓存用户信息
        /// </summary>
        /// <returns></returns>
       public void UpdateUser(UserInfo u)
        {
            string sessionId = Request["sessionId"];
            MemClientFactory.WriteCache(sessionId, u, 30);
          
           
        }


       

    }
}
