using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 获取彩种分类
        /// </summary>
        /// <returns></returns>
        protected IList<LotteryType> GetLotteryTypeList()
        {

            var list = MemClientFactory.GetCache<IList<LotteryType>>("base_lottery_type");
            if (list == null)
            {
                list = Util.GetEntityAll<LotteryType>().OrderBy(x => x.SortCode).ToList();
                MemClientFactory.WriteCache("base_lottery_type", list);
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
            string memKey = $"base_news_type_{ltype}";
            var list = MemClientFactory.GetCache<IList<NewsType>>(memKey);

            if (list != null && list.Any()) return list;

            string newsTypeSql = $"SELECT TOP 100 [Id],[TypeName],[ShowType] FROM [dbo].[NewsType] WHERE [lType]={ltype} AND [Layer]={layer} ORDER BY SortCode ";
            list = Util.ReaderToList<NewsType>(newsTypeSql) ?? new List<NewsType>();

            MemClientFactory.WriteCache(memKey, list);
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
    }
}
