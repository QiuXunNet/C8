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

            int userId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(userId);

            //查询 未读消息提醒数量
            string noticeCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and UserId=" + userId;
            object noticeCount = SqlHelper.ExecuteScalar(noticeCountSql);
            ViewBag.NoticeCount = noticeCount;

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
                    if (!string.IsNullOrEmpty(newpwd))
                    {
                        if (newpwd.Length < 6 || newpwd.Length > 12)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "密码长度为6-12位";
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
                        jsonmsg.Msg = "密码不能为空";
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

                string namesql = "select count(1) from UserInfo where Name=@value";

                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@value",value),
                new SqlParameter("@Mobile",user.Mobile)

                };
                if (type == 1)
                {
                    int count = Convert.ToInt32(SqlHelper.ExecuteScalar(namesql, sp));
                    if (count > 0)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "该昵称已存在";
                        return Json(jsonmsg);
                    }
                    else
                    {
                        if (value.Trim().Length > 20)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "签名长度不能超过20";
                            return Json(jsonmsg);
                        }
                        else
                        {
                            bool iscz = Tool.CheckSensitiveWords(value);
                            if (iscz == true)
                            {
                                jsonmsg.Success = false;
                                jsonmsg.Msg = "该昵称包含敏感字符";
                                return Json(jsonmsg);
                            }
                        }

                    }
                }
                else if (type == 2)
                {

                    bool iscz = Tool.CheckSensitiveWords(value);
                    if (iscz == true)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "个性签名包含敏感字符";
                        return Json(jsonmsg);
                    }
                }

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
            string strsql = "select * from MakeMoneyTask where State=1";
            List<MakeMoneyTask> tasklist = Util.ReaderToList<MakeMoneyTask>(strsql);
            foreach (var item in tasklist)
            {
                TaskModel tm = new TaskModel();
                tm.Code = item.Code;
                tm.Id = item.Id;
                tm.TaskItem = item.TaskItem;
                tm.Coin = item.Coin;
                tm.Count = item.Count;
                tm.SubTime = item.SubTime;
                tm.CompletedCount = GetCompletedCount(item.Code);
                list.Add(tm);
            }

            var model = list;

            return View(model);
        }



        /// <summary>
        /// 赚钱任务规则
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskRule()
        {
            ViewBag.Platform = Request.Params["pl"].ToInt32();
            return View();
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
                if (UserId == followed_userId)
                {
                    jsonmsg.Msg = "自己不能关注自己";
                    jsonmsg.Success = false;
                }
                else
                {
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
 inner join UserInfo b on b.Id = a.UserId
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
            string strsql = @" select Number,Coin,Coupon from
                              (select count(1) as Number from UserInfo where Pid = @Pid) t1,
                              (select sum([Money])as Coin from ComeOutRecord  where [UserId]=@UserId and [Type]=7) t2,
							  (select count(1)as Coupon  from UserCoupon where UserId=@UserId and FromType=2) t3";

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
            ViewBag.Platform = Request.Params["pl"].ToInt32();
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
        /// 粉丝榜数据 只取前100条
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult FansBangList(int typeId, string type, int pageIndex = 1, int pageSize = 20)
        {

            try
            {




                string strsql = string.Format(@"select  * from ( select top 100 row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=@ResourceType)
where  f.Status=1  {0}
 group by Followed_UserId,Name,RPath
)t
WHERE Rank BETWEEN @Start AND @End", Tool.GetTimeWhere("FollowTime", type));


                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
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

        [HttpGet]
        public JsonResult MyRank(string type)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            int userId = UserHelper.GetByUserId();


            string strsql = string.Format(@" select  * from ( select top 100 row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name,isnull(RPath,'/images/default_avater.png') as HeadPath
 from Follow f 
 left join UserInfo u on f.Followed_UserId=u.id
 left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=@ResourceType)
where  f.Status=1  {0}
 group by Followed_UserId,Name,RPath
)t
where t.Followed_UserId=@Followed_UserId", Tool.GetTimeWhere("FollowTime", type));

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Followed_UserId",userId)
            };
            try
            {

                FansBangListModel fansbang = Util.ReaderToModel<FansBangListModel>(strsql, sp);
                if (fansbang == null)
                {
                    FansBangListModel fansbangs = new FansBangListModel();
                    string countsql = string.Format("select count(1) from Follow where Followed_UserId={0}  {1} and Status=1 ", userId, Tool.GetTimeWhere("FollowTime", type));
                    int number = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));

                    UserInfo user = UserHelper.GetUser(userId);
                    fansbangs.Followed_UserId = Convert.ToInt32(user.Id);
                    fansbangs.Name = user.Name;
                    fansbangs.Rank = 0;
                    fansbangs.Number = number;
                    fansbangs.HeadPath = user.Headpath;
                    fansbang = fansbangs;
                }

                jsonmsg.Success = true;
                jsonmsg.data = fansbang;

            }
            catch (Exception e)
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = e.Message;
                throw;
            }
            return Json(jsonmsg, JsonRequestBehavior.AllowGet);

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

            bool showFollow = false;

            ViewBag.Followed = false;
            #region 添加访问记录

            if (loginUserId != id)
            {
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
                    new SqlParameter("@UserId", loginUserId),
                    new SqlParameter("@RespondentsUserId", id),
                    new SqlParameter("@Module", 1) //默认访问主页
                };

                SqlHelper.ExecuteNonQuery(visitSql, sqlParameter);

                //查询是否存在当前用户对受访人的已关注记录 Status=1:已关注
                string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + loginUserId +
                             " and [Followed_UserId]=" + id;

                object obj = SqlHelper.ExecuteScalar(sql);

                ViewBag.Followed = obj != null && Convert.ToInt32(obj) > 0;


                showFollow = true;

            }
            ViewBag.ShowFollow = showFollow;

            #endregion



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
	SELECT row_number() over(order by WinState,SubTime DESC,lType) as rowNumber,* FROM (
		SELECT distinct lType,Issue, (case WinState when 1 then 1 else 2 end) as WinState,SubTime FROM [dbo].[BettingRecord] a
		WHERE SubTime=(select max(SubTime) from [BettingRecord] b where a.lType=b.lType and a.Issue=b.Issue  and a.UserId=b.UserId) and UserId=@UserId{0}{1}
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


                    var info = GetLotteryTypeName(x.Type, x.ArticleId, x.ArticleUserId, x.RefCommentId);

                    x.LotteryTypeName = info == null ? "" : info.TypeName;
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
	  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=@ResourceType
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
                x.TypeName = Util.GetLotteryTypeName((int)x.Id);
            });
            return View(list);
        }

        /// <summary>
        /// 通知消息
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice()
        {
            long userId = UserHelper.LoginUser.Id;
            ViewBag.UserId = userId;

            //查询动态消息未读数量
            string unreadDynamicCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and Type=2 and UserId=" + userId;
            object dynamicCount = SqlHelper.ExecuteScalar(unreadDynamicCountSql);
            ViewBag.DynamicCount = dynamicCount;

            //查询系统消息未读数量
            string unreadSysMessageCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and Type=1 and UserId=" + userId;
            object sysMessageCount = SqlHelper.ExecuteScalar(unreadSysMessageCountSql);
            ViewBag.SysMessageCount = sysMessageCount;
            return View();
        }

        /// <summary>
        /// 获取评论提醒
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCommentNotice(int uid, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<DynamicMessage>>();
            try
            {
                var pager = new PagedList<DynamicMessage>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;

                #region 分页查询动态消息
                string sql = @"SELECT * FROM (
    select row_number() over(order by m.SubTime DESC ) as rowNumber,m.*,a.PId,a.Content,
	a.[Type] as CommentType,a.ArticleId,a.ArticleUserId,a.RefCommentId,
	a.UserId as FromUserId,b.Name as FromNickName,c.RPath as FromAvater 
	from UserInternalMessage m
	left join Comment a on a.Id=m.RefId
	left join UserInfo b on b.Id = a.UserId
	left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
	where m.IsDeleted=0 and m.UserId=@UserId and m.Type=2
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

                pager.PageData = Util.ReaderToList<DynamicMessage>(sql, sqlParameters);
                #endregion

                #region 其他处理
                List<long> unreadList = new List<long>();
                pager.PageData.ForEach(x =>
                {
                    if (x.PId > 0)
                    {
                        var comment = GetComment(x.PId);
                        if (comment == null)
                        {
                            x.MyContent = "已删除";
                        }
                        else
                        {
                            x.MyContent = comment.Content;
                        }

                    }
                    var info = GetLotteryTypeName(x.Type, x.ArticleId, x.ArticleUserId, x.RefCommentId);
                    x.LotteryTypeName = info.TypeName ?? "";
                    x.RefNickName = info.NickName;

                    if (string.IsNullOrEmpty(x.FromAvater))
                    {
                        x.FromAvater = "/images/default_avater.png";
                    }

                    if (x.IsRead == false)
                    {
                        unreadList.Add(x.Id);
                    }
                });
                #endregion

                #region 处理未读消息

                if (unreadList.Count > 0)
                {
                    string executeSql = string.Format(@"
update dbo.UserInternalMessage 
set IsRead=1,ReadTime=GETDATE()
where UserId={0} and Id in({1});
select count(1) from UserInternalMessage 
where [Type]=1 and IsDeleted=0 and IsRead=0 and UserId={0}"
   , uid, string.Join(",", unreadList));

                    object objUnreadCount = SqlHelper.ExecuteScalar(executeSql);
                    pager.ExtraData = new { Unread = objUnreadCount.ToInt32() };
                }
                else
                {
                    pager.ExtraData = new { Unread = 0 };
                }

                #endregion

                #region 查询动态总条数
                string countSql = "SELECT count(1) FROM Comment WHERE IsDeleted=0 AND UserId=" + uid;
                object obj = SqlHelper.ExecuteScalar(countSql);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);
                #endregion

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
        /// 获取系统消息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSysMessage(int uid, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<SystemMessage>>();
            try
            {
                var pager = new PagedList<SystemMessage>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string sql = @"SELECT * FROM (
	select row_number() over(order by a.SubTime DESC ) as rowNumber,
	a.*,b.Title,b.Content,b.Link from UserInternalMessage a
	left join InternalMessage b on b.Id=a.RefId and a.[Type]=1
	where a.[Type]=1 and a.IsDeleted=0  and a.UserId=@UserId
) tt
WHERE rowNumber BETWEEN @Start AND @End";

                if (uid == 0)
                    uid = UserHelper.GetByUserId();

                var sqlParameters = new[]
                {
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)
                };

                pager.PageData = Util.ReaderToList<SystemMessage>(sql, sqlParameters);

                #region 未读消息处理
                if (pager.PageData.Any(x => x.IsRead == false))
                {
                    //若存在未读消息，更新未读消息状态为已读
                    var idList = pager.PageData.Where(x => x.IsRead == false).Select(x => x.Id);
                    string executeSql = string.Format(@"
update dbo.UserInternalMessage 
set IsRead=1,ReadTime=GETDATE()
where UserId={0} and Id in({1});
select count(1) from UserInternalMessage 
where [Type]=1 and IsDeleted=0 and IsRead=0 and UserId={0}"
, uid, string.Join(",", idList));

                    object objUnreadCount = SqlHelper.ExecuteScalar(executeSql);
                    pager.ExtraData = new { Unread = objUnreadCount.ToInt32() };

                }
                else
                {
                    pager.ExtraData = new { Unread = 0 };
                }
                #endregion

                string countSql = "select count(1) from UserInternalMessage where [Type]=1 and IsDeleted=0 and UserId=" + uid;
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
        /// <param name="id">彩种Id（上级评论Id）/文章 Id</param>
        /// <param name="articleUserId">发表计划用户Id type=1时</param>
        /// <param name="refCommentId">相关一级评论Id</param>
        /// <returns></returns>
        private DynamicRelatedInfo GetLotteryTypeName(int type, int id, int articleUserId, int refCommentId)
        {
            DynamicRelatedInfo info = null;
            if (type == 1)
            {

                info = new DynamicRelatedInfo();
                if (refCommentId <= 0)
                {
                    //id为彩种Id
                    info.LType = id;
                    info.TypeName = Util.GetLotteryTypeName(id);

                    var user = UserHelper.GetUser(articleUserId);
                    if (user != null)
                    {
                        info.NickName = user.Name;
                    }
                }
                else
                {
                    var comment = Util.GetEntityById<Comment>(refCommentId);
                    if (comment != null)
                    {
                        info.LType = comment.ArticleUserId;
                        info.TypeName = Util.GetLotteryTypeName(comment.ArticleUserId);

                        var user = UserHelper.GetUser(comment.ArticleUserId);
                        if (user != null)
                        {
                            info.NickName = user.Name;
                        }
                    }
                }

            }
            else
            {
                //查询文章的彩种类型
                string sql = @"select b.Id,d.TypeName, 2 as RelatedType from  news b 
left join NewsType c on c.Id=b.TypeId
left join LotteryType d on d.Id= c.lType
where b.Id=" + id;
                var list = Util.ReaderToList<DynamicRelatedInfo>(sql);
                if (list.Any())
                    info = list.First();
            }

            return info ?? new DynamicRelatedInfo();
        }


        /// <summary>
        /// 参与竞猜
        /// </summary>
        /// <returns></returns>
        public ActionResult TakeBet()
        {
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            List<LotteryType2> list = Util.ReaderToList<LotteryType2>(strsql);

            return View(list);
        }
        /// <summary>
        /// 获取竞猜数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBet(int PId = 0, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<BetModel>>();
            int userId = UserHelper.GetByUserId();
            try
            {
                List<BetModel> list = new List<BetModel>();
                var pager = new PagedList<BetModel>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string strsql = @"SELECT * FROM(
      select row_number() over(order by Position) as rowNumber,
     (select isnull(sum(Score), '0')  from BettingRecord where[UserId] =@UserId
and lType = l.lType) as Score,* from LotteryType2 l
   
      where PId = @PId 
)t
WHERE rowNumber BETWEEN @Start AND @End";
                string countsql = @"select count(1) from LotteryType2 where PId=@PId ";
                SqlParameter[] sp = new SqlParameter[]
                {
                        new SqlParameter("@PId",PId),
                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@Start",  pager.StartIndex ),
                        new SqlParameter("@End", pager.EndIndex)

                 };

                pager.PageData = Util.ReaderToList<BetModel>(strsql, sp);

                object obj = SqlHelper.ExecuteScalar(countsql, sp);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

                pager.PageData.ForEach(x =>
                {
                    x.LotteryIcon = Util.GetLotteryIcon(x.lType);
                });
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
            ACHVModel model = new ACHVModel();
            try
            {
                string lotterytypesql = @"select * from LotteryType2 where PId=0  order by Position ";
                List<LotteryType2> LotteryTypelist = Util.ReaderToList<LotteryType2>(lotterytypesql);//频道

                string lotterysql = @"select * from [dbo].[LotteryType2] where PId<>0  order by Position,PId ";
                List<LotteryType2> Lotterylist = Util.ReaderToList<LotteryType2>(lotterysql);//采种

                string IntegralRulesql = @"select * from IntegralRule";
                List<IntegralRule> IntegralRuleList = Util.ReaderToList<IntegralRule>(IntegralRulesql);//玩法
                model.LotteryType = LotteryTypelist;
                model.Lottery = Lotterylist;
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
                string strsql = string.Format("select * from LotteryType2 where PId={0} ", ltype);
                List<C8.Lottery.Model.LotteryType2> list = Util.ReaderToList<C8.Lottery.Model.LotteryType2>(strsql);

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
        public PartialViewResult GetIntegralRule(int ltype, int Pid)
        {

            try
            {
                string strsql = string.Format("select * from IntegralRule where lType={0}", ltype);
                List<IntegralRule> list = Util.ReaderToList<IntegralRule>(strsql);
                ViewBag.ltype = ltype;
                ViewBag.Pid = Pid;
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
        [HttpGet]
        public JsonResult GetMyBet(int lType, string PlayName, int pageIndex = 1, int pageSize = 20)
        {
            string strsql = string.Empty;
            string numsql = string.Empty;
            string countsql = string.Empty;
            var result = new AjaxResult<PagedList<AchievementModel>>();
            int UserId = UserHelper.GetByUserId();

            var pager = new PagedList<AchievementModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            SqlParameter[] sp = new SqlParameter[] { };
            if (PlayName == "全部")//全部
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}", UserId, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber, Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1}  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {2} AND {3}  ", UserId, lType, pager.StartIndex, pager.EndIndex);
                countsql = string.Format(@"	  select count(distinct Issue)from BettingRecord  
	     where UserId={0} and lType={1} and WinState in(3,4)", UserId, lType);
            }
            else
            {
                strsql = string.Format(@"
                select * from BettingRecord   where UserId ={0} and lType = {1}  and PlayName = @PlayName", UserId, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1} and b.PlayName=@PlayName  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {2} AND {3} ", UserId, lType, pager.StartIndex, pager.EndIndex);
                countsql = string.Format(@"select count(distinct Issue)from BettingRecord  
	     where UserId={0} and lType={1}   and PlayName=@PlayName and WinState in(3,4)", UserId, lType);

                sp = new SqlParameter[]{
                    new SqlParameter("@PlayName",PlayName)
                };

            }

            try
            {
                List<LotteryNum> listnum = Util.ReaderToList<LotteryNum>(numsql, sp);//我对应的开奖数据
                List<BettingRecord> listbet = Util.ReaderToList<BettingRecord>(strsql, sp);
                List<AchievementModel> list = new List<AchievementModel>();
                if (listnum.Count > 0)
                {
                    foreach (var item in listnum)
                    {
                        AchievementModel model = new AchievementModel();
                        LotteryNum l = new LotteryNum();
                        l.Issue = item.Issue;
                        l.Num = item.Num;
                        l.SubTime = Convert.ToDateTime(item.SubTime).ToString("yyyy-MM-dd");
                        model.LotteryNum = l;
                        if (listbet.Count() > 0)
                            model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                        list.Add(model);

                    }
                }
                pager.PageData = list;

                object obj = SqlHelper.ExecuteScalar(countsql, sp);
                pager.TotalCount = Convert.ToInt32(obj ?? 0);

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
                strsql = string.Format("select * from Lottery where lType={0} and IsHot=0", ltype);
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



        /// <summary>
        /// 交易记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TransactionRecord()
        {
            return View();
        }
        /// <summary>
        /// 交易记录数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult RecordList(int Type, int pageIndex, int pageSize)
        {
            var result = new AjaxResult<PagedList<ComeOutRecordModel>>();
            try
            {

                var pager = new PagedList<ComeOutRecordModel>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
                string strstate = "";
                int UserId = UserHelper.GetByUserId();

                string strsql = "";

                if (Type == 1)//充值
                {
                    strstate = "1";

                    strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
 where UserId =@UserId and Type in(" + strstate + @")  and State=3
 )t
 where   rowNumber BETWEEN  @Start AND @End  
   order by SubTime desc";
                }
                else if (Type == 2)//消费
                {
                    strstate = "3,5";

                    strsql = @"select * from ( select row_number() over (order by c.Id) as rowNumber,b.UserId as BUserId,b.Issue,b.lType,u.Name as UserName,c.* from ComeOutRecord c
inner join BettingRecord b
inner join UserInfo u
on b.UserId=u.Id
on c.OrderId=b.Id
 where c.UserId=@UserId and c.Type in(" + strstate + @")
 )t
 where   rowNumber BETWEEN @Start AND @End
    order by SubTime desc";

                }
                else if (Type == 3)//赚钱 只看任务奖励
                {
                    //strstate = "4,6,7,8,9 ";
                    strstate = "8";
                    //                    strsql = @"select * from (
                    //select row_number() over (order by Id) as rowNumber,* from (

                    //select  Id, UserId, OrderId, Type, Money, State, SubTime, PayType from ComeOutRecord a
                    //where UserId =@UserId and Type in(6,7,8)
                    //UNION 
                    //select c.Id, c.UserId, OrderId, Type, Money, State, c.SubTime, PayType from ComeOutRecord c 
                    //left join BettingRecord b on b.Id=c.OrderId 
                    //where b.UserId=@UserId and Type in(4,9)
                    //)t
                    //)t1
                    // where   t1.rowNumber BETWEEN  @Start AND @End
                    //     order by SubTime desc  ";

                    strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
 where UserId = @UserId and Type in(" + strstate + @")  
 )t
 where   rowNumber BETWEEN  @Start AND @End
   order by SubTime desc";


                }

                string countsql = @" select count(1) from ComeOutRecord where UserId=@UserId and Type in(" + strstate + @")";

                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)


                };
                List<ComeOutRecordModel> list = Util.ReaderToList<ComeOutRecordModel>(strsql, sp);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql, sp));
                if (list != null)
                {
                    if (Type == 1)
                    {
                        list.ForEach(x =>
                        {

                            x.LotteryIcon = Tool.GetPayImg(Convert.ToInt32(x.PayType));
                        });

                    }
                    else if (Type == 2)
                    {
                        list.ForEach(x =>
                        {

                            x.LotteryIcon = "/images/" + Util.GetLotteryIcon(x.lType) + ".png";
                        });
                    }
                    else if (Type == 3)
                    {
                        list.ForEach(x =>
                        {

                            x.LotteryIcon = Tool.GetZqImg(Convert.ToInt32(x.OrderId));
                        });
                    }

                }
                pager.PageData = list;
                pager.TotalCount = count;

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
        /// 我的佣金
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCommission()
        {

            try
            {
                int UserId = UserHelper.GetByUserId();
                string strsql = @"select MyYj,Txing,Txleiji,XfYj,KeTx from 
 (select isnull(sum([Money]),0)as MyYj from ComeOutRecord c inner join BettingRecord b on c.OrderId=b.Id  where  b.[UserId]=@UserId and Type in(4,9))t1,
 (select isnull(sum([Money]),0)as Txing from ComeOutRecord where  [UserId]=@UserId and Type=2 and State=1)t2,
 (select isnull(sum([Money]),0)as Txleiji  from ComeOutRecord where  [UserId]=@UserId and Type=2 and State=3 )t3,
 (select isnull(sum([Money]),0)as XfYj  from ComeOutRecord where  [UserId]=@UserId and Type in(3,5))t4,
 (select isnull([Money],0)as KeTx  from UserInfo  where  [Id]=@UserId )t5";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",UserId)
                };
                DrawMoneyModel dr = Util.ReaderToModel<DrawMoneyModel>(strsql, sp);
                ViewBag.MyYj = Tool.Rmoney(dr.KeTx + dr.Txing);
                ViewBag.Txing = Tool.Rmoney(dr.Txing);
                ViewBag.Txleiji = Tool.Rmoney(dr.Txleiji);
                ViewBag.KeTx = Tool.Rmoney(dr.KeTx);

            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        /// <summary>
        /// 我的佣金数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult MyCommissionList(int Type, int pageIndex, int pageSize)
        {
            var result = new AjaxResult<PagedList<ComeOutRecordModel>>();
            try
            {

                var pager = new PagedList<ComeOutRecordModel>();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;

                int UserId = UserHelper.GetByUserId();

                string strsql = "";
                string strstate = "";
                if (Type == 1)//收入明细
                {
                    strsql = @"select * from (  select row_number() over (order by c.Id) as rowNumber,
 c.UserId as BUserId,b.Issue,b.lType,u.Name as UserName, OrderId, Type, c.Money, c.State, c.SubTime, PayType 
 from ComeOutRecord c
inner join BettingRecord b on c.OrderId=b.Id
inner join UserInfo u on  c.UserId=u.Id
 where b.UserId=@UserId and c.Type in(4,9)

 )t
 where   rowNumber BETWEEN @Start AND @End
       order by SubTime desc ";
                    strstate = "4,9";

                }
                else if (Type == 2)//提现明细
                {
                    strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
 where UserId =@UserId and Type=2 

 )t
 where   rowNumber BETWEEN  @Start AND @End order by SubTime desc";
                    strstate = "2";
                }
                string countsql = @" select count(1) from ComeOutRecord where UserId=@UserId and Type in(" + strstate + @")";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)


                };

                List<ComeOutRecordModel> list = Util.ReaderToList<ComeOutRecordModel>(strsql, sp);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql, sp));
                if (list != null)
                {
                    if (Type == 2)
                    {
                        list.ForEach(x =>
                        {

                            x.LotteryIcon = "/images/41.png";
                        });
                    }
                    else if (Type == 1)
                    {
                        list.ForEach(x =>
                        {

                            x.LotteryIcon = "/images/" + Util.GetLotteryIcon(x.lType) + ".png";
                        });
                    }
                }
                pager.PageData = list;
                pager.TotalCount = count;

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
        /// 佣金规则
        /// </summary>
        /// <returns></returns>
        public ActionResult CommissionRules()
        {
            ViewBag.Platform = Request.Params["pl"].ToInt32();
            return View();
        }

        /// <summary>
        /// 卡劵
        /// </summary>
        /// <returns></returns>
        public ActionResult Voucher()
        {

            return View();
        }
        /// <summary>
        /// 卡劵数据
        /// </summary>
        /// <param name="type">0 未使用 1已使用 2已过期</param>
        /// <returns></returns>
        public JsonResult VoucherList(int type)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            int UserId = UserHelper.GetByUserId();

            string strwhere = string.Format(" UserId={0}", UserId);
            switch (type)
            {
                case 0:
                    strwhere += " and State=1 and getdate()<EndTime";
                    break;

                case 1:
                    strwhere += " and State=2";
                    break;

                case 2:
                    strwhere += " and getdate()>EndTime";
                    break;
            }
            string strsql = string.Format("select * from UserCoupon where {0} ", strwhere);
            try
            {
                List<UserCoupon> list = Util.ReaderToList<UserCoupon>(strsql);
                msg.Success = true;
                msg.data = list;
            }
            catch (Exception e)
            {
                msg.Success = false;
                msg.Msg = e.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 卡劵规则
        /// </summary>
        /// <returns></returns>
        public ActionResult VoucherRules()
        {
            ViewBag.Platform = Request.Params["pl"].ToInt32();
            return View();
        }
        /// <summary>
        /// 我的积分
        /// </summary>
        /// <returns></returns>
        public ActionResult MyIntegral()
        {
            UserInfo user = UserHelper.GetUser();
            ViewBag.Integral = user.Integral;
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            List<LotteryType2> list = Util.ReaderToList<LotteryType2>(strsql);
            ViewBag.LotteryType2List = list;
            return View();
        }

        /// <summary>
        /// 获取我的积分数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMyIntegral(int PId)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            int userId = UserHelper.GetByUserId();
            try
            {
                List<BetModel> list = new List<BetModel>();
          
                string strsql = @"  select 
     (select isnull(sum(Score), '0')  from BettingRecord where[UserId] =@UserId
     and lType = l.lType) as Score,* from LotteryType2 l
     where PId = @PId";
            
                SqlParameter[] sp = new SqlParameter[]
                {
                        new SqlParameter("@PId",PId),
                        new SqlParameter("@UserId",userId),
                    

                 };

                list = Util.ReaderToList<BetModel>(strsql, sp);
                list.ForEach(x =>
                {
                    x.LotteryIcon = Util.GetLotteryIcon(x.lType);
                });
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


