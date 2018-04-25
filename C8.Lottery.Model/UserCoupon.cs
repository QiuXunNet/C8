using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 用户优惠券表
    /// </summary>
    [Serializable]
    public partial class UserCoupon
    {
        public UserCoupon()
        { }
        #region Model
        private int _id;
        private long _userid;
        private string _couponcode;
        private long _planid;
        private DateTime _begintime;
        private DateTime _endtime;
        private int _fromtype;
        private int _state;
        private DateTime? _subtime;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 优惠券编号
        /// </summary>
        public string CouponCode
        {
            set { _couponcode = value; }
            get { return _couponcode; }
        }
        /// <summary>
        /// 计划ID 已使用情况下 记录在哪个计划下使用
        /// </summary>
        public long PlanId
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 来源 1=注册 2=邀请注册
        /// </summary>
        public int FromType
        {
            set { _fromtype = value; }
            get { return _fromtype; }
        }
        /// <summary>
        /// 状态 1=未使用 2=已使用
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        #endregion Model

    }
}
