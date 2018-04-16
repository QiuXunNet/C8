using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    public class TaskModel
    {
        public int Id
        {
            get;
            set;
           
        }
        public int Code {
            get;set;
        }
        /// <summary>
        /// 任务项
        /// </summary>
        public string TaskItem
        {
            get;
            set;
        }
        /// <summary>
        /// 奖励金币
        /// </summary>
        public int Coin
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime
        {
            get;
            set;
        }
        /// <summary>
        /// 任务次数
        /// </summary>
        public int Count
        {
            get;
            set;
        }

     
        /// <summary>
        /// 已完成次数
        /// </summary>
        public int CompletedCount
        {
            get;
            set;
        }

    }
}