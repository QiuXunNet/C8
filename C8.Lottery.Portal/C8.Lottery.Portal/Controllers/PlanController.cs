using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Model.Enum;
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
            ViewBag.title4 =  name + "玩法积分规则:";


            return View();
        }
        

        /// <summary>
        /// 近期竞猜
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="type">查看类型 0=具体玩法 1=全部玩法</param>
        /// <returns></returns>
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

            //step6.查询


            return View(model);
        }

        /// <summary>
        /// 最新预测
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="playName">玩法名称</param>
        /// <returns></returns>
        public ActionResult LastPlay(int id, int uid, string playName)
        {

            string redirectUrl = string.Format("/Plan/PlayRecord/{0}?uid={1}", id, uid);
            if (string.IsNullOrEmpty(playName))
            {
                Response.Redirect(redirectUrl, true);
            }

            var loginUserId = UserHelper.LoginUser.Id;
            ViewBag.lType = id;
            //step1.查询用户信息
            var model = UserHelper.GetUser(uid);

            //step2.查询是否关注过该用户
            string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + loginUserId +
                         " and [Followed_UserId]=" + uid;

            object obj = SqlHelper.ExecuteScalar(sql);

            ViewBag.Followed = obj != null && Convert.ToInt32(obj) > 0;

            //step3.查询最新发帖
            string lastBettingSql = @" select top 1 * from BettingRecord where UserId=@UserId 
                 and lType=@lType and WinState=1 and PlayName=@PlayName order by SubTime desc";
            var lastBettingParameter = new[]
            {
                new SqlParameter("@UserId",uid),
                new SqlParameter("@lType",id),
                new SqlParameter("@PlayName",playName),
            };
            var records = Util.ReaderToList<BettingRecord>(lastBettingSql, lastBettingParameter);
            var lastBettingRecord = records.FirstOrDefault();

            if (lastBettingRecord == null)
            {
                Response.Redirect(redirectUrl, true);
            }

            ViewBag.LastBettingRecord = lastBettingRecord;

            //step4.查询是否开奖
            string time = Util.GetOpenRemainingTime(id);
            if (time != "正在开奖")
            {
                time = "未开奖";
            }
            ViewBag.Time = time;
            //step5.查询该彩种玩法列表
            ViewBag.LTypeName = Util.GetLotteryTypeName(id);

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


    }
}
