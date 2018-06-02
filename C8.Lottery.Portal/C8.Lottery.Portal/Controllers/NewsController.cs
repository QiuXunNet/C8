using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Model.Enum;
using C8.Lottery.Public;
using C8.Lottery.Portal.Models;
using Newtonsoft.Json;
using C8.Lottery.Portal.Business;
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
        private static NewsService _newsservice = new NewsService();
        /// <summary>
        /// 新闻首页（改） 20180518 ZZH
        /// </summary>
        /// <param name="id">彩种ID</param>
        /// <param name="ntype">栏目类型ID</param>
        /// <returns></returns>
        public ActionResult NewIndex(int id, int ntype = 0)
        {
            //NewsService _newsservice = new NewsService();
            ViewBag.SEOInfo = _newsservice.GetSEOInfo(id, ntype);
            int ChildlType = Util.GetlTypeById(id);
            string icon = Util.GetLotteryIcon(ChildlType);
            ViewBag.lType = id;
            ViewBag.ChannelId = ntype;
            ViewBag.icon = icon;
            ViewBag.ChildlType = ChildlType;
            ViewBag.CityId = Tool.GetCityId();
            return View();
        }

        /// <summary>
        /// 获取大彩种数据 20180518 ZZH
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadlTypesAndChannel(int lType, int clType)
        {
            //NewsService _newsservice = new NewsService();
            List<Business.BusinessData.LotteryType> list = _newsservice.GetLotteryTypeList();
            List<Business.BusinessData.LotteryNewsChannel> list2 = _newsservice.GetLotteryNewsChannelList(lType);
            Dictionary<string, string> dic = _newsservice.GetLastBetInfo(clType);
            return Json(new { TypeList = list, ChannelList = list2, betDic = dic });
        }

        [HttpPost]
        public JsonResult GetNewsList(int typeId, int pageIndex, int pageSize)
        {
            var newsType = Util.GetEntityById<NewsType>(typeId);
            if (newsType == null)
            {
                ViewBag.NewsList = new List<News>();
                return Json(new { total = 0, state = 0 });
            }
            string sql = @"SELECT  * ,
                            (SELECT    COUNT(1)
                              FROM[dbo].[Comment]
                              WHERE[ArticleId] = S.Id
                                        AND RefCommentId = 0
                            ) AS CommentCount,
                            STUFF((SELECT  ',' + RPath
                                    FROM    dbo.ResourceMapping
                                    WHERE   Type = 1
                                            AND FkId = S.Id
                                  FOR
                                    XML PATH('')
                                  ), 1, 1, '') AS ThumbListStr
                    FROM(SELECT *
                              FROM(SELECT    ROW_NUMBER() OVER(ORDER BY LotteryNumber DESC, ReleaseTime DESC) AS rowNumber,
                                                    [Id],
                                                    [FullHead],
                                                    [SortCode],
                                                    [ReleaseTime],
                                                    [ThumbStyle]
                                          FROM      dbo.[News]
                                          WHERE[TypeId] = @TypeId
                                                    AND DeleteMark = 0
                                                    AND EnabledMark = 1
                                        ) T
                              WHERE     T.rowNumber >= @Start
                                        AND T.rowNumber <= @End
                            ) S";
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
            return Json(new { total = 1, state = 0, data = list });
        }

        /// <summary>
        /// 获取最后一期开奖期号、号码
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLastBetInfo(int lType)
        {
            Dictionary<string, string> dic = _newsservice.GetLastBetInfo(lType);
            return Json(dic);
        }

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
                ViewBag.CurrentNewsType = list.FirstOrDefault(x => x.Id == ntype);
            }
            else
            {
                var currentNewsType = list.FirstOrDefault();
                ViewBag.CurrentNewsType = currentNewsType;
            }
            ViewBag.NewsTypeList = list;

            var model = lotteryTypeList.FirstOrDefault(x => x.Id == id);


            int lType = Util.GetlTypeById(id);
            ViewBag.lType = lType;

            //图标
            string icon = Util.GetLotteryIcon(lType);
            ViewBag.Icon = icon;

            //3.最后一期
            string sql = "";
            if (lType == 5) //如果是六合彩，则查询六合彩专用表
            {
                sql = "select top(1)* from LotteryRecordToLhc";
            }
            else
            {
                sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            }
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);

            ViewBag.lastIssue = lr.Issue;
            ViewBag.lastNum = lr.Num;

            //剩余时间
            //string time = Util.GetOpenRemainingTime(lType);
            string time = C8.Lottery.Public.LotteryTime.GetTime(lType.ToString());


            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');

                ViewBag.min = timeArr[1];
                ViewBag.sec = timeArr[2];

                if (lType < 9)
                {
                    ViewBag.hour = timeArr[0];
                }

            }
            //else
            //{
            ViewBag.time = time;
            // }

            ViewBag.CityId = Tool.GetCityId();



            return View(model);
        }

        /// <summary>
        /// 栏目列表
        /// </summary>
        /// <param name="id">彩种Id</param>
        /// <returns></returns>
        public ActionResult TypeList(int id)
        {
            var lotteryTypeList = GetLotteryTypeList();

            //step2.查询当前彩种下的新闻栏目
            var list = GetNewsTypeList(id)
                .Where(x => x.TypeName != "看图解码"
                    && x.TypeName != "幸运彩图"
                    && x.TypeName != "精选彩图"
                    && x.TypeName != "香港图库"
                    && x.TypeName != "香港挂牌"
                    && x.TypeName != "跑狗玄机"
                    ).ToList();

            var model = lotteryTypeList.FirstOrDefault(x => x.Id == id);

            string sql = "select top(1)* from LotteryRecordToLhc";
            LotteryRecord lr = Util.ReaderToModel<LotteryRecord>(sql);

            //剩余时间
            string time = C8.Lottery.Public.LotteryTime.GetTime(id.ToString());
            if (time != "正在开奖")
            {
                string[] timeArr = time.Split('&');
                ViewBag.time = "<span id='openTime'><t id='hour2'>" + timeArr[0] + "</t>:<t id='minute2'>" + timeArr[1] + "</t>:<t id='second2'>" + timeArr[2] + "</t></span>";
            }
            else
            {
                ViewBag.time = "<span id='openTime'>" + time + "</span>";
            }

            ViewBag.NewsTypeList = JsonConvert.SerializeObject(list.Select(e => new { e.Id, e.LType, e.TypeName }));
            ViewBag.lType = id;
            ViewBag.lastIssue = lr.Issue;
            ViewBag.lastNum = lr.Num;
            ViewBag.showInfo = lr.ShowInfo;
            ViewBag.LotteryTypeList = JsonConvert.SerializeObject(lotteryTypeList.Select(e => new { e.Id, e.TypeName }));
            ViewBag.CityId = Tool.GetCityId();

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
            NewsType newsType = _newsservice.GetNewTypeById(typeId);
            if (newsType == null)
            {
                ViewBag.NewsList = new List<News>();
                return PartialView("NewsList");
            }

            if (newsType.ShowType == 2)
            {
                return NewsGalleryCategoryList(newsType.LType, typeId);
            }

            ViewBag.NewsList = _newsservice.GetNewList(typeId, pageIndex, pageSize);

            ViewBag.AdList = _newsservice.GetAdvertisementList(typeId, 1);
            ViewBag.PageIndex = pageIndex;
            ViewBag.CityId = Tool.GetCityId();
            ViewBag.SiteSetting = GetSiteSetting();

            return PartialView("NewsList");
        }

        /// <summary>
        ///获取广告位
        /// </summary>
        /// <param name="location">栏目ID</param>
        /// <param name="adtype">广告类型 1=栏目 2=文章 3=六彩栏目</param>
        /// <returns></returns>
        public List<Advertisement> GetAdvertisementList(int location, int adtype)
        {
            string strsql = "";
            if (location == -1)//针对6彩栏目
            {
                strsql = string.Format(@"select  * from [dbo].[Advertisement] where charindex(',1,',','+[where]+',')>0 and 
                   AdType={1} and (State=1 or (State=0 and getdate()>=BeginTime and EndTime>getdate()))", location, adtype);

            }
            else
            {
                strsql = string.Format(@"select  * from [dbo].[Advertisement] where charindex(',1,',','+[where]+',')>0   
            and charindex(',{0},',','+[Location]+',')>0 and AdType={1} and (State=1 or (State=0 and getdate()>=BeginTime and EndTime>getdate()))", location, adtype);

            }


            List<Advertisement> list = Util.ReaderToList<Advertisement>(strsql);

            return list;
        }
        /// <summary>
        /// 获取广告位
        /// </summary>
        /// <param name="location"></param>
        /// <param name="adtype"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAdvertisementListJson(int location, int adtype)
        {
            ReturnMessageJson msg = new ReturnMessageJson();

            try
            {
                //List<Advertisement> list = GetAdvertisementList(location, adtype);
                //int adimgtype = (int)ResourceTypeEnum.广告图;
                //list.ForEach(j =>
                //{
                //    j.ThumbList = GetResources(adimgtype, j.Id).Select(z => z.RPath).ToList();
                //});
                List<Advertisement> list = _newsservice.GetAdvertisementList(location, adtype);
                msg.Success = true;
                msg.data = list;
            }
            catch (Exception e)
            {
                msg.Success = false;
                msg.Msg = e.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
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

            List<GalleryType> list = _newsservice.GetGalleryTypeList(ltype, newsTypeId);
            ViewBag.TypeList = list.Where(x => x.QuickQuery != "#").ToList();
            ViewBag.OtherTypeList = list.Where(x => x.QuickQuery == "#").ToList();
            ViewBag.ltype = ltype;

            //查询推荐图
            ViewBag.RecommendGalleryList = _newsservice.GetGalleryList(ltype, newsTypeId);

            ViewBag.AdList = _newsservice.GetAdvertisementList(newsTypeId, 1);
            ViewBag.CityId = Tool.GetCityId();

            return PartialView("NewsGalleryCategoryList");
        }

        /// <summary>
        /// 开奖时间
        /// </summary>
        /// <returns></returns>
        public ActionResult LotteryTime()
        {
            ViewBag.Platform = Request.Params["pl"].ToInt32();
            return View();
        }

        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewsDetail(int id)
        {
            AddPv(id);
            //            new Task(() =>
            //            {
            //                try
            //                {
            //                    string pvSql = @"if exists(
            //	select 1 from dbo.PageView where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
            //  )
            //  begin
            //   update dbo.PageView set ViewTotal+=1 where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
            //  end
            //  else
            //  begin
            //  insert into dbo.PageView(ViewDate,ViewTotal,[Type],FkId) values(GETDATE(),1,@Type,@Id)
            //  end;
            //UPDATE dbo.News SET PV+=1 WHERE Id=@Id";
            //                    var pvParam = new[]
            //                    {
            //                        new SqlParameter("@Type",1),//新闻类型=1
            //                        new SqlParameter("@Id",id),
            //                        new SqlParameter("@ViewDate",DateTime.Today),
            //                    };
            //                    SqlHelper.ExecuteScalar(pvSql, pvParam);
            //                }
            //                catch (Exception ex)
            //                {
            //                    LogHelper.WriteLog(string.Format("新闻PV增加异常，Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            //                }

            //            }).Start();

            //获取新闻实体
            var model = Util.GetEntityById<News>(id);
            var thumbList = GetResources((int)ResourceTypeEnum.新闻缩略图, model.Id);
            if (thumbList.Any())
                model.Thumb = thumbList.First().RPath;

            //查询新闻栏目信息
            var newstype = Util.GetEntityById<NewsType>((int)model.TypeId);
            ViewBag.NewsType = newstype;
            //获取彩种类型信息和SEO信息
            var lotteryType = Util.GetEntityById<LotteryType>((int)newstype.LType);
            ViewBag.Lottery = lotteryType;

            #region 上一篇 下一篇
            //查询上一篇
            string sql = @"SELECT TOP 1
[Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle]
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND DeleteMark=0 AND EnabledMark=1 AND [Id] > @CurrentId 
ORDER BY SortCode,Id";
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
WHERE [TypeId]=@TypeId AND DeleteMark=0 AND EnabledMark=1 AND [Id] < @CurrentId 
ORDER BY SortCode desc,Id DESC";
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

            #region 查询推荐阅读
            //查询推荐阅读
            List<News> list = CacheHelper.GetCache<List<News>>(("z_newstop3list_" + model.TypeId));
            if (list == null || list.Count <= 0)
            {
                string recommendArticlesql = @"SELECT TOP 3 [Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],
                            (SELECT COUNT(1) FROM[dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
                            FROM [dbo].[News] a
                            WHERE [TypeId] = @TypeId AND DeleteMark=0 AND EnabledMark=1
                            ORDER BY ModifyDate DESC,SortCode ASC ";
                //AND RecommendMark = 1

                var recommendArticleParameters = new[]
                {
                    new SqlParameter("@TypeId",model.TypeId),
                };

                list = Util.ReaderToList<News>(recommendArticlesql, recommendArticleParameters);
                int sourceType = (int)ResourceTypeEnum.新闻缩略图;
                list.ForEach(x =>
                {
                    x.ThumbList = GetResources(sourceType, x.Id)
                                    .Select(n => n.RPath).ToList();
                });
                CacheHelper.AddCache<List<News>>(("z_newstop3list_" + model.TypeId), list, 120);
            }
            ViewBag.RecommendArticle = list;
            #endregion

            #region 竞猜红人
            List<RankIntegralModel> ListRankIntegral = CacheHelper.GetCache<List<RankIntegralModel>>(("z_ListRankIntegral_" + newstype.LType));
            if (ListRankIntegral == null || ListRankIntegral.Count <= 0)
            {
                string strsql = @"	select  top 10 row_number() over(order by Score DESC ) as [Rank],  * from (
                                      SELECT Top 100 isnull(sum(a.Score),0) as Score,a.UserId, a.lType,b.Name as NickName,c.RPath as Avater 
                                      FROM dbo.SuperiorRecord a
                                      left join UserInfo b on b.Id=a.UserId
                                      left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=@ResourceType
                                      WHERE a.lType=@ltype 
                                      GROUP BY a.lType,a.UserId,b.Name,c.RPath
                                  ) tt WHERE Score > 0";

                SqlParameter[] sp = new SqlParameter[] {
                 new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                 new SqlParameter("@ltype",newstype.LType)
                };
                ListRankIntegral = Util.ReaderToList<RankIntegralModel>(strsql, sp);
                CacheHelper.AddCache<List<RankIntegralModel>>(("z_ListRankIntegral_" + newstype.LType), ListRankIntegral, 120);
            }
            ViewBag.ListRankIntegral = ListRankIntegral;

            #endregion


            ViewBag.CityId = Tool.GetCityId();

            ViewBag.SiteSetting = GetSiteSetting();

            return View(model);
        }


        /// <summary>
        /// 玄机图库浏览页
        /// </summary>
        /// <param name="id">新闻Id</param>
        /// <returns></returns>
        public ActionResult Gallery(int id)
        {
            AddPv(id);

            //            new Task(() =>
            //            {
            //                try
            //                {
            //                    string pvSql = @"if exists(
            //	select 1 from dbo.PageView where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
            //  )
            //  begin
            //   update dbo.PageView set ViewTotal+=1 where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
            //  end
            //  else
            //  begin
            //  insert into dbo.PageView(ViewDate,ViewTotal,[Type],FkId) values(GETDATE(),1,@Type,@Id)
            //  end;
            //UPDATE dbo.News SET PV+=1 WHERE Id=@Id";
            //                    var pvParam = new[]
            //                    {
            //                        new SqlParameter("@Type",1),//新闻类型=1
            //                        new SqlParameter("@Id",id),
            //                        new SqlParameter("@ViewDate",DateTime.Today),
            //                    };
            //                    SqlHelper.ExecuteScalar(pvSql, pvParam);
            //                }
            //                catch (Exception ex)
            //                {
            //                    LogHelper.WriteLog(string.Format("新闻PV增加异常，Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            //                }

            //            }).Start();


            var news = Util.GetEntityById<News>(id);
            var model = new Gallery()
            {
                Id = news.Id,
                Issue = news.Issue,
                Name = news.FullHead,
                TypeId = news.TypeId
            };

            //查询新闻栏目信息
            var newstype = Util.GetEntityById<NewsType>((int)model.TypeId);
            ViewBag.NewsType = newstype;
            //查询当前图库所有期信息
            var galleryList = GetGalleries(news.Id, news.FullHead, (int)model.TypeId);
            ViewBag.GalleryList = galleryList;

            //查询推荐图
            List<Gallery> recGalleryList = CacheHelper.GetCache<List<Gallery>>(("z_recGalleryList_" + model.TypeId));
            if (recGalleryList == null || recGalleryList.Count <= 0)
            {
                string recGallerySql = @" SELECT TOP 10   FullHead as Name, Id,LotteryNumber as Issue 
                                         from News where Id  in(
	                                        select max(id) from News where TypeId=" + model.TypeId + @" group by FullHead having count(FullHead)>=1
                                         )
                                         and DeleteMark=0 and EnabledMark=1 
                                         order by RecommendMark DESC,LotteryNumber DESC,ModifyDate DESC";
                recGalleryList = Util.ReaderToList<Gallery>(recGallerySql);
                CacheHelper.AddCache<List<Gallery>>(("z_recGalleryList_" + model.TypeId), recGalleryList, 120);
            }
            ViewBag.RecommendGalleryList = recGalleryList;

            ViewBag.CityId = Tool.GetCityId();

            return View(model);
        }

        public void AddPv(int id)
        {
            //var ids = CacheHelper.GetCache<string>("SavePageViewNewsIdsWebSite");
            //if (string.IsNullOrEmpty(ids))
            //    ids = ",";
            //if (ids.IndexOf("," + id + ",") == -1)
            //    ids += id + ",";
            //CacheHelper.SetCache<string>("SavePageViewNewsIdsWebSite", ids, DateTime.Now.AddDays(30));

            var pageView = CacheHelper.GetCache<PageView>("SavePageViewWebSite_1_" + id);

            if (pageView == null)
            {
                pageView = new PageView()
                {
                    FkId = id,
                    Type = 1,
                    ViewTotal = 1
                };
            }
            else
            {
                pageView.ViewTotal = pageView.ViewTotal + 1;
            }

            CacheHelper.SetCache<PageView>("SavePageViewWebSite_1_" + id, pageView, DateTime.Now.AddDays(10));
        }

        /// <summary>
        /// 精彩评论
        /// </summary>
        /// <param name="id">文章Id或计划Id</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="refUid">相关用户Id</param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult WonderfulComment(int id, int type = 2, int refUid = 0)
        {
            string cachekey = string.Format("z_WonderfulComment_{0}_{1}_{2}", id, type, refUid);
            List<Comment> list = CacheHelper.GetCache<List<Comment>>(cachekey);
            if (list == null)
            {
                string sql = string.Format(@"select top 3  a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater,
                                            (select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes,
                                            (select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount 
                                            from Comment a
                                              left join UserInfo b on b.Id = a.UserId
                                              left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
                                              where a.IsDeleted = 0 and a.RefCommentId=0  and a.ArticleId = @ArticleId {0} and a.Type=@Type
                                              order by StarCount desc", type == 1 ? " and a.ArticleUserId = @ArticleUserId" : "");

                var parameters = new[]
                {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.BigInt),
                new SqlParameter("@ArticleId",SqlDbType.BigInt),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@ArticleUserId",SqlDbType.Int),
            };
                long userId = 0;
                if (UserHelper.LoginUser != null)
                {
                    userId = UserHelper.LoginUser.Id;
                }

                parameters[0].Value = userId;
                parameters[1].Value = (int)ResourceTypeEnum.用户头像;
                parameters[2].Value = id;
                parameters[3].Value = type;
                parameters[4].Value = refUid;

                list = Util.ReaderToList<Comment>(sql, parameters);
                CacheHelper.AddCache<List<Comment>>(cachekey, list, 60);
            }
            list.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = LuoUtil.DefaultAvater;
                }
            });

            ViewBag.ArticleId = id;
            ViewBag.Type = type;//1=计划 2=文章
            ViewBag.RefUid = refUid;

            //查询新闻/计划 总评论数量
            int commentTotalCount = 0;
            string commentTotalCountSql = "select count(1) from Comment where IsDeleted = 0 and Type=" + type +
                                          " and ArticleId=" + id;
            if (type == 1)
            {
                commentTotalCountSql += " and ArticleUserId=" + refUid;
            }

            var obj = SqlHelper.ExecuteScalar(commentTotalCountSql);

            if (obj != null)
            {
                commentTotalCount = Convert.ToInt32(obj);
            }

            ViewBag.CommentTotalCount = commentTotalCount;

            return PartialView(list);
        }

        /// <summary>
        /// 评论列表页
        /// </summary>
        /// <param name="id">文章/彩种 Id</param>
        /// <param name="type">评论类型 1=计划 2=文章 </param>
        /// <param name="refUid">相关用户Id</param>
        /// <returns></returns>
        public ActionResult CommentList(int id, int type = 2, int refUid = 0)
        {
            string cachekey = string.Format("z_WonderfulComment_{0}_{1}_{2}", id, type, refUid);
            List<Comment> list = CacheHelper.GetCache<List<Comment>>(cachekey);
            if (list == null)
            {
                string sql =
               string.Format(@"select top 3  a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes 
                            ,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
                              from Comment a
                              left join UserInfo b on b.Id = a.UserId
                              left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
                              where a.ArticleId = @ArticleId {0} and a.RefCommentId=0 and a.IsDeleted = 0  and a.Type=@Type
                              order by StarCount desc", type == 1 ? " and a.ArticleUserId = @ArticleUserId" : "");
                var parameters = new[]
                {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.Int),
                new SqlParameter("@ArticleId",SqlDbType.BigInt),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@ArticleUserId",SqlDbType.Int),
            };
                long userId = 0;
                if (UserHelper.LoginUser != null)
                {
                    userId = UserHelper.LoginUser.Id;
                }
                parameters[0].Value = userId;
                parameters[1].Value = (int)ResourceTypeEnum.用户头像;
                parameters[2].Value = id;
                parameters[3].Value = type;
                parameters[4].Value = refUid;

                list = Util.ReaderToList<Comment>(sql, parameters);
                CacheHelper.AddCache<List<Comment>>(cachekey, list, 60);
            }
            list.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = LuoUtil.DefaultAvater;
                }
            });

            ViewBag.ArticleId = id;
            ViewBag.Type = type;//1=计划 2=文章
            ViewBag.RefUid = refUid;

            //查询新闻/文章 总评论数量
            int commentTotalCount = 0;
            string commentTotalCountSql = "select count(1) from Comment where IsDeleted=0 and RefCommentId=0 and Type=" + type + " and ArticleId=" + id;
            if (type == 1)
            {
                commentTotalCountSql += " and ArticleUserId=" + refUid;
            }
            var obj = SqlHelper.ExecuteScalar(commentTotalCountSql);

            if (obj != null)
            {
                commentTotalCount = Convert.ToInt32(obj);
            }

            ViewBag.CommentTotalCount = commentTotalCount;

            return View(list);
            //return Json(result);
        }

        /// <summary>
        /// 最新评论
        /// </summary>
        /// <param name="id">文章或彩种Id</param>
        /// <param name="type">评论类型 1=计划 2=文章</param>
        /// <param name="refUid">相关用户Id Type=1时使用</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LastComment(int id, int type = 2, int refUid = 0, int pageIndex = 1, int pageSize = 10)
        {

            var result = new AjaxResult<PagedList<Comment>>();
            string cachekey = string.Format("z_LastComment_{0}_{1}_{2}_{3}", id, type, refUid, pageIndex);
            List<Comment> list = CacheHelper.GetCache<List<Comment>>(cachekey);
            if (list == null)
            {
                string sql = string.Format(@"SELECT * FROM ( 
                                select row_number() over(order by a.SubTime DESC ) as rowNumber,a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
                                ,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
                                from Comment a
                                  left join UserInfo b on b.Id = a.UserId
                                  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
                                  where a.ArticleId = @ArticleId {0} and a.IsDeleted = 0 and a.RefCommentId=0 and a.Type=@Type 
                                  ) T
                                WHERE rowNumber BETWEEN @Start AND @End", type == 1 ? " and a.ArticleUserId = @ArticleUserId" : "");
                var parameters = new[]
                {
                    new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                    new SqlParameter("@ArticleId",id),
                    new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                    new SqlParameter("@End",pageSize * pageIndex),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@ArticleUserId",refUid),
                };

                list = Util.ReaderToList<Comment>(sql, parameters);
                CacheHelper.AddCache<List<Comment>>(cachekey, list, 60);
            }
            string countSql = "select count(1) from Comment where IsDeleted = 0 and RefCommentId=0 and Type=" + type + " and ArticleId=" + id;
            if (type == 1)
            {
                countSql += " and ArticleUserId=" + refUid;
            }
            object obj = SqlHelper.ExecuteScalar(countSql);

            var pager = new PagedList<Comment>();
            pager.PageData = list;
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            pager.TotalCount = obj != null ? Convert.ToInt32(obj) : 0;

            result.Data = pager;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 评论详情
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <returns></returns>
        public ActionResult CommentDetail(int id, int type = 2)
        {
            string cachekey = string.Format("z_CommentDetail_{0}_{1}", id, type);
            List<Comment> list = CacheHelper.GetCache<List<Comment>>(cachekey);
            if (list == null)
            {
                string sql =
               @"select a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater
                ,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes 
                ,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
                  from Comment a
                  left join UserInfo b on b.Id = a.UserId
                  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
                  where a.Id = @Id and a.IsDeleted = 0 and a.Type=@Type
                  order by StarCount desc";
                var parameters = new[]
                {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.Int),
                new SqlParameter("@Id",SqlDbType.BigInt),
                new SqlParameter("@Type",SqlDbType.Int),
            };
                long userId = 0;
                if (UserHelper.LoginUser != null)
                {
                    userId = UserHelper.LoginUser.Id;
                }

                parameters[0].Value = userId;
                parameters[1].Value = (int)ResourceTypeEnum.用户头像;
                parameters[2].Value = id;
                parameters[3].Value = type;

                list = Util.ReaderToList<Comment>(sql, parameters);
                CacheHelper.AddCache<List<Comment>>(cachekey, list, 60);
            }
            if (!list.Any())
            {
                Response.Redirect("/News/CommentList/" + id);
                return View();
            }

            list.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = LuoUtil.DefaultAvater;
                }
            });

            var model = list.FirstOrDefault();

            ViewBag.CommentId = id;
            ViewBag.Type = type;//1=计划 2=文章

            return View(model);
        }


        /// <summary>
        /// 查询评论的回复列表
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LastReply(int id, int type = 2, int pageIndex = 1, int pageSize = 10)
        {

            var result = new AjaxResult<PagedList<Comment>>();

            string sql = @"SELECT * FROM ( 
select row_number() over(order by a.SubTime DESC ) as rowNumber,a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes 
,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.RefCommentId = @RefCommentId and a.IsDeleted = 0 and a.Type=@Type
  ) T
WHERE rowNumber BETWEEN @Start AND @End";
            long userId = 0;
            if (UserHelper.LoginUser != null)
            {
                userId = UserHelper.LoginUser.Id;
            }

            var parameters = new[]
            {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@RefCommentId",id),
                new SqlParameter("@Start",pageSize *( pageIndex-1)+1),
                new SqlParameter("@End",pageSize * pageIndex),
                new SqlParameter("@Type",type),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);

            list.ForEach(x =>
            {

                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = LuoUtil.DefaultAvater;
                }
                //查询是否有上级回复
                // 2 40 42
                if (x.PId > 0)
                {
                    x.ParentComment = GetComment(x.PId);
                }

            });


            string countSql = "select count(1) from Comment where IsDeleted = 0 and RefCommentId=" + id;
            object obj = SqlHelper.ExecuteScalar(countSql);

            int totalCount = obj != null ? Convert.ToInt32(obj) : 0;
            var pager = new PagedList<Comment>(pageIndex, pageSize, totalCount, list);

            result.Data = pager;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="ctype">操作类型 1=点赞 2=取消点赞</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <returns></returns>
        [Authentication]
        public JsonResult ClickLike(int id, int ctype, int type = 2)
        {
            var result = new AjaxResult();
            long userId = UserHelper.LoginUser.Id;
            string sql = "select [Id],[CommentId],[UserId],[Status],[Type] from [dbo].[LikeRecord] where [Type]=" + type + " and CommentId=" + id + " and UserId=" + userId;

            var list = Util.ReaderToList<LikeRecord>(sql);

            if (ctype == 1)
            {
                if (list.Any())
                {
                    //已存在点赞记录
                    var likeRecord = list.FirstOrDefault();
                    if (likeRecord.Status == (int)LikeStatusEnum.Canceled)
                    {
                        //已存在的点赞记录为取消状态
                        //修改点赞状态
                        result = MoidfyLike(id, type, userId, (int)LikeStatusEnum.Clicked);
                    }
                    else
                    {
                        result = new AjaxResult(10000, "你已经点过赞");
                    }
                }
                else
                {
                    #region 添加点赞
                    try
                    {
                        //添加点赞
                        //SqlHelper.ExecuteTransaction();
                        string insert = @"INSERT INTO [dbo].[LikeRecord]
           ([CommentId]
           ,[UserId]
           ,[CreateTime]
           ,[Status]
           ,[UpdateTime]
           ,[Type])
     VALUES
           (@CommentId
           ,@UserId
           ,GETDATE()
           ,1
           ,GETDATE()
           ,@Type);
        UPDATE [dbo].[Comment] SET [StarCount]+=1 WHERE Id=@CommentId;";

                        var insertParameters = new[]
                        {
                        new SqlParameter("@CommentId", id),
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@Type", type),
                    };

                        SqlHelper.ExecuteTransaction(insert, insertParameters);
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.WriteLog($"点赞异常，用户：{UserHelper.GetUser().Name},点赞类型：{type},Id:{id}。堆栈：{ex.StackTrace}");
                        result = new AjaxResult(500, ex.Message);
                    }
                    #endregion

                }
            }
            else if (ctype == 2)
            {
                //取消点赞
                if (list.Any())
                {
                    result = MoidfyLike(id, type, userId, (int)LikeStatusEnum.Canceled);
                }
            }
            else
            {
                result = new AjaxResult(400, "Bad Request");
            }

            return Json(result);
        }

        /// <summary>
        /// 修改点赞
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="type">评论类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="likeStatus">点赞操作类型 </param>
        /// <returns></returns>
        private static AjaxResult MoidfyLike(int id, int type, long userId, int likeStatus)
        {
            AjaxResult result = new AjaxResult();
            try
            {
                //修改点赞
                //SqlHelper.ExecuteTransaction();
                string updateSql = @"
        UPDATE [dbo].[LikeRecord] SET [Status]=@Status,[UpdateTime]=GETDATE() 
        WHERE [CommentId]=@CommentId AND [UserId]=@UserId AND [Type]=@Type;";

                if (likeStatus == 1)
                {
                    updateSql += "UPDATE [dbo].[Comment] SET [StarCount] +=1 WHERE [StarCount]>0 and Id=@CommentId;";
                }
                else
                {
                    updateSql += "UPDATE [dbo].[Comment] SET [StarCount] -=1 WHERE [StarCount]>0 and Id=@CommentId;";
                }

                var updateParameters = new[]
                {
                    new SqlParameter("@CommentId", id),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@Status", likeStatus)
                };

                SqlHelper.ExecuteTransaction(updateSql, updateParameters);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog($"取消点赞异常，用户：{UserHelper.GetUser().Name},点赞类型：{type},Id:{id}。堆栈：{ex.StackTrace}");
                LogHelper.WriteLog("取消点赞异常，用户：" + UserHelper.LoginUser.Name + ",点赞类型：" + type + ", Id:" + id + "。堆栈：" + ex.StackTrace);

                result = new AjaxResult(500, ex.Message);
            }
            return result;
        }

        private Comment GetComment(long id)
        {
            string sql = @"select a.*,isnull(b.Name,'') as NickName
 from Comment a
left join UserInfo b on b.Id=a.UserId
where  a.Id=@Id and a.IsDeleted = 0";
            var parameters = new[]
           {
                new SqlParameter("@Id",id),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);


            if (list.Any())
            {
                return list.FirstOrDefault();
            }

            return null;
        }

    }


}
