using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace C8.Lottery.Portal.api
{
    public class LhcNewsController : ApiController
    {
        /// <summary>
        /// 获取六合彩开奖记录
        /// </summary>
        /// <returns></returns>
        public string GetLhcLotteryRecord()
        {
            string cachekey = "home:child_lottery_type:5:lhc";
            //LotteryRecrdToLhc model = CacheHelper.GetCache<LotteryRecrdToLhc>("GetLhcLotteryRecordWebSite1");
            LotteryRecrdToLhc model = CacheHelper.GetCache<LotteryRecrdToLhc>(cachekey);
            if (model == null)
            {
                string sql = "select top(1) Issue,Num from LotteryRecordToLhc";
                model = Util.ReaderToModel<LotteryRecrdToLhc>(sql);

                //CacheHelper.AddCache<LotteryRecrdToLhc>("GetLhcLotteryRecordWebSite1", model,24*60);
                CacheHelper.AddCache<LotteryRecrdToLhc>(cachekey, model, 24 * 60);
            }

            string time = LotteryTime.GetTime("5");
            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');
                model.Time = "<span id='openTime'><t id='hour2'>" + timeArr[0] + "</t>:<t id='minute2'>" + timeArr[1] + "</t>:<t id='second2'>" + timeArr[2] + "</t></span>";
            }
            else
            {
                model.Time = "<span id='openTime'>" + time + "</span>";
            }

            return model.ToJsonString();
        }
       
    }
}
