using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class GoController : BaseController
    {
        /// <summary>
        /// 跳出
        /// </summary>
        [HttpGet]
        public void Index(string id)
        {
            string url = "/Home/Index";

            var friendLinkList =
                Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=2 and state = 0 and Code=@Code",
                    new SqlParameter("@Code", id));


            if (friendLinkList != null && friendLinkList.Count > 0)
            {
                var friendLink = friendLinkList.FirstOrDefault();
                url = friendLink.TransferUrl.ToLower();

                //判断是否存在http://
                if (url.IndexOf("http://") != 0)
                {
                    url = "http://" + url;
                }

                string visitRecordSql = "select * from dbo.LinkVisitRecord where RefId=@RefId and SubTime=@Date and [Type]=2";
                //添加
                var list = Util.ReaderToList<LinkVisitRecord>(visitRecordSql,
                    new[] {
                        new SqlParameter("@RefId",friendLink.Id),
                        new SqlParameter("@Date",DateTime.Today),
                    });

                //TODO:需修改IP，UV重复的问题；cookie存储链接来源，注册时获取并记录

                if (list == null || list.Count < 1)
                {
                    //新增
                    string insertRecordSql = string.Format(
                            @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},1,0,0,0,0,2,'{1}')", friendLink.Id, DateTime.Today);
                    SqlHelper.ExecuteScalar(insertRecordSql);

                }
                else
                {
                    //修改
                    string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set ClickCount+=1 where RefId={0} and Type=2 and SubTime='{1}'", friendLink.Id, DateTime.Today);
                    SqlHelper.ExecuteScalar(updateRecordSql);
                }
            }

            Response.Redirect(url);
        }

    }
}
