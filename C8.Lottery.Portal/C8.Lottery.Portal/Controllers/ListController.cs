﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
using C8.Lottery.Portal.Models;
using C8.Lottery.Model.Enum;

namespace C8.Lottery.Portal.Controllers
{
    public class ListController : BaseController
    {
        /// <summary>
        /// 排行榜
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 高手榜
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult Expert(int id = 0)
        {
            //step1.查询彩种分类列表
            string strsql = @"select * from LotteryType2 where PId=0  order by Position,Pid ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            ViewBag.TypeList = list;
            //step2.查询具体彩种
            string lotteryListSql = @"select * from LotteryType2 where PId>0  order by Position,Pid ";
            var queryList = Util.ReaderToList<LotteryType2>(lotteryListSql);
            var lotteryList = queryList.GroupBy(x => x.PId);
            ViewBag.LotteryList = lotteryList.OrderBy(x => x.Key);



            //step3.查询当前彩种信息
            var model = id < 1 ? queryList.FirstOrDefault() : queryList.FirstOrDefault(x => x.Id == id);
            //Util.GetEntityById<LotteryType2>(id);

            //step2.查询当前
            return View(model);
        }

        /// <summary>
        /// 榜单数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authentication]
        public ActionResult ExpertData(int id)
        {
            int lType = GetlType(id);
            long userId = UserHelper.LoginUser.Id;
            //step1.查询日榜
            var dailyList = GetSuperiorListDay(lType, DateTime.Today);
            ViewBag.DailyList = dailyList;

            var userDailyInfo = GetUserRankingInfo(lType, "day");

            if (dailyList != null && dailyList.Count > 0)
            {
                var userRankInfo = dailyList.FirstOrDefault(x => x.UserId == userId);

                if (userRankInfo != null)
                {
                    userDailyInfo.Rank = userRankInfo.Rank;
                }
            }
            ViewBag.UserDailyInfo = userDailyInfo;

            //step2.查询周榜
            var weekList = GetSuperiorListWeek(lType);
            ViewBag.WeekList = weekList;

            var userWeekInfo = GetUserRankingInfo(lType, "week");
            if (weekList != null && weekList.Count > 0)
            {
                var userWeekRank = weekList.FirstOrDefault(x => x.UserId == userId);
                if (userWeekRank != null)
                {
                    userWeekInfo.Rank = userWeekRank.Rank;
                }
            }
            ViewBag.UserWeekInfo = userWeekInfo;

            //step3.查询月榜
            var monthList = GetSuperiorListMonth(lType);
            ViewBag.MonthList = monthList;

            var userMonthInfo = GetUserRankingInfo(lType, "month");
            if (monthList != null && monthList.Count > 0)
            {
                var userMonthRank = monthList.FirstOrDefault(x => x.UserId == userId);
                if (userMonthRank != null)
                {
                    userMonthInfo.Rank = userMonthRank.Rank;
                }
            }
            ViewBag.UserMonthInfo = userMonthInfo;

            //step4.查询总榜
            var totalList = GetSuperiorListAll(lType);
            ViewBag.TotalList = totalList;

            var userTotalInfo = GetUserRankingInfo(lType, "all");
            if (totalList != null && totalList.Count > 0)
            {
                var userTotalRank = totalList.FirstOrDefault(x => x.UserId == userId);
                if (userTotalRank != null)
                {
                    userTotalInfo.Rank = userTotalRank.Rank;
                }
            }
            ViewBag.UserTotalInfo = userTotalInfo;
            

            return View();
        }

        public int GetlType(int id)
        {
            string strsql = string.Format("select lType from [dbo].[LotteryType2]  where Id={0}", id);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(strsql));
        }

        /// <summary>
        /// 最新榜
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            return View();
        }
        /// <summary>
        /// 打赏榜
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult Gratuity()
        {
            //step1.查询彩种分类列表

            int i = Tool.GetCacheTime("week");
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            ViewBag.TypeList = list;
            //step2.查询具体彩种
            string lotteryListSql = @"select * from LotteryType2 where PId<>0  order by Position,PId ";
            var lotteryList = Util.ReaderToList<LotteryType2>(lotteryListSql).GroupBy(x => x.PId);
            ViewBag.LotteryList = lotteryList;

            return View();

        }



        /// <summary>
        /// 盈利榜=点阅佣金榜
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult Profit()
        {   //step1.查询彩种分类列表

            int i = Tool.GetCacheTime("week");
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            ViewBag.TypeList = list;
            //step2.查询具体彩种
            string lotteryListSql = @"select * from LotteryType2 where PId<>0  order by Position,PId ";
            var lotteryList = Util.ReaderToList<LotteryType2>(lotteryListSql).GroupBy(x => x.PId);
            ViewBag.LotteryList = lotteryList;

            return View();

        }
        /// <summary>
        /// 佣金排行数据 （打赏、盈利）
        /// </summary>
        /// <returns></returns>
        [Authentication]
        [HttpGet]

        public JsonResult GetRankMoneyList(string queryType, int RType, int lType)
        {
            List<RankMonyModel> list;
            string cachekey = string.Format(RedisKeyConst.Rank_MoneyList, lType, queryType, RType); //"rank:moneylist:website:" + lType + ":" + queryType + "_" + RType;
            //list = CacheHelper.GetCache<List<RankMonyModel>>("GetRankMoneyListWebSite" + queryType + RType + lType);
            list = CacheHelper.GetCache<List<RankMonyModel>>(cachekey);
            if (list == null)
            {
                string strsql = string.Format(@"select top 100  row_number() over(order by sum(Money) desc  )as Rank,sum(Money)as Money,lType,UserId,NickName,Avater from
                            (select 
                            b.UserId,a.Money,b.lType,c.Name as NickName,d.RPath as Avater  from [dbo].[ComeOutRecord] a
                            left join BettingRecord b on b.Id=a.OrderId 
                            left join UserInfo c on c.Id=b.UserId
                            left join ResourceMapping d on d.FkId=b.UserId and d.[Type]=@ResourceType
                            where a.Type=@RType  and b.lType=@lType
                            {0}

                            )t
                            group by t.UserId ,t.NickName,t.Avater,t.lType
                            order by Money desc,NickName asc
                            ", Tool.GetTimeWhere("a.SubTime", queryType));

                SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@RType",RType),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType) };

                list = Util.ReaderToList<RankMonyModel>(strsql, sp);

                //CacheHelper.SetCache<List<RankMonyModel>>("GetRankMoneyListWebSite" + queryType + RType + lType, list, DateTime.Parse("23:59:59"));
                CacheHelper.SetCache<List<RankMonyModel>>(cachekey, list, DateTime.Parse("23:59:59"));
            }

            int UserId = UserHelper.GetByUserId();
            var model = list.Where(e => e.UserId == UserId).FirstOrDefault();

            MyRankMonyModel my = new MyRankMonyModel();
            UserInfo user = UserHelper.GetUser();
            user.Headpath = string.IsNullOrEmpty(user.Headpath) ? LuoUtil.DefaultAvater : user.Headpath;

            if (model != null)
            {
                my.Avater = user.Headpath;
                my.Rank = model.Rank;
                my.NickName = model.NickName;
                my.Money = model.Money;
                my.LType = model.LType;
                my.UserId = model.UserId;
            }
            else
            {
                my.NickName = user.Name;
                my.Avater = user.Headpath;
                my.Rank = 0;
                my.UserId = user.Id;
                my.Money = 0;
            }

            RankMoneyListModel rankMoneyListModel = new RankMoneyListModel();
            rankMoneyListModel.RankMonyModelList = list;
            rankMoneyListModel.MyRankMonyModel = my;

            ReturnMessageJson msgjson = new ReturnMessageJson();
            msgjson.Success = true;
            msgjson.data = rankMoneyListModel;

            return Json(msgjson, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 我的排名 打赏盈利
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="RType"></param>
        /// <param name="lType"></param>
        /// <returns></returns>
        public MyRankMonyModel GetMyRankMony(string queryType, int RType, int lType)
        {
            int UserId = UserHelper.GetByUserId();
            string strsql = string.Format(@"select  row_number() over(order by sum(Money) desc  )as Rank,sum(Money)as Money,lType,UserId,NickName,Avater from
(select 
b.UserId,a.Money,b.lType,c.Name as NickName,d.RPath as Avater  from [dbo].[ComeOutRecord] a
left join BettingRecord b on b.Id=a.OrderId 
left join UserInfo c on c.Id=b.UserId
left join ResourceMapping d on d.FkId=b.UserId and d.[Type]=@ResourceType
where a.Type=@RType and b.lType=@lType
{0}

)t
group by t.UserId ,t.NickName,t.Avater,t.lType
order by Money desc,NickName asc
", Tool.GetTimeWhere("a.SubTime", queryType));
            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@RType",RType),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType)
            };
            List<RankMonyModel> list = Util.ReaderToList<RankMonyModel>(strsql, sp);
            MyRankMonyModel myrank = new MyRankMonyModel();
            UserInfo user = UserHelper.GetUser();
            myrank.NickName = user.Name;
            myrank.Avater = user.Headpath;
            myrank.Rank = 0;
            myrank.UserId = user.Id;
            myrank.Money = 0;
            if (list.Count > 0)
            {
                RankMonyModel rmodel = list.Where(x => x.UserId == UserId).FirstOrDefault();
                if (rmodel != null)
                {
                    myrank.Avater = rmodel.Avater;
                    myrank.Rank = rmodel.Rank;
                    myrank.NickName = rmodel.NickName;
                    myrank.Money = rmodel.Money;
                    myrank.LType = rmodel.LType;
                    myrank.UserId = rmodel.UserId;
                }
            }
            return myrank;


        }



        /// <summary>
        /// 积分榜
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult Integral()
        {
            return View();
        }
        /// <summary>
        /// 积分榜数据
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetIntegralList(string queryType)
        {
            List<RankIntegralModel> list;
            string cachekey = string.Format(RedisKeyConst.Rank_IntegralList, queryType); //"rank:integrallist:website:" + queryType;
            //list = CacheHelper.GetCache<List<RankIntegralModel>>("GetIntegralListWebSite" + queryType);
            list = CacheHelper.GetCache<List<RankIntegralModel>>(cachekey);
            list = null;
            if (list == null)
            {
                string strsql = string.Format(@"
                select top 100 row_number() over(order by Sum(Score) DESC) as [Rank],Sum(Score)Score,UserId,NickName,Avater from
                (
                  SELECT  UserId, Date, Score,b.Name as NickName,c.RPath as Avater 
                  FROM dbo.SuperiorRecord a
                  join UserInfo b on b.Id=a.UserId
                  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=@ResourceType
                 )t
                 where 1=1   {0}
                 group by UserId,NickName,Avater", Tool.GetTimeWhere("Date", queryType));

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ResourceType", (int)ResourceTypeEnum.用户头像) };
                list = Util.ReaderToList<RankIntegralModel>(strsql, sp);
           
                //CacheHelper.SetCache<List<RankIntegralModel>>("GetIntegralListWebSite" + queryType, list, DateTime.Parse("23:59:59"));
                CacheHelper.SetCache<List<RankIntegralModel>>(cachekey, list, DateTime.Parse("23:59:59"));
            }

            ReturnMessageJson msgjson = new ReturnMessageJson();
            RankIntegralListModel rlist = new RankIntegralListModel();
            int userId = UserHelper.GetByUserId();
            RankIntegralModel my = list.Where(x => x.UserId == userId).FirstOrDefault();
            UserInfo u = UserHelper.GetUser(userId);
            u.Headpath = string.IsNullOrEmpty(u.Headpath) ? LuoUtil.DefaultAvater : u.Headpath;
            if (my != null)
            {
                my.Avater = u.Headpath;
            }
            else
            {
                my = new RankIntegralModel();

                my.Avater = u.Headpath;
                my.NickName = u.Name;
                my.Rank = 0;
                my.Score = 0;
                my.UserId = userId;
            }

            rlist.MyRankIntegralModel = my;
            rlist.RankIntegralList = list;
            msgjson.Success = true;
            msgjson.data = rlist;

            return Json(msgjson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取高手榜数据
        /// </summary>
        /// <param name="type">榜单类型</param>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public List<RankingList> GetSuperiorList(string type, int lType)
        {
            switch (type)
            {
                case "day":
                    return GetSuperiorListDay(lType, DateTime.Now);
                //case "week":
                //    return GetSuperiorListWeek(lType);
                //case "month":
                //    return GetSuperiorListMonth(lType);
                //case "all":
                //    return GetSuperiorListAll(lType);
                default:
                    return null;
            }
        }

        private List<RankingList> GetSuperiorListAll(int lType)
        {

            DateTime today = DateTime.Today;
            //string memberKey = "superior_" + lType + "_total_" + today.ToString("yyyyMMdd");
            string memberKey = string.Format(RedisKeyConst.Rank_SuperiorTotal, lType, today.ToString("yyyyMMdd")); //"rank:superior:" + lType + ":total:" + today.ToString("yyyyMMdd");

            // var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);
            var list = CacheHelper.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"
  select Top 100 row_number() over(order by Score DESC ) as [Rank], * from (
      SELECT  isnull(sum(a.Score),0) as Score,a.UserId, a.lType,b.Name as NickName,c.RPath as Avater 
      FROM dbo.SuperiorRecord a
      join UserInfo b on b.Id=a.UserId
      left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
      WHERE a.lType=@lType 
      GROUP BY a.lType,a.UserId,b.Name,c.RPath
  ) tt WHERE Score > 0";
                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType)
                    };

                list = Util.ReaderToList<RankingList>(sql, paras);
                if (list != null)
                {
                    //MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                    CacheHelper.AddCache(memberKey, list, 60 * 24);
                }
            }

            return list;


        }

        private List<RankingList> GetSuperiorListMonth(int lType)
        {
            DateTime today = DateTime.Today;
            DateTime endTime = today.GetMonthStart();
            DateTime beginTime = endTime.AddMonths(-1);

            //string memberKey = "superior_" + lType + "_month_" + beginTime.Month;
            string memberKey = string.Format(RedisKeyConst.Rank_SuperiorMonth, lType, beginTime.Month); //"rank:superior:" + lType + ":month:" + beginTime.Month;

            //var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);
            var list = CacheHelper.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"
  select Top 100 row_number() over(order by Score DESC ) as [Rank], * from (
      SELECT  isnull(sum(a.Score),0) as Score,a.UserId, a.lType,b.Name as NickName,c.RPath as Avater 
      FROM dbo.SuperiorRecord a
      join UserInfo b on b.Id=a.UserId
      left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
      WHERE a.lType=@lType AND a.[Date] Between @StartDate and @EndDate
      GROUP BY a.lType,a.UserId,b.Name,c.RPath
  ) tt WHERE Score > 0";

                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@StartDate",beginTime),
                        new SqlParameter("@EndDate",endTime)
                    };
                list = Util.ReaderToList<RankingList>(sql, paras);

                if (list != null)
                {
                    //MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                    CacheHelper.AddCache(memberKey, list, 60 * 24);
                }

            }

            return list;
        }

        private List<RankingList> GetSuperiorListWeek(int lType)
        {
            DateTime today = DateTime.Today;

            DateTime beginTime = today.GetWeekStart().AddDays(-7);
            DateTime endTime = beginTime.AddDays(7);

            //string memberKey = "superior_" + lType + "_week_" + beginTime.GetWeekOfYear();
            string memberKey = string.Format(RedisKeyConst.Rank_SuperiorWeek, lType, beginTime.GetWeekOfYear()); //"rank:superior:" + lType + ":week:" + beginTime.GetWeekOfYear();

            //var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);
            var list = CacheHelper.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"
  select Top 100 row_number() over(order by Score DESC ) as [Rank], * from (
      SELECT  isnull(sum(a.Score),0) as Score,a.UserId, a.lType,b.Name as NickName,c.RPath as Avater 
      FROM dbo.SuperiorRecord a
      join UserInfo b on b.Id=a.UserId
      left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
      WHERE a.lType=@lType AND a.[Date] Between @StartDate and @EndDate
      GROUP BY a.lType,a.UserId,b.Name,c.RPath
  ) tt WHERE Score > 0";

                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@StartDate",beginTime),
                        new SqlParameter("@EndDate",endTime)
                    };
                list = Util.ReaderToList<RankingList>(sql, paras);

                if (list != null)
                    //MemClientFactory.WriteCache(memberKey, list, 60 * 24 * 7);
                    CacheHelper.AddCache(memberKey, list, 60 * 24 * 7);
            }

            return list;
        }

        /// <summary>
        /// 高手日榜
        /// </summary>
        /// <returns></returns>
        private List<RankingList> GetSuperiorListDay(int lType, DateTime nowDate)
        {
            string date = nowDate.AddDays(-1).ToString("yyyy-MM-dd");
            string memberKey = string.Format(RedisKeyConst.Rank_SuperiorDay, lType, nowDate.ToString("yyyyMMdd")); //"rank:superior:" + lType + ":day:" + nowDate.ToString("yyyyMMdd");

            //var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);
            var list = CacheHelper.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
  FROM dbo.SuperiorRecord a
  join UserInfo b on b.Id=a.UserId
  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
  WHERE a.lType=@lType AND a.[Date]=@Date AND a.Score>0";
                SqlParameter[] paras = {
                        new SqlParameter("@lType",SqlDbType.Int),
                        new SqlParameter("@Date",SqlDbType.Date)
                    };
                paras[0].SqlValue = lType;
                paras[1].SqlValue = date;

                list = Util.ReaderToList<RankingList>(sql, paras);

                if (list != null)
                {
                    //MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                    CacheHelper.AddCache(memberKey, list, 60 * 24);
                }
            }

            return list;

            //          bool flag = CacheHelper.IsExistCache(memberKey);
            //          if (flag)
            //          {
            //              string key = "superior_" + lType + "_day_" + nowDate.AddDays(-1).ToString("yyyyMMdd");
            //              DelCache(key);
            //              key = "superior_" + lType + "_day_" + nowDate.AddDays(-2).ToString("yyyyMMdd");
            //              DelCache(key);
            //              return MemClientFactory.GetCache<List<RankingList>>(key);
            //          }
            //          else
            //          {
            //              DateTime refreshTime = Convert.ToDateTime(date + " 06:00:00");
            //              if (nowDate > refreshTime)
            //              {
            //                  string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
            //FROM dbo.SuperiorRecord a
            //left join UserInfo b on b.Id=a.UserId
            //left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
            //WHERE a.lType=@lType AND a.[Date]=@Date";
            //                  SqlParameter[] paras = {
            //                      new SqlParameter("@lType",SqlDbType.Int),
            //                      new SqlParameter("@Date",SqlDbType.Date)
            //                  };
            //                  paras[0].SqlValue = lType;
            //                  paras[1].SqlValue = date;

            //                  var list = Util.ReaderToList<RankingList>(sql, paras);

            //                  MemClientFactory.WriteCache(memberKey, list, 60 * 24);
            //                  return list;
            //              }
            //              else
            //              {
            //                  DateTime yesterday = Convert.ToDateTime(nowDate.AddDays(-1).ToString("yyyy-MM-dd") + " 07:00:00");
            //                  return GetSuperiorListDay(lType, yesterday);
            //              }
            //          }
        }

        private void DelCache(string key)
        {
            //if (CacheHelper.IsExistCache(key))
            //{
            //    CacheHelper.DeleteCache(key);
            //}
            CacheHelper.DeleteCache(key);
        }

        private RankingList GetUserRankingInfo(int lType, string queryType)
        {
            DateTime today = DateTime.Today;
            string sqlWhere = "";
            switch (queryType)
            {
                case "day":
                    sqlWhere = string.Format(" and Date='{0}'", today.AddDays(-1));
                    break;
                case "week":
                    DateTime endTime = today.GetWeekStart();
                    DateTime beginTime = today.GetWeekStart().AddDays(-7);
                    sqlWhere = string.Format(" and Date between '{0}' and '{1}'", beginTime, endTime);
                    break;
                case "month":
                    endTime = today.GetMonthStart();
                    beginTime = endTime.AddMonths(-1);
                    sqlWhere = string.Format(" and Date between '{0}' and '{1}'", beginTime, endTime);
                    break;
                case "all":
                    break;
            }

            int userId = UserHelper.GetByUserId();
            string sql = string.Format(@"SELECT isnull(sum(Score),0)
  FROM dbo.SuperiorRecord
  WHERE lType={0} and UserId={1}{2}", lType, userId, sqlWhere);

            object score = SqlHelper.ExecuteScalar(sql);

            var user = UserHelper.GetUser(userId);

            return new RankingList()
            {
                UserId = userId,
                Avater = user.Headpath,
                LType = lType,
                NickName = user.Name,
                Score = score.ToInt32()
            };
        }
    }
}
