using C8.Lottery.Public;
using C8.Lottery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Security;
using C8.Lottery.Portal.Models;
using C8.Lottery.Model.Enum;

namespace C8.Lottery.Portal.Controllers
{
    public class PersonalController : FilterController
    {
        //
        // GET: /Personal/

        public ActionResult Index()
        {

           

            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);


            return View(user);

        }
        /// <summary>
        /// 修改密码 KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPassword()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldpw">旧密码</param>
        /// <param name="newpw">新密码</param>
        /// <returns></returns>
        public JsonResult ModifyPWD(string oldpwd,string newpwd)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);
          
            if (user != null)
            {

                oldpwd = Tool.GetMD5(oldpwd);
                if (oldpwd != user.Password)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "旧密码不对,请重新输入";
                }
                else
                {
                    newpwd = Tool.GetMD5(newpwd);
                    try
                    {
                        string usersql = "update UserInfo set [Password]=@Password where Mobile =@Mobile ";
                        SqlParameter[] sp = new SqlParameter[] {
                            new SqlParameter("@Password",newpwd),
                            new SqlParameter("@Mobile",user.Mobile)

                        };
                        int date = SqlHelper.ExecuteNonQuery(usersql, sp);
                        if (date > 0)
                        {
                            jsonmsg.Success = true;
                            jsonmsg.Msg = "ok";
                        }
                        else
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "fail";
                        }

                    }
                    catch (Exception e)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = e.Message;
                        throw;
                    }
                }
            }
            else
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = "登录超时，请重新登录";
            }
            return Json(jsonmsg);
        }


        /// <summary>
        /// 设置昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult SetNickName()
        {
            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);

            return View(user);
        }

        /// <summary>
        /// 设置签名
        /// </summary>
        /// <returns></returns>
        public ActionResult SetAutograph()
        {
            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);

            return View(user);
        }
        /// <summary>
        /// 设置性别
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSex()
        {
            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);

            return View(user);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type">1、昵称 2、签名 3、性别</param>
        /// <returns></returns>
        public JsonResult EditUser(string value,int type)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {



                string strsql = string.Empty;
                int UserId = UserHelper.GetByUserId();
                UserInfo user = UserHelper.GetUser(UserId);
                if (type == 1)
                {
                    strsql = "  Name=@value ";
                    user.Name = value;
                }
                else if (type == 2)
                {
                    strsql = "  Autograph=@value ";
                    user.Autograph = value;
                }
                else if (type == 3)
                {
                    strsql = " Sex=@value ";
                    user.Sex = Convert.ToInt32(value);
                }
                string usersql = "update  UserInfo set  " + strsql + "      where  Mobile=@Mobile";

                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@value",value),
                new SqlParameter("@Mobile",user.Mobile)

                };

                int data = SqlHelper.ExecuteNonQuery(usersql, sp);
                if (data > 0)
                {
                   
                    jsonmsg.Success = true;
                    jsonmsg.Msg = "ok";
                }
                else
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "fail";
                }
            }
            catch (Exception e)
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = e.Message;
                throw;
            }
            return Json(jsonmsg);


        }


        /// <summary>
        /// 设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Set()
        {
            int UserId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(UserId);
            return View(user);
        }

        /// <summary>
        /// 注销方法,退出登录
        /// </summary>
        public void logOut()
        {
            string sessionId = Request["UserId"];
            MemClientFactory.DeleteCache(sessionId);

            Response.Redirect("/Home/Login");
        }

        /// <summary>
        /// 赚钱任务
        /// </summary>
        /// <returns></returns>
        public ActionResult Task()
        {
            List<TaskModel> list = new List<TaskModel>();
            string strsql = "select * from MakeMoneyTask";
            List<MakeMoneyTask> tasklist = Util.ReaderToList<MakeMoneyTask>(strsql);
            foreach (var item in tasklist)
            {
                TaskModel tm = new TaskModel();
                tm.Id = item.Id;
                tm.TaskItem = item.TaskItem;
                tm.Coin = item.Coin;
                tm.Count = item.Count;
                tm.SubTime = item.SubTime;
                tm.CompletedCount = GetCompletedCount(item.Id);
                list.Add(tm);
            }

            var model = list;

            return View(model);
        }

        /// <summary>
        /// 获取任务完成数量
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public int GetCompletedCount(int taskid)
        {
            int result = 0;
            int UserId = UserHelper.GetByUserId();
       
            string strsql = "select CompletedCount from  UserTask where UserId =@UserId and TaskId=@TaskId ";
            SqlParameter[] sp = new SqlParameter[] {
                   new SqlParameter("@UserId",UserId),
                   new SqlParameter("@TaskId",taskid)
            };
            result = Convert.ToInt32(SqlHelper.ExecuteScalar(strsql, sp));

            return result;
        }
        /// <summary>
        /// 我的关注
        /// </summary>
        /// <returns></returns>
        public ActionResult Follow()
        {
            return View();
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MyFollow(int pageIndex = 1, int pageSize = 10)
        {
            var result = new AjaxResult<PagedList<Follow>>();
            try
            {
                int UserId = UserHelper.GetByUserId();
               
                string strsql = @"select * from ( 
select a.*, ROW_NUMBER() OVER(Order by a.Id DESC ) AS RowNumber,isnull(b.Name,'') as NickName,ISNULL(b.Autograph,'')as Autograph ,isnull(c.RPath,'')as HeadPath from Follow as a 
 left join UserInfo b on b.Id = a.Followed_UserId
 left join ResourceMapping c on c.FkId = a.UserId and c.Type =@Type
 where a.UserId=@UserId  and a.Status=1
) as d
where RowNumber BETWEEN @Start AND @End ";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",UserId),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                new SqlParameter("@End",pageSize * pageIndex),


            };
                var list = Util.ReaderToList<Follow>(strsql, sp);
                string countsql =string.Format("select count(1) from Follow where UserId={0}",UserId);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));
                var pager = new PagedList<Follow>();
                pager.PageData = list;
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                pager.TotalCount = count;

              

                result.Data = pager;
            }
            catch (Exception ex)
            {

                throw;
            }
         

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <returns></returns>
        public JsonResult UnFollow(long followed_userId)
        {
            int UserId = UserHelper.GetByUserId();

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                string strsql = "update Follow set Status=0,FollowTime=getdate() where UserId=@UserId  and Followed_UserId=@Followed_UserId";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",UserId),
                new SqlParameter("@Followed_UserId",followed_userId)
              };
                int data = SqlHelper.ExecuteNonQuery(strsql, sp);
                if (data > 0)
                {
                    jsonmsg.Msg = "ok";
                    jsonmsg.Success = true;
                }
                else
                {
                    jsonmsg.Msg = "fail";
                    jsonmsg.Success = false;
                }

            }
            catch (Exception e)
            {
                jsonmsg.Msg = e.Message;
                jsonmsg.Success = false;
                throw;
            }
            return Json(jsonmsg);
          
        }


        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="followed_userId"></param>
        /// <returns></returns>
        public JsonResult IFollow(long followed_userId)
        {
            int UserId = UserHelper.GetByUserId();
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                string yzsql = string.Format("select  count(1) from Follow where UserId={0}  and Followed_UserId={1}",UserId,followed_userId);
                string strsql = "";
                int count =Convert.ToInt32(SqlHelper.ExecuteScalar(yzsql));
                if (count > 0)
                {
                     strsql = "update Follow set Status=1,FollowTime=getdate() where UserId=@UserId  and Followed_UserId=@Followed_UserId";
                }
                else
                {
                     strsql = "insert into Follow(UserId, Followed_UserId, Status,FollowTime)values(@UserId, @Followed_UserId, 1,getdate())";
                }
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",UserId),
                new SqlParameter("@Followed_UserId",followed_userId)
                };
                int data = SqlHelper.ExecuteNonQuery(strsql, sp);
                if (data > 0)
                {
                    jsonmsg.Msg = "ok";
                    jsonmsg.Success = true;
                }
                else
                {
                    jsonmsg.Msg = "fail";
                    jsonmsg.Success = false;
                }


            }
            catch (Exception e)
            {
                jsonmsg.Msg = e.Message;
                jsonmsg.Success = false;
                throw;
            }
            return Json(jsonmsg);
        }


        /// <summary>
        /// 我的粉丝
        /// </summary>
        /// <returns></returns>
        public ActionResult Fans()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MyFans(int pageIndex = 1, int pageSize = 10)
        {
            var result = new AjaxResult<PagedList<Follow>>();
            try
            {
                int UserId = UserHelper.GetByUserId();
                string strsql = @"select * from ( 
select a.*, ROW_NUMBER() OVER(Order by a.Id DESC ) AS RowNumber,isnull(b.Name,'') as NickName,ISNULL(b.Autograph,'')as Autograph ,isnull(c.RPath,'')as HeadPath
,(select count(1) from Follow where UserId=@Followed_UserId and Followed_UserId=a.UserId and Status=1)as Isfollowed from Follow as a 
 left join UserInfo b on b.Id = a.UserId
 left join ResourceMapping c on c.FkId = a.UserId and c.Type =@Type
 where a.Followed_UserId=@Followed_UserId  and a.Status=1
) as d
where RowNumber BETWEEN @Start AND @End ";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Followed_UserId",UserId),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                new SqlParameter("@End",pageSize * pageIndex),


            };
                var list = Util.ReaderToList<Follow>(strsql, sp);
                string countsql =string.Format("select count(1) from Follow where Followed_UserId={0}",UserId);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));
                var pager = new PagedList<Follow>();
                pager.PageData = list;
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                pager.TotalCount = count;



                result.Data = pager;
            }
            catch (Exception ex)
            {

                throw;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        /// <summary>
        /// 邀请注册
        /// </summary>
        /// <returns></returns>
         public ActionResult InvitationReg()
        {
            int UserId = UserHelper.GetByUserId();
            ViewData["uid"] = UserId;
            string strsql = @" select Number,Coin from
                              (select count(1) as Number from UserInfo where Pid = @Pid) t1,
                              (select sum([Amount])as Coin from CoinRecord where [lType] = 0 and UserId = @UserId) t2";

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Pid",UserId),
                new SqlParameter("@UserId",UserId)
            };

            InvitationRegModel  irmodel= Util.ReaderToModel<InvitationRegModel>(strsql, sp);
            var model = irmodel;
            return View(model);
        }

        /// <summary>
        /// 奖励规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RewardRules()
        {
            return View();
        }

        /// <summary>
        /// 他人主页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserCenter(int id)
        {
            return View();
        }
    }
}


