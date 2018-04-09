﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;
namespace C8.Lottery.Portal.Models
{
    public class ComeOutRecordModel:ComeOutRecord
    {
        


        //针对消费记录
        
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { set; get; }
         /// <summary>
         /// 彩种图片
         /// </summary>
        public string LotteryIcon { get; set; }
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }

        /// <summary>
        ///  打赏人、点阅人用户ID
        /// </summary>
        public int BUserId { get; set; }

        /// <summary>
        /// 打赏人、点阅人名字
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPath { get; set; }


    }
}