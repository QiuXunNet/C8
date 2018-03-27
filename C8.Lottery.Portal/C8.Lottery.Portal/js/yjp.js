// JavaScript Document

$(function () {


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


    /*index-切换彩种*/
    $(".hdNav_cai li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var zhi = $(this).index();
        $(".hjc_indexmain .ind_mainUl2").eq(zhi).show().siblings().hide();
    });

    /*index-切换彩种*/
    $(".hdNav_cai li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var zhi = $(this).index();
        $(".hjc_indexmain .ind_mainUl2").eq(zhi).show().siblings().hide();

        $(".hdNav_cai_collect .hdNav_cai_lhs li").eq(zhi).addClass("current").siblings().removeClass("current");
    });

    /*** Added By LHS 2018-3-1  */
    $(".hdNav_cai_collect .hdNav_cai_lhs li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();
        $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        //展开项添加选中
        $(".hdNav_cai_expand .hdNav_cai li").eq(zhi).addClass("current").siblings().removeClass("current");
        $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();
    });

    /* 展开项点击 */
    $(".hdNav_cai_expand .hdNav_cai li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");

        var zhi = $(this).index();
        $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();

        $(".hdNav_cai_collect .hdNav_cai_lhs li").eq(zhi).addClass("current").siblings().removeClass("current");
        $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();
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
        $(".hdNav_cai_collect").hide();
        $(".hdNav_cai_expand").show();
        $(".mask").show();
    });

    $(".hdNav_cai_expand .collect").click(function () {
        $(".hdNav_cai_expand").hide();
        $(".hdNav_cai_collect").show();
        $(".mask").hide();
    });

    $(".info_hdNav_cai_collect .info_hdNav_cai_lhs_expand").click(function () {
        $(".info_hdNav_cai_collect").hide();
        $(".info_hdNav_cai_expand").show();
        $(".mask").show();
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
    $(".C8_JIlU li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var bang = $(this).index();
        $(".details_box .details_info1").eq(bang).show().siblings().hide();
    });

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
    $(".C8_JIlU li").click(function () {
        $(this).addClass("current").siblings().removeClass("current");
        var bang = $(this).index();
        $(".details_box .details_info1").eq(bang).show().siblings().hide();
    });

    /*高手榜*/
    $(".GS_btn").click(function () {
        $(".GS_box").toggleClass("GS_boxon");
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
        $(this).addClass("current").siblings().removeClass("current");
        $(".Plan_content .Plan_CInfo").eq(Trzy).show().siblings().hide();
    });

    /*C8 弹窗 js*/
    $(".C8_TcDl dd .C8_Qiu").click(function () {
        $(this).toggleClass("current");
    });



    /*资讯 切换彩种*/
    $(".hjc_logo").click(function () {
        $(".hjc_hdNav").toggle();
        $(".mask").toggle();
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
        var anchor = s + '1';
        if (_this.hasClass("toSearch")) {
            anchor = "searchinput"
        }
        $(window).scrollTop($('#' + anchor).offset().top);
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
        var obj = $(".gallery-list p");
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


    function searchlist() {

        var enter = $('#keyword').val();
        var obj = $('.p a');
        localStorage.searchkey = enter;
        if (!enter) {
            for (var i = 0; i < obj.length; i++) {
                var text = $(obj[i]).attr('title');
                $(obj[i]).html(text);
            }
            $('.onebox').show();
            $('.p').show();
            return false;
        }

        $('.onebox').hide();
        $('.p').hide();
        for (var i = 0; i < obj.length; i++) {
            var text = $(obj[i]).attr('title');
            if (text.indexOf(enter) >= 0) {
                var html = text.replace(new RegExp(enter), '<font color="red">' + enter + '</font>');
                $(obj[i]).parents('.p').show();
                $(obj[i]).parents('.onebox').show();
                $(obj[i]).html(html);
            }
        }

    }

    /**我的成绩 彩种切换 */
    $("#lottery_type li").on('click', function () {
        var _this = $(this);
        var _index = _this.attr("data-type-id");
        _this.addClass("current").siblings().removeClass("current");

        $(".wrapper-container .Grades" + _index).show().siblings().hide();
    });

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
                            _this.addClass("on").attr("data-ctype","2");
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


$.fn.extend({

    /**
     * 初始化输入区域Emoji
     */
    initEmoji: function () {
        this.emoji({
            button: '#btnEmoji',
            showTab: true,
            animation: 'fade',
            icons: [
                {
                    name: "经典表情",
                    path: "/images/emoji/qq/",
                    maxNum: 91,
                    //excludeNums: [41, 45, 54],
                    file: ".gif",
                    placeholder: "#{alias}#",
                    alias: {
                        1: "wx",
                        2: "pz",
                        3: "se",
                        4: "fd",
                        5: "dy",
                        6: "ll",
                        7: "hx",
                        8: "bz",
                        9: "shui",
                        10: "dk",
                        11: "gg",
                        12: "fn",
                        13: "tp",
                        14: "cy",
                        15: "jy",
                        16: "ng",
                        17: "kuk",
                        18: "lengh",
                        19: "zk",
                        20: "tuu",
                        21: "tx",
                        22: "ka",
                        23: "baiy",
                        24: "am",
                        25: "jie",
                        26: "kun",
                        27: "jk",
                        28: "lh",
                        29: "hanx",
                        30: "db",
                        31: "fendou",
                        32: "zhm",
                        33: "yiw",
                        34: "xu",
                        35: "yun",
                        36: "zhem",
                        37: "shuai",
                        38: "kl",
                        39: "qiao",
                        40: "zj",
                        41: "ch",
                        42: "kb",
                        43: "gz",
                        44: "qd",
                        45: "hx",
                        46: "zhh",
                        47: "yhh",
                        48: "hq",
                        49: "bs",
                        50: "wq",
                        51: "kk",
                        52: "yx",
                        53: "qq",
                        54: "xia",
                        55: "kel",
                        56: "cd",
                        57: "pj",
                        58: "cha",
                        59: "fan",
                        60: "zt",
                        61: "mg",
                        62: "dx",
                        63: "sa",
                        64: "xin",
                        65: "sa",
                        66: "dg",
                        67: "shd",
                        68: "zhd",
                        69: "dao",
                        70: "zq",
                        71: "pch",
                        72: "bb",
                        73: "yb",
                        74: "qiang",
                        75: "ruo",
                        76: "ws",
                        77: "shl",
                        78: "bq",
                        79: "gy",
                        80: "qt",
                        81: "cj",
                        82: "aini",
                        83: "bu",
                        84: "hd",
                        85: "tiao",
                        86: "fad",
                        87: "oh",
                        88: "kt",
                        89: "ht",
                        90: "jd",
                        91: "jw"
                    },
                    title: {
                        1: "微笑",
                        2: "撇嘴",
                        3: "色",
                        4: "发呆",
                        5: "得意",
                        6: "流泪",
                        7: "害羞",
                        8: "闭嘴",
                        9: "睡觉",
                        10: "大哭",
                        11: "尴尬",
                        12: "发怒",
                        13: "调皮",
                        14: "呲牙",
                        15: "惊讶",
                        16: "难过",
                        17: "酷",
                        18: "冷汗",
                        19: "抓狂",
                        20: "吐",
                        21: "偷笑",
                        22: "可爱",
                        23: "白眼",
                        24: "傲慢",
                        25: "饥饿",
                        26: "困",
                        27: "惊恐",
                        28: "流汗",
                        29: "憨笑",
                        30: "大兵",
                        31: "奋斗",
                        32: "咒骂",
                        33: "疑问",
                        34: "嘘",
                        35: "晕",
                        36: "折磨",
                        37: "衰",
                        38: "骷髅",
                        39: "敲打",
                        40: "再见",
                        41: "擦汗",
                        42: "扣鼻",
                        43: "鼓掌",
                        44: "糗大了",
                        45: "坏笑",
                        46: "左哼哼",
                        47: "右哼哼",
                        48: "哈欠",
                        49: "鄙视",
                        50: "委屈",
                        51: "快哭了",
                        52: "阴险",
                        53: "亲亲",
                        54: "吓",
                        55: "可怜",
                        56: "菜刀",
                        57: "啤酒",
                        58: "茶",
                        59: "饭",
                        60: "猪头",
                        61: "玫瑰",
                        62: "凋谢",
                        63: "示爱",
                        64: "心",
                        65: "心碎",
                        66: "蛋糕",
                        67: "闪电",
                        68: "炸弹",
                        69: "刀",
                        70: "足球",
                        71: "瓢虫",
                        72: "便便",
                        73: "拥抱",
                        74: "强",
                        75: "弱",
                        76: "握手",
                        77: "胜利",
                        78: "抱拳",
                        79: "勾引",
                        80: "拳头",
                        81: "差劲",
                        82: "爱你",
                        83: "NO",
                        84: "OK",
                        85: "跳跳",
                        86: "发抖",
                        87: "怄火",
                        88: "磕头",
                        89: "回头",
                        90: "激动",
                        91: "街舞"
                    }
                }, {
                    name: "贴吧表情",
                    path: "/images/emoji/tieba/",
                    maxNum: 50,
                    file: ".jpg",
                    placeholder: ":{alias}:",
                    alias: {
                        1: "hehe",
                        2: "haha",
                        3: "tushe",
                        4: "a",
                        5: "ku",
                        6: "lu",
                        7: "kaixin",
                        8: "han",
                        9: "lei",
                        10: "heixian",
                        11: "bishi",
                        12: "bugaoxing",
                        13: "zhenbang",
                        14: "qian",
                        15: "yiwen",
                        16: "yinxian",
                        17: "tu",
                        18: "yi",
                        19: "weiqu",
                        20: "huaxin",
                        21: "hu",
                        22: "xiaonian",
                        23: "neng",
                        24: "taikaixin",
                        25: "huaji",
                        26: "mianqiang",
                        27: "kuanghan",
                        28: "guai",
                        29: "shuijiao",
                        30: "jinku",
                        31: "shengqi",
                        32: "jinya",
                        33: "pen",
                        34: "aixin",
                        35: "xinsui",
                        36: "meigui",
                        37: "liwu",
                        38: "caihong",
                        39: "xxyl",
                        40: "taiyang",
                        41: "qianbi",
                        42: "dnegpao",
                        43: "chabei",
                        44: "dangao",
                        45: "yinyue",
                        46: "haha2",
                        47: "shenli",
                        48: "damuzhi",
                        49: "ruo",
                        50: "OK"
                    },
                    title: {
                        1: "呵呵",
                        2: "哈哈",
                        3: "吐舌",
                        4: "啊",
                        5: "酷",
                        6: "怒",
                        7: "开心",
                        8: "汗",
                        9: "泪",
                        10: "黑线",
                        11: "鄙视",
                        12: "不高兴",
                        13: "真棒",
                        14: "钱",
                        15: "疑问",
                        16: "阴脸",
                        17: "吐",
                        18: "咦",
                        19: "委屈",
                        20: "花心",
                        21: "呼~",
                        22: "笑脸",
                        23: "冷",
                        24: "太开心",
                        25: "滑稽",
                        26: "勉强",
                        27: "狂汗",
                        28: "乖",
                        29: "睡觉",
                        30: "惊哭",
                        31: "生气",
                        32: "惊讶",
                        33: "喷",
                        34: "爱心",
                        35: "心碎",
                        36: "玫瑰",
                        37: "礼物",
                        38: "彩虹",
                        39: "星星月亮",
                        40: "太阳",
                        41: "钱币",
                        42: "灯泡",
                        43: "茶杯",
                        44: "蛋糕",
                        45: "音乐",
                        46: "haha",
                        47: "胜利",
                        48: "大拇指",
                        49: "弱",
                        50: "OK"
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
                path: "/images/emoji/qq/",
                file: ".gif",
                placeholder: "#{alias}#",
                alias: {
                    1: "wx",
                    2: "pz",
                    3: "se",
                    4: "fd",
                    5: "dy",
                    6: "ll",
                    7: "hx",
                    8: "bz",
                    9: "shui",
                    10: "dk",
                    11: "gg",
                    12: "fn",
                    13: "tp",
                    14: "cy",
                    15: "jy",
                    16: "ng",
                    17: "kuk",
                    18: "lengh",
                    19: "zk",
                    20: "tuu",
                    21: "tx",
                    22: "ka",
                    23: "baiy",
                    24: "am",
                    25: "jie",
                    26: "kun",
                    27: "jk",
                    28: "lh",
                    29: "hanx",
                    30: "db",
                    31: "fendou",
                    32: "zhm",
                    33: "yiw",
                    34: "xu",
                    35: "yun",
                    36: "zhem",
                    37: "shuai",
                    38: "kl",
                    39: "qiao",
                    40: "zj",
                    41: "ch",
                    42: "kb",
                    43: "gz",
                    44: "qd",
                    45: "hx",
                    46: "zhh",
                    47: "yhh",
                    48: "hq",
                    49: "bs",
                    50: "wq",
                    51: "kk",
                    52: "yx",
                    53: "qq",
                    54: "xia",
                    55: "kel",
                    56: "cd",
                    57: "pj",
                    58: "cha",
                    59: "fan",
                    60: "zt",
                    61: "mg",
                    62: "dx",
                    63: "sa",
                    64: "xin",
                    65: "sa",
                    66: "dg",
                    67: "shd",
                    68: "zhd",
                    69: "dao",
                    70: "zq",
                    71: "pch",
                    72: "bb",
                    73: "yb",
                    74: "qiang",
                    75: "ruo",
                    76: "ws",
                    77: "shl",
                    78: "bq",
                    79: "gy",
                    80: "qt",
                    81: "cj",
                    82: "aini",
                    83: "bu",
                    84: "hd",
                    85: "tiao",
                    86: "fad",
                    87: "oh",
                    88: "kt",
                    89: "ht",
                    90: "jd",
                    91: "jw"
                }
            }, {
                path: "/images/emoji/tieba/",
                file: ".jpg",
                placeholder: ":{alias}:",
                alias: {
                    1: "hehe",
                    2: "haha",
                    3: "tushe",
                    4: "a",
                    5: "ku",
                    6: "lu",
                    7: "kaixin",
                    8: "han",
                    9: "lei",
                    10: "heixian",
                    11: "bishi",
                    12: "bugaoxing",
                    13: "zhenbang",
                    14: "qian",
                    15: "yiwen",
                    16: "yinxian",
                    17: "tu",
                    18: "yi",
                    19: "weiqu",
                    20: "huaxin",
                    21: "hu",
                    22: "xiaonian",
                    23: "neng",
                    24: "taikaixin",
                    25: "huaji",
                    26: "mianqiang",
                    27: "kuanghan",
                    28: "guai",
                    29: "shuijiao",
                    30: "jinku",
                    31: "shengqi",
                    32: "jinya",
                    33: "pen",
                    34: "aixin",
                    35: "xinsui",
                    36: "meigui",
                    37: "liwu",
                    38: "caihong",
                    39: "xxyl",
                    40: "taiyang",
                    41: "qianbi",
                    42: "dnegpao",
                    43: "chabei",
                    44: "dangao",
                    45: "yinyue",
                    46: "haha2",
                    47: "shenli",
                    48: "damuzhi",
                    49: "ruo",
                    50: "OK"
                }
            }]
        });
    }
});
/**评论End by LHS */