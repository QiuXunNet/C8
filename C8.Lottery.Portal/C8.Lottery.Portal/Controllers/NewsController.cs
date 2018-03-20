﻿using System;
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
        public ActionResult Index(int id)
        {
            ViewBag.CurrentLotteryType = id;
            var lotteryTypeList = GetLotteryTypeList();
            //step1.查询彩种分类列表
            ViewBag.LotteryTypeList = lotteryTypeList;
            //step2.查询当前彩种下的新闻栏目
            var list = GetNewsTypeList(id);
            ViewBag.CurrentNewsTypeId = list.Any() ? list.First().Id : 0;
            ViewBag.NewsTypeList = list;

            var model = lotteryTypeList.FirstOrDefault(x => x.Id == id);

            return View(model);
        }
        
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

            return View("TypeList",model);
        }

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
[Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle] 
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

        public ActionResult NewsDetail(int id)
        {
            var model = Util.GetEntityById<News>(id);

            return View(model);
        }

    }


}
