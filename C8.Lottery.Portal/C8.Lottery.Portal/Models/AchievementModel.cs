using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;
namespace C8.Lottery.Portal.Models
{
    public class AchievementModel
    {
        public LotteryNum LotteryNum { get; set; }//开奖号

        public List<BettingRecord> BettingRecord { get; set; }//成绩

    }
   

    public class LotteryNum
    {
        public string SubTime { get; set; }

        public int lType { get; set; }
        public string Issue { get; set; }
        public string Num { get; set; }
    }
}