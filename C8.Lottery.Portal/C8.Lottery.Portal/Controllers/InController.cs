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
                Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and state = 0 and Code=@Code",
                    new SqlParameter("@Code", id));

            if (friendLinkList != null && friendLinkList.Count > 0)
            {                
                Session["LinkCode"] = id;                

                var friendLink = friendLinkList.FirstOrDefault();
                url = friendLink.TransferUrl.ToLower();

                //判断是否存在http://
                if (url.IndexOf("http://") != 0)
                {
                    url = "http://" + url;
                }

                string visitRecordSql = "select count(*) from dbo.LinkVisitRecord where RefId=@RefId and SubTime=@Date and [Type]=1";
                //添加
                var sqlParameter = new[]
                {
                        new SqlParameter("@RefId",friendLink.Id),
                        new SqlParameter("@Date",DateTime.Today),
                };
                var count = Convert.ToInt32(SqlHelper.ExecuteScalar(visitRecordSql, sqlParameter));

                if (count == 0)
                {
                    //新增
                    string insertRecordSql = string.Format(
                            @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},1,0,0,0,0,1,'{1}')", friendLink.Id, DateTime.Today);
                    SqlHelper.ExecuteScalar(insertRecordSql);

                }
                else
                {
                    //修改
                    string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set ClickCount+=1 where RefId={0} and SubTime = '{1}' and [Type]=1 ", friendLink.Id, DateTime.Today);
                    SqlHelper.ExecuteScalar(updateRecordSql);
                }
            }

            Response.Redirect(url);
        }
    }
}
