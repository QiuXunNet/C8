﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：KCP
    /// 日 期：2018年3月20日
    /// 描 述：用户信息类
    /// </summary>	
    [Serializable]
    public partial class UserInfo
    {
        public UserInfo()
        { }
        #region Model
        private long _id;
        private string _username;
        private string _name;
        private string _password;
        private string _mobile;
        private int _coin;
        private decimal _money = 0M;
        private int _integral = 0;
        private DateTime _subtime;
        private DateTime _lastlogintime;
        private int _state;
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Coin
        {
            set { _coin = value; }
            get { return _coin; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral
        {
            set { _integral = value; }
            get { return _integral; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }
        /// <summary>
        /// 状态 0启用 1禁用
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }

    }
    #endregion Model
}
