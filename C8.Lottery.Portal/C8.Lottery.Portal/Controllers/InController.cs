using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class InController : BaseController
    {
        /// <summary>
        /// 链接接入
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public void Index(string id)
        {
            string url = "/Home/Index";

            var bo = ConfigurationManager.AppSettings["FriendshipLinks"];

            if (bo == "true")
            {
                string cachekey = "friendshipLinks:list:all";
                List<FriendLink> list = CacheHelper.GetCache<List<FriendLink>>(cachekey);

                if (list == null)
                {
                    list = Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and state = 0 ");

                    CacheHelper.AddCache(cachekey, list, 2*60);
                }

                var friendLink = list.FirstOrDefault();

                if (friendLink != null)
                {
                    Session["LinkCode"] = id;

                    url = friendLink.TransferUrl.ToLower();

                    //判断是否存在http://
                    if (!(url.IndexOf("http://") > -1 || url.IndexOf("https://") > -1))
                    {
                        url = "https://" + url;
                    }
                    string cachekeys = "friendshipLinks:link:" + id;
                    var obj = CacheHelper.GetCache<int>(cachekeys);
                    if (obj == default(int))
                    {
                        CacheHelper.AddCache(cachekeys, 1);
                    }
                    else
                    {
                        var uvNum = Convert.ToInt32(CacheHelper.GetCache<int>(cachekeys));
                        CacheHelper.SetCache(cachekeys, uvNum + 1);
                    }
                }
            }
            Response.Redirect(url);
        }
    }
}
