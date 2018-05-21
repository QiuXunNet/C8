$(function () {
    $.post("/Home/GetFatherLotteryType", {}, function (data) {
        if (data != null && data.length != 0) {
            var lis = "";
            var uls = "";
            $(data).each(function (i) {
                if (i == 0) {
                    lis += "<li data-Id='" + this.Id + "' class = 'current'><a href='javascript:;'>" + this.Name + "</a></li>";
                    uls += "<ul class='ind_mainUl2' id='mainUl2" + this.Id + "'></ul>";
                } else {
                    lis += "<li data-Id='" + this.Id + "'><a href='javascript:;'>" + this.Name + "</a></li>";
                    uls += "<ul class='ind_mainUl2' id='mainUl2" + this.Id + "' style='display: none;'></ul>";
                }
            });
            $(".hdNav_cai_lhs,.hdNav_cai").prepend(lis);
            $(".hjc_indexmain").append(uls);
            getChildLotteryType($(".hdNav_cai_lhs").find(".current").attr("data-Id"));
        }
    });
});

function getChildLotteryType(pId) {
    if ($.trim($("#mainUl2" + pId).html()).length != 0) return;
    $("#kongbaiDiv").show();
    $.post("/Home/GetChildLotteryType", {
        pId: pId
    }, function (data) {
        var lis = "";
        $(data).each(function () {
            lis += "<li>";
            lis += "     <div class='UL2_hd'>";
            lis += "         <a href='javascript:;'>";
            lis += "             <div class='UL2_hdtu f-l'><img src='/images/" + this.ShowIconName + ".png'></div>";
            lis += "             <div class='UL2_hdtit'><h3>" + this.ShowTypeName + "</h3></div>";
            lis += "             <span class='UL2_hdicon'></span>";
            lis += "         </a>";
            lis += "     </div>";
            lis += "     <div class='UL2_bd'>";
            lis += "         <div class='UL3_bdinfo1'>";
            lis += "           <p lType='" + this.lType + "'>";
            lis += "                 <font>第<span>" + this.Issue + "</span>期</font>";
            lis += "                 <font>下期开奖：";
            if (this.ShowOpenTime == "正在开奖" || this.ShowOpenTime == "休奖时间") {
                lis += "                 <span>" + this.ShowOpenTime + "</span>";
            } else {
                var arr = this.ShowOpenTime.split("&");
                lis += "                 <span><t class='hour'>" + arr[0] + "</t>:<t class='minute'>" + arr[1] + "</t>:<t class='second'>" + arr[2] + "</t></span>";
            }
            lis += "                   </font>";
            lis += "             </p>";
            lis += "         </div>";
            lis += "         <div class='UL3_bdinfo2'>";
            lis += "             <div class='ball_wrap'>";
            lis += getHtml(this);
            lis += "              </div>";
            //lis+="             <div class='UL3_ZZKJ'>";
            //lis+="                 <p>正在开奖中...</p>";
            //lis+="             </div>";
            lis += "         </div>";
            lis += "         <div class='UL3_bdinfo3'>";
            lis += "          <a href='/Plan/Index/" + this.lType + "' class='UL2_baif3a'>计划</a>";
            lis += "          <a href='/Record/OpenRecord/?lType=" + this.lType + "' class='UL2_baif3a'>历史</a>";
            lis += "          <a href='/Plan/Post/" + this.lType + "' class='UL2_baif3a'>发帖</a>";
            lis += "          <a href='/Plan/Index/" + this.lType + "?cur=2' class='UL2_baif3a'>排行</a>";
            lis += "          <a href='/News/NewIndex/" + this.lType + "' class='UL2_baif3a'>资讯</a>";
            lis += "             <div style='clear: both;'></div>";
            lis += "         </div>";
            lis += "     </div>";
            lis += " </li>";
        });
        $("#mainUl2" + pId).empty().append(lis).show();
        $("#kongbaiDiv").hide();
    });
}

function getHtml(data) {
    var html = "";
    var lType = data.lType;
    var numArr = data.Num.split(",");
    if (lType <= 62 && lType != 5 && lType != 2 && lType != 8 && lType != 4) {
        $(numArr).each(function () {
            var num = $.trim(this);
            if (num.length > 0) {
                html += "<span class='ball_spB ball_red HYJP_Red'>" + num + "</span>";
            }
        });
    }
    if (lType == 2 || lType == 8) {
        $(numArr).each(function (i) {
            var num = $.trim(this);
            if (num.length > 0) {
                if (i == numArr.length - 1) {
                    html += "<span class='ball_spB ball_red HYJP_Blue'>" + num + "</span>";
                } else {
                    html += "<span class='ball_spB ball_red HYJP_Red'>" + num + "</span>";
                }
            }
        });
    }
    if (lType == 4) {
        $(numArr).each(function (i) {
            var num = $.trim(this);
            if (num.length > 0) {
                if (i >= numArr.length - 2) {
                    html += "<span class='ball_spB ball_red HYJP_Blue'>" + num + "</span>";
                } else {
                    html += "<span class='ball_spB ball_red HYJP_Red'>" + num + "</span>";
                }
            }
        });
    }
    if (lType == 5) {
        $(numArr).each(function (i) {
            var num = $.trim(this);
            if (num.length > 0) {
                var color = getColor(num);
                if (i == numArr.length - 1) {
                    html += "<span class='ball_spB ball_blue HYJP_PcHao'>+</span>";
                }
                html += "<span class='ball_spB ball_blue " + color + "'>" + num + "</span>";
            }
        });
    }
    if (lType == 65) {
        html += "<span class='ball_spB ball_blue HYJP_Red'>" + numArr[0] + "</span>";
        html += "<span class='ball_spB ball_blue HYJP_PcHao'>+</span>";
        html += "<span class='ball_spB ball_blue HYJP_Red'>" + numArr[1] + "</span>";
        html += "<span class='ball_spB ball_blue HYJP_PcHao'>+</span>";
        html += "<span class='ball_spB ball_blue HYJP_Red'>" + numArr[2] + "</span>";
        html += "<span class='ball_spB ball_blue HYJP_PcHao'>=</span>";
        html += "<span class='ball_spB ball_blue HYJP_Red'>" + (Number(numArr[0]) + Number(numArr[1]) + Number(numArr[2])) + "</span>";
    }
    if (lType == 63 || lType == 64) {
        $(numArr).each(function () {
            var num = $.trim(this);
            if (num.length > 0) {
                var clazz = "ball_spA No" + num;
                html += "<span class='" + clazz + "'>" + num + "</span>";
            }
        });
    }
    return html;
}

/*** Added By LHS 2018-3-1  */
$(".hdNav_cai_collect").on("click", "li", function () {
    var pId = $(this).attr("data-Id");
    getChildLotteryType(pId);
    $(this).addClass("current").siblings().removeClass("current");
    var zhi = $(this).index();
    var lType = $(this).attr("data-Id");
    $("#mainUl2" + lType).closest("div").find("ul").hide();
    $("#mainUl2" + lType).show();
    // $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();
    //展开项添加选中
    $(".hdNav_cai_expand .hdNav_cai li").eq(zhi).addClass("current").siblings().removeClass("current");
    $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();
    $(".hdNav_cai_expand").hide();
    $(".mask").hide();
});

/* 展开项点击 */
$(".hdNav_cai_expand").on("click", "li", function () {
    var pId = $(this).attr("data-Id");
    getChildLotteryType(pId);
    $(this).addClass("current").siblings().removeClass("current");
    var zhi = $(this).index();
    $(".hjc_indexmain .ind_mainUl2,.hjc_indexmain .list_mainUl2,.hjc_news_tabContent .hjc_news_lhs").eq(zhi).show().siblings().hide();
    $(".hdNav_cai_collect .hdNav_cai_lhs li").eq(zhi).addClass("current").siblings().removeClass("current");
    $(".GS_box .GS_nav").eq(zhi).show().siblings().hide();
});