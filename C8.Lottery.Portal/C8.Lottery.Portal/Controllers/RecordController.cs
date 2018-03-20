using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class RecordController : Controller
    {
        //
        // GET: /Record/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult OpenRecord(int? lType, string date)
        {

            //lType
            if (lType != null)
            {
                Session["OpenRecordlType"] = lType;
            }
            else
            {
                lType = (int)Session["OpenRecordlType"];
            }

            ViewBag.lType = lType;
            ViewBag.lotteryTypeName = Util.GetLotteryTypeName((int)lType);

            string currentDate = "";

           

            string sql = "";

            if (lType >= 9)
            {
                //date
                if (string.IsNullOrEmpty(date))
                {
                    currentDate = DateTime.Now.ToString("MM月dd");
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    currentDate = date;
                    date = "2017-" + date.Replace('月', '-');
                }


                ViewBag.date = date;
                ViewBag.currentDate = currentDate;

                //查询日期
                ViewBag.queryDate = Util.GetQueryDate();


                //list
                string time1 = date;
                string time2 = date + " 23:59:59";

                sql = "select * from LotteryRecord where lType=" + lType + " and SubTime >'" + time1 + "' and SubTime < '" + time2 + "' order by Issue desc";
            }
            else
            {
                int year = DateTime.Now.Year;
                string time1 = year + "-1-1";
                ViewBag.date = year;
                ViewBag.currentDate = year + "年";
                sql = "select * from LotteryRecord where lType=" + lType + " and SubTime >'" + time1 + "' order by Issue desc";
            }

            ViewBag.list = Util.ReaderToList<LotteryRecord>(sql);

            return View();
        }

    }
}
