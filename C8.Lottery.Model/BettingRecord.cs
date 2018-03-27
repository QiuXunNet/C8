using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
