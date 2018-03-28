using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

    }




    /// <summary>
    /// 返回消息Json  KCP
    /// </summary>
    public class ReturnMessageJson
    {
        public bool Success;
        public object Msg;

    }

}
