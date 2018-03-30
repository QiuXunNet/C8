using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C8.Lottery.Public;
using Newtonsoft.Json;

namespace C8.Lottery.Model
{
    public class BettingRecord
    {
        public long Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public string PlayName { get; set; }
        public string BetNum { get; set; }
        public int WinState { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public DateTime SubTime { get; set; }

        public string LogoIndex
        {
            get { return Util.GetLotteryIcon(lType); }
        }

        public string LotteryTypeName
        {
            get { return Util.GetLotteryTypeName(lType); }
        }

        public string TimeStr
        {
            get { return SubTime.ToString("MM-dd HH:mm"); }
        }
    }
}
