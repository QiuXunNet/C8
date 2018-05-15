using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class PlanController : BaseController
    {
        //
        // GET: /Plan/

        //官方推荐
        public ActionResult Index(int id, int size = 10)
        {

            int lType = id;
            ViewBag.lType = lType;

            #region 官方推荐


            //1.获取数据
            int count = Util.GetGFTJCount(lType);
            int totalSize = (size + 1) * count;
            string sql = "select top(" + totalSize + ") * from [Plan] where lType = " + lType + " order by Issue desc";
            ViewBag.list2 = Util.ReaderToList<Plan>(sql);        //计划列表



            //2.取最新10期开奖号
            sql = "select top(10)* from LotteryRecord where lType = " + lType + " order by Issue desc";
            ViewBag.list = Util.ReaderToList<LotteryRecord>(sql);

            //3.最后一期
            sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);

            ViewBag.lastIssue = lr.Issue;
            ViewBag.lastNum = lr.Num;

            //剩余时间
            //string time = Util.GetOpenRemainingTime(lType);
            string time = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());

            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');

                ViewBag.min = timeArr[1];
                ViewBag.sec = timeArr[2];

                if (lType < 9)
                {
                    ViewBag.hour = timeArr[0];
                }

            }
            else
            {
                ViewBag.time = "正在开奖";
            }

            //彩种名字
            string lotteryName = Util.GetLotteryTypeName(lType);
            ViewBag.lotteryName = lotteryName;

            //当前期号
            string currentIssue = LuoUtil.GetCurrentIssue(lType); 
            ViewBag.currentIssue = currentIssue;
            ViewBag.msg = "查看" + currentIssue + "期" + lotteryName + "计划";

            //彩种图标
            string icon = Util.GetLotteryIcon(lType) + ".png";

            ViewBag.icon = icon;

            #endregion

            #region 高手推荐
            //查询玩法名称
            ViewBag.PlayList = GetPlayNames(lType);

            #endregion

            return View();
        }


        //官方推荐数据
        public ActionResult PlanData(int id, int pageIndex = 1, int pageSize = 10)
        {

            int lType = id;
            ViewBag.lType = lType;


            //1.获取数据
            int count = Util.GetGFTJCount(lType);
            int totalSize = (pageSize + 1) * count;


            string sql = "select top " + totalSize + " temp.* from ( select row_number() over(order by Id desc) as rownumber,* from [Plan] where lType = " + lType + ")as temp where rownumber>" + ((pageIndex - 1) * totalSize);
            ViewBag.list2 = Util.ReaderToList<Plan>(sql);        //计划列表



            //2.取最新10期开奖号
            string pageSql = "select top " + pageSize + " temp.* from ( select row_number() over(order by Id desc) as rownumber,* from LotteryRecord where lType = " + lType + ")as temp where rownumber>" + ((pageIndex - 1) * pageSize);
            ViewBag.list = Util.ReaderToList<LotteryRecord>(pageSql);



            return View();
        }


        public ActionResult GetOpenRemainingTime(int lType)
        {
            //string result = Util.GetOpenRemainingTime(lType);
            string result = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());
            return Content(result);
        }

        //当前投注期号 和时间
        public ActionResult GetCurrentIssueAndTime(int lType)
        {
            string result = "";

            //if (lType == 63)
            //{
            //    result = LuoUtil.GetCurrentIssue(lType)+ "|" + Util.GetRemainingTime(lType) + "|" + Util.GetOpenRemainingTime(lType);
            //}
            //else if (lType < 9)
            //{
            //    result = LuoUtil.GetCurrentIssue(lType) + "||" + Util.GetOpenRemainingTime(lType);
            //}
            //else
            //{
            //    result = LuoUtil.GetCurrentIssue(lType) + "|" + Util.GetRemainingTime(lType) + "|" + Util.GetOpenRemainingTime(lType);
            //}
            result = LuoUtil.GetCurrentIssue(lType) + "|" + Util.GetRemainingTime(lType) + "|" + LotteryTime.GetTime(lType.ToString());

            return Content(result);
        }


        //发帖
        [Authentication]
        public ActionResult Post(int id)
        {
            //彩种
            int lType = id;
            ViewBag.lType = id;

            //彩种名字
            string lotteryName = Util.GetLotteryTypeName(lType);
            ViewBag.lotteryName = lotteryName;

            //title
            ViewBag.title = "发布计划-" + lotteryName;

            //剩余时间
           // string time = Util.GetOpenRemainingTime(lType);
            string time = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());

            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');

                ViewBag.min = timeArr[1];
                ViewBag.sec = timeArr[2];

                if (lType < 9)
                {
                    ViewBag.hour = timeArr[0];
                }

            }
            else
            {
                ViewBag.time = "正在开奖";
            }



            //3.最后一期
            string sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by SubTime desc,Issue desc";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);
            ViewBag.lastIssueDesc = "第" + lr.Issue + "期开奖号码:";
            ViewBag.lastNum = lr.Num;



            //3.最新5期
            sql = "select top(5)* from LotteryRecord where lType =" + lType + " order by SubTime desc,Issue desc";
            ViewBag.lastFive = Util.ReaderToList<LotteryRecord>(sql);


            //当前期号
            string currentIssue = LuoUtil.GetCurrentIssue(lType);
            //ViewBag.currentIssue = currentIssue;
            ViewBag.issueDesc = "当前第<t id='currentIssue'>" + currentIssue + "</t>期";


            //



            return View();
        }


        //投注
        [Authentication]
        public ActionResult Bet(int lType, string currentIssue, string betInfo)
        {

            long uid = UserHelper.LoginUser.Id;

            #region 校验
            //获取当前状态
            //string time = Util.GetOpenRemainingTime(lType);
            string time = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());
            if (time == "正在开奖")
            {
                return Content("发帖失败，当期已封盘");
            }

            //校验当前这期是否已开奖
            string isOpenSql = "select count(1) from dbo.LotteryRecord where lType=" + lType + " and Issue=@Issue";
            object obj = SqlHelper.ExecuteScalar(isOpenSql, new SqlParameter("@Issue", currentIssue));
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Content("发帖失败，当期已封盘");
            }

            //获取当前最新期
            string currentLastIssue = LuoUtil.GetCurrentIssue(lType);
            if (string.IsNullOrEmpty(currentLastIssue) || currentIssue != currentLastIssue)
            {
                return Content("发帖失败，当期已封盘");
            }

            #endregion

            //数据清理
            string sql = "delete from BettingRecord where UserId=" + uid + " and lType =" + lType + " and Issue=@Issue";
            SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@Issue", currentIssue));



            string[] betInfoArr = betInfo.Split('$');

            string playName = "";
            string playName2 = "";            //单双  大小 五码
            string betNum = "";
            string s1 = "";


            foreach (string s in betInfoArr)
            {
                string[] arr = s.Split('*');

                playName = arr[0];
                betNum = arr[1];

                string[] betNumArr = betNum.Split('|');

                for (int i = 0; i < betNumArr.Length; i++)
                {
                    s1 = betNumArr[i];

                    if (!string.IsNullOrEmpty(s1))
                    {
                        playName2 = Util.GetPlayName(lType, playName, s1, i);

                        sql = "insert into BettingRecord(UserId,lType,Issue,BigPlayName,PlayName,BetNum,SubTime) values(" + uid + "," + lType + ",@Issue,@BigPlayName,@PlayName,@BetNum,'" + DateTime.Now.ToString() + "')";

                        SqlParameter[] pms =
                        {
                            new SqlParameter("@Issue", currentIssue),
                            new SqlParameter("@BigPlayName", playName),
                            new SqlParameter("@PlayName", playName + playName2),
                            new SqlParameter("@BetNum", s1),
                        };

                        SqlHelper.ExecuteNonQuery(sql, pms);
                    }
                }

            }



            return Content("ok");
        }


        //发帖规则
        public ActionResult Rule(int id)
        {
            string name = Util.GetLotteryTypeName(id);


            ViewBag.Platform = Request.Params["pl"].ToInt32();

            ViewBag.lType = id;
            ViewBag.title1 = "规则说明-" + name;
            ViewBag.title2 = name + "规则说明";
            ViewBag.title3 = name + "玩法规则说明:";
            ViewBag.title4 = name + "玩法积分规则:";


            return View();
        }


        /// <summary>
        /// 近期竞猜
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="type">查看类型 0=具体玩法 1=全部玩法</param>
        /// <returns></returns>
        [Authentication]
        public ActionResult PlayRecord(int id, int uid, int type = 0)
        {
            var loginUserId = UserHelper.LoginUser.Id;
            ViewBag.lType = id;
            //step1.查询用户信息
            var model = UserHelper.GetUser(uid);

            //step2.查询是否关注过该用户
            string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + loginUserId +
                         " and [Followed_UserId]=" + uid;

            object obj = SqlHelper.ExecuteScalar(sql);

            ViewBag.Followed = obj != null && Convert.ToInt32(obj) > 0;

            #region step3.查询开奖时间
            //step3.查询开奖时间
           // string time = Util.GetOpenRemainingTime(id);
            string time = C8.Lottery.Public.LotteryTime.GetTime(id.ToString());


            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');

                ViewBag.min = timeArr[1];
                ViewBag.sec = timeArr[2];

                if (id < 9)
                {
                    ViewBag.hour = timeArr[0];
                }

            }
            else
            {
                ViewBag.time = "正在开奖";
            }
            #endregion



            //step5.查询该彩种玩法列表
            ViewBag.LTypeName = Util.GetLotteryTypeName(id);
            ViewBag.Type = type;
            if (type == 0)
            {
                ViewBag.PlayList = GetPlayNames(id);
            }

            //step6.查询用户彩种积分，
            int totalIntegral = LuoUtil.GetUserIntegral(uid, id);
            //step7.根据用户该彩种积分，查询点阅所需金币
            var setting = GetLotteryCharge().FirstOrDefault(
                    x => x.MinIntegral <= totalIntegral
                    && x.MaxIntegral > totalIntegral
                    && x.LType == id
                );

            ViewBag.ReadCoin = setting != null ? setting.Coin : 0;

            //step4.查询当前用户是否发帖

            string isSubSql = "select count(1) from dbo.BettingRecord where lType=" + id + " and UserId=" + uid + " and WinState=1";
            object objIsSub = SqlHelper.ExecuteScalar(isSubSql);
            //查询可用金币
            int Myuid = UserHelper.GetByUserId();
            string CoinSql = "select Coin from UserInfo where Id=" + Myuid;
            object objcoin = SqlHelper.ExecuteScalar(CoinSql);
            ViewBag.Coin = Convert.ToInt32(objcoin);

            //查询可用卡劵
            string CouponSql = "select count(1) from [dbo].[UserCoupon] where UserId=" + Myuid + " and State=1 and getdate()<EndTime ";
            object objcoupon = SqlHelper.ExecuteScalar(CouponSql);
            ViewBag.Coupon = Convert.ToInt32(objcoupon);

            ViewBag.IsSub = objIsSub != null && Convert.ToInt32(objIsSub) > 0;
            ViewBag.MyUid = loginUserId;
            return View(model);
        }

        /// <summary>
        /// 最新预测
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="playName">玩法名称</param>
        /// <param name="paytype">支付类型 1金币 2查看劵</param>
        /// <returns></returns>
        [Authentication]
        public ActionResult LastPlay(int id, int uid, string playName, int paytype = 1)
        {
            var user = UserHelper.LoginUser;



            //step1:验证玩法名称是否为空
            string redirectUrl = string.Format("/Plan/PlayRecord/{0}?uid={1}", id, uid);
            if (string.IsNullOrEmpty(playName))
            {
                Response.Redirect(redirectUrl, true);
            }
            //step2.查询最新发帖
            string lastBettingSql = @" select top 1 * from BettingRecord where UserId=@UserId 
                 and lType=@lType and WinState=1 and PlayName=@PlayName order by SubTime desc";
            var lastBettingParameter = new[]
            {
                    new SqlParameter("@UserId", uid),
                    new SqlParameter("@lType", id),
                    new SqlParameter("@PlayName", playName),
                };
            var records = Util.ReaderToList<BettingRecord>(lastBettingSql, lastBettingParameter);
            var lastBettingRecord = records.FirstOrDefault();

            if (lastBettingRecord == null)
            {
                Response.Redirect(redirectUrl, true);
            }

            ViewBag.LastBettingRecord = lastBettingRecord;

            #region 校验,添加点阅记录，扣费，分佣
            if (paytype == 1)
            {
                if (user.Id != uid)
                {
                    //step3:查询用户是否点阅过该帖子。若未点阅过，则校验金币是否充足
                    string readRecordSql = @"select count(1) from ComeOutRecord 
where [Type]=@Type and UserId=@UserId and OrderId=@Id";

                    var readRecordParameter = new[]
                    {
                    new SqlParameter("@Type", (int) TransactionTypeEnum.点阅),
                    new SqlParameter("@UserId", user.Id),
                    new SqlParameter("@Id", lastBettingRecord.Id),
                };

                    object objReadRecord = SqlHelper.ExecuteScalar(readRecordSql, readRecordParameter);

                    //用户未点阅过该帖子
                    if (objReadRecord == null || Convert.ToInt32(objReadRecord) <= 0)
                    {
                        //step3.1:查询点阅所需金币
                        int totalIntegral = LuoUtil.GetUserIntegral(uid, id);
                        var setting = GetLotteryCharge().FirstOrDefault(
                            x => x.MinIntegral <= totalIntegral
                                 && x.MaxIntegral > totalIntegral
                                 && x.LType == id
                            );

                        int readCoin = 0; //点阅所需金币

                        if (setting != null) readCoin = setting.Coin;

                        StringBuilder executeSql = new StringBuilder();
                        if (readCoin > 0)
                        {
                            //step3.2:校验用户金币是否充足
                            if (user.Coin < readCoin)
                            {
                                //金币不足
                                Response.Redirect(redirectUrl, true);
                            }
                            else
                            {
                                //1.扣除用户金币
                                executeSql.AppendFormat("update UserInfo set Coin-={0} where Id={1};", readCoin, user.Id);
                                //2.添加点阅记录
                                executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅, readCoin);


                                //3:查询用户分佣比例
                                var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == GetlType(id) && x.Type == (int)CommissionTypeEnum.点阅佣金);

                                if (userRateSetting != null && userRateSetting.Percentage > 0)
                                {
                                    int commission = (int)(userRateSetting.Percentage * readCoin);
                                    executeSql.AppendFormat("update UserInfo set [Money]+={0} where Id={1}", commission, uid);

                                    executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅佣金, commission);
                                }
                            }
                        }
                        else
                        {
                            //免费专家，仅记录点阅记录
                            executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅, 0);
                        }

                        try
                        {

                            SqlHelper.ExecuteTransaction(executeSql.ToString());

                        }
                        catch (Exception ex)
                        {

                            LogHelper.WriteLog(string.Format("查看最新帖子异常。帖子Id:{0}，查看人：{1}，异常消息:{2}，异常堆栈：{3}",
                                lastBettingRecord.Id, user.Id, ex.Message, ex.StackTrace));

                            Response.Redirect(redirectUrl, true);
                        }


                    }


                }
            }
            else if (paytype == 2)
            {
                if (user.Id != uid)
                {
                    UserCoupon uc = GetUserCoupon(Convert.ToInt32(user.Id));
                    if (uc != null)
                    {
                        UpdateUserCoupon(lastBettingRecord.Id, uc.Id);
                    }
                    else
                    {
                        Response.Redirect(redirectUrl, true);
                    }
                }


            }

            #endregion

            #region View数据查询
            ViewBag.lType = id;
            //step4.查询发帖用户信息
            var model = UserHelper.GetUser(uid);

            //step5.查询是否关注过该用户
            string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + user.Id +
                         " and [Followed_UserId]=" + uid;
            object obj = SqlHelper.ExecuteScalar(sql);
            ViewBag.Followed = obj != null && Convert.ToInt32(obj) > 0;

            //step6.查询是否开奖
           // string time = Util.GetOpenRemainingTime(id);
            string time = C8.Lottery.Public.LotteryTime.GetTime(id.ToString());
            if (time != "正在开奖")
            {
                time = "未开奖";
            }
            ViewBag.Time = time;

            //step7.查询该彩种名称
            ViewBag.LTypeName = Util.GetLotteryTypeName(id);
            #endregion


            return View(model);
        }

        /// <summary>
        /// 使用卡劵记录
        /// </summary>
        /// <param name="uc"></param>
        /// <returns></returns>
        public int UpdateUserCoupon(long planId, int Id)
        {
            string strsql = "update [UserCoupon] set PlanId =@PlanId, State = 2, SubTime = getdate() where Id =@Id ";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@PlanId",planId),
                new SqlParameter("@Id",Id)

            };
            try
            {
                return SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取一张可用查看劵
        /// </summary>
        /// <returns></returns>
        public UserCoupon GetUserCoupon(int UserId)
        {
            string strsql = "select top 1 * from[dbo].[UserCoupon] where UserId =" + UserId + " and State = 1 and getdate() < EndTime Order by EndTime";
            return Util.ReaderToModel<UserCoupon>(strsql);

        }


        /// <summary>
        /// 获取大彩种lType
        /// </summary>
        /// <returns></returns>
        public int GetlType(int LotteryCode)
        {
            string strsql = string.Format("select lType from [dbo].[Lottery] where LotteryCode={0}", LotteryCode);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(strsql));
        }





        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="lType">彩种Id</param>
        /// <param name="playName">玩法名称</param>
        /// <param name="type">类型 1=高手推荐 2=免费专家</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ExpertList(int lType, string playName, int type = 1, int pageIndex = 1, int pageSize = 20)
        {
            var result = new AjaxResult<PagedList<Expert>>();

            var pager = new PagedList<Expert>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;

            string sqlWhere = type == 1 ? ">=" : "<";

            #region 分页查询专家排行数据行
            string sql = string.Format(@"select * from (
 select top 100 row_number() over(order by a.playTotalScore DESC ) as rowNumber,
    a.*,b.ltypeTotalScore,c.MinIntegral,isnull(d.Name,'') as Name,isnull(e.RPath,'') as avater 
from (
  select UserId,lType,PlayName, isnull( sum(score),0) AS playTotalScore from [C8].[dbo].[BettingRecord]
  where WinState>1 and lType=@lType and PlayName=@PlayName
  group by UserId, lType, PlayName
 ) a
  left join (
   select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [C8].[dbo].[BettingRecord]
   where WinState>1 and lType=@lType
   group by UserId, lType
  ) b on b.lType=a.lType and b.UserId=a.UserId
  left join ( 
	select lType, isnull( min(MinIntegral),0) as MinIntegral 
	from [dbo].[LotteryCharge] 
    where lType=@lType
    group by lType
  ) c on c.lType=a.lType
  left join UserInfo d on d.Id=a.UserId
  left join ResourceMapping e on e.FkId =a.UserId and e.[Type]=@ResourceType

  where b.ltypeTotalScore {0} c.MinIntegral and a.PlayName=@PlayName and a.lType=@lType 
  ) tt
  where tt.rowNumber between @Start and  @End", sqlWhere);

            var sqlParameter = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@PlayName",playName),
                new SqlParameter("@lType",lType),
                new SqlParameter("@Start",pager.StartIndex),
                new SqlParameter("@End",pager.EndIndex),

            };
            pager.PageData = Util.ReaderToList<Expert>(sql, sqlParameter);

            pager.PageData.ForEach(x =>
            {
                GetLastBettingRecord(x);
            });

            #endregion

            #region 数据总行数
            //查询分页总数量
            string countSql = string.Format(@"select count(1) from (
  select UserId,lType,PlayName, isnull( sum(score),0) AS playTotalScore from [dbo].[BettingRecord]
  where WinState>1 and lType=@lType and PlayName=@PlayName
  group by UserId, lType, PlayName
 ) a
  left join (
   select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [dbo].[BettingRecord]
   where WinState>1 and lType=@lType
   group by UserId, lType
  ) b on b.lType=a.lType and b.UserId=a.UserId
  left join ( 
	select lType, isnull( min(MinIntegral),0) as MinIntegral 
	from [dbo].[LotteryCharge]
    where lType=@lType
    group by lType
  ) c on c.lType=a.lType

  where b.ltypeTotalScore {0} c.MinIntegral and a.PlayName=@PlayName and a.lType=@lType", sqlWhere);

            var countSqlParameter = new[]
            {
                new SqlParameter("@PlayName",playName),
                new SqlParameter("@lType",lType),
            };
            object obj = SqlHelper.ExecuteScalar(countSql, countSqlParameter);
            int totalCount = Convert.ToInt32(obj ?? 0);
            pager.TotalCount = totalCount > 100 ? 100 : totalCount;
            #endregion

            result.Data = pager;
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// 近期竞猜
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="lType"></param>
        /// <param name="playName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetLastPlay(int uid, int lType, string playName)
        {
            string strsql = string.Empty;
            string numsql = string.Empty;
            string playSql = string.Empty;
            var result = new AjaxResult<PagedList<AchievementModel>>();

            var pager = new PagedList<AchievementModel>();
            pager.PageIndex = 1;
            pager.PageSize = 10;
            SqlParameter[] sp = new SqlParameter[] { };
            if (playName == "全部")//全部
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}", uid, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber, Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1}  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {2} AND {3}  ", uid, lType, pager.StartIndex, pager.EndIndex);
                playSql = string.Format(@" select top 1 * from BettingRecord where UserId={0} 
                 and lType={1} and WinState=1 order by SubTime desc", uid, lType);
            }
            else
            {
                strsql = string.Format(@"
                select * from BettingRecord   where UserId ={0} and lType = {1}  and PlayName = @PlayName", uid, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue from LotteryRecord l
	  ,BettingRecord b
	  where b.Issue=l.Issue and b.lType=l.lType
	  and b.UserId={0} and b.lType={1} and b.PlayName=@PlayName  and b.WinState in(3,4)
	  group by l.Issue,Num,l.SubTime
	  )t
	  where   rowNumber BETWEEN {2} AND {3} ", uid, lType, pager.StartIndex, pager.EndIndex);
                playSql = string.Format(@" select top 1 * from BettingRecord where UserId={0} 
                 and lType={1} and WinState=1 and PlayName=@PlayName order by SubTime desc", uid, lType);

                sp = new SqlParameter[]{
                    new SqlParameter("@PlayName",playName)
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
                        l.SubTime = Convert.ToDateTime(item.SubTime).ToString("MM-dd HH:mm");
                        model.LotteryNum = l;
                        if (listbet.Count() > 0)
                            model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                        list.Add(model);

                    }
                }
                pager.PageData = list;

                //查询最新一期玩法
                var lastPlay = Util.ReaderToList<BettingRecordViewModel>(playSql, sp);
                var extraData = lastPlay.FirstOrDefault();

                if (extraData != null)
                {
                    //查询当前用户是否点阅过该记录
                    string isSubSql = "select count(1) from dbo.ComeOutRecord where Type="
                        + (int)TransactionTypeEnum.点阅 + " and UserId=" + UserHelper.GetByUserId() + " and OrderId=" + extraData.Id;
                    object objIsSub = SqlHelper.ExecuteScalar(isSubSql);
                    extraData.IsRead = objIsSub != null && Convert.ToInt32(objIsSub) > 0;
                }

                pager.ExtraData = extraData;

                result.Data = pager;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看用户帖子
        /// </summary>
        /// <param name="id">帖子Id</param>
        /// <param name="ltype">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="coin">查看所需金币</param>
        /// <param name="paytype">支付类型 1金币 2查看劵</param>
        /// <returns></returns>
        public JsonResult ViewPlan(int id, int ltype, int uid, int coin, int paytype)
        {
            var result = new AjaxResult();

            try
            {
                //step1.查询用户是否点阅过该帖子
                string readRecordSql = @"select count(1) from ComeOutRecord 
where [Type]=@Type and UserId=@UserId and OrderId=@Id";

                var readRecordParameter = new[]
                {
                    new SqlParameter("@Type",(int)TransactionTypeEnum.点阅),
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Id",id),
                };

                object objReadRecord = SqlHelper.ExecuteScalar(readRecordSql, readRecordParameter);

                if (objReadRecord != null && Convert.ToInt32(objReadRecord) > 0)
                {
                    //已经点阅过，直接跳转
                    return Json(result);
                }

                //step6.查询用户彩种积分，
                //    string totalIntegralByLtypeSql = string.Format(@"select isnull(sum(score),0) 
                //from dbo.BettingRecord where lType={0} and UserId={1} and WinState=1", id, uid);
                //    object objTotalIntegral = SqlHelper.ExecuteScalar(totalIntegralByLtypeSql);
                //    int totalIntegral = objTotalIntegral != null ? Convert.ToInt32(objTotalIntegral) : 0;
                //    //step7.根据用户该彩种积分，查询点阅所需金币
                //    var setting = GetLotteryCharge().FirstOrDefault(
                //            x => x.MinIntegral <= totalIntegral
                //            && x.MaxIntegral > totalIntegral
                //            && x.LType == ltype
                //        );

                //step2.判断当前用户积分是否小于查看帖子所需金币
                if (paytype == 1)
                {
                    if (coin > UserHelper.LoginUser.Coin)
                    {
                        result = new AjaxResult(401, "余额不足");
                        return Json(result);
                    }
                }
                else if (paytype == 2)
                {
                    if (GetCoupon(Convert.ToInt32(UserHelper.LoginUser.Id)) <= 0)
                    {
                        result = new AjaxResult(401, "无可用查看劵");
                        return Json(result);
                    }
                }


            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = "服务器繁忙";
                LogHelper.WriteLog(string.Format("查询贴子权限异常。异常消息：{0},异常堆栈:{1}", ex.Message, ex.StackTrace));
            }

            return Json(result);


        }

        /// <summary>
        /// 获取可用查看劵
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetCoupon(int UserId)
        {
            string strsql = "select count(1) from [dbo].[UserCoupon] where UserId=" + UserId + " and State=1 and getdate()<EndTime ";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(strsql));
        }


        /// <summary>
        /// 获取用户在某一玩法中奖率，最大连中，上期是否中奖
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filterRow"></param>
        private void GetLastBettingRecord(Expert model, int filterRow = 1000)
        {
            string sql = string.Format(@"select top {0} Issue,WinState from dbo.BettingRecord
  where lType=@lType and PlayName=@PlayName and UserId=@UserId and WinState>1
  order by Issue desc", filterRow);

            var sqlParameter = new[]
            {
                new SqlParameter("@lType",model.lType),
                new SqlParameter("@PlayName",model.PlayName),
                new SqlParameter("@UserId",model.UserId),

            };

            var list = Util.ReaderToList<BettingRecord>(sql, sqlParameter) ?? new List<BettingRecord>();

            //step1.上期是否中奖
            var lastBettingRecord = list.FirstOrDefault();
            model.LastWin = lastBettingRecord != null && lastBettingRecord.WinState == 3;

            //step2.查询10中几
            int winCount = 0;//10中几

            //查询最后10期，并按顺序排序
            var last10Record = list.Take(10).OrderBy(x => x.Issue);
            int last10RecordLength = last10Record.Count();
            if (last10RecordLength > 0)
            {
                foreach (var record in last10Record)
                {
                    if (record.WinState == 3)
                        winCount++;
                }
                model.HitRate = winCount / last10RecordLength;
                model.HitRateDesc = string.Format("{0}中{1}", last10RecordLength, winCount);
            }

            //step3.查询最大连中
            int continuousWinCount = 0;//最大连中
            int tempContinuousWinCount = 0;
            //按期号顺序排序，并遍历
            foreach (var record in list.OrderBy(x => x.Issue))
            {
                if (record.WinState == 3)
                {
                    tempContinuousWinCount++;
                }
                else
                {
                    //当期未中时，将当前最大连中赋值给continuousWinCount，并重置临时计算最大连中
                    continuousWinCount = tempContinuousWinCount;
                    tempContinuousWinCount = 0;
                }
            }
            if (tempContinuousWinCount > continuousWinCount)
                continuousWinCount = tempContinuousWinCount;

            model.MaxWin = continuousWinCount;

        }


        //查看最新一期计划
        [Authentication]
        public ActionResult Look(int id)
        {
            int lType = id;
            ViewBag.lType = lType;
            string currentIssue = DateTime.Now.Year + LuoUtil.GetCurrentIssue(lType);

            //2.倒计时
            //string time = Util.GetOpenRemainingTime(lType);
            string time = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());

            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');

                ViewBag.min = timeArr[1];
                ViewBag.sec = timeArr[2];
            }
            else
            {
                ViewBag.time = "正在开奖";
            }


            ViewBag.currentIssue = currentIssue;
            ViewBag.msg = currentIssue + "期" + Util.GetLotteryTypeName(lType) + "计划";



            //1.最后一期开奖号码
            string sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);
            ViewBag.lastIssue = lr.Issue;
            ViewBag.lastNum = lr.Num;


            //4.1获取数据
            sql = "select top(" + Util.GetGFTJCount(lType) + ")* from [Plan] where lType = " + lType + " order by Issue desc";
            ViewBag.list = Util.ReaderToList<Plan>(sql);

            //彩种图标
            string icon = Util.GetLotteryIcon(lType) + ".png";

            ViewBag.icon = icon;

            string lotteryName = Util.GetLotteryTypeName(lType);
            ViewBag.lotteryName = lotteryName;


            return View();


        }

        /// <summary>
        /// 打赏页
        /// </summary>
        /// <param name="uid">帖子Id</param>
        /// <param name="ltype">彩种Id</param>
        /// <param name="playName">玩法Id</param>
        /// <returns></returns>
        [Authentication]
        public ActionResult Tip(int uid, int ltype, string playName)
        {
            ViewBag.SiteSetting = GetSiteSetting();

            //step1:查询帖子信息
            string bettingRecordSql =
                string.Format(@"select Top 1 * from BettingRecord 
    where UserId={0} and lType={1}{2}
 order by SubTime DESC", uid, ltype,
    string.IsNullOrWhiteSpace(playName) || playName == "全部" ? "" : " and PlayName=@PlayName");

            var bettingRecordParameter = new[]
            {
                new SqlParameter("@PlayName",playName),
            };

            var bettingRecordlist = Util.ReaderToList<BettingRecord>(bettingRecordSql, bettingRecordParameter);


            var bettingRecord = bettingRecordlist.FirstOrDefault();

            if (bettingRecord == null)
            {
                //用户在该彩种，没有发帖，无法打赏
                string redirectUrl = string.Format("/Plan/PlayRecord/{0}?uid={1}", ltype, uid);
                Response.Redirect(redirectUrl, true);
            }

            ViewBag.PlanId = bettingRecord.Id;

            //step2:该用户在当前彩种收到的前50条打赏记录
            string sql = @"select Top 50 a.Id,a.UserId,a.OrderId,a.[Type],a.[Money],a.[State],a.SubTime,c.Name,d.RPath as Avater 
from ComeOutRecord a
left join BettingRecord b on b.Id=a.OrderId 
left join UserInfo c on c.Id=a.UserId
left join ResourceMapping d on d.FkId=a.UserId and d.[Type]=@ResourceType
where b.UserId=@UserId and a.[Type]=@RecordType and b.lType=@lType
order by SubTime DESC";

            var sqlParameter = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@UserId",bettingRecord.UserId),
                new SqlParameter("@RecordType",(int)TransactionTypeEnum.打赏),
                new SqlParameter("@lType",ltype),
            };

            var list = Util.ReaderToList<TipRecordModel>(sql, sqlParameter);

            //step3:查询该用户在当前彩种收到的打赏总金额
            string sumSql = @"select isnull(sum(a.[Money]),0) from ComeOutRecord a
left join BettingRecord b on b.Id=a.OrderId
where b.UserId=" + bettingRecord.UserId + " and a.[Type]=" + (int)TransactionTypeEnum.打赏 + " and b.lType=" +
                            bettingRecord.lType;
            object sumObj = SqlHelper.ExecuteScalar(sumSql);

            ViewBag.TotalTip = sumObj.ToInt32();

            return View(list);
        }

        /// <summary>
        /// 打赏金币
        /// </summary>
        /// <param name="id">帖子Id</param>
        /// <param name="coin">金币数量</param>
        /// <returns></returns>
        [Authentication]
        public JsonResult GiftCoin(int id, int coin)
        {
            var user = UserHelper.LoginUser;
            #region 校验
            //step1.验证金币输入是否正确
            if (coin < 10)
            {
                return Json(new AjaxResult(400, "最低打赏10金币"));
            }
            //step2.验证帖子是否存在
            var model = Util.GetEntityById<BettingRecord>(id);
            if (model == null)
                return Json(new AjaxResult(404, "该帖子不存在"));

            //step3.验证用户金币是否充足
            if (user.Coin < coin)
            {
                return Json(new AjaxResult(402, "余额不足"));
            }
            #endregion

            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                //step4.扣除打赏人账户金币
                sqlBuilder.AppendFormat("UPDATE dbo.UserInfo SET Coin-={1} WHERE Id={0};", user.Id, coin);
                //step5.添加打赏记录
                sqlBuilder.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, id, (int)TransactionTypeEnum.打赏, coin);

                var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == GetlType(model.lType) && x.Type == (int)CommissionTypeEnum.打赏佣金);
                if (userRateSetting != null && userRateSetting.Percentage > 0)
                {
                    int commission = (int)(userRateSetting.Percentage * coin);
                    //step6.发放发帖人金币账户
                    sqlBuilder.AppendFormat("UPDATE dbo.UserInfo SET [Money]+={1} WHERE Id={0};", model.UserId, commission);
                    //step7.添加打赏佣金记录
                    sqlBuilder.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, id, (int)TransactionTypeEnum.打赏佣金, commission);
                }

                //LogHelper.WriteLog(sqlBuilder.ToString());
                SqlHelper.ExecuteTransaction(sqlBuilder.ToString());

                return Json(new AjaxResult(100, "打赏成功"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("打赏异常，帖子Id:{0},打赏人Id:{1}。异常消息：{2},异常堆栈：{3}。"
                    , id, user.Id, ex.Message, ex.StackTrace));
                return Json(new AjaxResult(500, "服务器繁忙"));
            }


        }

        /// <summary>
        /// 专家热搜
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult ExpertSearch(int id)
        {
            int MyUserId = UserHelper.GetByUserId();
            string strsql = @"select top 5 UserId,lType,isnull(u.Name, '') as Name,(select top 1 PlayName from [dbo].[IntegralRule] where lType=e.lType) as PlayName,isnull(r.RPath, '') as Avater,(select count(1)  from [dbo].[Follow] where UserId=@MyUserId and [Followed_UserId]=e.UserId and Status=1) isFollow 
from ExpertHotSearch e
left join UserInfo u    on e.UserId = u.Id
left join ResourceMapping r on r.FkId = e.UserId and r.[Type] = @ResourceType
 where lType=@lType and e.UserId<>@MyUserId
order by e.Count desc";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@MyUserId",MyUserId),
                new SqlParameter("@lType",id),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像)
            };
            List<ExpertSearchModel> list = Util.ReaderToList<ExpertSearchModel>(strsql, sp);
            ViewBag.HotList = list;
            ViewBag.lType = id;
            string memberKey = "history_" + MyUserId + "_" + id;
            //ViewBag.historyList = MemClientFactory.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();
            ViewBag.historyList = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();
            return View();
        }


        /// <summary>
        /// 插入搜索数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="lType"></param>
        /// <returns></returns>
        public JsonResult InsertHotSearch(int uid, int lType)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            string countsql = string.Format("select count(1) from ExpertHotSearch where UserId ={0} and lType = {1}", uid, lType);
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));
            string strsql = "";
            if (count > 0)
            {
                strsql = @"update ExpertHotSearch  set Count=Count+1 where UserId=@UserId and lType=@lType ";
            }
            else
            {
                strsql = @"insert into ExpertHotSearch(UserId, Count, lType)
                           values(@UserId,1,@lType)";
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",uid),
                new SqlParameter("@lType",lType)
            };
            try
            {
                int data = SqlHelper.ExecuteNonQuery(strsql, sp);

                if (data > 0)
                {
                    List<ExpertSearchModel> list = new List<ExpertSearchModel>();

                    int MyUserId = UserHelper.GetByUserId();
                    string memberKey = "history_" + MyUserId + "_" + lType;
                    // list = MemClientFactory.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();
                    list = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();

                    UserInfo u = UserHelper.GetUser(uid);

                    ExpertSearchModel e = new ExpertSearchModel();
                    e.Avater = u.Headpath;
                    e.UserId = uid;
                    e.Name = u.Name;
                    e.lType = lType;
                    e.isFollow = 0;
                    if (list.Count > 0)
                    {
                        ExpertSearchModel e1 = list.Where(x => x.UserId == uid && x.lType == lType).FirstOrDefault();
                        if (e1 == null)
                        {
                            list.Add(e);
                        }
                    }
                    else
                    {
                        list.Add(e);
                    }

                    //MemClientFactory.WriteCache(memberKey, list, 144000);
                    CacheHelper.AddCache(memberKey, list, 144000);
                    msg.Success = true;
                    msg.Msg = "ok";
                }
                else
                {
                    msg.Success = false;
                    msg.Msg = "fail";
                }

            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.Msg = ex.Message;
                throw;
            }

            return Json(msg);

        }
        /// <summary>
        /// 删除历史记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="lType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteHistory(int uid, int lType)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            try
            {
                int MyUserId = UserHelper.GetByUserId();
                string memberKey = "history_" + MyUserId + "_" + lType;
                //List<ExpertSearchModel> list = MemClientFactory.GetCache<List<ExpertSearchModel>>(memberKey);
                List<ExpertSearchModel> list = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey);
                if (uid > 0)
                {
                    ExpertSearchModel e1 = list.Where(x => x.UserId == uid && x.lType == lType).FirstOrDefault();
                    list.Remove(e1);
                    //MemClientFactory.WriteCache(memberKey, list, 144000);
                    CacheHelper.AddCache(memberKey, list, 144000);
                }
                else
                {
                    //MemClientFactory.DeleteCache(memberKey);
                    CacheHelper.DeleteCache(memberKey);
                }
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
        /// 搜索数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ExpertSearchList(int lType, string NickName)
        {
            ReturnMessageJson msg = new ReturnMessageJson();
            int MyUserId = UserHelper.GetByUserId();
            string strsql = @" select UserId,lType,
	isnull(u.Name,'') as Name,isnull(r.RPath,'') as Avater,(select top 1 PlayName from [dbo].[IntegralRule] where lType=b.lType) as PlayName,(select count(1)  from [dbo].[Follow] where UserId=@MyUserId and [Followed_UserId]=b.UserId and Status=1) isFollow 
	from [BettingRecord] b 
	left join UserInfo u	on b.UserId=u.Id
	left join ResourceMapping r on r.FkId =b.UserId and r.[Type]=@ResourceType
    where WinState>1 and lType=@lType and u.Name like '%'+@Name+'%'  and b.UserId<>@MyUserId
    group by UserId, lType,u.Name,r.RPath";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@MyUserId",MyUserId),
                 new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType),
                new SqlParameter("@Name",NickName)

            };
            try
            {
                List<ExpertSearchModel> list = Util.ReaderToList<ExpertSearchModel>(strsql, sp);
                msg.data = list;
                msg.Success = true;


            }
            catch (Exception e)
            {
                msg.Msg = e.Message;
                msg.Success = false;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }








        [Authentication]
        public ActionResult AlreadyPostData(int id)
        {
            string sql = "select * from BettingRecord where UserId  =" + UserHelper.LoginUser.Id + " and lType=" + id + " and Issue = '" + LuoUtil.GetCurrentIssue(id) + "'";
            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
