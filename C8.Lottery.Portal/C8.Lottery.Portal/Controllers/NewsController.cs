using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Model.Enum;
using C8.Lottery.Public;

namespace C8.Lottery.Portal.Controllers
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描 述：新闻控制器
    /// </summary>
    public class NewsController : BaseController
    {
        /// <summary>
        /// 新闻首页
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <param name="ntype">栏目类型Id</param>
        /// <returns></returns>
        public ActionResult Index(int id, int ntype = 0)
        {
            ViewBag.CurrentLotteryType = id;
            var lotteryTypeList = GetLotteryTypeList();
            //step1.查询彩种分类列表
            ViewBag.LotteryTypeList = lotteryTypeList;
            //step2.查询当前彩种下的新闻栏目
            var list = GetNewsTypeList(id);
            if (ntype > 0)
            {
                ViewBag.CurrentNewsTypeId = ntype;
            }
            else
            {
                ViewBag.CurrentNewsTypeId = list.Any() ? list.First().Id : 0;
            }
            ViewBag.NewsTypeList = list;

            var model = lotteryTypeList.FirstOrDefault(x => x.Id == id);

            return View(model);
        }

        /// <summary>
        /// 栏目列表
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <returns></returns>
        public ActionResult TypeList(int id)
        {
            ViewBag.CurrentLotteryType = id;
            var lotteryTypeList = GetLotteryTypeList();
            //step1.查询彩种分类列表
            ViewBag.LotteryTypeList = lotteryTypeList;
            //step2.查询当前彩种下的新闻栏目
            var list = GetNewsTypeList(id);
            //ViewBag.CurrentNewsTypeId = list.Any() ? list.First().Id : 0;
            ViewBag.NewsTypeList = list;

            var model = lotteryTypeList.FirstOrDefault(x => x.Id == id);

            return View("TypeList", model);
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult NewsList(int typeId, int pageIndex = 1, int pageSize = 20)
        {
            var newsType = Util.GetEntityById<NewsType>(typeId);
            if (newsType == null)
            {
                ViewBag.NewsList = new List<News>();
                return PartialView("NewsList");
            }

            //if (newsType.TypeName == "玄机图库")
            //{
            //    return NewsGalleryCategoryList(newsType.LType, typeId);
            //}

            string sql = @"SELECT * FROM ( 
SELECT row_number() over(order by SortCode ASC, ReleaseTime DESC ) as rowNumber,
[Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],(SELECT COUNT(1) FROM [dbo].[Comment] WHERE [PId]=Id) as CommentCount
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId ) T
WHERE rowNumber BETWEEN @Start AND @End";
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@Start",SqlDbType.Int),
                new SqlParameter("@End",SqlDbType.Int),
            };
            parameters[0].Value = typeId;
            parameters[1].Value = (pageIndex - 1) * pageSize + 1;
            parameters[2].Value = pageSize * pageIndex;
            var list = Util.ReaderToList<News>(sql, parameters) ?? new List<News>();

            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            list.ForEach(x =>
            {
                x.ThumbList = GetResources(sourceType, x.Id)
                                .Select(n => n.RPath).ToList();
            });

            ViewBag.NewsList = list;
            return PartialView("NewsList");
        }


        /// <summary>
        /// 图库类型列表
        /// </summary>
        /// <param name="ltype">彩种Id</param>
        /// <param name="newsTypeId">栏目Id</param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult NewsGalleryCategoryList(long ltype, int newsTypeId)
        {
            string sql = @" SELECT Max(a.Id) as Id, FullHead as Name, Max(a.LotteryNumber) as LastIssue,isnull(a.QuickQuery,'#') as QuickQuery
 from News  a
 left join NewsType b on b.Id= a.TypeId
 where a.TypeId=@NewsTypeId and b.lType=@LType 
 group by a.FullHead,a.QuickQuery
 order by a.QuickQuery";
            SqlParameter[] parameters =
            {
                new SqlParameter("@NewsTypeId",SqlDbType.Int),
                new SqlParameter("@LType",SqlDbType.BigInt)
            };
            parameters[0].Value = newsTypeId;
            parameters[1].Value = ltype;

            var list = Util.ReaderToList<GalleryType>(sql, parameters) ?? new List<GalleryType>();

            ViewBag.TypeList = list;
            ViewBag.ltype = ltype;

            //查询推荐图
            string recGallerySql = @" SELECT TOP 3 a.Id,FullHead as Name,LotteryNumber as Issue FROM News a 
 left join NewsType b on b.Id= a.TypeId
 where a.RecommendMark=1 and b.lType=" + ltype + " order by ModifyDate";
            var recGalleryList = Util.ReaderToList<Gallery>(recGallerySql);

            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            recGalleryList.ForEach(x =>
            {
                var pic = GetResources(sourceType, x.Id);
                if (pic.Count > 0)
                {
                    x.Picture = pic[0].RPath;
                }
            });
            ViewBag.RecommendGalleryList = recGalleryList;

            return PartialView("NewsGalleryCategoryList");
        }

        /// <summary>
        /// 开奖时间
        /// </summary>
        /// <returns></returns>
        public ActionResult LotteryTime()
        {
            return View();
        }

        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewsDetail(int id)
        {
            var model = Util.GetEntityById<News>(id);

            #region 上一篇 下一篇
            //查询上一篇
            string sql = @"SELECT TOP 1
[Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle]
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND [Id] < @CurrentId 
ORDER BY Id DESC";
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@CurrentId",SqlDbType.Int),
            };
            parameters[0].Value = model.TypeId;
            parameters[1].Value = id;
            var preview = Util.ReaderToList<News>(sql, parameters);

            if (preview != null && preview.Count > 0)
            {
                ViewBag.Preview = preview[0];
            }

            //查询下一篇
            string nextsql = @"SELECT TOP 1
[Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle]
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND [Id] > @CurrentId ";
            SqlParameter[] nextparameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@CurrentId",SqlDbType.Int),
            };
            nextparameters[0].Value = model.TypeId;
            nextparameters[1].Value = id;
            var nextview = Util.ReaderToList<News>(nextsql, nextparameters);

            if (nextview != null && nextview.Count > 0)
            {
                ViewBag.Nextview = nextview[0];
            }
            #endregion

            return View(model);
        }

        /// <summary>
        /// 玄机图库浏览页
        /// </summary>
        /// <param name="id">新闻Id</param>
        /// <returns></returns>
        public ActionResult Gallery(int id)
        {
            var news = Util.GetEntityById<News>(id);
            var model = new Gallery()
            {
                Id = news.Id,
                Issue = news.Issue,
                Name = news.FullHead
            };
            //查询当前图库所有期信息
            var galleryList = GetGalleries(news.Id, news.FullHead);
            ViewBag.GalleryList = galleryList;

            //查询推荐图
            string recGallerySql = @" SELECT TOP 10 a.Id,FullHead as Name,LotteryNumber as Issue FROM News a 
 join NewsType b on b.Id= a.TypeId
 where  b.lType in
 (select ltype from News a join NewsType b on b.Id=a.TypeId
 where a.Id=" + id + @" )
 and a.RecommendMark=1
 order by ModifyDate";
            var recGalleryList = Util.ReaderToList<Gallery>(recGallerySql);
            ViewBag.RecommendGalleryList = recGalleryList;

            return View(model);
        }

        public PartialViewResult WonderfulComment(int id)
        {
            string sql =
                @"select top 3  a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater,(select count(1) from LikeRecord where [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.ArticleId = @ArticleId and a.IsDeleted = 0 
  order by StarCount desc";
            var parameters = new[]
            {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.BigInt),
                new SqlParameter("@ArticleId",SqlDbType.BigInt),
            };
            var user = Session["UserInfo"] as UserInfo;
            parameters[0].Value = user.Id;
            parameters[1].Value = (int)ResourceTypeEnum.用户头像;
            parameters[2].Value = id;

            var list = Util.ReaderToList<Comment>(sql, parameters);
            list.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = "~/images/default_avater.png";
                }
            });

            ViewBag.ArticleId = id;
            return PartialView(list);
        }

    }


}
