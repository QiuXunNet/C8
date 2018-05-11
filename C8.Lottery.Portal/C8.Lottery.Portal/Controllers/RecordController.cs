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
                    currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    date = DateTime.Now.ToString("MM月dd日");
                }
                else
                {
                    currentDate = date;
                    string[] dateStrs = date.Split('-');
                    date = new DateTime(int.Parse(dateStrs[0]), int.Parse(dateStrs[1]), int.Parse(dateStrs[2])).ToString("MM月dd日");
                }  

                string time1 = currentDate;
                string time2 = currentDate + " 23:59:59";

                sql = "select * from LotteryRecord where lType=" + lType + " and SubTime >'" + time1 + "' and SubTime < '" + time2 + "' order by Issue desc";
            }
            else
            {
                
                if (string.IsNullOrEmpty(date))
                {
                    currentDate = DateTime.Now.ToString("yyyy");
                    date = DateTime.Now.ToString("yyyy年");
                   
                }
                else
                {
                    currentDate = date;
                    date = date.Substring(0, 4) +"年";
                }
                var time1 = currentDate.Substring(0, 4);
                sql = "select * from LotteryRecord where lType=" + lType + " and YEAR(SubTime) ='" + time1 + "' order by Issue desc";
            }

          
            ViewBag.currentDate = currentDate;

            //查询日期
            ViewBag.queryDate = Util.GetQueryDate(lType??0);

            ViewBag.list = Util.ReaderToList<LotteryRecord>(sql);

            ViewBag.date = date;

            return View();
        }

    }
}
