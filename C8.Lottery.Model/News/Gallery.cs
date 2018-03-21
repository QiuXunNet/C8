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
    /// 日 期：2018年3月12日
    /// 描 述：玄机图库推荐图
    /// </summary>
    [Serializable]
    public class Gallery
    {
        /// <summary>
        /// 图库Id
        /// </summary>
        public long Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 彩种分类Id
        /// </summary>
        public long LType { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        
    }
}
