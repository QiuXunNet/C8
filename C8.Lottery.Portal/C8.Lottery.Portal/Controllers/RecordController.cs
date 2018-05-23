using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class RecordController : BaseController
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
            string time1 = "";
            string time2 = "";

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
                    //currentDate = date;
                    //string[] dateStrs = date.Split('-');
                    //date = new DateTime(int.Parse(dateStrs[0]), int.Parse(dateStrs[1]), int.Parse(dateStrs[2])).ToString("MM月dd日");
                    currentDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                    date = Convert.ToDateTime(date).ToString("MM月dd日");
                }  

                 time1 = currentDate;
                 time2 = currentDate + " 23:59:59";

                sql = "select * from LotteryRecord where lType=" + lType + " and SubTime >'" + time1 + "' and SubTime < '" + time2 + "' order by Issue desc";
            }
            else
            {
                
                if (string.IsNullOrEmpty(date))
                {
                    currentDate = DateTime.Now.ToString("yyyy");
                    date = DateTime.Now.ToString("yyyy年");
                    DateTime dt = DateTime.Now;
                    time1 = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
                    time2 = dt.Year + "-12-31 23:59:59";
                    sql = "select * from LotteryRecord where lType=" + lType + " and YEAR(SubTime) ='" + currentDate + "' order by Issue desc";

                }
                else
                {
                    //currentDate = date;
                    //date = date.Substring(0, 4) +"年";
                    currentDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                    date = Convert.ToDateTime(date).ToString("MM月dd日");
                    time1 = currentDate;
                    time2 = currentDate + " 23:59:59";
                    sql = "select * from LotteryRecord where lType=" + lType + " and SubTime >'" + time1 + "' and SubTime < '" + time2 + "' order by Issue desc";

                }
                //var time1 = currentDate.Substring(0, 4);

            }

          
            ViewBag.currentDate = currentDate;

            //查询日期
            ViewBag.queryDate = Util.GetQueryDate(lType??0);
            List<LotteryRecord> list = new List<LotteryRecord>();
            if (lType == 5)
            {
                list = CacheHelper.GetCache<List<LotteryRecord>>("6cairecords");
                list = null;
                if (list==null)
                {
                    sql = "select * from [dbo].[LotteryRecord] where lType=5";
                    list = Util.ReaderToList<LotteryRecord>(sql);
                    
                   
                    CacheHelper.AddCache("6cairecords", list, 1440);
                }

                list = list.Where(x => x.SubTime > Convert.ToDateTime(time1) && x.SubTime < Convert.ToDateTime(time2)).ToList();


            }
            else
            {
                list = Util.ReaderToList<LotteryRecord>(sql);
            }


            ViewBag.list = list;

            ViewBag.date = date;

            return View();
        }

    }
}
