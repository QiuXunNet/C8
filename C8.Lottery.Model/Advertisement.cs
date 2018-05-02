using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    [Serializable]
    public partial class Advertisement
    {
        public Advertisement()
        { }
        #region Model
        private int _id;
        private int _adtype;
        private string _location;
        private string _title;
        private int _layer;
        private int _thumbstyle;
        private DateTime _begintime = DateTime.Now;
        private DateTime _endtime = DateTime.Now;
        private int _state = 0;
        private DateTime _subtime;
        private string _company;
        private string _where;
        private string _transferurl;
        private int _commentsnumber = 0;
        public List<string> ThumbList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 广告类型 1-栏目广告 2-文章广告
        /// </summary>
        public int AdType
        {
            set { _adtype = value; }
            get { return _adtype; }
        }
        /// <summary>
        /// 位置(栏目ID 1,2,3 逗号分隔)
        /// </summary>
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        /// <summary>
        /// 广告名
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 层级
        /// </summary>
        public int Layer
        {
            set { _layer = value; }
            get { return _layer; }
        }
        /// <summary>
        /// 缩略图类型 0=无图 1=1张小图 2=1张大图 3=大于1张小图
        /// </summary>
        public int ThumbStyle
        {
            set { _thumbstyle = value; }
            get { return _thumbstyle; }
        }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 0=有结束时间广告 1=无结束时间广告 2=已禁用 3=已删除
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        /// <summary>
        /// 广告主
        /// </summary>
        public string Company
        {
            set { _company = value; }
            get { return _company; }
        }
        /// <summary>
        /// 广告设备 (1,2,3)逗号分隔 1=网页 2=IOS 3=安卓
        /// </summary>
        public string Where
        {
            set { _where = value; }
            get { return _where; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TransferUrl
        {
            set { _transferurl = value; }
            get { return _transferurl; }
        }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentsNumber
        {
            set { _commentsnumber = value; }
            get { return _commentsnumber; }
        }

        public string TimeStr
        {
            get { return SubTime.ToString("MM-dd HH:mm"); }
        }
        #endregion Model

    }
}
