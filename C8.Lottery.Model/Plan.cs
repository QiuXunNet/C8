using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Model
{
    public class Plan
    {
        public int Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public int Sort { get; set; }
        public string Num { get; set; }
        public string Result { get; set; }


        //---------------------

        public string ShowSort1
        {
            get
            {
                string result = "";

                //时时彩
                if (lType == 5)
                {
                    #region 六合

                    if (Sort == 0)
                    {
                        result = "平码";
                    }
                    else if (Sort == 1)
                    {
                        result = "尾数";
                    }
                    else if (Sort == 2)
                    {
                        result = "特肖";
                    }
                    else if (Sort == 3)
                    {
                        result = "一肖";
                    }
                    else if (Sort == 4)
                    {
                        result = "特码大小";
                    }
                    else if (Sort == 5)
                    {
                        result = "特码单双";
                    }
                    else if (Sort == 6)
                    {
                        result = "特码20码";
                    }
                    else if (Sort == 7)
                    {
                        result = "五不中";
                    }

                    #endregion
                }
                else if ((lType >= 51 && lType < 60) || (lType == 2 || lType == 4 || lType == 8))
                {
                    #region 快乐十分

                    if (Sort == 0)
                    {
                        result = "第一球";
                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "第六球";
                    }
                    else if (Sort == 6)
                    {
                        result = "第七球";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八球";
                    }

                    #endregion
                }
                else if (lType < 9 || (lType >= 9 && lType < 51) || lType == 65 || (lType >= 60 && lType < 63))
                {
                    #region 时时彩

                    if (Sort == 0)
                    {
                        result = "第一球";
                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "龙虎";
                    }

                    #endregion
                }
                else if (lType == 63 || lType == 64)
                {
                    #region PK10

                    if (Sort == 0)
                    {
                        result = "冠军";
                    }
                    else if (Sort == 1)
                    {
                        result = "亚军";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三名";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四名";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五名";
                    }
                    else if (Sort == 5)
                    {
                        result = "第六名";
                    }
                    else if (Sort == 6)
                    {
                        result = "第七名";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八名";
                    }
                    else if (Sort == 8)
                    {
                        result = "第九名";
                    }
                    else if (Sort == 9)
                    {
                        result = "第十名";
                    }


                    #endregion
                }
               


                return result;
            }
        }




        public string ShowSort
        {
            get
            {
                string result = "";

                
                if (lType == 5)
                {
                    #region 六合彩

                    if (Sort == 0)
                    {
                        result = "特码单双";
                    }
                    else if (Sort == 1)
                    {
                        result = "特码大小";
                    }
                    else if (Sort == 2)
                    {
                        result = "特尾大小";
                    }
                    else if (Sort == 3)
                    {
                        result = "特合单双";
                    }
                    else if (Sort == 4)
                    {
                        result = "特码波色";
                    }
                    else if (Sort == 5)
                    {
                        result = "平特一肖";
                    }
                    else if (Sort == 6)
                    {
                        result = "四肖中特";
                    }
                    else if (Sort == 7)
                    {
                        result = "六肖中特";
                    }
                    else if (Sort == 8)
                    {
                        result = "二十码中特";
                    }
                    else if (Sort == 9)
                    {
                        result = "十码中特";
                    }
                    else if (Sort == 10)
                    {
                        result = "平五码";
                    }
                    else if (Sort == 11)
                    {
                        result = "五不中";
                    }
                    else if (Sort == 12)
                    {
                        result = "六不中";
                    }

                    #endregion
                }
                else if (lType == 1)
                {
                    #region 福彩3D
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "杀一码";
                    }
                    else if (Sort == 4)
                    {
                        result = "杀二码";

                    }
                    else if (Sort == 5)
                    {
                        result = "胆一码";

                    }
                    else if (Sort == 6)
                    {
                        result = "胆二码";
                    }
                    else if (Sort == 7)
                    {
                        result = "胆三码";
                    }
                    #endregion
                }else if (lType == 2)
                {
                    #region 双色球
                    if (Sort == 0)
                    {
                        result = "红2胆";

                    }
                    else if (Sort == 1)
                    {
                        result = "红3胆";

                    }
                    else if (Sort == 2)
                    {
                        result = "杀3红";
                    }
                    else if (Sort == 3)
                    {
                        result = "蓝单双";
                    }
                    else if (Sort == 4)
                    {
                        result = "蓝大小";

                    }
                    else if (Sort == 5)
                    {
                        result = "龙头单双";
                    }
                    else if (Sort == 6)
                    {
                        result = "凤尾单双";
                    }
                    else if (Sort == 7)
                    {
                        result = "蓝5码";
                    }
                    else if (Sort == 8)
                    {
                        result = "蓝8码";
                    }
                    #endregion
                }else if (lType == 3)
                {
                    #region 七星彩
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";

                    }
                    else if (Sort == 5)
                    {
                        result = "第六球";

                    }
                    else if (Sort == 6)
                    {
                        result = "第七球";
                    }
                   
                    #endregion
                }else if (lType == 6)
                {
                    #region 排列3
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "杀一码";
                    }
                    else if (Sort == 4)
                    {
                        result = "杀二码";

                    }
                    else if (Sort == 5)
                    {
                        result = "胆一码";

                    }
                    else if (Sort == 6)
                    {
                        result = "胆二码";
                    }
                    else if (Sort == 7)
                    {
                        result = "胆三码";
                    }

                    #endregion
                }else if (lType == 7)
                {
                    #region 排列5
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";

                    }
                    
                    #endregion
                }else if (lType == 8)
                {
                    #region 七乐彩
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";

                    }
                    else if (Sort == 5)
                    {
                        result = "第六球";

                    }
                    else if (Sort == 6)
                    {
                        result = "第七球";

                    }
                    else if (Sort == 7)
                    {
                        result = "胆一码";

                    }
                    else if (Sort == 8)
                    {
                        result = "胆二码";

                    }
                    else if (Sort == 9)
                    {
                        result = "胆三码";

                    }
                    else if (Sort == 10)
                    {
                        result = "杀三码";

                    }
                    else if (Sort == 11)
                    {
                        result = "杀五码";

                    }

                    #endregion
                }else if (lType == 4)
                {
                    #region 大乐透
                    if (Sort == 0)
                    {
                        result = "前区4码";

                    }
                    else if (Sort == 1)
                    {
                        result = "前区杀4码";

                    }
                    else if (Sort == 2)
                    {
                        result = "前区龙头单双";
                    }
                    else if (Sort == 3)
                    {
                        result = "前区凤尾单双";
                    }
                    else if (Sort == 4)
                    {
                        result = "后区3码";

                    }
                    else if (Sort == 5)
                    {
                        result = "后区杀3码";

                    }
                    else if (Sort == 6)
                    {
                        result = "后区龙头单双";

                    }
                    else if (Sort == 7)
                    {
                        result = "后区凤尾单双";

                    }
                    
                    #endregion
                }else if(lType>=38 && lType <= 50)
                {
                    #region 快三
                    if (Sort == 0)
                    {
                        result = "和值";

                    }
                    else if (Sort == 1)
                    {
                        result = "独胆";

                    }
                    else if (Sort == 2)
                    {
                        result = "双胆";
                    }
                    
                    #endregion
                }else if(lType>=15 && lType <= 37)
                {
                    #region 11选5
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "总和";
                    }
                    else if (Sort == 6)
                    {
                        result = "龙虎";
                    }
                    #endregion
                }else if(lType>=9 && lType <= 14)
                {
                    #region 时时彩
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "总和";
                    }
                    
                    #endregion
                }else if(lType>=63 && lType <= 64)
                {
                    #region pk10
                    if (Sort == 0)
                    {
                        result = "冠军";

                    }
                    else if (Sort == 1)
                    {
                        result = "亚军";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三名";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四名";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五名";
                    }
                    else if (Sort == 5)
                    {
                        result = "第六名";
                    }
                    else if (Sort == 6)
                    {
                        result = "第七名";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八名";
                    }
                    else if (Sort == 8)
                    {
                        result = "第九名";
                    }
                    else if (Sort == 9)
                    {
                        result = "第十名";
                    }
                    #endregion

                }else if(lType>=51 && lType <= 59)
                {
                    #region 快乐十分
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";

                    }
                    else if (Sort == 5)
                    {
                        result = "第六球";

                    }
                    else if (Sort == 6)
                    {
                        result = "第七球";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八球";
                    }
                    #endregion
                }else if(lType>=60 && lType <= 62)
                {
                    #region 其他
                    if (Sort == 0)
                    {
                        result = "第一球";

                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";

                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";

                    }
                    else if (Sort == 5)
                    {
                        result = "总和";

                    }
                    else if (Sort == 6)
                    {
                        result = "龙虎";
                    }
                    #endregion
                }

                return result;
            }
        }


        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNum { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// 开奖号码别名
        /// </summary>
        public string OpenNumAlias
        {
          
            get
            {
              
                if (string.IsNullOrWhiteSpace(OpenNum)) return OpenNum;

                return Util.GetShowInfo(lType, OpenNum, OpenTime);
            }
            set { }
        }
    }

    public class OpenRecord
    {
        public int lType { set; get; }

        public string Issue { set; get; }

        public string Num { set; get; }
        public DateTime SubTime { set; get; }

        public string ShowInfo
        {
            get
            {
                string result = "";

                string[] arr = this.Num.Split(',');


                if (lType >= 9 && lType < 15)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              int.Parse(arr[4]);

                    string daxiao = sum >= 23 ? "大" : "小";
                    string danshuang = sum % 2 == 0 ? "双" : "单";

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[4]);
                    string longhu = "";

                    if (a > b)
                    {
                        longhu = "龙";
                    }
                    else if (a < b)
                    {
                        longhu = "虎";
                    }
                    else if (a == b)
                    {
                        longhu = "和";
                    }

                    result = sum + "," + danshuang + "," + daxiao + "," + longhu;

                }
                else if (lType == 5)
                {
                    result += Util.GetShengxiaoByDigit(int.Parse(arr[0]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[1]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[2]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[3]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[4]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[5]), this.SubTime) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[6]), this.SubTime);
                }
                else if (lType == 63 || lType == 64)
                {
                    int guanyahe = int.Parse(arr[0]) + int.Parse(arr[1]);
                    string danshuang = guanyahe % 2 == 0 ? "双" : "单";
                    string daxiao = guanyahe > 11 ? "大" : "小";

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[1]);
                    int c = int.Parse(arr[2]);
                    int d = int.Parse(arr[3]);
                    int e = int.Parse(arr[4]);
                    int f = int.Parse(arr[5]);
                    int g = int.Parse(arr[6]);
                    int h = int.Parse(arr[7]);
                    int i = int.Parse(arr[8]);
                    int j = int.Parse(arr[9]);

                    string longhu1 = a > j ? "龙" : "虎";
                    string longhu2 = b > i ? "龙" : "虎";
                    string longhu3 = c > h ? "龙" : "虎";
                    string longhu4 = d > g ? "龙" : "虎";
                    string longhu5 = e > f ? "龙" : "虎";

                    result = guanyahe + "," + danshuang + "," + daxiao + "," + longhu1 + "," + longhu2 + "," + longhu3 +
                             "," + longhu4 + "," + longhu5;

                }
                else if (lType >= 15 && lType < 38)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              +int.Parse(arr[4]);

                    string danshuang = sum % 2 == 0 ? "双" : "单";
                    string daxiao = sum >= 31 ? "大" : "小";

                    if (sum == 30)
                    {
                        daxiao = "和";
                    }

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[4]);

                    string longhu = a > b ? "龙" : "虎";

                    result = sum + "," + danshuang + "," + daxiao + "," + longhu;
                }
                else if (lType >= 51 && lType < 60)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              int.Parse(arr[4]) + int.Parse(arr[5]) + int.Parse(arr[6]) + int.Parse(arr[7]);


                    string danshuang = sum % 2 == 0 ? "双" : "单";
                    string daxiao = "";
                    if (sum >= 85 && sum <= 132)
                    {
                        daxiao = "大";
                    }
                    else if (sum >= 36 && sum <= 83)
                    {
                        daxiao = "小";
                    }
                    else if (sum == 84)
                    {
                        daxiao = "和";
                    }

                    string weishu = sum.ToString();

                    int wei = int.Parse(weishu.Substring(weishu.Length - 1, 1));

                    string weidaxiao = wei >= 5 ? "尾大" : "尾小";


                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[7]);

                    string longhu = a > b ? "龙" : "虎";

                    result = sum + "," + danshuang + "," + daxiao + "," + weidaxiao + "," + longhu;

                }

                return result;
            }
        }
    }

}