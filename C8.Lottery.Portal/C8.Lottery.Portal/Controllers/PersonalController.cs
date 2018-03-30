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
using System.Security.Policy;

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
        /// 粉丝榜
        /// </summary>
        /// <returns></returns>
        public ActionResult FansBang()
        {
 
            return View();
        }



        /// <summary>
        /// 粉丝榜数据 只取前50条
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult FansBangList(int typeId, int pageIndex = 1, int pageSize = 20)
        {
         
            try
            {

                string strsql = string.Empty;
                string countsql = string.Empty;
                if (typeId == 1) //日榜
                {
                    strsql = @"  select  * from ( select top 50 row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)
where
FollowTime>=convert(varchar(10),Getdate(),120) and FollowTime<convert(varchar(10),dateadd(d,1,Getdate()),120)
group by Followed_UserId,Name,RPath
)t
WHERE Rank BETWEEN @Start AND @End";
                 
                }
                else if (typeId == 2)//周榜
                {
                    strsql = @"select * from ( select  top 50 row_number() over(order by count(1) desc ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

where year(FollowTime)=year(getdate()) 
group by datename(week,FollowTime), Followed_UserId,Name,RPath
)t
WHERE Rank BETWEEN @Start AND @End";
               
                }
                else if (typeId == 3)//月榜
                {
                    strsql = @"select  * from ( select top 50 row_number() over( order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

where year(FollowTime)=year(getdate()) 

group by month(FollowTime), Followed_UserId,Name,RPath

)t
WHERE Rank BETWEEN @Start AND @End";
                   
                }
                else if (typeId == 4)//总榜
                {
                    strsql = @" select * from ( select top 50 row_number() over(order by count(1) desc) as Rank, 
     count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

group  by Followed_UserId,Name,RPath
)t
WHERE Rank BETWEEN @Start AND @End";
                 

                }
                SqlParameter[] sp = new SqlParameter[] {

                new SqlParameter("@Start", (pageIndex - 1) * pageSize + 1),
                new SqlParameter("@End", pageSize * pageIndex)
            };
                var list = Util.ReaderToList<FansBangListModel>(strsql, sp)??new List<FansBangListModel>();
                ViewBag.FansBangList = list;
                ViewBag.typeId = typeId;
            }
            catch (Exception)
            {

                throw;
            }
          

            return PartialView("FansBangList");
        }


        public JsonResult MyRank(int typeId)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            int userId = UserHelper.GetByUserId();
            string strsql = string.Empty;
            if (typeId == 1)
            {
                 strsql = @" select  * from ( select  row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
  from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)
where
FollowTime>=convert(varchar(10),Getdate(),120) and FollowTime<convert(varchar(10),dateadd(d,1,Getdate()),120)
group by Followed_UserId,Name,RPath
)t
where t.Followed_UserId=@Followed_UserId";
            }else if (typeId == 2)
            {
                strsql = @"select * from ( select   row_number() over(order by count(1) desc ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

where year(FollowTime)=year(getdate()) 
group by datename(week,FollowTime), Followed_UserId,Name,RPath
)t
WHERE t.Followed_UserId=@Followed_UserId";

            }else if (typeId == 3)
            {
                strsql = @" select  * from ( select  row_number() over( order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

where year(FollowTime)=year(getdate()) 

group by month(FollowTime), Followed_UserId,Name,RPath

)t
WHERE t.Followed_UserId=@Followed_UserId";
            }
            else if (typeId == 4)
            {
                strsql = @" select * from ( select  row_number() over(order by count(1) desc) as Rank, 
     count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

group  by Followed_UserId,Name,RPath
)t
WHERE t.Followed_UserId=@Followed_UserId";
            }

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Followed_UserId",userId)
            };
            try
            {
                FansBangListModel fansbang = Util.ReaderToModel<FansBangListModel>(strsql, sp);
                
               
                jsonmsg.Success = true;
                jsonmsg.data = fansbang;
              
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
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public JsonResult ReplacePhotos(string img)
        {
            string error = "";

            string url = Request.Url.GetLeftPart(UriPartial.Authority);
            string FilePath= "/File/" + DateTime.Now.ToString("yyyyMM")+"/";
            Phonto p = Tool.SaveImage(Server.MapPath(FilePath), img, ref error);
            
            int UserId = UserHelper.GetByUserId();
            ReturnMessageJson msg = new ReturnMessageJson();
            if (p!=null)
            {
                try
                {
                    string RPath = url + FilePath + p.ImgName + p.Extension;
                    string strsql = string.Empty;
                    string countsql = "select * from ResourceMapping where  FkId=@FkId and Type=@Type";
                    SqlParameter[] countsp = new SqlParameter[] {
                    new SqlParameter("@FkId",UserId),
                    new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像)

                };
                    ResourceMapping rsmodel = Util.ReaderToModel<ResourceMapping>(countsql, countsp);
                    if (rsmodel!=null)
                    {
                        strsql = @"update ResourceMapping set Extension=@Extension,RPath=@RPath,Title=@Title,CreateTime=getdate(),Type=@Type, RSize=@RSize
                                  where Type =@Type and FkId = @FkId";
                    }
                    else
                    {
                        strsql = @"insert into ResourceMapping(Extension, RPath, Title, SortCode, CreateTime, Type, FkId, RSize) 
                                   values(@Extension, @RPath, @Title, 1, getdate(), @Type, @FkId, @RSize)";
                    }
                    SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@FkId",UserId),
                    new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
                    new SqlParameter("@Extension",p.Extension),
                    new SqlParameter("@RPath",RPath),
                    new SqlParameter("@RSize",p.RSize),
                    new SqlParameter("@Title",p.ImgName)

                    };
                   
                    int data = SqlHelper.ExecuteNonQuery(strsql, sp);
                    if (data > 0)
                    {
                        Uri uri = new Uri(rsmodel.RPath);
                        string oldpath = uri.PathAndQuery;//旧头像地址
                        Tool.DeleteFile(Server.MapPath(oldpath));
                        msg.Success = true;
                        p.RPath = RPath;
                        msg.data = p;
                    }
                    else
                    {
                        msg.Msg = "上传头像失败";
                        msg.Success = false;
                       
                    }
                }
                catch (Exception e)
                {
                    msg.Success = false;
                    msg.Msg = e.Message;
                    throw;
                }
              
            }
            return Json(msg);

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


