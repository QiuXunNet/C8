using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using C8.Lottery.Portal.Business.BusinessData;
using System.Text;

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
                if (result != null) seoInfo = Convert.ToString(result); CacheHelper.AddCache<string>(string.Format("SEOINFO_{0}_{1}", id, ntype), seoInfo);
            }
            return seoInfo.Contains("&*&") ? seoInfo.Split(new string[] { "&*&" }, StringSplitOptions.RemoveEmptyEntries) : new string[4];
        }

        internal List<LotteryType> GetLotteryTypeList()
        {
            List<LotteryType> list = CacheHelper.GetCache<List<LotteryType>>("NEWSLTYPELIST");
            if (list != null && list.Count >= 0) return list;
            string sql = "SELECT Id,TypeName FROM LotteryType";
            list = Util.ReaderToList<LotteryType>(sql);
            CacheHelper.AddCache<List<LotteryType>>("NEWSLTYPELIST", list, 0);
            return list;
        }

        internal List<LotteryNewsChannel> GetLotteryNewsChannelList(int lType)
        {
            List<LotteryNewsChannel> list = CacheHelper.GetCache<List<LotteryNewsChannel>>(string.Format("NEWSCHANNELLIST_{0}", lType));
            if (list != null && list.Count >= 0) return list;
            string sql = "SELECT Id,TypeName,lType FROM dbo.NewsType WHERE lType=@lType ORDER BY SortCode";
            SqlParameter[] paras = { new SqlParameter("@lType", lType) };
            list = Util.ReaderToList<LotteryNewsChannel>(sql, paras);
            CacheHelper.AddCache<List<LotteryNewsChannel>>(string.Format("NEWSCHANNELLIST_{0}", lType), list, 0);
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
                        tempHtml.AppendLine("<span class=\"" + clazz + "\">@s</span>");
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
    }
}