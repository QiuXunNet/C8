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
    public class InController : Controller
    {
        /// <summary>
        /// 链接接入
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public void Index(string id)
        {
            string url = "/Home/Index";

            var friendLinkList =
                Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and Code=@Code",
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

                string visitRecordSql = "select * from dbo.LinkVisitRecord where RefId=@RefId and SubTime=@Date and [Type]=1";
                //添加
                var sqlParameter = new[]
                {
                        new SqlParameter("@RefId",friendLink.Id),
                        new SqlParameter("@Date",DateTime.Today),
                };
                var list = Util.ReaderToList<LinkVisitRecord>(visitRecordSql, sqlParameter);


                //TODO:需调整统计访问IP，UV

                if (list == null || list.Count < 1)
                {
                    //新增
                    string insertRecordSql = string.Format(
                            @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},{1},{2},{3},{4},{5},{6},GETDATE())", friendLink.Id, 1, 1, 1, 1, 0, 1);
                    SqlHelper.ExecuteScalar(insertRecordSql);

                }
                else
                {
                    //修改
                    string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set ClickCount+=1,UV+=1,IP+=1,PV+=1
where RefId={0}", friendLink.Id);
                    SqlHelper.ExecuteScalar(updateRecordSql);
                }


            }

            Response.Redirect(url);
        }

    }
}
