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
        public static List<LotteryTimeModel> GetLotteryTimeList()
        {
            var list = CacheHelper.GetCache<List<LotteryTimeModel>>("GetLotteryTimeListss");
            
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
            int nextLotteryTimeSeconds = 0; //下次开奖间隔秒数

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
            var list = GetLotteryTimeList();
            foreach (var model in list)
            {
                model.BeginTimeDate = Convert.ToDateTime(nowTimeStr + " " + model.BeginTime);
                model.EndTimeDate = Convert.ToDateTime(nowTimeStr + " " + model.EndTime);

                //如果开奖的开始时间大于了结束时间，说明时间跨天了，需要对开始结束时间重新处理
                if (model.BeginTimeDate > model.EndTimeDate)
                {
                    //如果当前时间小于23:59:59  说明还在跨天的前一天，则EndTime加一天
                    if (nowTime <= Convert.ToDateTime(nowTimeStr + " 23:59:59"))
                    {
                        model.EndTimeDate.AddDays(1);
                    }
                    //如果当前时间小于01:00:00  说明在跨天的后一天，则BeginTime减一天
                    if (nowTime <= Convert.ToDateTime(nowTimeStr + " 01:00:00"))
                    {
                        model.BeginTimeDate.AddDays(-1);
                    }
                }
            }

            lotteryTimeModel = list.FirstOrDefault(e => e.LType == lType &&
            nowTime > e.BeginTimeDate && nowTime < e.EndTimeDate && e.IsStop == "0");

            //如果当前时间不在配置的彩种开奖时间范围
            if (lotteryTimeModel == null)
            {
                var lotteryTimeModel2 = list.Where(e => e.LType == lType);
                if (!lotteryTimeModel2.Any())
                    return "找不到彩种";

                //如果只有一条记录
                if (lotteryTimeModel2.Count() == 1)
                {
                    var model = lotteryTimeModel2.FirstOrDefault();

                    if (nowTime < model.BeginTimeDate)
                    {
                        nextLotteryTimeSeconds = (int)(model.BeginTimeDate - nowTime).TotalSeconds;
                    }
                    else
                    {
                        nextLotteryTimeSeconds = (int)(model.BeginTimeDate.AddDays(1) - nowTime).TotalSeconds;
                    }
                }
                else //如果存在多条记录，则取离当前时间最近的一条
                {
                    var model = lotteryTimeModel2.Where(e=> nowTime < e.BeginTimeDate).OrderBy(e=>e.BeginTimeDate).FirstOrDefault();

                    if (model == null)
                    {
                        model = lotteryTimeModel2.Where(e => nowTime < e.BeginTimeDate.AddDays(1)).OrderBy(e => e.BeginTimeDate.AddDays(1)).FirstOrDefault();
                        model.BeginTimeDate.AddDays(1);
                    }

                    nextLotteryTimeSeconds = (int)(model.BeginTimeDate - nowTime).TotalSeconds;

                }
                return (nextLotteryTimeSeconds / 3600).ToString("00") + "&" + (nextLotteryTimeSeconds / 60).ToString("00") + "&" + (nextLotteryTimeSeconds % 60).ToString("00");
            }

            //获取当前时间已过首次开奖计时时间的秒数
            int timeLong = (int)(nowTime - lotteryTimeModel.BeginTimeDate).TotalSeconds;

            if (timeLong < 0)
            {
                return "休奖时间";
            }
            //时间间隔秒数
            int timeIntervalSeconds = int.Parse(lotteryTimeModel.TimeInterval) * 60;

            //if ((timeLong % timeIntervalSeconds) <= int.Parse(lotteryTimeModel.LotteryInterval))
            //{
            //    return "正在开奖";
            //}

            nextLotteryTimeSeconds = timeIntervalSeconds - (timeLong % timeIntervalSeconds);

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
        /// 
        /// </summary>
        public string BeginIssue { get; set; }

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
        /// 开奖开始时间
        /// </summary>
        public DateTime BeginTimeDate { get; set; }

        /// <summary>
        /// 开奖结束时间
        /// </summary>
        public DateTime EndTimeDate { get; set; }

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