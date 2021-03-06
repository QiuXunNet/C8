﻿using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace C8.Lottery.Portal.api
{
    public class HomeApiController : ApiController
    {
        /// <summary>
        /// 根据父彩种获取子彩种列表
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public string GetChildLotteryType(int pId)
        {
            string sql = "";
            string cachekey = string.Format(RedisKeyConst.Home_ChildLotteryType, pId);  //"home:child_lottery_type:" + pId + ":website";
            //List<LotteryType2> list = CacheHelper.GetCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pId);
            List<LotteryType2> list = CacheHelper.GetCache<List<LotteryType2>>(cachekey);
            if (list == null)
            {
                sql = "select * from LotteryType2 where PId = " + pId + " order by Position";

                list = Util.ReaderToList<LotteryType2>(sql);

                //CacheHelper.AddCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pId, list, 30 * 24 * 60);
                CacheHelper.AddCache<List<LotteryType2>>(cachekey, list, 30 * 24 * 60);
            }

            string lTypes = "";
            list.ForEach(e => lTypes += e.lType + ",");
            lTypes = lTypes.Trim(',');

            var dateTime = DateTime.Now.AddDays(-5);

            sql = @"select lr.*,l.lType as BigLType from LotteryRecord lr
                    join(
                    select lType, max(SubTime) SubTime from lotteryRecord where lType in(" + lTypes + ") and SubTime >'" + dateTime + @"' group by lType
                    ) tab on lr.lType = tab.lType and lr.SubTime = tab.SubTime
                    left join LotteryType2 lt on lr.lType = lt.lType
                    left join Lottery l on lr.lType = l.LotteryCode
                    order by Position";

            var newList = new List<LotteryRecordToJson>();
            List<LotteryRecordToJson> rmcList = null;

            string rmccachekey = string.Format(RedisKeyConst.Home_IndexLotteryList, pId); //"home:index_Lottery_List:" + pId;
            //rmcList = CacheHelper.GetCache<List<LotteryRecordToJson>>("GetIndexLotteryList_" + pId);
            rmcList = CacheHelper.GetCache<List<LotteryRecordToJson>>(rmccachekey);
            if (rmcList == null)
            {
                Util.ReaderToList<LotteryRecord>(sql).ForEach(e =>
                {
                    LotteryRecordToJson newModel = new LotteryRecordToJson();
                    newModel.OpenNum = e.Num;
                    newModel.Issue = e.Issue;
                    newModel.OpenTime = e.ShowOpenTime;
                    newModel.OpenNumAlias = "";
                    newModel.CurrentIssue = "";
                    newModel.LType = e.lType;
                    newModel.LTypeName = Util.GetLotteryTypeName(e.lType);
                    newModel.Logo = Util.GetLotteryIcon(e.lType);
                    newModel.BigLType = e.BigLType;

                    newList.Add(newModel);
                });

                //如果是热门彩，则写入缓存
                if (pId == 1)
                {
                    //CacheHelper.AddCache<List<LotteryRecordToJson>>("GetIndexLotteryList_" + pId, newList, 24 * 60);
                    CacheHelper.AddCache<List<LotteryRecordToJson>>(rmccachekey, newList, 24 * 60);
                }
                else
                {
                    //CacheHelper.AddCache<List<LotteryRecordToJson>>("GetIndexLotteryList_" + pId, newList, 10*60);
                    CacheHelper.AddCache<List<LotteryRecordToJson>>(rmccachekey, newList, 10 * 60);
                }
            }
            else
            {
                rmcList.ForEach(e =>
                {
                    e.OpenTime = LotteryTime.GetTime(e.LType.ToString());
                });
                newList = rmcList;
            }

            return newList.ToJsonString();
        }
    }
}
