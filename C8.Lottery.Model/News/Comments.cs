using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描 述：新闻评论实体
    /// </summary>
    public class Comments
    {
        /// <summary>
		/// 评论ID
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>		
        public string Comment { get; set; }
        /// <summary>
        /// 上级评论ID
        /// </summary>		
        public long ParentId { get; set; }
        /// <summary>
        /// 新闻ID
        /// </summary>		
        public string NewsId { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>		
        public int Likes { get; set; }
        /// <summary>
        /// 评论人Id
        /// </summary>		
        public string CommentUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更改时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>		
        public bool DeleteMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
    }
}
