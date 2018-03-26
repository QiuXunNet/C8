using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal
{
    public static class UserHelp
    {
        /// <summary>
        /// 获取缓存用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUser()
        {
           
           
            string sessionId = HttpContext.Current.Request["sessionId"];
            UserInfo user = MemClientFactory.GetCache<UserInfo>(sessionId);
            return user;

        }
        /// <summary>
        /// 更新缓存用户信息
        /// </summary>
        /// <returns></returns>
        public static void UpdateUser(UserInfo u)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            MemClientFactory.WriteCache(sessionId, u, 30);

        }
    }
}