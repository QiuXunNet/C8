﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;

namespace C8.Lottery.Portal.Models
{
    /// <summary>
    /// 动态消息
    /// </summary>
    public class DynamicMessage : UserInternalMessage
    {
        /// <summary>
        /// 来源用户Id
        /// </summary>
        public int RefUserId { get; set; }

        public int RefId { get; set; }
        /// <summary>
        /// 来源用户昵称
        /// </summary>
        public string RefNickName { get; set; }
        /// <summary>
        /// 评论人头像
        /// </summary>
        public string FromAvater { get; set; }
        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string FromNickName { get; set; }
        /// <summary>
        /// 评论人用户Id
        /// </summary>
        public string FromUserId { get; set; }
        /// <summary>
        /// 评论类型
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 评论上级Id
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 我的评论内容
        /// </summary>
        public string MyContent { get; set; }
        /// <summary>
        /// 评论关联Id （计划/文章/评论 Id）
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LotteryTypeName { get; set; }

        /// <summary>
        /// 相关评论用户Id（Type=1时）
        /// </summary>
        public int ArticleUserId { get; set; }
        /// <summary>
        /// 动态消息的评论的上级评论
        /// </summary>
        public int RefCommentId { get; set; }
    }
}