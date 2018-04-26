using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    [Serializable]
    public class ExpertSearchModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 采种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 是否关注 0未关注 1关注
        /// </summary>
        public int isFollow { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }
    }
}