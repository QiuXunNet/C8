using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace C8.Lottery.Public
{
    public static class LotteryTime
    {
        private static List<LotteryTimeModel> GetLotteryTimeList()
        {
            var list = CacheHelper.GetCache<List<LotteryTimeModel>>("GetLotteryTimeListss");
            list = null;
            if (list == null || list.Count == 0)
            {
                list = new List<LotteryTimeModel>();

                string path = HttpContext.Current.Server.MapPath("/XML/LotteryTime.xml");

                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNode root = xml.SelectSingleNode("/LotteryRule");
                XmlNodeList childlist = root.ChildNodes;


                foreach (XmlNode child in childlist)
                {
                    list.Add(new LotteryTimeModel()
                    {
                        BeginTime = child.Attributes["BeginTime"].Value,
                        EndTime = child.Attributes["EndTime"].Value,
                        IsStop = child.Attributes["IsStop"].Value,
                        LotteryInterval = child.Attributes["LotteryInterval"].Value,
                        LType = child.Attributes["LType"].Value,
                        Name = child.Attributes["Name"].Value,
                        TimeInterval = child.Attributes["TimeInterval"].Value
                    });
                }

                CacheHelper.AddCache("GetLotteryTimeListss", list, 8 * 60);
            }

            return list;
        }

        /// <summary>
        /// 根据彩种，获取彩种开奖倒计时间
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static string GetTime(string lType)
        {
            var nowTime = DateTime.Now;
            var nowTimeStr = nowTime.ToString("yyyy-MM-dd");
            var timeStr = nowTime.ToString("hh:mm:ss");

            if (int.Parse(lType) < 9)
            {
                #region 全国彩

                string sql = "select OpenLine from DateLine where lType = " + lType;
                DateTime target = (DateTime)SqlHelper.ExecuteScalar(sql);

                if (nowTime > target) return "正在开奖";

                return Util.GetTwoDateCha(nowTime, target);

                #endregion
            }

            LotteryTimeModel lotteryTimeModel;

            lotteryTimeModel = GetLotteryTimeList().FirstOrDefault(e => e.LType == lType &&
            nowTime > Convert.ToDateTime(nowTimeStr + " " + e.BeginTime) &&
            nowTime < (e.EndTime == "24:00" ? DateTime.Today.AddDays(1) : Convert.ToDateTime(nowTimeStr + " " + e.EndTime)) && e.IsStop == "0");

            //如果当前时间不在配置的彩种开奖时间范围
            if (lotteryTimeModel == null)
            {
                return "休奖时间";
            }

            //获取当前时间已过首次开奖计时时间的秒数
            int timeLong = (int)(nowTime - Convert.ToDateTime(nowTimeStr + " " + lotteryTimeModel.BeginTime)).TotalSeconds;

            if (timeLong < 0)
            {
                return "休奖时间";
            }
            //时间间隔秒数
            int timeIntervalSeconds = int.Parse(lotteryTimeModel.TimeInterval) * 60;

            if ((timeLong % timeIntervalSeconds) <= int.Parse(lotteryTimeModel.LotteryInterval))
            {
                return "正在开奖";
            }

            int nextLotteryTimeSeconds = timeIntervalSeconds - (timeLong % timeIntervalSeconds);

            return (nextLotteryTimeSeconds / 3600).ToString("00") + "&" + (nextLotteryTimeSeconds / 60).ToString("00") + "&" + (nextLotteryTimeSeconds % 60).ToString("00");
        }
    }

    public class LotteryTimeModel
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        public string LType { get; set; }

        /// <summary>
        /// 彩种名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开奖开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 开奖结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 开奖间隔(分)
        /// </summary>
        public string TimeInterval { get; set; }

        /// <summary>
        /// 摇奖时间(秒)
        /// </summary>
        public string LotteryInterval { get; set; }

        /// <summary>
        /// 是否停售
        /// </summary>
        public string IsStop { get; set; }
    }
}