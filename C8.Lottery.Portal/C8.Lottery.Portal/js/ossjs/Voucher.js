﻿$(function () {
    getlist(0);
    $(".My_KJHeader li").click(function () {
        var type = $(this).attr("data-type")
        getlist(type)
    });
});

function getlist(type) {
    $.get("/Personal/VoucherList", {
        type: type
    }, function (data) {
        if (data.Success == true) {
            console.log(data.data);
            if (data.data.length == 0) {
                $(".My_KaJuanK").html('<div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无卡券~</p></div>');
            } else {
                buildHtml(data.data, type);
            }
        } else {
            alertmsg(data.Msg);
        }
    });
}
/* 获取日期时间格式*/

function ChangeDateFormat(dateStr, type) {
    var date = eval('new ' + dateStr.substr(1, dateStr.length - 2));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    if (type == 1) {
        return conver(year) + "-" + conver(month) + "-" + conver(day);
    } else {
        return conver(year) + "-" + conver(month) + "-" + conver(day) + " " + conver(hh) + ":" + conver(mm) + ":" + conver(ss);
    }
}

function DateDiff(sDate1, sDate2) { //sDate1和sDate2是yyyy-MM-dd格式
    var aDate, oDate1, oDate2, iDays;
    aDate = sDate1.split("-");
    oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0]); //转换为yyyy-MM-dd格式
    aDate = sDate2.split("-");
    oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0]);
    iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24); //把相差的毫秒数转换为天数
    return iDays; //返回相差天数
}
//日期时间处理

function conver(s) {
    return s < 10 ? '0' + s : s;
}

function getDate() {
    var myDate = new Date();
    //获取当前年
    var year = myDate.getFullYear();
    //获取当前月
    var month = myDate.getMonth() + 1;
    //获取当前日
    var date = myDate.getDate();
    var h = myDate.getHours(); //获取当前小时数(0-23)
    var m = myDate.getMinutes(); //获取当前分钟数(0-59)
    var s = myDate.getSeconds();
    //获取当前时间
    var now = year + '-' + conver(month) + "-" + conver(date);
    return now;
}

function buildHtml(data, type) {
    var html = [];
    var img1 = '';
    var img2 = '';
    var img3 = '';
    var dhtml = '';
    var t = getDate();
    var text = '';
    $.each(data, function (index, value) {
        var day = DateDiff(ChangeDateFormat(value.EndTime, 1), t);
        if (type == 0) {
            img1 = '<img src="/images/w3_1.png">';
            if (day == 1) {
                img2 = '<img src="/images/w4_1.png">';
            }
        } else if (type == 1) {
            img1 = '<img src="/images/w3_2.png">';
            img2 = '<img src="/images/w4_3.png">';
        } else if (type == 2) {
            img1 = '<img src="/images/w3_2.png">'
            img2 = '<img src="/images/w4_2.png">';
        }
        if (value.FromType == 1) {
            text = "新用户注册"
        } else {
            text = "邀请用户注册"
        }
        dhtml = '<div class="KaJuanInfo"><div class="KJI_Top">' + '<div class="KJI_Ttu">' + img2 + '</div><dl class="KJI_Tdl">' + '<dt>' + img1 + '</dt>' + '<dd>' + '<h3>查看券</h3>' + '<p>有效期至：' + ChangeDateFormat(value.BeginTime, 2) + ' - ' + ChangeDateFormat(value.EndTime, 2) + '</p>' + '</dd></dl></div>' + '<div class="KJI_Bt">' + '<div class="KJI_Btu KJI_BtuL"><img src="/images/w2_1.png"></div>' + '<div class="KJI_Btu KJI_BtuR"><img src="/images/w2_2.png"></div>' + '<p class="KJI_BtFont">查看券来源于' + text + '成功可获取1张查看券，查看券当天有效，过期无效</p></div></div>'
        html.push(dhtml);
    });
    $(".My_KaJuanK").html(html.join(''));
}