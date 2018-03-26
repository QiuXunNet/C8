using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 用户关注粉丝
    /// </summary>
    [Serializable]
    public partial class Follow
    {
        public Follow()
        { }
        #region Model
        private long _id;
        private long _userid;
        private long _followed_userid;
        private int _status = 1;
        /// <summary>
        /// 
        /// </summary>
        public long Id
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
        /// 关注ID
        /// </summary>
        public long Followed_UserId
        {
            set { _followed_userid = value; }
            get { return _followed_userid; }
        }
        /// <summary>
        /// 是否关注  1、关注 0、取消关注 默认1
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}
