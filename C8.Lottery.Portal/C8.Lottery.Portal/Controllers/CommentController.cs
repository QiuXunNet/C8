using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Model.Enum;
using C8.Lottery.Portal.Models;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class CommentController : FilterController
    {
        /// <summary>
        /// 发表评论View
        /// </summary>
        /// <param name="id">文章/计划Id或评论Id</param>
        /// <param name="ctype">评论操作类型 1=一级评论 2=回复</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(int id, int ctype, int type = 2)
        {
            var model = new CommentViewModel();
            model.Id = id;
            model.CommentType = type;
            model.OperateType = ctype;

            if (ctype == 2)
            {
                //查询评论人名称
                string sql = $"select a.*,b.Name as NickName from [Comment] a join UserInfo b on b.Id=a.UserId where a.Id={id}";

                var list = Util.ReaderToList<Comment>(sql);
                if (list == null || list.Count == 0)
                    Response.Redirect("News/Lose");//跳转到评论丢失页面
                model.RefCommentName = list.FirstOrDefault().NickName;
            }


            return View(model);
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ctype">发表评论操作类型（1=一级评论 2=回复）</param>
        /// <param name="content">评论内容</param>
        /// <param name="type">评论类型 1=计划 2=文章</param>
        /// <returns></returns>
        public ActionResult Publish(int id, int ctype, string content, int type = 2)
        {
            var result = new AjaxResult();

            var user = Session["UserInfo"] as UserInfo;


            //TODO:脏字处理，编码处理，特殊字符串过滤
            content = HttpUtility.HtmlEncode(content);

            string sql = @"INSERT INTO [dbo].[Comment]
           ([PId]
           ,[UserId]
           ,[Content]
           ,[SubTime]
           ,[Type]
           ,[ArticleId]
           ,[StarCount]
           ,[IsDeleted]
           ,[RefCommentId])
     VALUES
           (@PId
           ,@UserId
           ,@Content
           ,GETDATE()
           ,@Type
           ,@ArticleId
           ,0
           ,0
           ,@RefCommentId)";
            SqlParameter[] parameters = null;
            if (ctype == 1)
            {
                int pid = 0;
                //发表评论
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@PID",pid),
                    new SqlParameter("@UserId",user.Id),
                    new SqlParameter("@Content",content),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@ArticleId",id),
                    new SqlParameter("@RefCommentId",pid),
                };
            }
            else if (ctype == 2)
            {
                var comment = Util.GetEntityById<Comment>(id);
                if (comment == null)
                {
                    LogHelper.WriteLog($"发表回复失败，未知上级Id:{id}");
                    result = new AjaxResult(10002, "发表回复失败");
                    return Json(result);
                }

                int refCommentId = 0;

                if (comment.RefCommentId > 0)
                {
                    //第N级回复，则关联评论Id=上一级评论关联Id
                    refCommentId = comment.RefCommentId;
                }
                else
                {
                    //第一级回复，则关联评论Id=上一级评论Id
                    refCommentId = id;
                }

                //发表回复
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@PID",id),
                    new SqlParameter("@UserId",user.Id),
                    new SqlParameter("@Content",content),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@ArticleId",comment.ArticleId),
                    new SqlParameter("@RefCommentId",refCommentId),
                };
            }

            try
            {

                int row = SqlHelper.ExecuteNonQuery(sql, parameters);
                if (row <= 0)
                {
                    result = new AjaxResult(10001, "服务器繁忙");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"发表评论失败。异常消息：{ex.Message}，异常堆栈：{ex.StackTrace}");
                result = new AjaxResult(10001, "服务器繁忙");
            }

            return Json(result);
        }



        ////
        //// POST: /Comment/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


    }
}
