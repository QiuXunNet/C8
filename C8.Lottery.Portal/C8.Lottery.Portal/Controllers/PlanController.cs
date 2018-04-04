using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class PlanController : FilterController
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
        
    }
}
