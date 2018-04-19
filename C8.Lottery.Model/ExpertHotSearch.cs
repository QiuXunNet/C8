using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// ExpertHotSearch:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ExpertHotSearch
    {
        public ExpertHotSearch()
        { }
        #region Model
        private int _id;
        private long _userid;
        private int _count = 0;
        private int _ltype;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 被搜次数
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 采种Id
        /// </summary>
        public int lType
        {
            set { _ltype = value; }
            get { return _ltype; }
        }
        #endregion Model

    }
}
