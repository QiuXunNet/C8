using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 优惠券类型
    /// </summary>
    [Serializable]
    public partial class Coupon
    {
        public Coupon()
        { }
        #region Model
        private int _id;
        private string _code;
        private int _type;
        private string _name;
        private int _expirydate;
        private int? _ltype;
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
        /// 优惠券编号
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 优惠券类型 0=全部查看券
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 有效期 单位天
        /// </summary>
        public int ExpiryDate
        {
            set { _expirydate = value; }
            get { return _expirydate; }
        }
        /// <summary>
        /// 彩种 小彩种ID
        /// </summary>
        public int? lType
        {
            set { _ltype = value; }
            get { return _ltype; }
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
