using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using C8.Lottery.Portal.Business.BusinessData;
using System.Text;
using C8.Lottery.Model;
using System.Data;

namespace C8.Lottery.Portal.Business
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：ZZH
    /// 日 期：2018年5月18日
    /// 描 述：新闻业务类
    /// </summary>
    public class NewsService
    {
        internal string[] GetSEOInfo(int id, int ntype)
        {
            string seoInfo = CacheHelper.GetCache<string>(string.Format("SEOINFO_{0}_{1}", id, ntype));
            if (string.IsNullOrWhiteSpace(seoInfo))
            { 
                string sql = "";
                if (ntype != 0) sql = "SELECT a.SeoSubject+'&*&'+a.SeoKeyword+'&*&'+a.SeoDescription+'&*&'+b.TypeName FROM dbo.NewsType AS a LEFT JOIN dbo.LotteryType AS b ON a.lType=b.Id WHERE a.lType=@id AND a.Id=@ntype";
                else sql = "SELECT SeoSubject+'&*&'+SeoKeyword+'&*&'+SeoDescription+'&*&'+TypeName FROM dbo.LotteryType WHERE Id=@id";
                SqlParameter[] paras = {
                     new SqlParameter("@id",id),
                     new SqlParameter("@ntype",ntype)
                };
                object result = SqlHelper.ExecuteScalar(sql, paras);
                if (result != null) seoInfo = Convert.ToString(result); CacheHelper.AddCache<string>(string.Format("SEOINFO_{0}_{1}", id, ntype), seoInfo, 24 * 60 * 30);
            }
            return seoInfo.Contains("&*&") ? seoInfo.Split(new string[] { "&*&" }, StringSplitOptions.RemoveEmptyEntries) : new string[4];
        }

        internal List<BusinessData.LotteryType> GetLotteryTypeList()
        {
            List<BusinessData.LotteryType> list = CacheHelper.GetCache<List<BusinessData.LotteryType>>("NEWSLTYPELIST");
            if (list != null && list.Count >= 0) return list;
            string sql = "SELECT Id,TypeName FROM LotteryType";
            list = Util.ReaderToList<BusinessData.LotteryType>(sql);
            CacheHelper.AddCache<List<BusinessData.LotteryType>>("NEWSLTYPELIST", list, 24 * 60 * 30);
            return list;
        }

        internal List<LotteryNewsChannel> GetLotteryNewsChannelList(int lType)
        {
            List<LotteryNewsChannel> list = CacheHelper.GetCache<List<LotteryNewsChannel>>(string.Format("NEWSCHANNELLIST_{0}", lType));
            if (list != null && list.Count >= 0) return list;
            string sql = "SELECT Id,TypeName,lType FROM dbo.NewsType WHERE lType=@lType ORDER BY SortCode";
            SqlParameter[] paras = { new SqlParameter("@lType", lType) };
            list = Util.ReaderToList<LotteryNewsChannel>(sql, paras);
            CacheHelper.AddCache<List<LotteryNewsChannel>>(string.Format("NEWSCHANNELLIST_{0}", lType), list, 24 * 60 * 30);
            return list;
        }

        internal Dictionary<string,string> GetLastBetInfo(int lType)
        {
            string sql = "";
            if (lType == 5) //如果是六合彩，则查询六合彩专用表
            {
                sql = "select top(1)* from LotteryRecordToLhc";
            }
            else
            {
                sql = "select top(1)* from LotteryRecord where lType =" + lType + " order by Issue desc";
            }
            Model.LotteryRecord lr = Util.ReaderToModel<Model.LotteryRecord>(sql);
            string NumHtml = GetNumberHtml(lType, lr.Num);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Issue", lr.Issue);
            dic.Add("NumHtml", NumHtml);
            return dic;
        }

        private string GetNumberHtml(int lType, string num)
        {
            int countFor6 = 0;

            string[] numArr = num.Split(',');
            StringBuilder tempHtml = new StringBuilder();

            if (lType == 5)
            {
                foreach (string s in numArr)
                {
                    countFor6++;

                    string color = Util.GetColor(s);
                    string cl = "";
                    if (color == "red")
                    {
                        cl = "hdM_spliu hdM_spliured";
                    }
                    else if (color == "green")
                    {
                        cl = "hdM_spliu hdM_spliugreen";
                    }
                    else
                    {
                        cl = "hdM_spliu hdM_spliublue";
                    }

                    tempHtml.AppendLine("<span class=\"" + cl + "\">" + s + "</span>");
                    if (countFor6 == 6)
                    {
                        tempHtml.AppendLine("<span class=\"Mif_jgC\">+</span>");
                    }

                }
            }
            else
            {
                int i = -1;
                foreach (string s in numArr)
                {
                    i++;
                    if (lType == 63 || lType == 64)
                    {
                        string clazz = "hdM_spPK hdM_spPK" + s;
                        tempHtml.AppendLine("<span class=\"" + clazz + "\">" + s + "</span>");
                    }
                    else if (lType == 2)
                    {
                        if (i == 6)
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Blue\">" + s + "</span>");
                        }
                        else
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + s + "</span>");
                        }
                    }
                    else if (lType == 4)
                    {
                        if (i == 5 || i == 6)
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Blue\">" + s + "</span>");
                        }
                        else
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + s + "</span>");
                        }
                    }
                    else if (lType == 8)
                    {
                        if (i == 7)
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Blue\">" + s + "</span>");
                        }
                        else
                        {
                            tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + s + "</span>");
                        }
                    }
                    else if (lType == 65)
                    {
                        int a = int.Parse(numArr[0]);
                        int b = int.Parse(numArr[1]);
                        int c = int.Parse(numArr[2]);
                        int sum = a + b + c;


                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + numArr[0] + "</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_PcHao\">+</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + numArr[1] + "</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_PcHao\">+</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + numArr[2] + "</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_PcHao\">=</span>");
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + sum + "</span>");
                        break;
                    }

                    else
                    {
                        tempHtml.AppendLine("<span class=\"hdM_spssc HYJP_Red\">" + s + "</span>");
                    }

                }
            }
            return tempHtml.ToString();
        }

        internal NewsType GetNewTypeById(int typeId)
        {
            List<NewsType> list = CacheHelper.GetCache<List<NewsType>>("z_allnewstype");
            NewsType temp = null;
            if (list == null || list.Count <= 0)
            {
                string sql = "SELECT * FROM dbo.NewsType";
                list = Util.ReaderToList<NewsType>(sql);
                CacheHelper.AddCache<List<NewsType>>("z_allnewstype", list, 24 * 60 * 1000);
            }
            temp = list.Where(p => p.Id == typeId).FirstOrDefault();
            if (temp == null || temp.Id <= 0) return null;
            return temp;
        }

        internal List<NewNews> GetNewList(int typeId, int pageIndex, int pageSize)
        {
            LogHelper.WriteInfoLog(string.Format("资讯列表走缓存开始:newslist_{0}_{1}", typeId, pageIndex));
            List<NewNews> list = CacheHelper.GetCache<List<NewNews>>(string.Format("z_newslist_{0}_{1}", typeId, pageIndex));
            LogHelper.WriteInfoLog(string.Format("资讯列表走缓存结束:newslist_{0}_{1}", typeId, pageIndex));
            if (list == null || list.Count <= 0)
            {
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
                            ) S
                    ORDER BY S.rowNumber";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TypeId",SqlDbType.BigInt),
                    new SqlParameter("@Start",SqlDbType.Int),
                    new SqlParameter("@End",SqlDbType.Int),
                };
                parameters[0].Value = typeId;
                parameters[1].Value = (pageIndex - 1) * pageSize + 1;
                parameters[2].Value = pageSize * pageIndex;
                list = Util.ReaderToList<Business.BusinessData.NewNews>(sql, parameters) ?? new List<Business.BusinessData.NewNews>();
                //CacheHelper.AddCache<List<NewNews>>(string.Format("z_newslist_{0}_{1}", typeId, pageIndex), list, (24 * 90));
            }
            list.ForEach(x =>
            {
                x.ThumbList = !(string.IsNullOrWhiteSpace(x.ThumbListStr)) ? x.ThumbListStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            });
            return list;
        }

        internal List<Advertisement> GetAdvertisementList(int typeId, int adType)
        {
            List<Advertisement> list= CacheHelper.GetCache<List<Advertisement>>("z_adlistpc");
            if (list == null || list.Count <= 0)
            {
                string sql = @"SELECT  * ,
                                    STUFF((SELECT  ',' + RPath
                                            FROM    dbo.ResourceMapping
                                            WHERE   Type = 20
                                                    AND FkId = Advertisement.Id
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '') AS ThumbListStr
                            FROM dbo.Advertisement
                            WHERE   (State = 1
                                      OR(State = 0
                                           AND GETDATE() >= BeginTime
                                           AND EndTime > GETDATE()
                                      )
                                    )
                                    AND CHARINDEX(',1,', ',' + [Where] + ',') > 0";
                list = Util.ReaderToList<Advertisement>(sql) ?? new List<Advertisement>();
                CacheHelper.AddCache<List<Advertisement>>("z_adlistpc", list, (24 * 60 * 3));
            }
            var searchList = list.Where(p => ("," + p.Location + ",").Contains(("," + typeId + ","))).ToList();
            if (searchList != null && searchList.Count > 0)
            {
                searchList.ForEach(x =>
                {
                    x.ThumbList = !(string.IsNullOrWhiteSpace(x.ThumbListStr)) ? x.ThumbListStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
                });
                return searchList;
            }
            return new List<Advertisement>();
        }

        internal List<GalleryType> GetGalleryTypeList(long ltype, int newsTypeId)
        {
            List<GalleryType> list = CacheHelper.GetCache<List<GalleryType>>(string.Format("z_GalleryTypeList_{0}_{1}", ltype, newsTypeId));
            if (list == null || list.Count <= 0)
            {
                string sql = @" SELECT Max(a.Id) as Id, FullHead as Name, right(Max(a.LotteryNumber),3) as LastIssue,isnull(a.QuickQuery,'#') as QuickQuery
                             from News  a
                             left join NewsType b on b.Id= a.TypeId
                             where a.TypeId=@NewsTypeId and b.lType=@LType and DeleteMark=0 and EnabledMark=1
                             group by a.FullHead,a.QuickQuery
                             order by a.QuickQuery";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@NewsTypeId",SqlDbType.Int),
                    new SqlParameter("@LType",SqlDbType.BigInt)
                };
                parameters[0].Value = newsTypeId;
                parameters[1].Value = ltype;

                list = Util.ReaderToList<GalleryType>(sql, parameters) ?? new List<GalleryType>();
            }
            return list;
        }

        internal List<Gallery> GetGalleryList(long ltype, int newsTypeId)
        {
            List<Gallery> list = CacheHelper.GetCache<List<Gallery>>(string.Format("z_GalleryList_{0}_{1}", ltype, newsTypeId));
            if (list == null || list.Count <= 0)
            {
                string recGallerySql = @" SELECT TOP 3 a.Id,FullHead as Name,LotteryNumber as Issue,
                                            STUFF(( SELECT  ',' + RPath
                                                            FROM    dbo.ResourceMapping
                                                            WHERE   Type = 1
                                                                    AND FkId = a.Id
                                                          FOR
                                                            XML PATH('')
                                                          ), 1, 1, '') AS Picture
                                        FROM News a 
                                         left join NewsType b on b.Id= a.TypeId
                                         where a.RecommendMark=1 and DeleteMark=0 and EnabledMark=1 and TypeId=@NewsTypeId and b.lType=@LType order by ModifyDate DESC";
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@NewsTypeId",SqlDbType.Int),
                    new SqlParameter("@LType",SqlDbType.BigInt)
                };
                parameters[0].Value = newsTypeId;
                parameters[1].Value = ltype;
                list = Util.ReaderToList<Gallery>(recGallerySql, parameters);
            }
            return list ?? new List<Gallery>();
        }
    }
}