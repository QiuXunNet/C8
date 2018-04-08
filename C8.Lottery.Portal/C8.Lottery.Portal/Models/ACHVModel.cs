using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;
namespace C8.Lottery.Portal.Models
{
    public class ACHVModel
    {
     
        public List<LotteryType2> LotteryType { get; set; }//频道
        public List<C8.Lottery.Model.LotteryType2> Lottery { get; set; }//小彩种

        public List<IntegralRule> IntegralRule { get; set; }//玩法
    }
}