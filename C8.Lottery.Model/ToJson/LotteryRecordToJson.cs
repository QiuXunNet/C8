using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    [Serializable]
    public class LotteryRecordToJson
    {

        public int Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public string Num { get; set; }
        public System.DateTime SubTime { get; set; }


        //扩展属性
        public string ShowTypeName { get; set; }

        public string ShowIconName { get; set; }

        public string ShowOpenTime { get; set; }

        public string ShowIssue { get; set; }

        //开奖下面一排的信息
        public string ShowInfo { get; set; }
    }
}
