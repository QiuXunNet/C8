using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

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
        public ActionResult Expert(int id)
        {
            //step1.查询彩种分类列表
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            ViewBag.TypeList = list;
            //step2.查询具体彩种
            string lotteryListSql = @"select * from LotteryType2 where PId>0  order by Position ";
            var lotteryList = Util.ReaderToList<LotteryType2>(lotteryListSql).GroupBy(x => x.PId);
            ViewBag.LotteryList = lotteryList;

            //step3.查询高手榜列表



            //step2.查询当前
            return View();
        }

        /// <summary>
        /// 榜单数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lType"></param>
        /// <returns></returns>
        [Authentication]
        public ActionResult ExpertData(int lType)
        {
            //查询日榜
            var dailyList = GetSuperiorListDay(lType, DateTime.Today);
            ViewBag.DailyList = dailyList;

            //查询周榜

            //查询月榜

            //查询总榜
            return View();
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
            string memberKey = "superior_" + lType + "_total_" + today.ToString("yyyyMMdd");

            var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
  FROM dbo.SuperiorRecord a
  left join UserInfo b on b.Id=a.UserId
  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
  WHERE a.lType=@lType AND a.[Date]<@Date";
                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@Date",today)
                    };

                list = Util.ReaderToList<RankingList>(sql, paras);
                if (list != null)
                {
                    MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                }
            }

            return list;


        }

        private List<RankingList> GetSuperiorListMonth(int lType)
        {
            DateTime today = DateTime.Today;
            DateTime endTime = today.GetMonthStart();
            DateTime beginTime = endTime.AddMonths(-1);

            string memberKey = "superior_" + lType + "_month_" + beginTime.Month;

            var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
  FROM dbo.SuperiorRecord a
  left join UserInfo b on b.Id=a.UserId
  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
  WHERE a.lType=@lType AND a.[Date] Between @StartDate and @EndDate";

                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@StartDate",beginTime),
                        new SqlParameter("@EndDate",endTime)
                    };
                list = Util.ReaderToList<RankingList>(sql, paras);

                if (list.Any())
                {
                    MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                }

            }

            return list;
        }

        private List<RankingList> GetSuperiorListWeek(int lType)
        {
            DateTime today = DateTime.Today;

            DateTime beginTime = today.GetWeekStart().AddDays(-7);
            DateTime endTime = beginTime.AddDays(7);

            string memberKey = "superior_" + lType + "_week_" + beginTime.GetWeekOfYear();

            var list = MemClientFactory.GetCache<List<RankingList>>(memberKey);

            if (list == null || list.Count < 1)
            {
                string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
  FROM dbo.SuperiorRecord a
  left join UserInfo b on b.Id=a.UserId
  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
  WHERE a.lType=@lType AND a.[Date] Between @StartDate and @EndDate";

                SqlParameter[] paras = {
                        new SqlParameter("@lType",lType),
                        new SqlParameter("@StartDate",beginTime),
                        new SqlParameter("@EndDate",endTime)
                    };
                list = Util.ReaderToList<RankingList>(sql, paras);
                if (list.Any())
                    MemClientFactory.WriteCache(memberKey, list, 60 * 24 * 7);
            }

            return list;
        }

        /// <summary>
        /// 高手日榜
        /// </summary>
        /// <returns></returns>
        private List<RankingList> GetSuperiorListDay(int lType, DateTime nowDate)
        {
            string date = nowDate.ToString("yyyy-MM-dd");
            string memberKey = "superior_" + lType + "_day_" + nowDate.ToString("yyyyMMdd");
            bool flag = CacheHelper.IsExistCache(memberKey);
            if (flag)
            {
                string key = "superior_" + lType + "_day_" + nowDate.AddDays(-1).ToString("yyyyMMdd");
                DelCache(key);
                key = "superior_" + lType + "_day_" + nowDate.AddDays(-2).ToString("yyyyMMdd");
                DelCache(key);
                return MemClientFactory.GetCache<List<RankingList>>(key);
            }
            else
            {
                DateTime refreshTime = Convert.ToDateTime(date + " 06:00:00");
                if (nowDate > refreshTime)
                {
                    string sql = @"SELECT Top 100 row_number() over(order by Score DESC) as [Rank],a.*,b.Name as NickName,c.RPath as Avater 
  FROM dbo.SuperiorRecord a
  left join UserInfo b on b.Id=a.UserId
  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
  WHERE a.lType=@lType AND a.[Date]=@Date";
                    SqlParameter[] paras = {
                        new SqlParameter("@lType",SqlDbType.Int),
                        new SqlParameter("@Date",SqlDbType.Date)
                    };
                    paras[0].SqlValue = lType;
                    paras[1].SqlValue = date;

                    var list = Util.ReaderToList<RankingList>(sql, paras);

                    MemClientFactory.WriteCache(memberKey, list, 60 * 24);
                    return list;
                }
                else
                {
                    DateTime yesterday = Convert.ToDateTime(nowDate.AddDays(-1).ToString("yyyy-MM-dd") + " 07:00:00");
                    return GetSuperiorListDay(lType, yesterday);
                }
            }
        }

        private void DelCache(string key)
        {
            if (CacheHelper.IsExistCache(key))
            {
                CacheHelper.DeleteCache(key);
            }
        }

        private RankingList GetUserRankingInfo(int lType, string queryType)
        {
            DateTime today = DateTime.Today;
            string sqlWhere = "";
            switch (queryType)
            {
                case "day":
                    sqlWhere = string.Format("Date='{0}'", today);
                    break;
                case "week":
                    DateTime endTime = today.GetWeekStart();
                    DateTime beginTime = today.GetWeekStart().AddDays(-7);
                    sqlWhere = string.Format("Date between '{0}' and '{1}'", beginTime, endTime);
                    break;
                case "month":
                    sqlWhere = string.Format("Date between '{0}' and '{1}'");
                    break;
                case "all":
                    sqlWhere = string.Format("Date < '{0}'", today);
                    break;
            }

            return null;
        }
    }
}
