using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// ClientSourceVersion:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ClientSourceVersion
    {
        public ClientSourceVersion()
        { }
        #region Model
        private long _clientversionid;
        private long _versioncode;
        private string _versionname;
        private int _updatetype;
        private int _clientsource;
        private string _clientversiondesc;
        private long _updatetoversioncode;
        private string _updatetoversionname;
        private string _updatedesc;
        private string _updatelink;
        private DateTime _createtime;
        private DateTime _updatetime;
        private string _servercode;
        private int _clienttype;
        /// <summary>
        /// 主键自增
        /// </summary>
        public long ClientVersionId
        {
            set { _clientversionid = value; }
            get { return _clientversionid; }
        }
        /// <summary>
        /// 版本编号
        /// </summary>
        public long VersionCode
        {
            set { _versioncode = value; }
            get { return _versioncode; }
        }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName
        {
            set { _versionname = value; }
            get { return _versionname; }
        }
        /// <summary>
        /// 升级方式 1=无需升级 2=用户可选择是否升级 3=强制用户升级
        /// </summary>
        public int UpdateType
        {
            set { _updatetype = value; }
            get { return _updatetype; }
        }
        /// <summary>
        /// 客户端
        /// </summary>
        public int ClientSource
        {
            set { _clientsource = value; }
            get { return _clientsource; }
        }
        /// <summary>
        /// 版本描述信息
        /// </summary>
        public string ClientVersionDesc
        {
            set { _clientversiondesc = value; }
            get { return _clientversiondesc; }
        }
        /// <summary>
        /// 升级至版本号
        /// </summary>
        public long UpdateToVersionCode
        {
            set { _updatetoversioncode = value; }
            get { return _updatetoversioncode; }
        }
        /// <summary>
        /// 升级至的版本号名称
        /// </summary>
        public string UpdateToVersionName
        {
            set { _updatetoversionname = value; }
            get { return _updatetoversionname; }
        }
        /// <summary>
        /// 升级描述
        /// </summary>
        public string UpdateDesc
        {
            set { _updatedesc = value; }
            get { return _updatedesc; }
        }
        /// <summary>
        /// 下载链接
        /// </summary>
        public string UpdateLink
        {
            set { _updatelink = value; }
            get { return _updatelink; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 服务器编码
        /// </summary>
        public string ServerCode
        {
            set { _servercode = value; }
            get { return _servercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ClientType
        {
            set { _clienttype = value; }
            get { return _clienttype; }
        }
        #endregion Model

    }
}
