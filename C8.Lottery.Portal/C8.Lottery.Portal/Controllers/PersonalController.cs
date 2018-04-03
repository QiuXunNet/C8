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
        public JsonResult ModifyPWD(string oldpwd, string newpwd)
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
        public JsonResult EditUser(string value, int type)
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
 left join ResourceMapping c on c.FkId =  a.Followed_UserId and c.Type =@Type
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
                string countsql = string.Format("select count(1) from Follow where UserId={0}", UserId);
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
                string yzsql = string.Format("select  count(1) from Follow where UserId={0}  and Followed_UserId={1}", UserId, followed_userId);
                string strsql = "";
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(yzsql));
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
                string countsql = string.Format("select count(1) from Follow where Followed_UserId={0}", UserId);
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

            InvitationRegModel irmodel = Util.ReaderToModel<InvitationRegModel>(strsql, sp);
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
                var list = Util.ReaderToList<FansBangListModel>(strsql, sp) ?? new List<FansBangListModel>();
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
            }
            else if (typeId == 2)
            {
                strsql = @"select * from ( select   row_number() over(order by count(1) desc ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=2)

where year(FollowTime)=year(getdate()) 
group by datename(week,FollowTime), Followed_UserId,Name,RPath
)t
WHERE t.Followed_UserId=@Followed_UserId";

            }
            else if (typeId == 3)
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
            string FilePath = "/File/" + DateTime.Now.ToString("yyyyMM") + "/";
            Phonto p = Tool.SaveImage(Server.MapPath(FilePath), img, ref error);

            int UserId = UserHelper.GetByUserId();
            ReturnMessageJson msg = new ReturnMessageJson();
            if (p != null)
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
                    if (rsmodel != null)
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
        /// <param name="id">受访人用户Id</param>
        /// <returns></returns>
        public ActionResult UserCenter(int id)
        {
            var loginUserId = UserHelper.LoginUser.Id;

            #region 添加访问记录
            //TODO:添加访问记录
            string visitSql = @"
      if exists
      (
        select 1 from [dbo].[AccessRecord] where UserId = @UserId and RespondentsUserId = @RespondentsUserId and Module=@Module and Datediff(day,AccessDate,GETDATE())=0
      )
      begin
      update [dbo].[AccessRecord] set AccessTime=GETDATE() WHERE  UserId = @UserId and RespondentsUserId = @RespondentsUserId and Module=@Module and Datediff(day,AccessDate,GETDATE())=0
      end
      else
      begin
      insert into  [dbo].[AccessRecord] (UserId,RespondentsUserId,Module,AccessTime,AccessDate) values(@UserId,@RespondentsUserId,@Module,GETDATE(),GETDATE())
      end";

            var sqlParameter = new[]
            {
                new SqlParameter("@UserId",loginUserId),
                new SqlParameter("@RespondentsUserId",id),
                new SqlParameter("@Module",1) //默认访问主页
            };

            SqlHelper.ExecuteNonQuery(visitSql, sqlParameter);
            #endregion

            //查询是否存在当前用户对受访人的已关注记录 Status=1:已关注
            string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + loginUserId +
                         " and [Followed_UserId]=" + id;

            object obj = SqlHelper.ExecuteScalar(sql);

            ViewBag.Followed = obj != null && Convert.ToInt32(obj) > 0;

            var model = UserHelper.GetUser(id);
            return View(model);
        }


        /// <summary>
        /// 获取计划列表
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户计划列表</param>
        /// <param name="ltype">彩种类型Id</param>
        /// <param name="winState">开奖状态</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPlan(int uid = 0, int ltype = 0, int winState = 0, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<BettingRecord>>();
            try
            {
                var pager = new PagedList<BettingRecord>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;

                string winStateWhere = "";

                if (winState > 0)
                {
                    if (winState == 2)
                    {
                        //未开奖
                        winStateWhere = " AND WinState=1";
                    }
                    else if (winState == 1)
                    {
                        //已开奖
                        winStateWhere = " AND WinState in(3,4)";
                    }
                }

                string ltypeWhere = "";
                if (ltype > 0) ltypeWhere = " AND lType=" + ltype;

                string sql = string.Format(@"SELECT * FROM ( 
	SELECT row_number() over(order by Issue DESC,lType ) as rowNumber,* FROM (
		SELECT distinct lType,Issue, (case WinState when 1 then 1 else 2 end) as WinState FROM [dbo].[BettingRecord]
		WHERE UserId=@UserId{0}{1}
		) t
	) tt
WHERE rowNumber BETWEEN @Start AND @End", ltypeWhere, winStateWhere);

                if (uid == 0)
                    uid = UserHelper.GetByUserId();

                var sqlParameters = new[]
                {
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)
                };

                pager.PageData = Util.ReaderToList<BettingRecord>(sql, sqlParameters);

                string countSql = string.Format(@"SELECT count(1) FROM ( SELECT  distinct lType,Issue  FROM [dbo].[BettingRecord] 
WHERE UserId = {0}{1}{2} ) tt", uid, ltypeWhere, winStateWhere);
                object obj = SqlHelper.ExecuteScalar(countSql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

                result.Data = pager;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("异常消息：{0}，异常堆栈：{1}", ex.Message, ex.StackTrace));
                result.Code = 500;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取动态
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户动态</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDenamic(int uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<Comment>>();
            try
            {
                var pager = new PagedList<Comment>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string sql = @"SELECT * FROM (
	select row_number() over(order by a.SubTime DESC ) as rowNumber,
	a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
	from Comment a
	left join UserInfo b on b.Id = a.UserId
	left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
	where a.IsDeleted=0 and a.UserId=@UserId
) tt
WHERE rowNumber BETWEEN @Start AND @End";

                if (uid == 0)
                    uid = UserHelper.GetByUserId();

                var sqlParameters = new[]
                {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@UserId",uid),
                new SqlParameter("@Start",pager.StartIndex),
                new SqlParameter("@End",pager.EndIndex)
            };

                pager.PageData = Util.ReaderToList<Comment>(sql, sqlParameters);

                pager.PageData.ForEach(x =>
                {
                    if (x.PId > 0)
                    {
                        x.ParentComment = GetComment(x.PId);
                    }
                    if (string.IsNullOrEmpty(x.Avater))
                    {
                        x.Avater = "/images/default_avater.png";
                    }
                    x.LotteryTypeName = GetLotteryTypeName(x.Type, x.ArticleId);
                });

                string countSql = "SELECT count(1) FROM Comment WHERE IsDeleted=0 AND UserId=" + uid;
                object obj = SqlHelper.ExecuteScalar(countSql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

                result.Data = pager;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("异常消息：{0}，异常堆栈：{1}", ex.Message, ex.StackTrace));
                result.Code = 500;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取访问记录
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户访问记录</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetVisitRecord(int uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<AccessRecord>>();
            try
            {
                var pager = new PagedList<AccessRecord>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string sql = @"SELECT * FROM (
	  select row_number() over(order by SubTime DESC ) as rowNumber,a.Id,a.UserId,a.Module,a.AccessTime,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater from  [dbo].[AccessRecord] a
	  left join UserInfo b on b.Id=a.UserId
	  left join ResourceMapping c on c.Id=a.UserId and c.[Type]=@ResourceType
	  where a.RespondentsUserId=@RespondentsUserId
) tt
WHERE rowNumber BETWEEN @Start AND @End";

                if (uid == 0)
                    uid = UserHelper.GetByUserId();

                var sqlParameters = new[]
                {
                    new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                    new SqlParameter("@RespondentsUserId",uid),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)
                };

                pager.PageData = Util.ReaderToList<AccessRecord>(sql, sqlParameters);

                pager.PageData.ForEach(x =>
                {
                    if (string.IsNullOrEmpty(x.Avater))
                    {
                        x.Avater = "/images/default_avater.png";
                    }
                });

                string countSql = "SELECT count(1) FROM [AccessRecord] WHERE RespondentsUserId=" + uid;
                object obj = SqlHelper.ExecuteScalar(countSql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

                result.Data = pager;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("异常消息：{0}，异常堆栈：{1}", ex.Message, ex.StackTrace));
                result.Code = 500;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我的计划
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <returns></returns>
        public ActionResult MyPlan(int id = 0)
        {
            ViewBag.UserId = UserHelper.LoginUser.Id;
            ViewBag.LType = id;

            string sql = "SELECT lType as Id FROM [dbo].[BettingRecord] WHERE UserId=" + UserHelper.LoginUser.Id +
                         " GROUP BY lType";
            var list = Util.ReaderToList<LotteryType>(sql);

            list.ForEach(x =>
            {
                x.TypeName = Util.GetLotteryTypeName((int) x.Id);
            });
            return View(list);
        }

        private Comment GetComment(long id)
        {
            string sql = @"select a.*,isnull(b.Name,'') as NickName
 from Comment a
left join UserInfo b on b.Id=a.UserId
where  a.Id=@Id and a.IsDeleted = 0";
            var parameters = new[]
           {
                new SqlParameter("@Id",id),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);


            if (list.Any())
            {
                return list.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 查询彩种类型名称
        /// </summary>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="id">计划/文章 Id</param>
        /// <returns></returns>
        private string GetLotteryTypeName(int type, int id)
        {
            if (type == 1)
            {
                //查询计划所属彩种
                var record = Util.GetEntityById<BettingRecord>(id);
                if (record != null)
                {
                    return Util.GetLotteryTypeName(record.lType);
                }
            }
            else
            {
                //查询文章的彩种类型
                string sql = @"select d.TypeName,a.* from Comment a
left join news b on b.Id=a.ArticleId
left join NewsType c on c.Id=b.TypeId
left join LotteryType d on d.Id= c.lType
where a.[Type]=2 and a.Id=" + id;
                var list = Util.ReaderToList<LotteryType>(sql);
                if (list.Any())
                    return list.First().TypeName;
            }

            return "";
        }


        /// <summary>
        /// 参与竞猜
        /// </summary>
        /// <returns></returns>
        public ActionResult TakeBet()
        {
            string strsql = @"select  * from LotteryType
                              order by SortCode asc";
            List<LotteryType> list = Util.ReaderToList<LotteryType>(strsql);

            return View(list);
        }
        /// <summary>
        /// 获取竞猜数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBet(int ltype = 0, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<BetModel>>();
            int userId = UserHelper.GetByUserId();
            try
            {
                List<BetModel> list = new List<BetModel>();
                var pager = new PagedList<BetModel>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string strsql = "";
                SqlParameter[] sp = new SqlParameter[] { };
                if (ltype == 0)//热门
                {
                     strsql = @"SELECT * FROM (
	  select row_number() over(order by SortCode  ) as rowNumber,
	 (select isnull(sum(Score),'0')  from [dbo].[BettingRecord] where [UserId]=UserId
and lType=l.LotteryCode)as Score,* from Lottery l
	  where IsHot=1
)t
WHERE rowNumber BETWEEN @Start AND @End";

                     sp = new SqlParameter[] {

                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@Start",  pager.PageIndex ),
                        new SqlParameter("@End", pager.PageSize)

                    };

                }
                else
                {
                    strsql = @"SELECT * FROM (
	  select row_number() over(order by SortCode  ) as rowNumber,
	 (select isnull(sum(Score),'0')  from [dbo].[BettingRecord] where [UserId]=2
and lType=l.LotteryCode)as Score,* from Lottery l
	  where lType=@lType and IsHot=0
)t
WHERE rowNumber BETWEEN @Start AND @End";
                        sp = new SqlParameter[] {
                        new SqlParameter("@lType",ltype),
                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@Start",  pager.PageIndex ),
                        new SqlParameter("@End", pager.PageSize)

                    };
                }
                pager.PageData = Util.ReaderToList<BetModel>(strsql, sp);

                string countSql = string.Format("select count(1) from Lottery where lType ={0} and IsHot = 0", ltype);
                object obj = SqlHelper.ExecuteScalar(countSql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);
               
                //pager.PageData.ForEach(x=>
                //{
                //    x.LotteryIcon = "/images/" + x.LotteryIcon + ".png";
                //});
                result.Data = pager;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 我的成绩
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAchievement()
        {
           ACHVModel model =new ACHVModel();
            try
            {
                string lotterytypesql = @"select  * from LotteryType
                              order by SortCode asc";
                List<LotteryType> LotteryTypelist = Util.ReaderToList<LotteryType>(lotterytypesql);//频道
                string lotterysql = @"select * from Lottery
                                where IsHot = 0";
                List<C8.Lottery.Model.Lottery> LotteryList = Util.ReaderToList<C8.Lottery.Model.Lottery>(lotterysql);//采种

                string lotteryhotsql = @"select * from Lottery
                                where IsHot = 1";
                List<C8.Lottery.Model.Lottery> HotLotteryTypeList = Util.ReaderToList<C8.Lottery.Model.Lottery>(lotteryhotsql);//热门采种

                string IntegralRulesql = @"select * from IntegralRule";
                List<IntegralRule> IntegralRuleList = Util.ReaderToList<IntegralRule>(IntegralRulesql);//玩法
                model.LotteryType = LotteryTypelist;
                model.HotLottery = HotLotteryTypeList;
                model.Lottery = LotteryList;
                model.IntegralRule = IntegralRuleList;


            }
            catch (Exception)
            {

                throw;
            }
           


            return View(model);
     
        }

        /// <summary>
        /// 彩种
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        public PartialViewResult GetLottery(int ltype)
        {
            try
            {
                string strsql = string.Format("select * from Lottery where lType={0} and IsHot=0", ltype);
                List<C8.Lottery.Model.Lottery> list = Util.ReaderToList<C8.Lottery.Model.Lottery>(strsql);
              
                ViewBag.ltype = ltype;
                ViewBag.LotteryList = list;
     

            }
            catch (Exception)
            {

                throw;
            }
 

            return PartialView("GetLottery");
        }
        
        /// <summary>
        /// 玩法
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        public PartialViewResult GetIntegralRule(int ltype,int ishot)
        {
            
            try
            {
                string strsql = string.Format("select * from IntegralRule where lType={0}", ltype);
                List<IntegralRule> list = Util.ReaderToList<IntegralRule>(strsql);
                ViewBag.ltype = ltype;
                ViewBag.ishot = ishot;
                ViewBag.IntegralRuleList = list;
            }
            catch (Exception)
            {

                throw;
            }
            return PartialView("GetIntegralRule");

        }


        /// <summary>
        /// 获取成绩
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public JsonResult GetMyBet(int lType,string PlayName, int pageIndex = 1, int pageSize = 20)
        {
            string strsql = string.Empty;
            string numsql = string.Empty;
            string countsql = string.Empty;
            var result = new AjaxResult<PagedList<AchievementModel>>();
            int UserId = UserHelper.GetByUserId();
          
            var pager = new PagedList<AchievementModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            if (PlayName == "全部")//全部
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}", UserId, lType);
                numsql =string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1}  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {2} AND {3}  ", UserId,lType,pager.PageIndex,pager.PageSize);
                countsql = string.Format(@"	  select count(distinct Issue)from BettingRecord  
	     where UserId={0} and lType={1} and WinState in(3,4)",UserId,lType);
            }
            else
            {
                strsql = string.Format(@"
                select * from BettingRecord   where UserId ={0} and lType = {1}  and PlayName = '{2}'", UserId, lType, PlayName);
                numsql =string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1} and b.PlayName='{2}'  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {3} AND {4} ", UserId,lType,PlayName, pager.PageIndex, pager.PageSize);
              countsql = string.Format(@"select count(distinct Issue)from BettingRecord  
	     where UserId={0} and lType={1}   and PlayName='{2}' and WinState in(3,4)", UserId, lType,PlayName);

            }

            try
            {
                List<LotteryNum> listnum = Util.ReaderToList<LotteryNum>(numsql);//我对应的开奖数据
                List<BettingRecord> listbet = Util.ReaderToList<BettingRecord>(strsql);
                List<AchievementModel> list = new List<AchievementModel>();
                if (listnum.Count > 0)
                {
                    foreach (var item in listnum)
                    {
                        AchievementModel model = new AchievementModel();
                        LotteryNum l = new LotteryNum();
                        l.Issue = item.Issue;
                        l.Num = item.Num;
                        l.SubTime =Convert.ToDateTime(item.SubTime).ToString("yyyy-MM-dd");
                        model.LotteryNum = l;
                        if (listbet.Count() > 0)
                            model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                        list.Add(model);

                    }
                }
                pager.PageData = list;

                object obj = SqlHelper.ExecuteScalar(countsql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

                result.Data = pager;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                throw;
            }
          

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取采种
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLottery1(int ltype)
        {
            string strsql = string.Empty;
            ReturnMessageJson msg = new ReturnMessageJson();
            if (ltype == 0)//热门
            {
                strsql = "select * from Lottery where IsHot=1";
            }
            else
            {
                strsql =string.Format("select * from Lottery where lType={0} and IsHot=0",ltype);
            }
            try
            {
                List<C8.Lottery.Model.Lottery> list = Util.ReaderToList<C8.Lottery.Model.Lottery>(strsql);
                msg.data = list;
                msg.Success = true;
            }
            catch (Exception ex)
            {
             
                msg.Success = false;
                msg.Msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


    }
}


