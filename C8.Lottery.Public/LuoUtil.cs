using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Public
{
    public class LuoUtil
    {
        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public static int GetUserIntegral(int userId, int lType)
        {
            string totalIntegralByLtypeSql = string.Format(@"select isnull(sum(score),0) 
        from dbo.BettingRecord where lType={0} and UserId={1} and WinState in(3,4)", lType, userId);
            object objTotalIntegral = SqlHelper.ExecuteScalar(totalIntegralByLtypeSql);
            int totalIntegral = objTotalIntegral != null ? Convert.ToInt32(objTotalIntegral) : 0;
            return totalIntegral;
        }

        /// <summary>
        /// 查询当前期号
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static string GetCurrentIssue(int lType)
        {
            DateTime nowTime = DateTime.Now;

            return GetCurrentIssue(lType, nowTime);
        }


        /// <summary>
        /// 查询指定时间的期号，（期号一直递增的则查询最新一期期号）
        /// </summary>
        /// <param name="lType">彩种Id</param>
        /// <param name="queryTime">查询时间</param>
        /// <returns></returns>
        public static string GetCurrentIssue(int lType, DateTime queryTime)
        {

            string issue = "";


            if (lType <= 8 || lType == 39 || lType == 54 || lType == 63 || lType == 65)

            {
                //期号一直递增
                return Util.GetPK10Issue(lType);
            }
            else
            {
                //期号按天递增


                DateTime nowTime = queryTime;
                string dateStr = nowTime.ToString("yyyyMMdd");
                //step1.查询当前彩种开奖配置
                string lotteryType = lType.ToString();
                var list = LotteryTime.GetLotteryTimeList().Where(x => x.LType == lotteryType);

                var lotteryTimeModel = list.FirstOrDefault(e => nowTime >= Convert.ToDateTime(e.BeginTime) &&
                                                    nowTime <
                                                    (e.EndTime == "24:00"
                                                        ? DateTime.Today.AddDays(1)
                                                        : Convert.ToDateTime(e.EndTime)) && e.IsStop == "0");

                //step2.判断是否获取到开奖配置，未获取到则返回已封盘
                if (lotteryTimeModel == null) return "已封盘";

                //step3.获取该彩种的开奖间隔时长。并是否小于等于0, true则返回空
                int intervalCount = 0;

                if (lType == 9)
                {
                    if (lotteryTimeModel.BeginTime == "09:50")
                    {
                        intervalCount = 24;//初始24期
                    }
                    else if (lotteryTimeModel.BeginTime == "22:00")
                    {
                        intervalCount = 96;//初始96期
                    }
                }
                else if (lType == 13)
                {
                    if (lotteryTimeModel.BeginTime == "00:00")
                    {
                        intervalCount = 83;//初始83期
                    }
                }
                else if (lType == 51)
                {
                    if (lotteryTimeModel.BeginTime == "09:53")
                    {
                        intervalCount = 12;//初始12期
                    }
                }
                else if (lType == 64)
                {
                    if (lotteryTimeModel.BeginTime == "00:00")
                    {
                        intervalCount = 131;//初始24期
                    }
                }

                //获取当前彩种开奖间隔时长(毫秒）
                int lotteryInterval = int.Parse(lotteryTimeModel.TimeInterval) * 60000;
                if (lotteryInterval == 0) return dateStr;

                //step4.获取当前彩种当前阶段开始时间，并计算当前时间与开始时间的间隔（秒）

                //获取当前彩种当前阶段开始时间（重庆时时彩 9，新疆时时彩 13，重庆快乐十分 51， 幸运飞艇 64 会分多个阶段）
                DateTime lotteryBeginTime = DateTime.Parse(lotteryTimeModel.BeginTime);
                //获取当前时间与开始时间间隔（秒）
                var intervalTimeSpan = (nowTime - lotteryBeginTime);
                int intervalMilliseconds = (int)intervalTimeSpan.TotalMilliseconds;

                //step5.计算当前第几期
                intervalCount += (int)(intervalMilliseconds / lotteryInterval);
                if (intervalMilliseconds % lotteryInterval != 0)
                {
                    intervalCount += 1;
                }
                //step6.判断彩种类型，返回不同长度的期号
                if ((lType >= 9 && lType <= 14) || lType == 64)
                {
                    issue = intervalCount.ToString("000");
                }
                else
                {
                    issue = intervalCount.ToString("00");
                }

                //step7.拼接当日期号
                string result = dateStr + issue;

                //step8.处理晚上 结束后的特殊情况
                string date = nowTime.ToString("yyyy-MM-dd");
                Util.HandIssueSpecial(lType, nowTime, date, issue, result);

                return result;

            }
        }

    }
}
