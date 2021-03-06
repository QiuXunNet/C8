﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
using System.Data.SqlClient;
using C8.Lottery.Portal;
using CryptSharp;
using C8.Lottery.Model.Enum;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace C8.Lottery.Portal.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
        
            ViewBag.UserInfo = UserHelper.LoginUser;
            ViewBag.SiteSetting = GetSiteSetting();

            ViewBag.NewsList = GetNewList();
            return View();
        }

        /// <summary>
        /// 获取父彩种（大彩种）
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFatherLotteryType()
        {
            string cachekey = RedisKeyConst.Home_FatherLotteryType; //"home:father_lottery_type:website";
            List<LotteryType2> list = CacheHelper.GetCache<List<LotteryType2>>(cachekey);
            if (list == null)
            {
                string sql = "select * from LotteryType2 where PId = 0 order by Position";
                list = Util.ReaderToList<LotteryType2>(sql);

                CacheHelper.AddCache<List<LotteryType2>>(cachekey, list, 30 * 24 * 60);
            }

            return Json(list);
        }

        /// <summary>
        /// 获取首页新闻
        /// </summary>
        /// <returns></returns>
        public List<News> GetNewList()
        {
            string cachekey = RedisKeyConst.Home_NewsList;//"home:news_list:website";
            List<News> list = CacheHelper.GetCache<List<News>>(cachekey);
            if (list == null)
            {

                //string sql = @"
                //   select top(5) * from (
                //     select n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle,
                //     count(c.id) as CommentCount,
                //     STUFF(( SELECT '!'+ [RPath] 
                //     FROM ResourceMapping rm 
                //     WHERE rm.fkid = n.id and rm.Type = 1
                //     FOR XML PATH('')),1 ,1, '') ThumbListStr
                //     from News n 
                //     left join Comment c on n.id = c.ArticleId and RefCommentId = 0
                //     where n.DeleteMark=0 and n.EnabledMark = 1
                //     group by n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle
                //    ) as tab order by CommentCount desc
                //    ";
                string sql = @"select n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle ,
                    count(c.id) as CommentCount
                    from News n
                    left join Comment c on n.id = c.ArticleId and RefCommentId = 0
                    where n.id in(
                    select max(n.Id) from News n 
                    join newsType nt on n.TypeId = nt.Id
                    where nt.lType in (1,2,3,4,6) and nt.SortCode = 1 and n.DeleteMark=0 and n.EnabledMark = 1
                    group by nt.lType
                    )
                    group by n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle";

                list = Util.ReaderToList<News>(sql);

                //新闻缓存2小时
                CacheHelper.AddCache<List<News>>(cachekey, list, 2 * 60);
            }

            return list;
        }

        /// <summary>
        /// 根据父彩种获取子彩种列表
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public string GetChildLotteryType(int pId)
        {
            string interfaceUrl = ConfigurationManager.AppSettings["InterfaceUrl"];
            var url = interfaceUrl + "/Common/GetIndexLotteryList?parentId=" + pId;
            var data = HttpCommon.HttpGet(url);

            //string sql = "";

            //List<LotteryType2> list = CacheHelper.GetCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pId);
            //if (list == null)
            //{
            //    sql = "select * from LotteryType2 where PId = " + pId + " order by Position";

            //    list = Util.ReaderToList<LotteryType2>(sql);

            //    CacheHelper.AddCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pId, list, 30 * 24 * 60);
            //}

            //string lTypes = "";
            //list.ForEach(e => lTypes += e.lType + ",");
            //lTypes = lTypes.Trim(',');

            //var dateTime = DateTime.Now.AddDays(-5);

            //sql = @"select lr.* from LotteryRecord lr
            //        join(
            //        select lType, max(SubTime) SubTime from lotteryRecord where lType in(" + lTypes + ") and SubTime >'" + dateTime + @"' group by lType
            //        ) tab on lr.lType = tab.lType and lr.SubTime = tab.SubTime
            //        left join LotteryType2 lt on lr.lType = lt.lType
            //        order by Position";

            //var newList = new List<LotteryRecordToJson>();

            //Util.ReaderToList<LotteryRecord>(sql).ForEach(e=> {
            //    LotteryRecordToJson newModel = new LotteryRecordToJson();
            //    newModel.Id = e.Id;
            //    newModel.Issue = e.Issue;
            //    newModel.lType = e.lType;
            //    newModel.Num = e.Num;
            //    newModel.ShowIconName = e.ShowIconName;
            //    newModel.ShowOpenTime = e.ShowOpenTime;
            //    newModel.ShowTypeName = e.ShowTypeName;
            //    newList.Add(newModel);
            //});

            //return Json(newList);
            return data;
        }

        //object obj = new object();

        public ActionResult GetRemainOpenTime()
        {
            StringBuilder str = new StringBuilder("[");
            for (int i = 1; i <= 65; i++)
            {
                var time = LotteryTime.GetTime(i.ToString());
                string[] arr = time.Split('&');
                if (arr.Length == 3)
                {
                    time = "<t class='hour'>" + arr[0] + "</t>:<t class='minute'>" + arr[1] + "</t>:<t class='second'>" + arr[2] + "</t>";
                }

                str.Append("{\"lType\":\"" + i + "\",\"value\":\"" + time + "\"},");
            }
            string s = str.ToString().Trim(',');
            s += "]";
            return Content(s);
        }

        public ActionResult GetRemainOpenTimeByType(int lType)
        {
            string time = "";
            time = LotteryTime.GetTime(lType.ToString());
            string[] arr = time.Split('&');

            if (arr.Length == 3)
            {
                time = "<t class='hour'>" + arr[0] + "</t>:<t class='minute'>" + arr[1] + "</t>:<t class='second'>" + arr[2] + "</t>";
            }
            return Content(time);
        }

        public string GetRemainOpenTimeToLhc()
        {
            var returnTime = "";
            string time = LotteryTime.GetTime("5");
            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');
                returnTime = "<span id='openTime'><t id='hour2'>" + timeArr[0] + "</t>:<t id='minute2'>" + timeArr[1] + "</t>:<t id='second2'>" + timeArr[2] + "</t></span>";
            }
            else
            {
                returnTime = "<span id='openTime'>" + time + "</span>";
            }

            return returnTime;
        }

        /// <summary>
        /// app下载页
        /// </summary>
        /// <returns></returns>
        public ActionResult down()
        {
            return View();
        }
        /// <summary>
        /// 获取下载数据
        /// </summary>
        /// <param name="ClientSource"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDown(int ClientSource)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            string cachekey = string.Format(RedisKeyConst.Installationpackage_Sourceversion, ClientSource); //"installationpackage:sourceversion:" + ClientSource;
            List<ClientSourceVersion> list = CacheHelper.GetCache<List<ClientSourceVersion>>(cachekey);
            if (list == null)
            {
                string strsql = @"
                    select top 1 * from ClientSourceVersion where ClientSource =@ClientSource and ClientType =@ClientSource
                    order by VersionCode desc";
                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@ClientSource",ClientSource)
                };
                list = Util.ReaderToList<ClientSourceVersion>(strsql, sp);
                CacheHelper.AddCache<List<ClientSourceVersion>>(cachekey, list, 60 * 24);
            }
          
            try
            {
               
                msg.data = list;
                msg.Success = true;
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
        /// 获取验证码KCP
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetCode(string mobile)
        {
            try
            {
                bool result;
                //随机生成短信验证码    
                string code = SmsSender.GetVCode();
                // 短信验证码存入session(session的默认失效时间30分钟) 
                //Session.Add("code", code);
                //Session["CodeTime"] = DateTime.Now;//存入时间一起存到Session里
                // 短信内容+随机生成的6位短信验证码              
                // 单个手机号发送短信  去掉注释可开启短信功能
                int sendResult = SmsSender.SendMsgByTXY("", mobile, code);
                if (sendResult == 0)
                {
                    var sendLog = new SmsSendLog()
                    {
                        UserId = 0,
                        Type = 1,
                        Code = code,
                        Count = 1,
                        Receiver =mobile,
                        Sender = "",
                        SendPort = 0,
                        SendTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        Status = 1,
                        SendResult = sendResult
                    };
                    AddCode(sendLog);
                    result = true;// 成功    
                }
                else
                {
                    result = false;//失败    
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 插入验证码日志
        /// </summary>
        /// <param name="sendlog"></param>
        public void AddCode(SmsSendLog sendLog)
        {
           
          string sql = string.Format(@"INSERT INTO [dbo].[SmsSendLog]
          ([UserId],[Sender],[Receiver],[Type],[Code],[Status],[Count],[SendTime],[SendPort],[SendResult],[UpdateTime])
          VALUES({0},'{1}','{2}',{3},'{4}',{5},{6},GETDATE(),{7},{8},GETDATE())",
          sendLog.UserId, sendLog.Sender, sendLog.Receiver, sendLog.Type, sendLog.Code,
          sendLog.Status, sendLog.Status, sendLog.SendPort, sendLog.SendResult);
            try
            {
                SqlHelper.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {

                throw;
            }
           


        }
        /// <summary>
        /// 更新消息日志
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateMsgLog(SmsSendLog dto)
        {
            string sql = @"UPDATE dbo.SmsSendLog SET Count=@Count,[Status]=@Status,UpdateTime=GETDATE()
    WHERE Id=@Id";
            var paramters = new[]
             {
                new SqlParameter("@Count",dto.Count),
                new SqlParameter("@Status",dto.Status),
                new SqlParameter("@Id",dto.Id),
            };

            SqlHelper.ExecuteNonQuery(sql, paramters);
        }

        public string ValidateSmsCode(string Phone,int Type,string Code)
        {
            string msgcode = "200";
            //查询十分钟内未使用的验证码
            string sql = @"SELECT Id,UserId,Receiver,Type,Code,Count,[Status] FROM dbo.SmsSendLog
    WHERE [Status]=1 AND SendTime > @SendTime AND Receiver=@Receiver AND Type=@Type ORDER BY Id DESC";

            var sinceTime = DateTime.Now.AddMinutes(-10);
            var paramters = new[]
            {
                new SqlParameter("@SendTime",sinceTime),
                new SqlParameter("@Receiver",Phone),
                new SqlParameter("@Type",Type),
            };

            var list = Util.ReaderToList<SmsSendLog>(sql, paramters);

            var valid = list.Any(x => x.Code == Code);
            if (valid)
            {
                list.ForEach(x =>
                {
                    x.Count++;
                    x.Status = (!valid && x.Count < 5) ? 2 : 3;
                    UpdateMsgLog(x);
                });
            }
            

            if (!valid)
            {
                msgcode = "验证码错误，请重新输入";
            }
            return msgcode;

        }



        /// <summary>
        /// 注册 KCP  
        /// </summary>
        /// <returns></returns>

        public ActionResult Register(int? id)
        {

            ViewData["id"] = id == null ? 0 : id;

            ViewBag.SiteSetting = GetSiteSetting();

            return View();


        }


        [HttpPost]

        public ActionResult Create(FormCollection form)
        {



            int inviteid = Convert.ToInt32(form["inviteid"]);

            string mobile = form["mobile"];
            string password = form["password"];

            string vcode = form["vcode"];
            string name = form["name"];
            string usersql = "select * from UserInfo where Mobile =@Mobile";

            string namesql = "select count(1) from UserInfo where Name=@Name";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                List<UserInfo> list = new List<UserInfo>();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                SqlParameter[] namesp = new SqlParameter[] { new SqlParameter("@Name", name) };
                int namecount = Convert.ToInt32(SqlHelper.ExecuteScalar(namesql, namesp));
                list = Util.ReaderToList<UserInfo>(usersql, sp);

                if (list.Count > 0)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "该手机号已被注册";
                }
                else
                {
                    if (namecount > 0)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "该昵称已被注册";
                    }
                    else
                    {
                        bool iscz = Tool.CheckSensitiveWords(name);
                        if (iscz == true)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "该昵称包含敏感字符";
                        }
                        else
                        {
                            string msgcode = ValidateSmsCode(mobile, 1, vcode);

                            if (msgcode == "200")
                            {
                                password = Tool.GetMD5(password);
                                string ip = Tool.GetIP();
                                string regsql = @"
  insert into UserInfo(UserName, Name, Password, Mobile, Coin, Money, Integral, SubTime, LastLoginTime, State,Pid,RegisterIP)
  values(@UserName, @Name, @Password, @Mobile, 0,0, 0, getdate(), getdate(), 0,@Pid,@RegisterIP);select @@identity ";
                                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserName",mobile),
                     new SqlParameter("@Name",name),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Mobile",mobile),
                    new SqlParameter("@Pid",inviteid),
                    new SqlParameter("@RegisterIP",ip)

                 };
                                int data = Convert.ToInt32(SqlHelper.ExecuteScalar(regsql, regsp));
                                if (data > 0)
                                {
                                    #region 如果是外链过来的，则增加注册数
                                    var linkCode = Session["LinkCode"];
                                    if (linkCode != null)
                                    {
                                        var friendLinkList = Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and state = 0 and Code=@Code", new SqlParameter("@Code", linkCode.ToString()));
                                        var friendLink = friendLinkList.FirstOrDefault();

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
                                                        values({0},0,0,0,0,1,1,'{1}')", friendLink.Id, DateTime.Today);

                                            SqlHelper.ExecuteScalar(insertRecordSql);
                                        }
                                        else
                                        {
                                            //修改
                                            string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set RegCount+=1 where RefId={0} and SubTime = '{1}' and [Type]=1 ", friendLink.Id, DateTime.Today);
                                            SqlHelper.ExecuteScalar(updateRecordSql);
                                        }

                                    }
                                    #endregion

                                    jsonmsg.Success = true;
                                    jsonmsg.Msg = "ok";
                                    string guid = string.Format(RedisKeyConst.Login_LoginGuid, Guid.NewGuid().ToString());
                                    Response.Cookies["UserId"].Value = guid;
                                    Response.Cookies["UserId"].Expires = DateTime.Now.AddMonths(1);
                                    //CacheHelper.SetCache(guid, data, DateTime.Now.AddMonths(1));
                                    CacheHelper.AddCache(guid, data, 30 * 24 * 60);
                                    Coupon cou = GetCoupon("A0001");//查看劵
                                    DateTime BeginTime = DateTime.Now;
                                    DateTime EndTime = DateTime.Now.AddDays(cou.ExpiryDate);
                                    UserCoupon uc = new UserCoupon();
                                    uc.UserId = data;
                                    uc.CouponCode = "A0001";
                                    uc.PlanId = 0;
                                    uc.BeginTime = BeginTime;
                                    uc.EndTime = EndTime;
                                    uc.FromType = 1;
                                    uc.State = 1;
                                    AddUserCoupon(uc);
                                    if (inviteid > 0)
                                    {
                                        UserInfo invite = GetByid(inviteid);
                                        if (invite != null)
                                        {
                                            hufen(data, inviteid);//邀请注册默认互粉
                                            int mynum = GetNum(3);
                                            AddCoin(data, mynum);//受邀自己得3级奖励
                                                                 //AddCoinRecord(2, data, inviteid, mynum);//受邀得奖记录
                                            AddComeOutRecord(data, inviteid.ToString(), 6, mynum, 1);//受邀得奖记录
                                            int upnum = GetNum(1);
                                            AddCoin(Convert.ToInt32(invite.Id), upnum);//上级得奖
                                            UserCoupon uc1 = new UserCoupon();
                                            uc1.UserId = invite.Id;
                                            uc1.CouponCode = "A0001";
                                            uc1.PlanId = 0;
                                            uc1.BeginTime = BeginTime;
                                            uc1.EndTime = EndTime;
                                            uc1.FromType = 2;
                                            uc1.State = 1;
                                            AddUserCoupon(uc1);//上级得卡劵

                                            //AddCoinRecord(1, inviteid, data, upnum);//上级得奖记录
                                            AddComeOutRecord(inviteid, data.ToString(), 7, upnum, 1);//上级得奖记录
                                            AddUserTask(inviteid, 105);//上级完成邀请任务额外奖励
                                                                       //int CompletedCount = GetCompletedCount(105, inviteid);
                                                                       //MakeMoneyTask mt = GetMakeMoneyTaskCount(105);

                                            //if (CompletedCount == mt.Count)//上级完成邀请任务额外奖励
                                            //{
                                            //    AddComeOutRecord(inviteid, Convert.ToString(105), 8, mt.Coin, 1);
                                            //}



                                            UserInfo super = GetByid(Convert.ToInt32(invite.Pid));//上上级
                                            if (super != null)
                                            {
                                                int supernum = GetNum(2);
                                                AddCoin(Convert.ToInt32(super.Id), supernum);//上上级得奖
                                                AddComeOutRecord(Convert.ToInt32(super.Id), Convert.ToString(inviteid), 7, supernum, data);
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    jsonmsg.Success = false;
                                    jsonmsg.Msg = "fail";

                                }
                            }else
                            {
                                jsonmsg.Success = false;
                                jsonmsg.Msg = msgcode;
                            }                           
                        }
                    }


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
        /// 邀请注册互粉
        /// </summary>
        public void hufen(long userId, long followuid)
        {
            string strsql = @"insert into [dbo].[Follow]([UserId],[Followed_UserId],[FollowTime])
                              values(@UserId,@Followed_UserId,getdate());
                              insert into [dbo].[Follow]([UserId],[Followed_UserId],[FollowTime])
                              values(@Followed_UserId,@UserId,getdate());";
            try
            {
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Followed_UserId",followuid)
                };
                SqlHelper.ExecuteTransaction(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取任务所需完成数量
        /// </summary>
        /// <param name="taskcode"></param>
        /// <returns></returns>
        public MakeMoneyTask GetMakeMoneyTaskCount(int taskcode)
        {

            string strsql = "select * from[MakeMoneyTask] where Code =@Code";
            SqlParameter[] sp = new SqlParameter[] {

                   new SqlParameter("@Code",taskcode)
            };

            return Util.ReaderToModel<MakeMoneyTask>(strsql, sp);
        }

        /// <summary>
        /// 获取任务完成数量
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public int GetCompletedCount(int taskid, int UserId)
        {
            int result = 0;
            string strsql = "select CompletedCount from  UserTask where UserId =@UserId and TaskId=@TaskId ";
            SqlParameter[] sp = new SqlParameter[] {
                   new SqlParameter("@UserId",UserId),
                   new SqlParameter("@TaskId",taskid)
            };
            result = Convert.ToInt32(SqlHelper.ExecuteScalar(strsql, sp));

            return result;
        }

        /// <summary>
        /// 添加用户优惠券
        /// </summary>
        /// <param name="uc"></param>
        /// <returns></returns>
        public int AddUserCoupon(UserCoupon uc)
        {
            int data = 0;
            string strsql = @"insert into [UserCoupon](UserId, CouponCode, PlanId, BeginTime, EndTime, FromType, State, SubTime)
                              values(@UserId, @CouponCode, @PlanId, @BeginTime, @EndTime, @FromType, @State, getdate());";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",uc.UserId),
                new SqlParameter("@CouponCode",uc.CouponCode),
                new SqlParameter("@PlanId",uc.PlanId),
                new SqlParameter("@BeginTime",uc.BeginTime),
                new SqlParameter("@EndTime",uc.EndTime),
                new SqlParameter("@FromType",uc.FromType),
                new SqlParameter("@State",uc.State)
            };
            try
            {
                data = SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public Coupon GetCoupon(string Code)
        {
            string strsql = "select* from[dbo].[Coupon] where Code = @Code";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Code",Code)

            };
            return Util.ReaderToModel<Coupon>(strsql, sp);
        }



        /// <summary>
        /// 增加邀请成功注册任务记录
        /// </summary>
        public void AddUserTask(int UserId, int TaskCode)
        {
            string countsql = "select * from UserTask where UserId=@UserId and TaskId=@TaskId ";
            string strsqlup = "update UserTask set CompletedCount=CompletedCount+1 where UserId=@UserId and TaskId=@TaskId";
            string strsqlins = "insert into UserTask(UserId, TaskId, CompletedCount)values(@UserId, @TaskId, 1)";
            string strtasksql = "select * from MakeMoneyTask   where Code = 105";//邀请成功注册任务

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",UserId),
                new SqlParameter("@TaskId",TaskCode)

            };
            try
            {
                UserTask task = Util.ReaderToModel<UserTask>(countsql, sp);
                if (task != null)
                {
                    int update = SqlHelper.ExecuteNonQuery(strsqlup, sp);
                    if (update > 0)
                    {
                        UserTask task1 = Util.ReaderToModel<UserTask>(string.Format("select * from UserTask where UserId={0} and TaskId={1}", UserId, TaskCode));
                        MakeMoneyTask mtask = Util.ReaderToModel<MakeMoneyTask>(strtasksql);
                        if (task1.CompletedCount == mtask.Count)
                        {
                            string strsqlinstask = string.Format(@"insert into ComeOutRecord(UserId, OrderId, Type, Money, SubTime)
                                   values({0}, {1}, 8, {2}, getdate())", UserId, TaskCode, mtask.Coin);
                            SqlHelper.ExecuteNonQuery(strsqlinstask);
                            AddCoin(UserId, mtask.Coin);

                        }
                    }
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(strsqlins, sp);
                }
            }
            catch (Exception)
            {

                throw;
            }






        }


        /// <summary>
        /// 增加金币
        /// </summary>
        public void AddCoin(int id, int num)
        {
            try
            {
                string strsql = "update UserInfo set Coin=Coin+@num where Id =@Id";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@num", num),
                    new SqlParameter("@Id", id),
                };
                SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 邀请获得金币记录 type:1邀请者  2受邀者 (作废：已换表)
        /// </summary>
        public void AddCoinRecord(int type, int userId, int otherId, int amount)
        {
            try
            {
                string strsql = @"insert into CoinRecord(lType, UserId, OtherId, Type, Amount, SubTime)
                                  values(0, @UserId, @OtherId, @Type, @Amount, getdate()) ";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@OtherId", otherId),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@Amount", amount),
                };
                SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 邀请获得金币记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderId"></param>
        /// <param name="Type">6=受邀奖励 7=邀请奖励</param>
        /// <param name="Money"></param>
        public void AddComeOutRecord(int UserId, string OrderId, int Type, int Money, int State)
        {
            try
            {
                string strsql = @"insert into ComeOutRecord(UserId, OrderId, Type, Money,State, SubTime)
                                 values(@UserId, @OrderId, @Type, @Money, @State,getdate())";
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@OrderId",OrderId),
                    new SqlParameter("@Type",Type),
                    new SqlParameter("@Money",Money),
                    new SqlParameter("@State",State)
                };
                SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 随机抽取金币 根据金币等级
        /// </summary>
        /// <returns></returns>
        public int GetNum(int GradeId)
        {
            int Num = 0;

            List<int> listNum = new List<int>();
            try
            {
                string strsql = "select * from CoinRate where GradeId = @GradeId";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@GradeId", GradeId)
                };
                List<CoinRate> list = Util.ReaderToList<CoinRate>(strsql, sp);
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        for (int i = 0; i < item.Rate; i++)
                        {
                            listNum.Add(item.Num);
                        }

                    }
                }
                Random rm = new Random();
                int j = rm.Next(listNum.Count);
                Num = listNum[j];

            }
            catch (Exception)
            {

                throw;
            }
            return Num;
        }


        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public UserInfo GetByid(int id)
        {
            UserInfo user = new UserInfo();
            try
            {
                string usersql = "select * from UserInfo where Id =@Id";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Id", id) };
                user = Util.ReaderToModel<UserInfo>(usersql, sp);


            }
            catch (Exception)
            {

                throw;
            }
            return user;

        }





        /// <summary>
        /// 登录KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            ViewBag.SiteSetting = GetSiteSetting();
            return View();
        }
        [HttpPost]
        public ActionResult Logins(string mobile, string password)
        {
            //string usersql = "select * from UserInfo where Mobile =@Mobile";
            string usersql = @"select * from UserInfo where Mobile=@Mobile ";
            UserInfo user = new UserInfo();
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {


                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                List<UserInfo> list = Util.ReaderToList<UserInfo>(usersql, sp);
                if (list != null)
                {
                    user = list.FirstOrDefault(x => x.Mobile == mobile);
                }

                if (user != null)
                {
                    bool iscor = false;
                    if (user.Password.Contains("$2y"))
                    {
                        iscor = Crypter.CheckPassword(password, user.Password);
                    }

                    if (Tool.GetMD5(password) != user.Password && iscor == false)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "密码不正确";

                    }
                    else
                    {
                        if (user.State != 0)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "账号被禁、暂时无法登录";
                        }
                        else
                        {

                            string guid = string.Format(RedisKeyConst.Login_LoginGuid, Guid.NewGuid().ToString()); //"login:logon_guid:" + Guid.NewGuid().ToString();
                            string webdomain = ConfigurationManager.AppSettings["WebDomain"];
                            string debug = ConfigurationManager.AppSettings["debug"];
                            if (debug == "0")
                            {
                                Response.Cookies["UserId"].Domain = webdomain;
                            }
                            Response.Cookies["UserId"].Value = guid;

                            //Session[guid] = user.Id;
                            Response.Cookies["UserId"].Expires = DateTime.Now.AddMonths(1);
                            //MemClientFactory.WriteCache<string>(sessionId.ToString(), user.Id.ToString(), 30);
                            // CacheHelper.SetCache(guid, user.Id, DateTime.Now.AddMonths(1));
                            CacheHelper.AddCache(guid, user.Id, 30 * 24 * 60);


                            jsonmsg.Success = true;
                            jsonmsg.Msg = "ok";
                            string ip = Tool.GetIP();
                            string editsql = "update UserInfo set LastLoginTime=getdate(),LastLoginIP=@LastLoginIP where Mobile=@Mobile";//记录最后一次登录时间
                            SqlParameter[] editsp = new SqlParameter[] { new SqlParameter("@Mobile", mobile), new SqlParameter("@LastLoginIP", ip) };
                            SqlHelper.ExecuteNonQuery(editsql, editsp);

                        }

                    }
                }
                else
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "手机号不存在";

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
        /// 忘记密码 KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult Forget()
        {
            return View();

        }
        /// <summary>
        /// 验证 KCP
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ActionResult Validate(string mobile, string vcode)
        {
            string usersql = "select Count(1) from UserInfo where Mobile =@Mobile";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(usersql, sp));

                if (count <= 0)
                {
                    jsonmsg.Msg = "该手机未注册、请先注册账号";
                    jsonmsg.Success = false;
                }
                else
                {

                    string msgcode = ValidateSmsCode(mobile, 1, vcode);
                    if (msgcode == "200")
                    {
                        //Session["Mobile"] = mobile;
                        HttpCookie cookies = Response.Cookies["Mobile"];
                        cookies.Value = mobile;
                        cookies.Expires = DateTime.Now.AddMinutes(10);
                        jsonmsg.Success = true;
                        jsonmsg.Msg = "ok";
                    }
                    else
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = msgcode;
                    }
                   
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
        /// 设置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult SetPassword()
        {
            //string Mobile =Convert.ToString(Session["Mobile"]);
            HttpCookie cookies = Request.Cookies["Mobile"];
            string mobile = string.Empty;
            if (cookies != null)
            {
                mobile = cookies.Value;
            }
            

            if (string.IsNullOrEmpty(mobile) || cookies==null)
            {
                return RedirectToAction("Forget");
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult SetPw(string password)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            //string Mobile = Convert.ToString(Session["Mobile"]);
            string mobile = Request.Cookies["Mobile"].Value;
            if (!string.IsNullOrEmpty(mobile))
            {
                try
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        if (password.Length < 6 || password.Length > 12)
                        {
                            jsonmsg.Msg = "密码长度为6-12位";
                            jsonmsg.Success = false;
                        }
                        else
                        {
                           
                            password = Tool.GetMD5(password);
                            string usersql = "update UserInfo set[Password] = @Password where Mobile=@Mobile";
                            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@Mobile",mobile),
                    new SqlParameter("@Password",password)
                };
                            int data = SqlHelper.ExecuteNonQuery(usersql, sp);
                            if (data > 0)
                            {
                                jsonmsg.Msg = "ok";
                                jsonmsg.Success = true;
                            }
                            else
                            {
                                jsonmsg.Success = false;
                                jsonmsg.Msg = "fail";

                            }
                            HttpCookie cookies = Request.Cookies["Mobile"];
                            if (cookies != null)
                            {
                                cookies.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(cookies);
                            }
                            //Session.Remove("Mobile");
                            //CacheHelper.DeleteCache("Mobile");

                        }
                    }
                    else
                    {
                        jsonmsg.Msg = "密码不能为空";
                        jsonmsg.Success = false;
                    }

                }
                catch (Exception e)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = e.Message;
                    throw;
                }

            }
            return Json(jsonmsg);
        }


        /// <summary>
        /// 计划
        /// </summary>
        /// <returns></returns>
        public ActionResult Plan()
        {
            string sql = "select * from LotteryType2 where PId = 0 order by Position";
            ViewBag.list = Util.ReaderToList<LotteryType2>(sql);


            return View();
        }

        /// <summary>
        /// 开奖
        /// </summary>
        /// <returns></returns>
        public ActionResult Open()
        {

            ViewBag.UserInfo = UserHelper.LoginUser;
            ViewBag.SiteSetting = GetSiteSetting();
            return View();
        }

        /// <summary>
        /// 苹果安装说明
        /// </summary>
        /// <returns></returns>
        public ActionResult Explain()
        {
            return View();
        }
    }
}
