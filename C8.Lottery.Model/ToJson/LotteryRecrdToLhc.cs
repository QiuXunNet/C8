using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    [Serializable]
    public class LotteryRecrdToLhc
    {
        public string Issue { get; set; }
        public string Num { get; set; }

        public string Time { get; set; }
    }
}
