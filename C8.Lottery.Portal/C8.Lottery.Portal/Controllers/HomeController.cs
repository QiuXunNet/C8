using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //1.最后一期开奖号码
            string sql = "";

            List<LotteryRecord> list = new List<LotteryRecord>();

            for (int i = 0; i < 65; i++)
            {
                sql = "select top(1)* from LotteryRecord where lType = " + (i + 1) + " order by Id desc";
                list.Add(Util.ReaderToModel<LotteryRecord>(sql));
            }

            ViewBag.openList = list;

            return View();
        }


        public ActionResult GetRemainOpenTimeByType(int lType)
        {
            string time = Util.GetOpenRemainingTimeWithHour(lType);
            string[] arr = time.Split('&');

            if (arr.Length == 3)
            {
                time = "<t class='hour'>" + arr[0] + "</t>:<t class='minute'>" + arr[1] + "</t>:<t class='second'>" + arr[2] + "</t>";
            }

            return Content(time);
        }
    }
}
