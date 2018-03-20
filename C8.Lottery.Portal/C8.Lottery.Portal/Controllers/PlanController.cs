using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class PlanController : Controller
    {
        //
        // GET: /Plan/

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

    }
}
