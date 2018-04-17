﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using C8.Lottery.Public;
using System.IO;
using System.Drawing;

namespace C8.Lottery.Public
{
   public class Tool
    {
        /// <summary>
        /// md5 加密 kcp
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
     public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i<targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }

       

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string userIP = string.Empty;
            // HttpRequest Request = HttpContext.Current.Request;  
            HttpRequest Request = System.Web.HttpContext.Current.Request; // 如果使用代理，获取真实IP  
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            else
                userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
                userIP = Request.UserHostAddress;

            return userIP;
        }


        /// <summary>
        /// 获取排名前三图片
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static string GetRankImg(int rank)
        {
            string imgsrc = string.Empty;
            switch (rank)
            {
                case 1:
                    imgsrc = "/images/44_1.png";
                    break;
                case 2:
                    imgsrc = "/images/44_2.png";
                    break;
                case 3:
                    imgsrc = "/images/44_3.png";
                    break;
            }
            return imgsrc;
        }

        /// <summary>
        /// 获取充值图片
        /// </summary>
        /// <returns></returns>
        public static string GetPayImg(int paytype)
        {
            string imgsrc = string.Empty;
            switch (paytype)
            {
                case 1:
                    imgsrc = "/images/41_1.png";
                    break;
                case 2:
                    imgsrc = "/images/41_2.png";
                    break;
                case 3:
                    imgsrc = "/images/41_3.png";
                    break;
            }
            return imgsrc;
        }

        /// <summary>
        /// 验证是否包含敏感字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="excludeWordList"></param>
        /// <returns></returns>
       public static bool CheckSensitiveWords(string str)
        {
            string words = WebHelper.GetSensitiveWords();
            string[] zang = words.Split(',');

            if (str.Trim().Length <= 0 || zang == null || zang.Count() <= 0)
            {
                return false;
            }
            var contain = false;
            foreach (var el in zang)
            {
                if (str.Contains(el))
                {
                    contain = true;
                    break; ;
                }
            }
            return contain;
        }


        /// <summary>
        /// 赚钱图片
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static string GetZqImg(int Type)
        {
            string imgsrc = string.Empty;
            switch (Type)
            {
                case 1:
                    imgsrc = "/images/47_1.png";
                    break;
                case 7:
                    imgsrc = "/images/47_3.png";
                    break;
                case 4:
                    imgsrc = "/images/47_7.png";
                    break;
                case 9:
                    imgsrc = "/images/47_6.png";
                    break;
              
            }
            return imgsrc;
        }


        /// <summary>
        /// 货币保留后两位小数
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string Rmoney(decimal money)
        {
           return string.Format("{0:N}", money);
        }





        #region 截取data:image/jpeg;base64,提取图片，并保存图片
        /// <summary>
        /// 截取data:image/jpeg;base64,提取图片，并保存图片
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="img_string">base64的字符串</param>
        /// <param name="error">错误的图片格式</param>
        /// <returns>路径 + 图片的名称</returns>
        public static Phonto SaveImage(string file_name, string img_string, ref string error)
        {
            //try
            //{

            string[] img_array = img_string.Split(',');
            byte[] arr = Convert.FromBase64String(img_array[1]);
            using (MemoryStream ms = new MemoryStream(arr))
            {
                Bitmap bmp = new Bitmap(ms);
                if (!Directory.Exists(file_name))
                    Directory.CreateDirectory(file_name);
                if (img_array[0].ToLower() == "data:image/jpeg;base64")
                {
                    //bmp.Save(file_name + ".jpg");
                    return SetImg(file_name,Guid.NewGuid().ToString().Replace('-', 'p').Substring(4), "jpg", arr);
                }
                else if (img_array[0].ToLower() == "data:image/png;base64")
                {
                    //bmp.Save(file_name + ".png");
                    return SetImg(file_name, Guid.NewGuid().ToString().Replace('-', 'p').Substring(4), "png", arr);
                }
                else if (img_array[0].ToLower() == "data:image/gif;base64")
                {
                    //bmp.Save(file_name + ".gif");
                    
                    return SetImg(file_name, Guid.NewGuid().ToString().Replace('-', 'p').Substring(4), "gif", arr);
                }
                else
                {
                    error = "不支持该文件格式。";
                    return new Phonto();
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    error = "生成图片发生错误。" + ex.ToString();
            //    return "错";
            //}
        }
        #endregion
        #region 保存图片路径及设置名称
        /// <summary>
        /// 保存到文件路径 
        /// </summary>
        /// <param name="ImgName">保存的文件名称</param>
        /// <param name="suffix">后缀名</param>
        /// <param name="arr">base64</param>
        /// <returns>图片的路径</returns>
        public static Phonto SetImg(string path,string ImgName, string suffix, byte[] arr)
        {

            Phonto p = new Phonto();
            System.IO.File.WriteAllBytes(path + ImgName + "." + suffix + "", arr);
            p.Extension = "." + suffix;
            p.RPath= path + ImgName + "." + suffix + "";
            p.RSize = arr.Length;
            p.ImgName = ImgName;
            return p;
        }
        #endregion


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 排行榜 时间条件
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static string GetTimeWhere(string Timefiled,string queryType)
        {
            DateTime today = DateTime.Today;
            string sqlWhere = "";
            switch (queryType)
            {
                case "day":
                    DateTime dayStartTime = today.AddDays(-1);
                    DateTime dayEndTime = today;
                    sqlWhere = string.Format(" and  {0}>='{1}' and {0}<'{2}'", Timefiled, dayStartTime, dayEndTime);
                    break;
                case "week":
                    DateTime weekEndTime = today.GetWeekStart();
                    DateTime weekStartTime = today.GetWeekStart().AddDays(-7);
                    sqlWhere = string.Format(" and {0}>='{1}' and {0}<'{2}'", Timefiled, weekStartTime, weekEndTime);
                    break;
                case "month":
                    DateTime monthEndTime = today.GetMonthStart();
                    DateTime monthStartTime = today.GetMonthStart().AddMonths(-1);
                    sqlWhere = string.Format(" and {0}>='{1}' and {0}<'{2}'", Timefiled, monthStartTime, monthEndTime);
                    break;
                case "all":
                    sqlWhere = "";
                    break;
            }
            return sqlWhere;
        }




        /// <summary>
        /// 设置缓存时间
        /// </summary>
        /// <returns></returns>
        public static int GetCacheTime(string queryType)
        {
            int time=0;
            DateTime today = DateTime.Today;
            switch (queryType)
            {
                case "day":
                    DateTime dayStartTime = today.AddDays(-1);
                    DateTime dayEndTime = today;
                    time = ExecDateDiff(dayStartTime, dayEndTime);
                    break;
                case "week":
                    DateTime weekEndTime = today.GetWeekStart();
                    DateTime weekStartTime = today.GetWeekStart().AddDays(-7);
                    time = ExecDateDiff(weekStartTime, weekEndTime);
                    break;
                case "month":
                    DateTime monthEndTime = today.GetMonthStart();
                    DateTime monthStartTime = today.GetWeekStart().AddMonths(-1);
                    time = ExecDateDiff(monthStartTime, monthEndTime);
                    break;
                case "all":
                    time = 60 * 24;
                    break;
            }
            return time;
        }

        /// 程序执行时间测试
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0.00239秒</returns>
        public static int  ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return Convert.ToInt32(ts3.TotalMinutes);
        }




    }


    public class Phonto
    {
        public string ImgName;
        public string Extension;
        public string RPath;
        public int RSize;
    }


    /// <summary>
    /// 返回消息Json  KCP
    /// </summary>
    public class ReturnMessageJson
    {
        public bool Success;
        public object data;
        public object Msg;

    }

}
