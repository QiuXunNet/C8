﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
using System.Configuration;
using System.Web.Routing;

namespace C8.Lottery.Portal.Controllers
{
    public class BaseController : Controller
    {




        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            string osshost = LuoUtil.OssHost;
            string ishttps = ConfigurationManager.AppSettings["ishttps"];
            ViewBag.osshost = osshost;
            ViewBag.ishttps = ishttps;
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 获取站点配置
        /// </summary>
        /// <returns></returns>
        protected SiteSetting GetSiteSetting()
        {
            //var setting = MemClientFactory.GetCache<SiteSetting>("base_site_setting");
            string cachekey = RedisKeyConst.Base_SiteSetting; //"base:site_setting:site";
            var setting = CacheHelper.GetCache<SiteSetting>(cachekey);

            if (setting == null)
            {
                string sql = "select top 1 * from dbo.SiteSetting";
                setting = Util.ReaderToList<SiteSetting>(sql).FirstOrDefault();
                if (setting != null)
                    // MemClientFactory.WriteCache("base_site_setting", setting, 60 * 24);
                    CacheHelper.AddCache(cachekey, setting, 60 * 24 * 1000);
            }
            return setting ?? new SiteSetting();
        }

        /// <summary>
        /// 获取彩种分类
        /// </summary>
        /// <returns></returns>
        protected IList<LotteryType> GetLotteryTypeList()
        {

            // IList<LotteryType> list = MemClientFactory.GetCache<IList<LotteryType>>("base_lottery_type");
            string cachekey = RedisKeyConst.Base_LotteryType; //"base:lottery_type:type";
            IList<LotteryType> list = CacheHelper.GetCache<IList<LotteryType>>(cachekey);
            if (list == null)
            {
                list = Util.GetEntityAll<LotteryType>().OrderBy(x => x.SortCode).ToList();
                //MemClientFactory.WriteCache("base_lottery_type", list);
                CacheHelper.AddCache(cachekey, list);
                return list;
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// 获取新闻栏目分类列表
        /// </summary>
        /// <param name="ltype">彩种Id</param>
        /// <param name="layer">查询层级</param>
        /// <returns></returns>
        protected IList<NewsType> GetNewsTypeList(long ltype, int layer = 1)
        {
            string memKey = string.Format(RedisKeyConst.Base_NewsType, ltype); //"base:news_type:" + ltype;
            // IList<NewsType> list = MemClientFactory.GetCache<IList<NewsType>>(memKey);
            IList<NewsType> list = CacheHelper.GetCache<IList<NewsType>>(memKey);

            if (list != null && list.Any()) return list;

            string newsTypeSql = "SELECT TOP 100 [Id],[TypeName],[ShowType],[lType],[SeoSubject],[SeoKeyword],[SeoDescription] FROM [dbo].[NewsType] WHERE [lType]=" + ltype + " AND [Layer]=" + layer + " ORDER BY SortCode ";
            list = Util.ReaderToList<NewsType>(newsTypeSql) ?? new List<NewsType>();

            //MemClientFactory.WriteCache(memKey, list);
            CacheHelper.AddCache(memKey, list);
            return list;

        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="fkId">关联Id</param>
        /// <returns></returns>
        protected List<ResourceMapping> GetResources(int type, long fkId)
        {
            string thumbSql = @"SELECT * FROM ResourceMapping WHERE [Type]=@type and [FKId]=@FKId ";
            SqlParameter[] parameters =
            {
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@FKId",SqlDbType.BigInt),
            };
            parameters[0].Value = type;
            parameters[1].Value = fkId;

            return Util.ReaderToList<ResourceMapping>(thumbSql, parameters) ?? new List<ResourceMapping>();
        }

        /// <summary>
        /// 查询当前分类的玄机图库
        /// </summary>
        /// <param name="newsId">新闻Id</param>
        /// <param name="newsTitle">新闻标题</param>
        /// <returns></returns>
        protected IList<Gallery> GetGalleries(long newsId, string newsTitle, int lType)
        {
            string memKey = string.Format(RedisKeyConst.Base_GalleryId, newsId); //"base:gallery_id:" + newsId;
            //var list = MemClientFactory.GetCache<IList<Gallery>>(memKey);
            var list = CacheHelper.GetCache<IList<Gallery>>(memKey);

            if (list != null && list.Any()) return list;

            string sql = @"select a.Id, a.FullHead as Name,a.[TypeId],right(ISNULL(a.LotteryNumber,''),3) as Issue, c.RPath as Picture 
from News a
left join ResourceMapping c on c.FkId=a.Id and c.[Type]=1
where a.FullHead=@FullHead and a.[TypeId]=@TypeId and DeleteMark=0 and EnabledMark=1  
order by a.LotteryNumber desc";

            var parameters = new[]
            {
                new SqlParameter("@FullHead",SqlDbType.NVarChar),
                new SqlParameter("@TypeId",SqlDbType.Int)
            };
            parameters[0].Value = newsTitle;
            parameters[1].Value = lType;

            list = Util.ReaderToList<Gallery>(sql, parameters) ?? new List<Gallery>();

            // MemClientFactory.WriteCache(memKey, list);
            CacheHelper.AddCache(memKey, list);
            return list;
        }

        /// <summary>
        /// 获取彩种玩法
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        protected IList<Play> GetPlayNames(int ltype)
        {
            string memKey = string.Format(RedisKeyConst.Base_PlayName, ltype); //"base:play_name:" + ltype;

            //var list = MemClientFactory.GetCache<IList<Play>>(memKey);
            var list = CacheHelper.GetCache<IList<Play>>(memKey);
            if (list != null && list.Any()) return list;

            string sql = "SELECT Id,lType,PlayName FROM IntegralRule WHERE ltype=" + ltype;
            list = Util.ReaderToList<Play>(sql);
            if (list != null)
            {
                //MemClientFactory.WriteCache(memKey, list);
                CacheHelper.AddCache(memKey, list);
                return list;
            }

            return new List<Play>();
        }

        /// <summary>
        /// 获取贴子点阅扣费配置表
        /// </summary>
        /// <returns></returns>
        protected IList<LotteryCharge> GetLotteryCharge()
        {
            string memKey = RedisKeyConst.Base_LotteryChargeSettings; //"base:lottery_charge_settings:set";
            //var list = MemClientFactory.GetCache<IList<LotteryCharge>>(memKey);
            var list = CacheHelper.GetCache<IList<LotteryCharge>>(memKey);
            if (list != null && list.Any()) return list;

            string sql = "SELECT Id,lType,MinIntegral,MaxIntegral,Coin FROM dbo.LotteryCharge";
            list = Util.ReaderToList<LotteryCharge>(sql);

            if (list != null)
            {
                // MemClientFactory.WriteCache(memKey, list);
                CacheHelper.AddCache(memKey, list);
                return list;
            }

            return new List<LotteryCharge>();
        }

        /// <summary>
        /// 获取分佣配置
        /// </summary>
        /// <returns></returns>
        protected IList<CommissionSetting> GetCommissionSetting()
        {
            string memKey = RedisKeyConst.Base_CommissionSettings; //"base:commission_settings:set";
            // var list = MemClientFactory.GetCache<IList<CommissionSetting>>(memKey);
            var list = CacheHelper.GetCache<IList<CommissionSetting>>(memKey);
            if (list == null || list.Count < 1)
            {
                string sql = "SELECT [Id],[lType],[Percentage],[Type] FROM [dbo].[SharedRevenue] WHERE IsDeleted=0";
                list = Util.ReaderToList<CommissionSetting>(sql);
                if (list != null)
                {
                    //MemClientFactory.WriteCache(memKey, list, 60);
                    CacheHelper.AddCache(memKey, list, 60);
                }
            }

            return list ?? new List<CommissionSetting>();
        }






        /// <summary>
        /// 获取cdn js地址
        /// </summary>
        /// <returns></returns>
        protected void GetWebHot()
        {


            string osshost = LuoUtil.OssHost;

            HttpContext.Application["osshost"] = osshost;

        }
    }
}
