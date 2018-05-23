using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Business.BusinessData
{
    public class NewNews
    {
        /// <summary>
		/// 新闻主键
        /// </summary>		
        public long Id { get; set; }

        /// <summary>
        /// 缩率图样式 0=无图 1=1张小图 2=1张大图 3=大于1张小图 
        /// </summary>
        public int ThumbStyle { get; set; }

        public List<string> ThumbList { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 完整标题
        /// </summary>		
        public string FullHead { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime ReleaseTime { get; set; }

        public string ThumbListStr { get; set; }
    }
}