﻿using System;
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


            if (lType <= 8 || lType == 10 || lType == 39 || lType == 54 || lType == 63 || lType == 65)

            {
                //期号一直递增,获取最后一次开奖号码+1
                return Util.GetPK10Issue(lType);
            }


            //期号按天递增
            DateTime nowTime = queryTime;
            string dateStr = nowTime.ToString("yyyyMMdd");
            //step1.查询当前彩种开奖配置
            string lotteryType = lType.ToString();

            var lotteryTimeModel = LotteryTime.GetLotteryModel(lotteryType);


            int intervalCount = 0;
            //step2.判断是否获取到开奖配置，未获取到则查询最近的将要开奖的配置
            if (lotteryTimeModel == null)
            {
                lotteryTimeModel = LotteryTime.GetModelUseIssue(lotteryType);
                intervalCount = lotteryTimeModel.BeginIssue.ToInt32();
            }
            else
            {
                //获取当前阶段初始期号
                if (lType == 9)
                {
                    intervalCount += lotteryTimeModel.BeginIssue.ToInt32();
                }
                //step3.获取该彩种的开奖间隔时长。并是否小于等于0, true则返回空

                //获取当前彩种开奖间隔时长(毫秒）
                int lotteryInterval = int.Parse(lotteryTimeModel.TimeInterval) * 60000;
                if (lotteryInterval == 0) return dateStr;

                //step4.获取当前彩种当前阶段开始时间，并计算当前时间与开始时间的间隔（秒）

                //获取当前彩种当前阶段开始时间（重庆时时彩 9 会分多个阶段）
                //DateTime lotteryBeginTime = DateTime.Parse(lotteryTimeModel.BeginTime);
                DateTime lotteryBeginTime = lotteryTimeModel.BeginTimeDate;


                #region 处理重庆时时彩 重庆快乐十分跨天的期号 凌晨为第一期
                if ((lType == 9 || lType==51) && lotteryTimeModel.BeginTimeDate.Day != lotteryTimeModel.EndTimeDate.Day)
                {
                    //处理 0点到2点
                    if (nowTime > lotteryTimeModel.EndTimeDate.Date)
                    {
                        intervalCount = 0;
                        lotteryBeginTime = lotteryTimeModel.EndTimeDate.Date;
                    }

                }
                #endregion

                var intervalTimeSpan = (nowTime - lotteryBeginTime);
                //获取当前时间与开始时间间隔（秒）
                int intervalMilliseconds = (int)intervalTimeSpan.TotalMilliseconds;

                //step5.计算当前第几期
                intervalCount += (int)(intervalMilliseconds / lotteryInterval);
                if (intervalMilliseconds % lotteryInterval != 0)
                {
                    intervalCount += 1;
                }
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


        public static string GetOpenRemainingTime(int lType)
        {

            DateTime d = DateTime.Now;
            if (lType < 9)
            {
                #region 3D 双色球 七星彩 大乐透 六合彩 排3 排5 七乐彩

                string sql = "select OpenLine from DateLine where lType = " + lType;
                DateTime target = (DateTime)SqlHelper.ExecuteScalar(sql);

                if (d > target) return "正在开奖";

                return Util.GetTwoDateCha(d, target);

                #endregion
            }
            else if (lType >= 9 && lType < 15)
            {
                #region 时时彩
                //8点-10点
                var list = LotteryTime.GetLotteryTimeList().Where(x => x.LType == lType.ToString());

                #endregion
            }

            return string.Empty;
        }


        /// <summary>
        /// 查询当前彩种封盘倒计时
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static string GetRemainingTime(int lType)
        {
            DateTime d = DateTime.Now;

            #region 49彩 七星彩 3D 排3 排5 七乐彩
            if (lType < 9)
            {
                //查询设置的开奖时间，进行比较

                string sql = "select OpenLine from DateLine where lType = " + lType;
                DateTime target = (DateTime)SqlHelper.ExecuteScalar(sql);

                if (d > target) return "正在开奖";

                return CompareTime(d, target);

            }
            #endregion


            int hour = d.Hour;
            int min = d.Minute;
            int sec = d.Second;

            #region 时时彩

            var lotterySetting = LotteryTime.GetModelUseIssue(lType.ToString());
            if (lType >= 9 && lType < 15)
            {

                //开奖前30秒封盘

                //step1.当期开奖时间和当前时间差，获取相差总时长（毫秒）
                TimeSpan diff = d - lotterySetting.BeginTimeDate;

                int totalMilliseconds = (int)diff.TotalMilliseconds;

                //step2.计算除数
                int divisorMilliseconds = lotterySetting.TimeInterval.ToInt32() * 60 * 1000;

                //step3.计算余数
                int diffCount = totalMilliseconds / divisorMilliseconds;
                int remainderMilliseconds = totalMilliseconds % divisorMilliseconds;

                //step4.判断是否封盘的30秒
                int disableMilliseconds = divisorMilliseconds - remainderMilliseconds;

                if (0 <= disableMilliseconds && disableMilliseconds <= 30000)
                {
                    return "已封盘";
                }

                //return GetDiffTime();

            }
            #endregion

            return string.Empty;
        }

        /// <summary>
        /// 比较时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns>相差时间字符串 (eg: 02&02&02)</returns>
        public static string CompareTime(DateTime time1, DateTime time2)
        {
            var diff = time2 - time1;

            return GetDiffTime(diff);
        }

        /// <summary>
        /// 获取时间差字符串
        /// </summary>
        /// <param name="diffTimsSpan"></param>
        /// <returns></returns>
        public static string GetDiffTime(TimeSpan diffTimsSpan)
        {
            string hour = ((int)diffTimsSpan.TotalHours).ToString("D2");
            string minute = diffTimsSpan.Minutes.ToString("D2");
            string seconds = diffTimsSpan.Seconds.ToString("D2");

            return $"{hour}&{minute}&{seconds}";
        }

        /// <summary>
        /// 转换倒计时模板
        /// </summary>
        /// <param name="milliseconds"></param>
        public static string ConvertTimeString(int milliseconds)
        {

            int seconds = milliseconds / 60000;
            int minute = milliseconds / 3600000;
            int hour = milliseconds / (3600000 * 24);

            return string.Empty;
        }

    }
}
