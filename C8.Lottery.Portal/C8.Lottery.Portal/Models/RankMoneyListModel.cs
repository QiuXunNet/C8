using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    [Serializable]
    public class RankMoneyListModel
    {
        public List<RankMonyModel> RankMonyModelList { get; set; }
        public MyRankMonyModel MyRankMonyModel { get; set; }

    }
    public class MyRankMonyModel : RankModel
    {

    }
    public class RankMonyModel:RankModel
    {

    }
    public class RankModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
    }
  
   
    public class RankIntegralListModel
    {
        public List<RankIntegralModel> RankIntegralList { get; set; }
        public RankIntegralModel MyRankIntegralModel { get; set; }

    }

  
    public class RankIntegralModel
    {
        public int Rank { set; get; }
        public int Score { set; get; }
        public int UserId { set; get; }
        public DateTime Date { set; get; }
        public string NickName { set; get; }
        public string Avater { set; get; }
    }

}