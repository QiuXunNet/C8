
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QX.Common.Utils
{
    /// <summary>
    /// 各种杂项
    /// </summary>
    public static class UtilsHelper
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public static string GetIPAddress
        {
            get { return ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]); }
        }

        #region old utils
        /// <summary>
        /// 获取是否绑定公众号状态
        /// </summary>
        /// <param name="nullity"></param>
        /// <returns></returns>
        public static string GetNullityStatus2(short nullity)
        {
            return nullity == 0 ? "未绑" : "已绑";
            //if (nullity == 0)
            //{
            //    return "<span>未绑</span>";
            //}
            //else
            //{
            //    return "<span style='color:red'>已绑</span>";
            //}
        }
        /// <summary>
        /// 将字符转换为数字，如果不是数字则直接返回0，针对int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetInt(string str)
        {
            if (StrIsInt(str))
            {
                return Convert.ToInt32(str);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断字符是否为数字
        /// </summary>
        /// <param name="Str">被判断字符</param>
        /// <returns></returns>
        public static bool StrIsInt(string Str)
        {
            try
            {
                Int32.Parse(Str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 给出状态(全局通用0:启用; 1:禁止)
        /// </summary>
        /// <param name="nullity"></param>
        /// <returns></returns>
        public static string GetNullityStatus(byte nullity)
        {
            return nullity == 0 ? "启用" : "禁用";
            //if (nullity == 0)
            //{
            //    return "<span >启用</span>";
            //}
            //else
            //{
            //    return "<span  style='color:red'>禁用</span>";
            //}
        }

        /// <summary>
        /// 管理员等级
        /// </summary>
        /// <param name="masterOrder"></param>
        /// <returns></returns>
        public static string GetMasterOrderStatus(byte masterOrder)
        {
            return masterOrder > 0 ? "管理员" : "普通用户";
        }

        public static string ScoreFormat(object fstr)
        {
            if (fstr==null)
            {
                return "";
            }
            string str = "";
            string sNum = fstr.ToString();
            bool isfs = false;

            if (sNum.Length <= 4)
            {
                return sNum;
            }
            else
            {
                if (sNum.Length == 5 && sNum.IndexOf("-") >= 0)
                {
                    return sNum;
                }
                if (sNum.IndexOf('-') >= 0)
                {
                    isfs = true;
                    sNum = sNum.TrimStart('-');
                }
                int len = sNum.Length;
                str = ScoreFormat(sNum.Substring(0, len - 4)) + ',' + sNum.Substring(len - 4);

            }
            if (isfs)
            {
                str = "-" + str;
            }
            return str;

        }


        /// <summary>
        ///  从给定字符串(originalVal)的(startIndex)索引位置开始截取指定长度(cutLength)的字符串
        /// </summary>
        /// <param name="originalVal"></param>
        /// <param name="startIndex"></param>
        /// <param name="cutLength"></param>
        /// <returns></returns>
        public static string CutString(string originalVal, int startIndex, int cutLength)
        {
            if (startIndex >= 0)
            {
                if (cutLength < 0)
                {
                    cutLength *= -1;
                    if ((startIndex - cutLength) < 0)
                    {
                        cutLength = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= cutLength;
                    }
                }
                if (startIndex > originalVal.Length)
                {
                    return "";
                }
            }
            else if ((cutLength >= 0) && ((cutLength + startIndex) > 0))
            {
                cutLength += startIndex;
                startIndex = 0;
            }
            else
            {
                return "";
            }
            if ((originalVal.Length - startIndex) < cutLength)
            {
                cutLength = originalVal.Length - startIndex;
            }
            try
            {
                return originalVal.Substring(startIndex, cutLength);
            }
            catch
            {
                return originalVal;
            }
        }


        /// <summary>
        /// 将秒数转为 天 小时 分钟
        /// </summary>
        /// <returns></returns>
        public static string GetDayHour(int second)
        {
            string retstr = "";
            int day = second / (3600 * 24);
            if (day > 0)
            {
                retstr += day + "天";
            }
            int hour = second % (3600 * 24) / 3600;
            if (hour > 0)
            {
                retstr += hour + "时";
            }
            int min = second % 3600 / 60;
            if (min > 0)
            {
                retstr += min + "分";
            }
            if (retstr == "")
            {
                retstr = second + "秒";
            }
            return retstr;
        }

     

        #endregion

  
        /// <summary>
        /// 获取文件完整路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            if (root.Contains(@"\bin\"))
            {
                root = root.Split(new[] { @"\bin\" }, StringSplitOptions.None).First();
            }

            var fullPath = root + path;
            return fullPath;
        }
        /// <summary>
        /// 微信领取奖励类型
        /// </summary>
        /// <param name="nType"></param>
        /// <returns></returns>
        public static string GetSingLikesType(int nType)
        {
            if (nType == 1)
                return "微信分享";
            else
                return "绑定公众号";
        }
        /// <summary>
        /// 交易类别
        /// </summary>
        /// <param name="TradeType"></param>
        /// <returns></returns>
        public static string GetTradeTypeName(int TradeType)
        {
            string retStr = "";
            switch (TradeType)
            {
                case 1: retStr = "存款"; break;
                case 2: retStr = "取款"; break;
                case 3: retStr = "转账"; break;

            }
            return retStr;
        }

        /// <summary>
        /// 根据订单状态 获取状态字符
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public static string GetOrderStatus(this byte orderStatus)
        {
            switch (orderStatus)
            {
                case 0:
                    return "未付款";
                case 2:
                    return "完成";
                default:
                    return "未定义";
            }
        }

        /// <summary>
        /// 获取指定字符的长度，汉字按两个计算
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetByteLen(string str)
        {
            if (str == null)
            {
                return 0;
            }
            byte[] bytes = Encoding.Default.GetBytes(str);
            return bytes.Length;
        }


        #region [颜色：16进制转成RGB]
        /// <summary>
        /// [颜色：16进制转成RGB]
        /// </summary>
        /// <param name="strColor">设置16进制颜色 [返回RGB]</param>
        /// <returns></returns>
        public static System.Drawing.Color colorHx16toRGB(this string strHxColor)
        {
            try
            {
                if (strHxColor.Length == 0)
                {//如果为空
                    return System.Drawing.Color.FromArgb(0, 0, 0);//设为黑色
                }
                else
                {//转换颜色
                    return System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            catch
            {//设为黑色
                return System.Drawing.Color.FromArgb(0, 0, 0);
            }
        }
        #endregion
        #region [颜色：RGB转成16进制]
        /// <summary>
        /// [颜色：RGB转成16进制]
        /// </summary>
        /// <param name="R">红 int</param>
        /// <param name="G">绿 int</param>
        /// <param name="B">蓝 int</param>
        /// <returns></returns>
        public static string colorRGBtoHx16(int R, int G, int B)
        {
            return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(R, G, B));
        }

        public static string ToRGBString(this int color)
        {
            int r = 0xFF & color;
            int g = 0xFF00 & color;
            g >>= 8;
            int b = 0xFF0000 & color;
            b >>= 16;
            return colorRGBtoHx16(r, g, b);
        }
        public static int ParseRGB(this System.Drawing.Color color)
        {
            return (int)(((int)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }


        public static System.Drawing.Color RGB(this int color)
        {
            int r = 0xFF & color;
            int g = 0xFF00 & color;
            g >>= 8;
            int b = 0xFF0000 & color;
            b >>= 16;
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        #endregion

        /// <summary>
        /// 获取Logon结果
        /// </summary>
        /// <param name="nType"></param>
        /// <param name="logonpass"></param>
        /// <param name="lastlogonpass"></param>
        /// <param name="LogonMachineID"></param>
        /// <param name="LastMachineID"></param>
        /// <param name="LogonPassPortID"></param>
        /// <param name="LastPassPortID"></param>
        /// <returns></returns>
        public static int GetLogonResult(int nType, string logonpass, string lastlogonpass,
            string LogonMachineID, string LastMachineID, string LogonPassPortID, string LastPassPortID)
        {
            var rs = 6;

            if (!logonpass.ToUpper().Equals(lastlogonpass.ToUpper()))
            {
                return 1;
            }
            else if (!LogonMachineID.ToUpper().Equals(LastMachineID.ToUpper()))
            {
                if (nType < 4)
                {
                    if (!LogonPassPortID.ToUpper().Equals(LastPassPortID.ToUpper()))
                    {
                        if (LogonPassPortID.Length != 18)
                            return 2;
                        else
                            return 3;
                    }
                    else if (LogonPassPortID.ToUpper().Equals(LastPassPortID.ToUpper()) && LogonPassPortID.Length == 18)
                        return 4;
                    else
                        return 5;
                }
                else
                {
                    return rs;
                }
            }
            else
            {
                return rs;
            }
        }


        /// <summary>
        /// 是否有彩金
        /// </summary>
        /// <param name="kindId"></param>
        /// <param name="sortId"></param>
        /// <returns></returns>
        public static bool HasHandsel(int kindId, int sortId)
        {
            return kindId == 29 && sortId % 2 == 0;
        }

        /// <summary>
        /// 交易类别
        /// </summary>
        /// <param name="tradeType"></param>
        /// <param name="isSameUserId">equals SourceUserID</param>
        /// <returns></returns>
        public static string GetTradeTypeName(int tradeType, bool isSameUserId)
        {
            string retStr = "";
            switch (tradeType)
            {
                case 1: retStr = "存款"; break;
                case 2: retStr = "取款"; break;
                case 3:
                    if (isSameUserId)
                    { retStr = "转出"; }
                    else
                    {
                        retStr = "转入";
                    }
                    break;

            }
            return retStr;
        }


        /// <summary>
        /// 获取tgType对应的中文说明
        /// </summary>
        /// <param name="tgType"></param>
        /// <returns></returns>
        public static string GetTgTypeName(string tgType)
        {
            string retstr = "";
            switch (tgType)
            {
                case "download": retstr = "下载"; break;
                case "install": retstr = "安装"; break;
                case "unins": retstr = "卸载"; break;
            }
            return retstr;
        }
    }
}
