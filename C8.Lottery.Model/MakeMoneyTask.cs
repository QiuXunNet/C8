using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    ///赚钱任务 实体
    /// </summary>
    [Serializable]
    public partial class MakeMoneyTask
    {
        public MakeMoneyTask()
        { }
        #region Model
        private int _id;
        private string _taskitem;
        private int _coin = 0;
        private DateTime _subtime;
        private int _count = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 任务项
        /// </summary>
        public string TaskItem
        {
            set { _taskitem = value; }
            get { return _taskitem; }
        }
        /// <summary>
        /// 奖励金币
        /// </summary>
        public int Coin
        {
            set { _coin = value; }
            get { return _coin; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        /// <summary>
        /// 任务次数
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        #endregion Model

    }
}
