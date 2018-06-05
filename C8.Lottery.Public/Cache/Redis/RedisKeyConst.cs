using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Public
{
    /// <summary>
    /// Redis键 常量类
    /// </summary>
    public sealed class RedisKeyConst
    {
        /// <summary>
        /// 首页子彩种列表缓存：{0}为父类ID
        /// </summary>
        public const string Home_ChildLotteryType = "home:child_lottery_type:{0}:website";

        /// <summary>
        /// 首页彩种列表缓存：{0}为父类ID
        /// </summary>
        public const string Home_IndexLotteryList = "home:index_Lottery_List:{0}";

        /// <summary>
        /// 六合彩开奖记录
        /// </summary>
        public const string Home_ChildLotteryTypeLHC = "home:child_lottery_type:5:lhc";

        /// <summary>
        /// 新闻栏目SEO信息,{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_SeoInfo = "news:seoinfo:{0}:{1}";

        /// <summary>
        /// 新闻频道列表
        /// </summary>
        public const string Base_NewslTypeList = "base:news_ltype_list:all";

        /// <summary>
        /// 新闻频道栏目列表,{0}频道ID
        /// </summary>
        public const string News_ChannelList = "news:channel_list:{0}";

        /// <summary>
        /// 新闻频道栏目总列表
        /// </summary>
        public const string News_ChannelListALL = "news:channel_list:all";

        /// <summary>
        /// 新闻列表数据，{0}栏目ID,{1}分页页码
        /// </summary>
        public const string News_NewsList = "news:news_list:{0}:{1}";

        /// <summary>
        /// 新闻列表广告列表总数据
        /// </summary>
        public const string News_AdvertisementAll = "news:advertisement:all";

        /// <summary>
        /// 新闻图库类型列表，{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_GalleryTypeList = "news:gallery_type_list:{0}:{1}";

        /// <summary>
        /// 新闻图库列表，{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_GalleryList = "news:gallery_list:{0}:{1}";

        /// <summary>
        /// 网站信息
        /// </summary>
        public const string Base_SiteSetting = "base:site_setting:site";

        /// <summary>
        /// 彩种分类信息
        /// </summary>
        public const string Base_LotteryType = "base:lottery_type:type";

        /// <summary>
        /// 新闻栏目分类列表,{0}彩种ID
        /// </summary>
        public const string Base_NewsType = "base:news_type:{0}";

        /// <summary>
        /// 当前分类的玄机图库,{0}新闻Id
        /// </summary>
        public const string Base_GalleryId = "base:gallery_id:{0}";

        /// <summary>
        /// 彩种玩法,{0}彩种ID
        /// </summary>
        public const string Base_PlayName = "base:play_name:{0}";

        /// <summary>
        /// 贴子点阅扣费配置表
        /// </summary>
        public const string Base_LotteryChargeSettings = "base:lottery_charge_settings:set";

        /// <summary>
        /// 分佣配置
        /// </summary>
        public const string Base_CommissionSettings = "base:commission_settings:set";

        /// <summary>
        /// 安装包,{0}客户端来源
        /// </summary>
        public const string Installationpackage_Sourceversion = "installationpackage:sourceversion:{0}";

        /// <summary>
        /// 登录信息,{0}guid
        /// </summary>
        public const string Login_LoginGuid = "login:logon_guid:{0}";

        /// <summary>
        /// 父彩种
        /// </summary>
        public const string Home_FatherLotteryType = "home:father_lottery_type:website";

        /// <summary>
        /// 首页新闻
        /// </summary>
        public const string Home_NewsList = "home:news_list:website";
    }
}
