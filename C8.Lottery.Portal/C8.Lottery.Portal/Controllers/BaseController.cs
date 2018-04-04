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
            string memKey = "base_news_type_" + ltype;
            var list = MemClientFactory.GetCache<IList<NewsType>>(memKey);

            if (list != null && list.Any()) return list;

            string newsTypeSql = "SELECT TOP 100 [Id],[TypeName],[ShowType],[lType],[SeoSubject],[SeoKeyword],[SeoDescription] FROM [dbo].[NewsType] WHERE [lType]=" + ltype + " AND [Layer]=" + layer + " ORDER BY SortCode ";
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

        /// <summary>
        /// 查询当前分类的玄机图库
        /// </summary>
        /// <param name="newsId">新闻Id</param>
        /// <param name="newsTitle">新闻标题</param>
        /// <returns></returns>
        protected IList<Gallery> GetGalleries(long newsId, string newsTitle)
        {
            string memKey = "base_gallery_id_" + newsId;
            var list = MemClientFactory.GetCache<IList<Gallery>>(memKey);

            if (list != null && list.Any()) return list;

            string sql = @"select a.Id, a.FullHead as Name,right(ISNULL(a.LotteryNumber,''),3) as Issue, c.RPath as Picture 
from News a
left join ResourceMapping c on c.FkId=a.Id and c.[Type]=1
where a.FullHead=@FullHead
order by a.LotteryNumber desc";

            var parameters = new[]
            {
                new SqlParameter("@FullHead",SqlDbType.NVarChar)
            };
            parameters[0].Value = newsTitle;

            list = Util.ReaderToList<Gallery>(sql, parameters) ?? new List<Gallery>();

            MemClientFactory.WriteCache(memKey, list);
            return list;
        }

        /// <summary>
        /// 获取彩种玩法
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        protected IList<Play> GetPlayNames(int ltype)
        {
            string memKey = "base_play_name_" + ltype;

            var list = MemClientFactory.GetCache<IList<Play>>(memKey);
            if (list != null && list.Any()) return list;

            string sql = "SELECT Id,lType,PlayName FROM IntegralRule WHERE ltype=" + ltype;
            list = Util.ReaderToList<Play>(sql);
            if (list != null)
            {
                MemClientFactory.WriteCache(memKey, list);
                return list;
            }

            return new List<Play>();
        }
    }
}
