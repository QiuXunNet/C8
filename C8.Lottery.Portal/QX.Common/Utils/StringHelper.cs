
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QX.Common
{
    /// <summary>
    /// 与String有关的公共方法
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 功能：将object转成string
        /// 日期：2013-04-01        
        /// </summary>
        /// <param name="obj">要转的对象</param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == DBNull.Value || obj == null)
            {
                return "";
            }
            return obj.ToString();
        }

        /// <summary>
        /// 功能：根据给定的长度取得随机字符
        /// 日期：2013-03-25        
        /// </summary>   
        /// <param name="passwordLen">字符串长度</param>
        /// <returns>取得字符串</returns>    
        public static string GetRandomStr(int passwordLen)
        {
            string randomChars = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }

        /// <summary>
        /// 功能：精确截取字符串（超过部分会加上 “...”)
        /// 日期：2013-03-25        
        /// </summary>   
        /// <param name="length">截取长度</param>
        /// <param name="obj">截取对象</param>
        /// <returns>截取后的字符串</returns>  
        public static string CutStr(int length, object obj)
        {
            if (length > 0 && !Convert.IsDBNull(obj) && obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                string src = obj.ToString();
                string result = "";
                int n = 0;
                foreach (char s in src)
                {
                    if (n > length)
                    {
                        result = result + "...";
                        break;
                    }
                    n = n + System.Text.Encoding.GetEncoding("gb2312").GetBytes(s.ToString()).Length;
                    result = result + s;
                }
                return result;
            }
            return "";
        }
        public static List<int> ToIntList(this string ids)
        {
            if (ids != null && ids.IndexOf(',') >= 0)
            {
                string[] ary = ids.Split(',');
                List<int> list = new List<int>();
                ary.ForEach(p =>
                {
                    list.Add(p.ToInt());
                });
                return list;
            }
            else
            {
                if (ids.ToInt() != 0)
                {
                    return new List<int> { ids.ToInt() };
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public static List<long> TolongList(this string ids)
        {
            if (ids != null && ids.IndexOf(',') >= 0)
            {
                string[] ary = ids.Split(',');
                List<long> list = new List<long>();
                ary.ForEach(p =>
                {
                    list.Add(p.ToInt());
                });
                return list;
            }
            else
            {
                if (ids.ToInt64() != 0)
                {
                    return new List<long> { ids.ToInt64() };
                }
                else
                {
                    return new List<long>();
                }
            }
        }
        /// <summary>
        /// 功能：获取字符串长度
        /// 日期：2013-03-25        
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串长度</returns>
        public static int GetStrLength(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 功能：将参数转成全角
        /// 时间：2013-03-25        
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 功能：将参数转成半角
        /// 时间：2013-03-25        
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 功能：汉字转拼音缩写
        /// 日期：2013-03-25         
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// 功能:取单个字符的拼音声母
        /// 日期：2013-03-25         
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = ((short)(array[0] - '\0')) * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";

            if (i < 0xB0C5) return "a";

            if (i < 0xB2C1) return "b";

            if (i < 0xB4EE) return "c";

            if (i < 0xB6EA) return "d";

            if (i < 0xB7A2) return "e";

            if (i < 0xB8C1) return "f";

            if (i < 0xB9FE) return "g";

            if (i < 0xBBF7) return "h";

            if (i < 0xBFA6) return "j";

            if (i < 0xC0AC) return "k";

            if (i < 0xC2E8) return "l";

            if (i < 0xC4C3) return "m";

            if (i < 0xC5B6) return "n";

            if (i < 0xC5BE) return "o";

            if (i < 0xC6DA) return "p";

            if (i < 0xC8BB) return "q";

            if (i < 0xC8F6) return "r";

            if (i < 0xCBFA) return "s";

            if (i < 0xCDDA) return "t";

            if (i < 0xCEF4) return "w";

            if (i < 0xD1B9) return "x";

            if (i < 0xD4D1) return "y";

            if (i < 0xD7FA) return "z";
            return "*";
        }

        /// <summary>
        /// 判定字符串IsNullOrEmpty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(this string source, int length = 10)
        {
            if (source.IsNullOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                if (source.Length > length)
                {
                    return source.Substring(0, length);
                }
                else
                {
                    return source;
                }
            }
        }
        /// <summary>
        /// 转Int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt32(this string source)
        {
            int result = 0;
            if (int.TryParse(source, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(this DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
        /// <summary>
        /// 转decimal
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string source, int length = -1)
        {
            decimal result = 0;
            if (decimal.TryParse(source, out result))
            {
                if (length < 0)
                {
                    return result;
                }
                else
                {
                    return Math.Round(result, 2);
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 转double
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double ToDouble(this string source)
        {
            double result = 0;
            if (double.TryParse(source, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 转String
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToStrings(this object source)
        {
            if (source != null)
            {
                return source.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 转Long
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long ToInt64(this string source)
        {
            long result = 0;
            if (long.TryParse(source, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string source)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse(source, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判定字符串IsNullOrWhiteSpace
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 判定两个字符串是否相等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool IsEqual(this string str, string val, bool ignoreCase = false)
        {
            if (str == null && val == null)
                return true;

            if (str == null || val == null)
                return false;

            return String.Compare(str, val, ignoreCase) == 0;
        }

        /// <summary>
        /// 移除尾部不需要的字符
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="trimChars"></param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder builder, params char[] trimChars)
        {
            if (trimChars == null)
            {
                trimChars = new char[] { ',' };
            }
            while (builder.Length > 0 && trimChars.Contains(builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder;
        }

        /// <summary>
        /// 将字符串传化为Boolean值
        /// </summary>
        /// <param name="strVal">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>bool值</returns>
        public static bool ToBoolean(this string strVal, bool defaultValue = false)
        {
            bool result;
            return bool.TryParse(strVal, out result) ? result : defaultValue;
        }

        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="sWord"></param>
        /// <returns></returns>
        public static bool Is_ExitsKeyWord(this string key)
        {
            var result = false;
            var keyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
            // string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            string StrRegex = @"[|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            if (Regex.IsMatch(key.ToUpper(), keyWord.ToUpper(), RegexOptions.IgnoreCase) || Regex.IsMatch(key.ToUpper(), StrRegex.ToUpper()))
                return true;

            return result;
        }
        /// <summary>
        /// 过滤SQL关键字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSqlKeyWord(this string str)
        {
            if (str.IsNullOrEmpty()) return String.Empty;
            // str = str.ToLower();
            str = str.ReplaceKey("'", "‘");
            str = str.ReplaceKey("'", "");
            str = str.ReplaceKey("\"", "");
            //str = str.ReplaceKey("&", "&amp");
            //str = str.ReplaceKey("<", "&lt");
            //str = str.ReplaceKey(">", "&gt");
            str = str.ReplaceKey("delete", "");
            str = str.ReplaceKey("update", "");
            str = str.ReplaceKey("insert", "");

            //单引号替换成两个单引号
            str = str.ReplaceKey("'", "");
            //半角封号替换为全角封号，防止多语句执行
            str = str.ReplaceKey(";", "；");
            //半角括号替换为全角括号
            str = str.ReplaceKey("(", "（");
            str = str.ReplaceKey(")", "）");
            //去除执行存储过程的命令关键字
            str = str.ReplaceKey("exec", "");
            str = str.ReplaceKey("execute", "");

            //去除系统存储过程或扩展存储过程关键字
            str = str.ReplaceKey("xp_", "x p_");
            str = str.ReplaceKey("sp_", "s p_");
            //防止16进制注入
            str = str.ReplaceKey("0x", "0 x");

            str = str.ReplaceKey("-", " ");
            str = str.ReplaceKey(";", "；");
            str = str.ReplaceKey(",", ",");
            str = str.ReplaceKey("?", "?");
            str = str.ReplaceKey("<", "＜");
            str = str.ReplaceKey(">", "＞");
            str = str.ReplaceKey("(", "(");
            str = str.ReplaceKey(")", ")");
            str = str.ReplaceKey("@", "＠");
            str = str.ReplaceKey("=", "＝");
            str = str.ReplaceKey("+", "＋");
            str = str.ReplaceKey("*", "＊");
            str = str.ReplaceKey("&", "＆");
            str = str.ReplaceKey("#", "＃");
            str = str.ReplaceKey("%", "％");
            str = str.ReplaceKey("$", "￥");

            return str;
        }

        /// <summary>
        /// 是否包含非法关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainsIllegalKeyword(this string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var result = false;
            var keyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and|xp_|sp_|0x";
            // string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|'|?|<|>|(|)|@|=|+|*|&|#|%|$]";
            if (Regex.IsMatch(key.ToUpper(), keyWord.ToUpper(), RegexOptions.IgnoreCase) || Regex.IsMatch(key.ToUpper(), StrRegex.ToUpper()))
                return true;

            return result;
        }

        /// <summary>
        /// 将list转换为SQL字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ToStringIdList<T>(this IEnumerable<T> list, string split = ",")
        {
            string type = typeof(T).FullName;
            string idlist = string.Empty;
            if (list != null && list.Count() > 0)
            {
                list = list.Distinct().ToList();
                switch (type)
                {
                    case "System.Int32":
                    case "System.Int64":
                        idlist = string.Join(split, list);
                        break;
                    case "System.Guid":
                    case "System.String":
                        idlist = string.Format("'{0}'", string.Join("','", list));
                        break;
                }
            }
            return idlist;
        }

        /// <summary>
        /// 是否包含字符串，并忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
        /// <summary>
        /// 忽略大小写，包含特殊字符过滤
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="rlt"></param>
        /// <returns></returns>
        public static string ReplaceKey(this string source, string key, string rlt)
        {
            if (key.IsNullOrEmpty()) return String.Empty;
            return source.Contains(key, StringComparison.CurrentCultureIgnoreCase) ? Regex.Replace(source, key, rlt, RegexOptions.IgnoreCase) : source;

        }
    }
}
