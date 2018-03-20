using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    /// <summary>
    /// 新闻列表
    /// </summary>
    public class NewsListResViewModel
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public string NewsId { get; set; } 
        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime ReleaseTime { get; set; }
        /// <summary>
        /// 发布时间字符串
        /// </summary>
        public string ReleaseTimeStr {
            get { return ReleaseTime.ToString("M-dd HH:mm"); }
        }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumb { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        
    }
}