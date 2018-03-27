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
            UserInfo user = UserHelper.GetUser();
            string usersql = @"select (select count(1)from Follow where UserId=u.Id and Status=1)as follow,(select count(1)from Follow where Followed_UserId=u.Id and Status=1)as fans, r.RPath as Headpath,u.* from UserInfo  u 
                              left  JOIN (select RPath,FkId from ResourceMapping where Type = @Type)  r 
                              on u.Id=r.FkId  where u.Mobile=@Mobile ";
          
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", user.Mobile), new SqlParameter("@Type", (int)ResourceTypeEnum.用户头像) };
                List<UserInfo> list = Util.ReaderToList<UserInfo>(usersql, sp);
                if (list != null)
                {
                    user = list.FirstOrDefault(x => x.Mobile == user.Mobile);
                }
             
            }
            catch (Exception)
            {

                throw;
            }
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
          
            UserInfo user = GetUser();
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
                        if(date > 0)
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
            var user = GetUser();
          
            return View(user);
        }

        /// <summary>
        /// 设置签名
        /// </summary>
        /// <returns></returns>
        public ActionResult SetAutograph()
        {
            var user = GetUser();

            return View(user);
        }
        /// <summary>
        /// 设置性别
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSex()
        {
            var user = GetUser();

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
                UserInfo user = GetUser();
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
                    user.Sex =Convert.ToInt32(value);
                }
                string usersql = "update  UserInfo set  " + strsql + "      where  Mobile=@Mobile";
              
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@value",value),
                new SqlParameter("@Mobile",user.Mobile)
                
                };

                int data = SqlHelper.ExecuteNonQuery(usersql, sp);
                if (data > 0)
                {
                    UpdateUser(user);
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
            UserInfo user = GetUser();
            return View(user);
        }

        /// <summary>
        /// 注销方法,退出登录
        /// </summary>
        public void logOut()
        {
            string sessionId = Request["sessionId"];
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
            UserInfo user = GetUser();
            string strsql= "select CompletedCount from  UserTask where UserId =@UserId and TaskId=@TaskId ";
            SqlParameter[] sp = new SqlParameter[] {
                   new SqlParameter("@UserId",user.Id),
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
                UserInfo user = UserHelper.GetUser();
                string strsql = @"select * from ( 
select a.*, ROW_NUMBER() OVER(Order by a.Id DESC ) AS RowNumber,isnull(b.Name,'') as NickName,ISNULL(b.Autograph,'')as Autograph ,isnull(c.RPath,'')as HeadPath from Follow as a 
 left join UserInfo b on b.Id = a.Followed_UserId
 left join ResourceMapping c on c.FkId = a.UserId and c.Type =@Type
 where a.UserId=@UserId  and a.Status=1
) as d
where RowNumber BETWEEN @Start AND @End ";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",user.Id),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                new SqlParameter("@End",pageSize * pageIndex),


            };
                var list = Util.ReaderToList<Follow>(strsql, sp);
                string countsql = $"select count(1) from Follow where UserId={user.Id}";
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
            UserInfo user = UserHelper.GetUser();
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                string strsql = "update Follow set Status=0,FollowTime=getdate() where UserId=@UserId  and Followed_UserId=@Followed_UserId";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",user.Id),
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
            UserInfo user = UserHelper.GetUser();
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                string yzsql = $"select  count(1) from Follow where UserId={user.Id}  and Followed_UserId={followed_userId}";
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
                new SqlParameter("@UserId",user.Id),
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
                UserInfo user = UserHelper.GetUser();
                string strsql = @"select * from ( 
select a.*, ROW_NUMBER() OVER(Order by a.Id DESC ) AS RowNumber,isnull(b.Name,'') as NickName,ISNULL(b.Autograph,'')as Autograph ,isnull(c.RPath,'')as HeadPath
,(select count(1) from Follow where UserId=@Followed_UserId and Followed_UserId=a.UserId and Status=1)as Isfollowed from Follow as a 
 left join UserInfo b on b.Id = a.UserId
 left join ResourceMapping c on c.FkId = a.UserId and c.Type =@Type
 where a.Followed_UserId=@Followed_UserId  and a.Status=1
) as d
where RowNumber BETWEEN @Start AND @End ";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Followed_UserId",user.Id),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                new SqlParameter("@End",pageSize * pageIndex),


            };
                var list = Util.ReaderToList<Follow>(strsql, sp);
                string countsql = $"select count(1) from Follow where Followed_UserId={user.Id}";
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
    }
}


