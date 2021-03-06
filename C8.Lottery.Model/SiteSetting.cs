﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 站点设置
    /// Author:LHS
    /// Date:2018年4月5日
    /// </summary>
    [Serializable]
    public class SiteSetting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string KeyWord { get; set; }

        public string Description { get; set; }

        public string LogoPath { get; set; }

        /// <summary>
        /// 网站名字
        /// </summary>
        public string WebName { get; set; }
    }
}
