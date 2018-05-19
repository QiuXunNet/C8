using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common.Utils
{
    /// <summary>
    /// 加解密类
    /// </summary>
    public static class EncryptionHelper
    {
        private static string encryptKey = "ciic@^DS3102?";
        /// <summary>
        /// 获取数组类型的密钥
        /// </summary>
        /// <returns></returns>
        private static byte[] GetEncryptKey(string key)
        {
            var keyBytes = new byte[8]; //des加密密钥为8位
            var defaultBytes = Encoding.ASCII.GetBytes(key);
            for (int i = 0, j = 0; i < keyBytes.Length; i++, j = i - defaultBytes.Length)
            {
                keyBytes[i] = i < defaultBytes.Length ? defaultBytes[i] : (byte)(defaultBytes[j] >> j);
            }
            return keyBytes;
        }

        /// <summary>
        /// 功能：将加密的字符解析出来(Base64)       
        /// </summary>     
        /// <param name="decryptString">加密过的字符串</param>
        /// <param name="key"></param>
        /// <returns>解密后的字符串</returns>
        public static string GetDecryptionByBase64(string decryptString, string key)
        {
            try
            {
                if (String.IsNullOrEmpty(decryptString))
                {
                    return "";
                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[decryptString.Length / 2];
                for (int x = 0; x < decryptString.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = GetEncryptKey(key ?? encryptKey); //Key
                des.IV = GetEncryptKey(key ?? encryptKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder(); //CreateDecrypt
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch 
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 功能：将传入的参数进行加密(Base64)     
        /// </summary>     
        /// <param name="encryptString">未加密码的字符串</param>
        /// <param name="key">加密密钥</param>
        /// <returns>加密之后的字符串</returns>
        public static string GetEncryptionByBase64(string encryptString, string key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(encryptString);
                var keyBytes = GetEncryptKey(key ?? encryptKey);
                des.Key = keyBytes;
                des.IV = keyBytes;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 对字符串进行Base64编码
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string ToBase64(this string value)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 对字符串进行Base64解码
        /// </summary>
        /// <param name="value">Base64编码的字符串</param>
        /// <returns>Base64解码的字符串</returns>
        public static string FromBase64(this string value)
        {
            byte[] encodedBytes = Convert.FromBase64String(value);
            return UTF8Encoding.UTF8.GetString(encodedBytes);
        }
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string strText,string salt)
        {
            string newWord = String.Concat(encryptKey, strText, salt);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(newWord);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

    }
}
