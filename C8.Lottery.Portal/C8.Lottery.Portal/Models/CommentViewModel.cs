using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    public class CommentViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 关联评论用户Id
        /// </summary>
        public int RefUserId { get; set; }
        /// <summary>
        /// 操作类型 1=评论 2=回复
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 评论类型 1=计划 2=文章
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 上级评论/回复人昵称，用于回复
        /// </summary>
        public string RefCommentName { get; set; }

    }
}