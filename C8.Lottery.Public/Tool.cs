using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
