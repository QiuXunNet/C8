// JavaScript Document

$(function () {
    /*首页滑动*/
    //$(window).scroll(function (event) {
    //    var val = $(document).scrollTop();
    //    if (val > 131) {
    //        $(".hjc_CZQieBox").addClass("hjc_CZQieBoxDing");
    //        $(".hjc_indexmain").addClass("M-T");
    //    }
    //    if (val < 131) {
    //        $(".hjc_CZQieBox").removeClass("hjc_CZQieBoxDing");
    //        $(".hjc_indexmain").removeClass("M-T");
    //    }
    //});

    window.addEventListener('scroll', function () {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        if (scrollTop > 131) {
            $(".hjc_CZQieBox").addClass("hjc_CZQieBoxDing");
            $(".hjc_indexmain").addClass("M-T");
        }
        if (scrollTop < 131) {
            $(".hjc_CZQieBox").removeClass("hjc_CZQieBoxDing");
            $(".hjc_indexmain").removeClass("M-T");
        }
    });

    /*返回顶部*/
    $(".Back_Top").click(function (event) {
        $("body,html").stop().animate({ "scrollTop": 0 }, 500);
    });



    /*我的卡券*/
    $(".My_KJHeader li").click(function () {
        var KJ = $(this).index();
        $(this).addClass("current").siblings().removeClass("current");
        // $(".My_KaJuanBox .My_KaJuanK").eq(KJ).show().siblings().hide();
    });

    /*资讯-内容详情*/
    $(".ZX_Sidebar").click(function () {
        $(".ZX_CLBox").toggleClass("CLBox_current");
        $(".CLB_cNav").toggleClass("cNav_current");
    });
    $(".CLB_YY").click(function () {
        $(".ZX_CLBox").removeClass("CLBox_current");
        $(".CLB_cNav").removeClass("cNav_current");
    });
    /*支付  弹窗    js*/
    $(".zfC_XZ li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
    });

    /*清除文本框值*/
    $(".Bul1_del").click(function () {
        $(this).prev().val('');

    });


    /*切换 首页 双面 长龙 走势 技巧*/
    /*$(".CZ_hdNL li").click(function(){
    	$(this).addClass("current").siblings().removeClass("current");
    	var zhi = $(this).index();
    	$(".CZ_main .CZ_Minfo").eq(zhi).show().siblings().hide();
    });*/


    /*开奖切换 号码 大小 单双*/
    $(".zsB_cBTnav .cBTnav_a").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var zhi = $(this).index();
        $(".zsB_cBTmain").each(function () {
            $(this).find('.cBTm_info').eq(zhi).show().siblings().hide();
        });
    });




    /*个人中心-好友列表*/
    $(".buddy_info1").click(function () {
        $(this).siblings().toggle();
    });

    /*个人中心-金币充值*/
    $(".rech_conDl").click(function () {
        $(this).addClass("conDlcurrent").siblings().removeClass("conDlcurrent");
    });
    $(".recharge_list li").click(function () {
        $(".recharge_popup").show();
    });
    $(".recharge_yy").click(function () {
        $(".recharge_popup").hide();
    });

    /*个人中心-个人主页*/
    $(".fans_nav li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var zhi = $(this).index();
        $(".fans_box .fans_Bkuai").eq(zhi).show().siblings().hide();
    });

    $(".info_hdNav_cai_collect .info_hdNav_cai_lhs li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();
        $(".hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        //展开项添加选中
        $(".info_hdNav_cai_expand .info_hdNav_cai li").eq(zhi).addClass("current").siblings().removeClass("current");

    });

    /* 展开项点击 */
    $(".info_hdNav_cai_expand .info_hdNav_cai li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();
        $(".hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        $(".info_hdNav_cai_collect .info_hdNav_cai_lhs li").eq(zhi).addClass("current").siblings().removeClass("current");

    });

    $(".hdNav_cai_collect .hdNav_cai_lhs_expand").click(function () {

        //$(".hdNav_cai_collect").hide();
        $(".hdNav_cai_expand").toggle();
        $(".mask").toggle();
    });

    $(".hdNav_cai_expand .collect").click(function () {
        $(".hdNav_cai_expand").hide();
        $(".hdNav_cai_collect").show();
        $(".mask").hide();
    });

    $(".info_hdNav_cai_collect .info_hdNav_cai_lhs_expand").click(function () {

        // $(".info_hdNav_cai_collect").hide();
        $(".info_hdNav_cai_expand").toggle();
        $(".mask").toggle();
    });

    $(".info_hdNav_cai_expand .collect").click(function () {

        $(".info_hdNav_cai_expand").hide();
        $(".info_hdNav_cai_collect").show();
        $(".mask").hide();
    });

    $(".list_mainUl2 .UL2_hd").click(function () {
        _this = $(this);
        if (_this.hasClass("expand")) {
            _this.removeClass("expand");
            _this.siblings().hide();
        } else {
            _this.addClass("expand");
            _this.siblings().show();
        }
    });

    // $(".list_mainUl2 .UL2_hd").toggle(function() {
    //     $(this).addClass("expand").siblings().show();
    // }, function() {
    //     $(this).removeClass("expand").siblings().hide();
    // });

    /*** Added By LHS 2018-3-1 */

    /*个人中心-参与竞猜*/
    $(".C8_cyjc li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var cyjc = $(this).index();
        $(".guessing_content .gues_info").eq(cyjc).show().siblings().hide();
    });

    /*个人中心-设置-设置性别*/
    $(".Set_Bxbul li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
    });

    /*打赏榜*/
    $(".C8_bang li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var bang = $(this).index();
        $(".Ranking_content .Ranking_Cinfo").eq(bang).show().siblings().hide();
    });

    /*个人中心-交易记录*/
    //$(".C8_JIlU li").click(function () {
    //    alert(11111);
    //    $(this).addClass("current").siblings().removeClass("current");
    //    var bang = $(this).index();
    //    $(".details_box .details_info1").eq(bang).show().siblings().hide();
    //});

    /*我的计划*/
    $(".SX_pop").click(function () {
        $(".ShaiXuan").toggle();
    });
    $(".shaiXuan_yy").click(function () {
        $(".ShaiXuan").hide();
    });

    $(".SX_Ul li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
    });

    /*我的计划*/
    //$(".C8_JIlU li").click(function () {
    //    $(this).addClass("current").siblings().removeClass("current");
    //    var bang = $(this).index();
    //    $(".details_box .details_info1").eq(bang).show().siblings().hide();
    //});

    /*高手榜*/
    $(".GS_btn").click(function () {
        $(".GS_box").toggleClass("GS_boxon");
        $(this).toggleClass("open");
    });
    $(".C8_GPQG li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var GPQG = $(this).index();
        $(".GS_box .GS_nav").eq(GPQG).show().siblings().hide();
    });
    $(".GS_nav li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
    });

    /*规则*/
    $(".Lnav_Ul li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var GPQG = $(this).index();
        $(".GZ_text .GZ_tInfo").eq(GPQG).show().siblings().hide();
    });

    /*发表评论*/
    $(".Com_File").change(function () {
        if ($(this).val()) {
            var objUrl = getObjectURL(this.files[0]);
            console.log("objUrl = " + objUrl);
            var objUrl = getObjectURL(this.files[0]);
            $(this).parent().before('<li><img src=' + objUrl + '><i class="Com_icon"></i></li>');
        }
    })

    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }

    /*我的佣金*/
    $(".C8_Myyj li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var Myyj = $(this).index();
        $(".sion_content .sion_conInfo").eq(Myyj).show().siblings().hide();
    });

    /*我的积分*/
    $(".C8_Myjf li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var Myjf = $(this).index();
        $(".points_text .points_TInfo").eq(Myjf).show().siblings().hide();
    });

    /*个人中心-他人主页*/
    $(".C8_Trzy li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var Trzy = $(this).index();
        $(".Other_text .Other_TInfo").eq(Trzy).show().siblings().hide();
    });



    /*六合彩近期竞猜*/
    $(".ShareBtn").click(function () {
        $(".Gues_share").show();
    });
    $(".Del_Share").click(function () {
        $(".Gues_share").hide();
    });

    /*玩法*/
    $(".WanFa_hdInfo2").click(function () {
        $(this).children(".WF_hdIF2Icpon").toggleClass("WF_hdIF2IcponON");
        $(".WanFa_History").toggle();
    });


    /*计划*/
    $(".QiHao_tit").bind("click", function () {
        $(this).siblings(".QiHao_text").toggle();
    });

    $(".C8_nav5 li").click(function () {
        var Trzy = $(this).index();
        if (Trzy == 0) {
            $(".GS_box").hide();
            $(".GS_btn").hide();
        } else {
            $(".GS_box").show();
            $(".GS_btn").show();
        }
        $("#curNavIndex").val(Trzy);
        $(".GS_nav li:eq(0)").trigger("click");
        $(this).addClass("current").siblings().removeClass("current");
        $(".Plan_content .Plan_CInfo").eq(Trzy).show().siblings().hide();
    });

    /*C8 弹窗 js*/
    //$(".C8_TcDl dd .C8_Qiu").click(function () {
    //    $(this).toggleClass("current");
    //});



    /*资讯 切换彩种*/
    $(".hjc_HeaderXL").click(function () {
        $(".hjc_hdNav").toggle();
        $(".mask2").toggle();
    });

    $(".info_hdnav li").click(function () {
        var thisIndex = $(this).index();
        $(this).addClass("current").siblings().removeClass("current");
        $(".hjc_news_lhs").eq(thisIndex).show().siblings().hide();
    });

    //点击索引查询
    $('body').on('click', '.letter a', function () {
        var _this = $(this);
        var s = _this.html();

        var anchor = (_this.attr("data-id") || "") + '1';
        if (_this.hasClass("toSearch")) {
            anchor = "searchinput"
        }
        $(window).scrollTop($('#' + anchor).offset().top-50);
        $("#showLetter span").html(s);
        $("#showLetter").show().delay(500).hide(0);
        _this.parent().addClass("current").siblings().removeClass("current");
    });
    //中间的标记显示
    $('body').on('onMouse', '.showLetter span', function () {
        $("#showLetter").show().delay(500).hide(0);
    });

    $("#searchinput").change(function () {
        searchList();
    });

    function searchList() {
        var enter = $("#searchinput").val();
        var obj = $(".gallery-list  p");
        localStorage.searchkey = enter;
        if (!enter) {
            for (var i = 0; i < obj.length; i++) {
                var item = $(obj[i]),
                    text = item.attr("data-name");
                item.find(".gallery-name").html(text);
            }
            $(".gallery-list p").show();
            $(".gallery-list").show();

            return;
        }

        $(".gallery-list").hide();
        $(".gallery-list p").hide();

        for (var i = 0; i < obj.length; i++) {
            var item = $(obj[i]),
                text = item.attr("data-name");
            if (text.indexOf(enter) >= 0) {
                var html = text.replace(new RegExp(enter), '<font color="red">' + enter + '</font>');

                item.find(".gallery-name").html(html);
                item.show();
                item.parents('.gallery-list').show();
            }
        }
    };


    $(".hdNav_cai_collect").on("click", "li", function () {

        if (typeof (getChildLotteryType)=="function") {
            var pId = $(this).attr("data-Id");
            getChildLotteryType(pId);
        }
        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();

        var lType = $(this).attr("data-Id");

        //$("#mainUl2" + lType).closest("div").find("ul").hide();
        //$("#mainUl2" + lType).show();
        $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        //展开项添加选中
        $(".hdNav_cai_expand .hdNav_cai li").eq(zhi).addClass("current").siblings().removeClass("current");
        $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();

        $(".hdNav_cai_expand").hide();
        $(".mask").hide();
    });

    /* 展开项点击 */
    $(".hdNav_cai_expand").on("click", "li", function () {
        if (typeof (getChildLotteryType) == "function") {
            var pId = $(this).attr("data-Id");
            getChildLotteryType(pId);
        }

        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();
        $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        $(".hdNav_cai_collect .hdNav_cai_lhs li").eq(zhi).addClass("current").siblings().removeClass("current");
        $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();
    });

    /**我的成绩 彩种切换 */
    //$("#lottery_type li").on('click', function () {
    //    var _this = $(this);
    //    var _index = _this.attr("data-type-id");

    //    _this.addClass("current").siblings().removeClass("current");
    //    alert(_index);
    //    $(".wrapper-container .Grades_" + _index).show().siblings().hide();
    //});

    //$("#Gues1 .comp_DZ i").click(function () {


    $("#Gues1").on("click", ".comp_DZ", function () {
        var _this = $(this).find("i"),
            id = _this.attr("data-id"),
            ctype = _this.attr("data-ctype"),
            type = _this.attr("data-commenttype"),
            likeElement = _this.next(),
            likeCount = parseInt(likeElement.html() || 0);
        $.post("/News/ClickLike",
            {
                id: id,
                ctype: ctype,
                type: type
            }, function (result) {

                if (result) {
                    if (result.Code === 401) {
                        location.href = "/Home/Login";
                    } else if (result.Code === 100) {
                        if (ctype === "1") {
                            _this.addClass("on").attr("data-ctype", "2");
                            likeCount += 1;

                        } else {
                            _this.removeClass("on").attr("data-ctype", "1");
                            likeCount -= 1;
                        }
                        likeElement.html(likeCount);

                        likeElement.html() == 0 ? likeElement.html("") : likeElement.html();

                    } else {
                        alertmsg(result.Message);
                    }
                } else {
                    alertmsg("服务器繁忙");
                }
            });
    });

    /**
     * 点赞
     */
    $("#Gues2").on("click", ".comp_DZ", function () {
        var _this = $(this).find("i"),
            id = _this.attr("data-id"),
            ctype = _this.attr("data-ctype"),
            type = _this.attr("data-commenttype"),
            likeElement = _this.next(),
            likeCount = parseInt(likeElement.html() || 0);
        $.post("/News/ClickLike",
            {
                id: id,
                ctype: ctype,
                type: type
            }, function (result) {
                if (result) {
                    if (result.Code === 401) {
                        location.href = "/Home/Login";
                    } else if (result.Code === 100) {
                        if (ctype === "1") {
                            _this.addClass("on").attr("data-ctype", "2");
                            likeCount += 1;

                        } else {
                            _this.removeClass("on").attr("data-ctype", "1");
                            likeCount -= 1;
                        }
                        likeElement.html(likeCount);

                    } else {
                        alertmsg(result.Message);
                    }
                } else {
                    alertmsg("服务器繁忙");
                }
            });
    });



















    /*********************首页轮播*********************/

    //焦点图                 手机是兼容的  只是在pc上面不兼容IE8
    //TouchSlide({ 
    //	slideCell:"#Gues1",   /*给最大的盒子取id名  要改的 */
    //	mainCell:".Gues_ul",    /* 只是图片  要改的 */
    //	titCell:".Gues_ol",   /*只是点点  要改的  */
    //	effect:"leftLoop",     /*这是方向  只向左  */
    //	interTime:5000,    /*时间*/
    //	autoPage:true,     //自动分页
    //	autoPlay:false      //自动播放  false
    //});

});

/**前三名橙色*/
function GetYsfont(rank)
{
    var color = "";
    if (rank <= 3)
    {
        color = "Ysfont";
    }
    return color;
}



function sortByRank(rank)
{
    var cls = "";
    if (rank == 1) {
        cls = "rank_33_1";
    }
    else if (rank == 2) {
        cls = "rank_33_2";
    }

    return cls;
}


function IsPC()
{
    var userAgentInfo =navigator.userAgent;
    var Agents = ["Android", "iPhone",
       "SymbianOS", "Windows Phone",
       "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++)
    {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}

/**评论 by LHS */
/**
 * 点击分享，显示分享模块
 */
function showShare() {
    $(".Gues_share").show();
}
/**
 * 隐藏分享模块
 */
function hideShare() {
    $(".Gues_share").hide();
}
//弹窗消息
function alertmsg(msg) {
    $(document).dialog({
        type: 'notice',
        infoText: msg,
        autoClose: 1500
    });
}

function follow(userid, type) {
    var url = type == 1 ? "/Personal/UnFollow" : "/Personal/IFollow";
    $.post(url, { followed_userId: userid }, function (data) {

        if (!data.Success) {
            alertmsg(data.Msg);
        } else {
            if (type == 1) {
                $("#follow").attr("data-type", "0").html(" 关注 ");
            } else {

                $("#follow").attr("data-type", "1").html("已关注");
            }
        }
    });
}
$.fn.extend({
    /**
     * 初始化输入区域Emoji
     */
    initEmoji: function () {
        this.emoji({
            button: '#btnEmoji',
            showTab: false,
            animation: 'fade',
            icons: [
                {
                    name: "QQ",
                    path: "/images/emoji/Face/",
                    maxNum: 54,
                    file: ".png",
                    placeholder: "#{alias}#",
                    alias: {
                        1: "am",
                        2: "baiy",
                        3: "bishi",
                        4: "bizui",
                        5: "piezui",
                        6: "cahan",
                        7: "ciya",
                        8: "dabing",
                        9: "daku",
                        10: "fadai",
                        11: "deyi",
                        12: "fanu",
                        13: "fendou",
                        14: "ganga",
                        15: "guzhang",
                        16: "dahaq",
                        17: "haixiu",
                        18: "daxiao",
                        19: "huaixiao",
                        20: "jie",
                        21: "jingkong",
                        22: "jingya",
                        23: "keai",
                        24: "kelian",
                        25: "koubi",
                        26: "kulou",
                        27: "ku",
                        28: "kuaikuliao",
                        29: "kun",
                        30: "lenghan",
                        31: "liuhan",
                        32: "liulei",
                        33: "nanguo",
                        34: "qiaoda",
                        35: "qinqin",
                        36: "qiudale",
                        37: "se",
                        38: "shui",
                        39: "tiaopi",
                        40: "touxiao",
                        41: "tu",
                        42: "weixiao",
                        43: "weiqu",
                        44: "xia",
                        45: "xu",
                        46: "yiwen",
                        47: "yinxian",
                        48: "youhengheng",
                        49: "yun",
                        50: "zaijian",
                        51: "zhemo",
                        52: "dama",
                        53: "zhuakuang",
                        54: "zuohengheng"
                    },
                    title: {
                        1: "傲慢",
                        2: "白眼",
                        3: "鄙视",
                        4: "闭嘴",
                        5: "撇嘴",
                        6: "擦汗",
                        7: "呲牙",
                        8: "大兵",
                        9: "大哭",
                        10: "发呆",
                        11: "得意",
                        12: "发怒",
                        13: "奋斗",
                        14: "尴尬",
                        15: "鼓掌",
                        16: "打哈欠",
                        17: "害羞",
                        18: "大笑",
                        19: "坏笑",
                        20: "饥饿",
                        21: "惊恐",
                        22: "惊讶",
                        23: "可爱",
                        24: "可怜",
                        25: "抠鼻",
                        26: "骷髅",
                        27: "酷",
                        28: "快哭了",
                        29: "困",
                        30: "冷汗",
                        31: "流汗",
                        32: "流泪",
                        33: "难过",
                        34: "敲打",
                        35: "亲亲",
                        36: "糗大了",
                        37: "色",
                        38: "睡",
                        39: "调皮",
                        40: "偷笑",
                        41: "吐",
                        42: "微笑",
                        43: "委屈",
                        44: "吓",
                        45: "嘘",
                        46: "疑问",
                        47: "阴险",
                        48: "右哼哼",
                        49: "晕",
                        50: "再见",
                        51: "折磨",
                        52: "大骂",
                        53: "抓狂",
                        54: "左哼哼"
                    }
                }
            ]
        });
    },

    /**
     * 转换Emoji
     */
    parseEmoji: function () {

        this.emojiParse({
            icons: [{
                path: "/images/emoji/Face/",
                file: ".png",
                placeholder: "#{alias}#",
                alias: {
                    1: "am",
                    2: "baiy",
                    3: "bishi",
                    4: "bizui",
                    5: "piezui",
                    6: "cahan",
                    7: "ciya",
                    8: "dabing",
                    9: "daku",
                    10: "fadai",
                    11: "deyi",
                    12: "fanu",
                    13: "fendou",
                    14: "ganga",
                    15: "guzhang",
                    16: "dahaq",
                    17: "haixiu",
                    18: "daxiao",
                    19: "huaixiao",
                    20: "jie",
                    21: "jingkong",
                    22: "jingya",
                    23: "keai",
                    24: "kelian",
                    25: "koubi",
                    26: "kulou",
                    27: "ku",
                    28: "kuaikuliao",
                    29: "kun",
                    30: "lenghan",
                    31: "liuhan",
                    32: "liulei",
                    33: "nanguo",
                    34: "qiaoda",
                    35: "qinqin",
                    36: "qiudale",
                    37: "se",
                    38: "shui",
                    39: "tiaopi",
                    40: "touxiao",
                    41: "tu",
                    42: "weixiao",
                    43: "weiqu",
                    44: "xia",
                    45: "xu",
                    46: "yiwen",
                    47: "yinxian",
                    48: "youhengheng",
                    49: "yun",
                    50: "zaijian",
                    51: "zhemo",
                    52: "dama",
                    53: "zhuakuang",
                    54: "zuohengheng"
                }
            }]
        });
    }

});

$.extend({
    icons: [
        {
            path: "/images/emoji/Face/",
            file: ".png",
            placeholder: "#{alias}#",
            alias: {
                1: "am",
                2: "baiy",
                3: "bishi",
                4: "bizui",
                5: "piezui",
                6: "cahan",
                7: "ciya",
                8: "dabing",
                9: "daku",
                10: "fadai",
                11: "deyi",
                12: "fanu",
                13: "fendou",
                14: "ganga",
                15: "guzhang",
                16: "dahaq",
                17: "haixiu",
                18: "daxiao",
                19: "huaixiao",
                20: "jie",
                21: "jingkong",
                22: "jingya",
                23: "keai",
                24: "kelian",
                25: "koubi",
                26: "kulou",
                27: "ku",
                28: "kuaikuliao",
                29: "kun",
                30: "lenghan",
                31: "liuhan",
                32: "liulei",
                33: "nanguo",
                34: "qiaoda",
                35: "qinqin",
                36: "qiudale",
                37: "se",
                38: "shui",
                39: "tiaopi",
                40: "touxiao",
                41: "tu",
                42: "weixiao",
                43: "weiqu",
                44: "xia",
                45: "xu",
                46: "yiwen",
                47: "yinxian",
                48: "youhengheng",
                49: "yun",
                50: "zaijian",
                51: "zhemo",
                52: "dama",
                53: "zhuakuang",
                54: "zuohengheng"
            }
        }
    ],

    stringParseEmoji: function (str) {

        var iconsGroup = this.icons;
        var groupLength = iconsGroup.length;
        var path,
            file,
            placeholder,
            alias,
            pattern,
            regexp,
            revertAlias = {};
        if (groupLength > 0) {
            for (var i = 0; i < groupLength; i++) {
                path = iconsGroup[i].path;
                file = iconsGroup[i].file || '.jpg';
                placeholder = iconsGroup[i].placeholder;
                alias = iconsGroup[i].alias;
                if (!path) {
                    continue;
                }
                if (alias) {
                    for (var attr in alias) {
                        if (alias.hasOwnProperty(attr)) {
                            revertAlias[alias[attr]] = attr;
                        }
                    }
                    pattern = placeholder.replace(new RegExp('{alias}', 'gi'), '([\\s\\S]+?)');
                    regexp = new RegExp(pattern, 'gm');
                    str = str.replace(regexp, function ($0, $1) {
                        var n = revertAlias[$1];
                        if (n) {
                            return '<img class="emoji_icon" src="' + path + n + file + '"/>';
                        } else {
                            return $0;
                        }
                    });
                } else {
                    pattern = placeholder.replace(new RegExp('{alias}', 'gi'), '(\\d+?)');
                    str = str.replace(new RegExp(pattern, 'gm'), '<img class="emoji_icon" src="' + path + '$1' + file + '"/>');
                }
            }
        }
        return str;
    }
});
/**评论End by LHS */


//获取六合彩球的颜色
function getColor(num) {
    num = "," + num + ",";

    var red = ",01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46,";
    var blue = ",03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48,";
    var green = ",05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49,";

    if (red.indexOf(num) > -1) {
        return "LHC_spAred HYJP_LhRed";
    }
    else if (blue.indexOf(num) > -1) {
        return "LHC_spAblue HYJP_LhBlue";
    }
    else {
        return "LHC_spAgreen HYJP_LhGreen";
    }
}

//获取六合彩球的生肖
function getShengXiao(digit) {
    var s = "";
    digit = digit < 10 ? "0" + digit : digit;

    var chineseZodiacJsonData = eval("[{ 'BeginTime':'2015-02-19','EndTime':'2016-02-07','Data':[{'Name':'羊','Nums':'01,13,25,37,49'},{'Name':'马','Nums':'02,14,26,38'},{'Name':'蛇','Nums':'03,15,27,39'},{'Name':'龙','Nums':'04,16,28,40'},{'Name':'兔','Nums':'05,17,29,41'},{'Name':'虎','Nums':'06,18,30,42'},{'Name':'牛','Nums':'07,19,31,43'},{'Name':'鼠','Nums':'08,20,32,44'},{'Name':'猪','Nums':'09,21,33,45'},{'Name':'狗','Nums':'10,22,34,46'},{'Name':'鸡','Nums':'11,23,35,47'},{'Name':'猴','Nums':'12,24,36,48'}]},{'BeginTime':'2016-02-08','EndTime':'2017-01-27','Data':[{'Name':'猴','Nums':'01,13,25,37,49'},{'Name':'羊','Nums':'02,14,26,38'},{'Name':'马','Nums':'03,15,27,39'},{'Name':'蛇','Nums':'04,16,28,40'},{'Name':'龙','Nums':'05,17,29,41'},{'Name':'兔','Nums':'06,18,30,42'},{'Name':'虎','Nums':'07,19,31,43'},{'Name':'牛','Nums':'08,20,32,44'},{'Name':'鼠','Nums':'09,21,33,45'},{'Name':'猪','Nums':'10,22,34,46'},{'Name':'狗','Nums':'11,23,35,47'},{'Name':'鸡','Nums':'12,24,36,48'}]},{'BeginTime':'2017-01-28','EndTime':'2018-02-15','Data':[{'Name':'鸡','Nums':'01,13,25,37,49'},{'Name':'猴','Nums':'02,14,26,38'},{'Name':'羊','Nums':'03,15,27,39'},{'Name':'马','Nums':'04,16,28,40'},{'Name':'蛇','Nums':'05,17,29,41'},{'Name':'龙','Nums':'06,18,30,42'},{'Name':'兔','Nums':'07,19,31,43'},{'Name':'虎','Nums':'08,20,32,44'},{'Name':'牛','Nums':'09,21,33,45'},{'Name':'鼠','Nums':'10,22,34,46'},{'Name':'猪','Nums':'11,23,35,47'},{'Name':'狗','Nums':'12,24,36,48'}]},{'BeginTime':'2018-02-16','EndTime':'2019-02-04','Data':[{'Name':'狗','Nums':'01,13,25,37,49'},{'Name':'鸡','Nums':'02,14,26,38'},{'Name':'猴','Nums':'03,15,27,39'},{'Name':'羊','Nums':'04,16,28,40'},{'Name':'马','Nums':'05,17,29,41'},{'Name':'蛇','Nums':'06,18,30,42'},{'Name':'龙','Nums':'07,19,31,43'},{'Name':'兔','Nums':'08,20,32,44'},{'Name':'虎','Nums':'09,21,33,45'},{'Name':'牛','Nums':'10,22,34,46'},{'Name':'鼠','Nums':'11,23,35,47'},{'Name':'猪','Nums':'12,24,36,48'}]},{'BeginTime':'2019-02-05','EndTime':'2020-01-24','Data':[{'Name':'猪','Nums':'01,13,25,37,49'},{'Name':'狗','Nums':'02,14,26,38'},{'Name':'鸡','Nums':'03,15,27,39'},{'Name':'猴','Nums':'04,16,28,40'},{'Name':'羊','Nums':'05,17,29,41'},{'Name':'马','Nums':'06,18,30,42'},{'Name':'蛇','Nums':'07,19,31,43'},{'Name':'龙','Nums':'08,20,32,44'},{'Name':'兔','Nums':'09,21,33,45'},{'Name':'虎','Nums':'10,22,34,46'},{'Name':'牛','Nums':'11,23,35,47'},{'Name':'鼠','Nums':'12,24,36,48'}]},{'BeginTime':'2020-01-25','EndTime':'2021-02-11','Data':[{'Name':'鼠','Nums':'01,13,25,37,49'},{'Name':'猪','Nums':'02,14,26,38'},{'Name':'狗','Nums':'03,15,27,39'},{'Name':'鸡','Nums':'04,16,28,40'},{'Name':'猴','Nums':'05,17,29,41'},{'Name':'羊','Nums':'06,18,30,42'},{'Name':'马','Nums':'07,19,31,43'},{'Name':'蛇','Nums':'08,20,32,44'},{'Name':'龙','Nums':'09,21,33,45'},{'Name':'兔','Nums':'10,22,34,46'},{'Name':'虎','Nums':'11,23,35,47'},{'Name':'牛','Nums':'12,24,36,48'}]},{'BeginTime':'2021-02-12','EndTime':'2022-01-31','Data':[{'Name':'牛','Nums':'01,13,25,37,49'},{'Name':'鼠','Nums':'02,14,26,38'},{'Name':'猪','Nums':'03,15,27,39'},{'Name':'狗','Nums':'04,16,28,40'},{'Name':'鸡','Nums':'05,17,29,41'},{'Name':'猴','Nums':'06,18,30,42'},{'Name':'羊','Nums':'07,19,31,43'},{'Name':'马','Nums':'08,20,32,44'},{'Name':'蛇','Nums':'09,21,33,45'},{'Name':'龙','Nums':'10,22,34,46'},{'Name':'兔','Nums':'11,23,35,47'},{'Name':'虎','Nums':'12,24,36,48'}]},{'BeginTime':'2022-02-01','EndTime':'2023-01-21','Data':[{'Name':'虎','Nums':'01,13,25,37,49'},{'Name':'牛','Nums':'02,14,26,38'},{'Name':'鼠','Nums':'03,15,27,39'},{'Name':'猪','Nums':'04,16,28,40'},{'Name':'狗','Nums':'05,17,29,41'},{'Name':'鸡','Nums':'06,18,30,42'},{'Name':'猴','Nums':'07,19,31,43'},{'Name':'羊','Nums':'08,20,32,44'},{'Name':'马','Nums':'09,21,33,45'},{'Name':'蛇','Nums':'10,22,34,46'},{'Name':'龙','Nums':'11,23,35,47'},{'Name':'兔','Nums':'12,24,36,48'}]}]");

    var nowTime = new Date();

    $(chineseZodiacJsonData).each(function () {
        if (nowTime >= new Date(this.BeginTime) && nowTime < new Date(this.EndTime + " 23:59:59")) {
            $(this.Data).each(function () {
                if (this.Nums.indexOf(digit) > -1) {
                    s = this.Name;
                }
            });
        }
    });

    return s;
}