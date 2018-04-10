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
            string time = Util.GetOpenRemainingTime(lType);

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
            string currentIssue = Util.GetCurrentIssue(lType);
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


        public ActionResult GetOpenRemainingTime(int lType)
        {
            string result = Util.GetOpenRemainingTime(lType);
            return Content(result);
        }

        //当前投注期号 和时间
        public ActionResult GetCurrentIssueAndTime(int lType)
        {
            string result = "";

            if (lType == 63)
            {
                result = Util.GetCurrentIssue(lType) + "|" + Util.GetRemainingTime(lType) + "|" + Util.GetOpenRemainingTime(lType);
            }
            else if (lType < 9)
            {
                result = Util.GetCurrentIssue(lType) + "||" + Util.GetOpenRemainingTime(lType);
            }
            else
            {
                result = Util.GetCurrentIssue(lType) + "|" + Util.GetRemainingTime(lType) + "|" + Util.GetOpenRemainingTime(lType);
            }

            return Content(result);
        }


        //发帖
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
            string time = Util.GetOpenRemainingTime(lType);

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
            string sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);
            ViewBag.lastIssueDesc = "第" + lr.Issue + "期开奖号码:";
            ViewBag.lastNum = lr.Num;



            //3.最新5期
            sql = "select top(5)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            ViewBag.lastFive = Util.ReaderToList<LotteryRecord>(sql);


            //当前期号
            string currentIssue = Util.GetCurrentIssue(lType);
            //ViewBag.currentIssue = currentIssue;
            ViewBag.issueDesc = "当前第<t id='currentIssue'>" + currentIssue + "</t>期";


            return View();
        }


        //投注
        public ActionResult Bet(int lType, string currentIssue, string betInfo)
        {
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

                        string sql = "insert into BettingRecord(UserId,lType,Issue,PlayName,BetNum,SubTime) values(" + UserHelper.LoginUser.Id + "," + lType + ",@Issue,@PlayName,@BetNum,'" + DateTime.Now.ToString() + "')";

                        SqlParameter[] pms =
                        {
                            new SqlParameter("@Issue", currentIssue),
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


            ViewBag.lType = id;
            ViewBag.title1 = "规则说明-" + name;
            ViewBag.title2 = name + "规则说明";
            ViewBag.title3 = "万彩" + name + "玩法规则说明:";
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
            string time = Util.GetOpenRemainingTime(id);


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

            //step4.查询当前用户是否发帖
            string isSubSql = "select count(1) from dbo.BettingRecord where lType=" + id + " and WinState=1";
            object objIsSub = SqlHelper.ExecuteScalar(isSubSql);
            ViewBag.IsSub = obj != null && Convert.ToInt32(objIsSub) > 0;

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


            return View(model);
        }

        /// <summary>
        /// 最新预测
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="playName">玩法名称</param>
        /// <returns></returns>
        [Authentication]
        public ActionResult LastPlay(int id, int uid, string playName)
        {
            var user = UserHelper.LoginUser;
            if (user.Id != uid)
            {
                #region 校验,添加点阅记录，扣费，分佣

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
                            var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == id && x.Type == (int)CommissionTypeEnum.点阅佣金);
                            if (userRateSetting != null && userRateSetting.Percentage > 0)
                            {
                                int commission = (int)(userRateSetting.Percentage * readCoin);
                                executeSql.AppendFormat("update UserInfo set Coin+={0} where Id={1}", commission, uid);

                                executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", uid, id, (int)TransactionTypeEnum.点阅佣金, commission);
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

                #endregion
            }

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
            string time = Util.GetOpenRemainingTime(id);
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
    a.*,b.ltypeTotalScore,c.MinIntegral,d.Name,e.RPath as avater 
from (
  select UserId,lType,PlayName, isnull( sum(score),0) AS playTotalScore from [C8].[dbo].[BettingRecord]
  where WinState>1
  group by UserId, lType, PlayName
 ) a
  left join (
   select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [C8].[dbo].[BettingRecord]
   where WinState>1
   group by UserId, lType
  ) b on b.lType=a.lType
  left join ( 
	select lType, isnull( min(MinIntegral),0) as MinIntegral 
	from [dbo].[LotteryCharge] group by lType
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
  where WinState>1
  group by UserId, lType, PlayName
 ) a
  left join (
   select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [dbo].[BettingRecord]
   where WinState>1
   group by UserId, lType
  ) b on b.lType=a.lType
  left join ( 
	select lType, isnull( min(MinIntegral),0) as MinIntegral 
	from [dbo].[LotteryCharge] group by lType
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
                        l.SubTime = Convert.ToDateTime(item.SubTime).ToString("yyyy-MM-dd");
                        model.LotteryNum = l;
                        if (listbet.Count() > 0)
                            model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                        list.Add(model);

                    }
                }
                pager.PageData = list;

                //查询最新一期玩法
                var lastPlay = Util.ReaderToList<BettingRecord>(playSql, sp);

                pager.ExtraData = lastPlay.FirstOrDefault();

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
        /// <returns></returns>
        public JsonResult ViewPlan(int id, int ltype, int uid, int coin)
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
                if (coin > UserHelper.LoginUser.Coin)
                {
                    result = new AjaxResult(401, "余额不足");
                    return Json(result);
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
            string currentIssue = DateTime.Now.Year + Util.GetCurrentIssue(lType);

            //2.倒计时
            string time = Util.GetOpenRemainingTime(lType);

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
    string.IsNullOrWhiteSpace(playName) ? "" : " and PlayName=@PlayName");

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

                var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == id && x.Type == (int)CommissionTypeEnum.打赏佣金);
                if (userRateSetting != null && userRateSetting.Percentage > 0)
                {
                    int commission = (int)(userRateSetting.Percentage * coin);
                    //step6.发放发帖人金币账户
                    sqlBuilder.AppendFormat("UPDATE dbo.UserInfo SET Coin+={1} WHERE Id={0};", model.UserId, commission);
                    //step7.添加打赏佣金记录
                    sqlBuilder.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
     VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, id, (int)TransactionTypeEnum.打赏佣金, commission);
                }

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
    }
}
