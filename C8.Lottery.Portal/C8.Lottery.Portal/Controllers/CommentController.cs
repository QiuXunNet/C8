using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
                string sql = "select a.*,b.Name as NickName from [Comment] a join UserInfo b on b.Id=a.UserId where a.Id=" + id;

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

            #region step1.黑名单和禁言验证
            //step1.黑名单和禁言验证
            string validateSql = @"select count(1) from [dbo].[UserState]
	  where ([CommentBlack]=1 or ([CommentShut]=1 and GETDATE() between [CommentShutBegin] and [CommentShutEnd])) and UserId=" +
                UserHelper.LoginUser.Id;

            object obj = SqlHelper.ExecuteScalar(validateSql);

            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Json(new AjaxResult(10001, "你已被禁言或拉黑"));
            }

            #endregion

            #region step2.评论/回复对象是否存在

            long refId = 0;
            long pid = 0;
            long refCommentId = 0;

            if (ctype == 1)
            {
                if (type == 1)
                {
                    //查询计划
                    var model = Util.GetEntityById<BettingRecord>(id);
                    if (model == null) return Json(new AjaxResult(400, "计划已不存在或已删除"));

                    refId = model.Id;
                    //refCommentId=
                }
                else if (type == 2)
                {
                    //查询文章
                    var model = Util.GetEntityById<News>(id);
                    if (model == null) return Json(new AjaxResult(400, "资讯不存在或已删除"));
                    refId = model.Id;
                }
            }
            else
            {
                //查询评论
                var model = Util.GetEntityById<Comment>(id);
                if (model == null) return Json(new AjaxResult(400, "评论不存在或已删除"));
                refId = model.Id;
                pid = model.Id;

                if (model.RefCommentId > 0)
                {
                    //第N级回复，则关联评论Id=上一级评论关联Id
                    refCommentId = model.RefCommentId;
                }
                else
                {
                    //第一级回复，则关联评论Id=上一级评论Id
                    refCommentId = model.Id;
                }
            }


            #endregion

            #region step3.评论内容过滤
            //去Html标签
            content = WebHelper.NoHtml(content);
            //脏字过滤
            content = WebHelper.FilterSensitiveWords(content);
            #endregion
            

            try
            {
                #region step4.添加评论
                string sql = @"INSERT INTO [dbo].[Comment]
           ([PId],[UserId],[Content],[SubTime],[Type],[ArticleId]
           ,[StarCount],[IsDeleted],[RefCommentId])
     VALUES(@PId,@UserId,@Content,GETDATE()
           ,@Type,@ArticleId,0,0,@RefCommentId);";

                SqlParameter[] parameters ={
                new SqlParameter("@PID",pid),
                new SqlParameter("@UserId",UserHelper.LoginUser.Id),
                new SqlParameter("@Content",content),
                new SqlParameter("@Type",type),
                new SqlParameter("@ArticleId",refId),
                new SqlParameter("@RefCommentId",refCommentId),
              };
                int row = SqlHelper.ExecuteNonQuery(sql, parameters);
                if (row <= 0)
                {
                    result = new AjaxResult(10001, "服务器繁忙");
                }

                #endregion
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("发表评论失败。异常消息：{ex.Message}，异常堆栈：{ex.StackTrace}");
                result = new AjaxResult(500, "服务器繁忙");
            }

            return Json(result);
        }


    }
}
