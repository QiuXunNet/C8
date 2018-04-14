using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;

namespace C8.Lottery.Portal.Models
{
    /// <summary>
    /// 帖子信息
    /// </summary>
    public class BettingRecordViewModel : BettingRecord
    {
        /// <summary>
        /// 是否阅读过该帖子
        /// </summary>
        public bool IsRead { get; set; }
    }
}