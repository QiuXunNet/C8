// JavaScript Document

$(function(){
	

	/*切换 首页 双面 长龙 走势 技巧*/
/*$(".CZ_hdNL li").click(function(){
	$(this).addClass("current").siblings().removeClass("current");
	var zhi = $(this).index();
	$(".CZ_main .CZ_Minfo").eq(zhi).show().siblings().hide();
});*/


	/*开奖切换 号码 大小 单双*/ 
$(".zsB_cBTnav .cBTnav_a").click(function(){
	$(this).addClass("current").siblings().removeClass("current");
	var zhi = $(this).index();
	$(".zsB_cBTmain").each(function(){
		$(this).find('.cBTm_info').eq(zhi).show().siblings().hide();
	});
});
 


	/*个人中心-好友列表*/
	$(".buddy_info1").click(function(){
		$(this).siblings().toggle();
	});

	/*个人中心-金币充值*/
	$(".rech_conDl").click(function(){
		$(this).addClass("conDlcurrent").siblings().removeClass("conDlcurrent");
	});
	$(".recharge_list li").click(function(){
		$(".recharge_popup").show();
	});
	$(".recharge_yy").click(function(){
		$(".recharge_popup").hide();
	});

	/*个人中心-个人主页*/
	$(".fans_nav li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var zhi = $(this).index();
		$(".fans_box .fans_Bkuai").eq(zhi).show().siblings().hide();
	});


	/*index-切换彩种*/
	$(".hdNav_cai li").click(function(){
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
    $(".hdNav_cai_expand .hdNav_cai li").click(function() {
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
	$(".C8_cyjc li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var cyjc = $(this).index();
		$(".guessing_content .gues_info").eq(cyjc).show().siblings().hide();
	});
	
	/*个人中心-设置-设置性别*/
	$(".Set_Bxbul li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
	});
	
	/*打赏榜*/
	$(".C8_bang li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var bang = $(this).index();
		$(".Ranking_content .Ranking_Cinfo").eq(bang).show().siblings().hide();
	});
	
	/*个人中心-交易记录*/
	$(".C8_JIlU li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var bang = $(this).index();
		$(".details_box .details_info1").eq(bang).show().siblings().hide();
	});
	
	/*我的计划*/
	$(".SX_pop").click(function(){
		$(".ShaiXuan").toggle();
	});
	$(".shaiXuan_yy").click(function(){
		$(".ShaiXuan").hide();
	});
	
	$(".SX_Ul li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
	});

	/*我的计划*/
	$(".C8_JIlU li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var bang = $(this).index();
		$(".details_box .details_info1").eq(bang).show().siblings().hide();
	});
	
	/*高手榜*/
	$(".GS_btn").click(function(){
		$(".GS_box").toggleClass("GS_boxon");
	});
	$(".C8_GPQG li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var GPQG = $(this).index();
		$(".GS_box .GS_nav").eq(GPQG).show().siblings().hide();
	});
	$(".GS_nav li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
	});
	
	/*规则*/
	$(".Lnav_Ul li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var GPQG = $(this).index();
		$(".GZ_text .GZ_tInfo").eq(GPQG).show().siblings().hide();
	});
	
	/*发表评论*/
	$(".Com_File").change(function(){
		if($(this).val()){
			var objUrl = getObjectURL(this.files[0]) ;
			console.log("objUrl = "+objUrl) ;
			var objUrl = getObjectURL(this.files[0]) ;
			$(this).parent().before('<li><img src='+objUrl+'><i class="Com_icon"></i></li>');
		}
	})
	function getObjectURL(file) {
		var url = null ; 
		if (window.createObjectURL!=undefined) { // basic
			url = window.createObjectURL(file) ;
		} else if (window.URL!=undefined) { // mozilla(firefox)
			url = window.URL.createObjectURL(file) ;
		} else if (window.webkitURL!=undefined) { // webkit or chrome
			url = window.webkitURL.createObjectURL(file) ;
		}
		return url ;
	}
	
	/*我的佣金*/
	$(".C8_Myyj li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var Myyj = $(this).index();
		$(".sion_content .sion_conInfo").eq(Myyj).show().siblings().hide();
	});
	
	/*我的积分*/
	$(".C8_Myjf li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var Myjf = $(this).index();
		$(".points_text .points_TInfo").eq(Myjf).show().siblings().hide();
	});
	
	/*个人中心-他人主页*/
	$(".C8_Trzy li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		var Trzy = $(this).index();
		$(".Other_text .Other_TInfo").eq(Trzy).show().siblings().hide();
	});
	
	
	
	/*六合彩近期竞猜*/
	$(".ShareBtn").click(function(){
		$(".Gues_share").show();
	});
	$(".Del_Share").click(function(){
		$(".Gues_share").hide();
	});
	
	/*玩法*/
	$(".WanFa_hdInfo2").click(function(){
		$(this).children(".WF_hdIF2Icpon").toggleClass("WF_hdIF2IcponON");
		$(".WanFa_History").toggle();
	});
	
	
	/*计划*/
	$(".QiHao_tit").bind("click",function(){
		$(this).siblings(".QiHao_text").toggle();
	});
	
	$(".C8_nav5 li").click(function(){
		var Trzy = $(this).index();
		if(Trzy==0){
			$(".GS_box").hide();
			$(".GS_btn").hide();
		}else{
			$(".GS_box").show();
			$(".GS_btn").show();
		}
		$(this).addClass("current").siblings().removeClass("current");
		$(".Plan_content .Plan_CInfo").eq(Trzy).show().siblings().hide();
	});
	
	/*C8 弹窗 js*/
	$(".C8_TcDl dd .C8_Qiu").click(function(){
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