using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;

namespace C8.Lottery.Portal.Models
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SystemMessage : UserInternalMessage
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 消息名称
        /// </summary>
        public string Title { get; set; }
    }
}