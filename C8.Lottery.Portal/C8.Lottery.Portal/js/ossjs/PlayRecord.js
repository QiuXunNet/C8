$(function () {

    $(".zfC_del").click(function () {
        $(".c8_TCpopupZf").hide();
        $(".mask").hide();
    });


    var p = GetQueryString("p");

    if (p.p != null) {
        var $curDd = $('.SX_Ul').find('li[data-title="' + p.p + '"]');
        $curDd.addClass("current").siblings().removeClass("current");

        getData(p.p);

    }

    $(".Gues_ul").parseEmoji();

    if (Type == 0) {
        $(".SX_Ul li").click(function () {
            var _this = $(this),
                playName = _this.attr("data-title") || "全部";
            getData(playName);

        });
        if (p.p == null) {
            $(".SX_Ul li:eq(0)").trigger("click");
        }

        //查看预测号码
        $("#viewBetting").click(function () {

        });
    } else {
        getData("全部");
    }
});

function GetQueryString(name) {
    var url = window.location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            //就是这句的问题
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
            //之前用了unescape()
            //才会出现乱码
        }
    }
    return theRequest;
}

function getData(playName) {
    var uid = UserId;
    $.ajax({
        type: 'GET',
        data: {
            lType: LType,
            playName: playName,
            uid: uid
        },
        url: '/Plan/GetLastPlay',
        dataType: 'json',
        success: function (res) {

            if (res.IsSuccess && res.Data) {
                //回调
                buildHtml(res.Data.PageData);

                buildLastPlay(res.Data.ExtraData, playName);


            } else {
                alertmsg("服务器繁忙");
            }
        },
        error: function (res, xhr) {
            alertmsg("服务器繁忙");
        }
    });
}

function buildLastPlay(data, playName) {
    if (data != null && typeof (data) != "undefined") {
        recordId = data.Id;
        isRead = data.IsRead;
        var html = '<span>已更新</span>';
        if (typeof (playName) != 'undefined' && playName != '' && playName != '全部') {
            html += '<a id="viewBetting" href="javascript:playUtil.view();">查看预测号码</a>';
        }
        $("#visitPlanContainer").html(html);
    } else {
        $("#visitPlanContainer").html("<span>未更新</span>");
    }
}

function buildHtml(pageData) {
    var itemList = [];

    if (!pageData || pageData.length < 1) {
        itemList.push('<div class="Plan_CInfo"><div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无竞猜~</p></div></div>');
    } else {

        $.each(pageData, function (index, item) {
            console.log(item);
            var kjhtml = '';
            var kjhtml1 = '';
            var num = item.LotteryNum.Num.split(',');
            var bethtml = '';
            var cl1 = "hdM_ddcon";
            var dttime = timeStamp2String(item.LotteryNum.SubTime, "yyyy-MM-dd");

            if (item.BettingRecord.length > 0) {
                for (var j = 0; j < item.BettingRecord.length; j++) {

                    var iswin = item.BettingRecord[j].WinState == 3 ? "中" : "未中";
                    var cliswin = item.BettingRecord[j].WinState == 3 ? "Forecast_Pred" : "";
                    var issym = item.BettingRecord[j].WinState == 3 ? "+" : "";
                    var mynum = item.BettingRecord[j].BetNum;
                    if (item.BettingRecord[j].WinState == 3) {
                        var mynum = item.BettingRecord[j].BetNum;
                        var mynumsplit = item.BettingRecord[j].BetNum.split(',');

                        for (var x = 0; x < mynumsplit.length; x++) {
                            var z = getwanfa(item.BettingRecord[j].PlayName);

                            if (z == -1) {
                                var ltnum = item.LotteryNum.Num;

                                if (item.BettingRecord[j].PlayName == "平特一肖") {
                                    //ltnum = getShengxiaoByDigit(item.LotteryNum.Num);
                                    ltnum = get6caiSx(ltnum, dttime);
                                }

                                if (ltnum.indexOf(mynumsplit[x]) >= 0) {

                                    mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                }
                            } else {
                                var lonum = item.LotteryNum.Num.split(',');

                                if (item.BettingRecord[j].PlayName == "四肖中特" || item.BettingRecord[j].PlayName == "六肖中特") {
                                    //lonum[z] = getShengxiaoByDigit(lonum[z]);
                                    lonum[z] = GetName(dttime, lonum[z]);
                                }
                                if (z == 10) {
                                    var lonum1 = lonum.slice(0, 6);

                                    if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                        mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                    }

                                } else if (z == 11) {
                                    var lonum1 = lonum.slice(0, 5);
                                    if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                        mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                    }
                                } else if (z == 12) {
                                    var lonum1 = lonum.slice(5);

                                    if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                        mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                    }

                                } else {
                                    if (lonum[z] == mynumsplit[x]) {
                                        console.log("中了")

                                        mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");

                                    }
                                }
                            }

                        }
                    }


                    bethtml += '<tr><td align="center" valign="middle" width="25%"><p>' + item.BettingRecord[j].PlayName + '</p> </td>' + ' <td align="center" valign="middle">   <p style="word-break:break-all; ">' + mynum + '<span class="For_Red"></span></p> </td>' + '<td align="center" valign="middle" width="25%"> <p class="' + cliswin + '">    <font class="P_TF">' + iswin + '</font><span>' + issym + '' + item.BettingRecord[j].Score + '</span></p>  </td> </tr>'

                }
            }
            for (var i = 0; i < num.length; i++) {
                console.log(getcolor("06"));
                console.log(num[i]);
                var cl = "hdM_spssc";
                var lt = LType;
                if (lt == 5) {
                    var color = getcolor(num[i]);

                    if (color == "red") {
                        cl = "LHC_spB HYJP_LhRed";
                    } else if (color == "green") {
                        cl = "LHC_spB HYJP_LhGreen";
                    } else {
                        cl = "LHC_spB HYJP_LhBlue";
                    }

                    if (i == 5) {
                        kjhtml1 += '<span class="' + cl + '">' + num[i] + '</span>';
                        kjhtml1 += '<span class="LHC_spB HYJP_PcHao ">+</span>';
                    } else {
                        kjhtml1 += '<span class="' + cl + '">' + num[i] + '</span>';
                    }
                } else if (lt == 2) {
                    if (i == 6) {
                        kjhtml1 += '<span class="LHC_spB HYJP_Blue">' + num[i] + '</span>';
                    } else {
                        kjhtml1 += '<span class="LHC_spB">' + num[i] + '</span>';
                    }

                } else if (lt == 63 || lt == 64) {
                    cl = " SaiChe_Text SaiChe_TA" + num[i];
                    kjhtml1 += '<span class="' + cl + '">' + num[i] + '</span>';
                } else if (lt == 8) {
                    if (i == 7) {
                        kjhtml1 += '<span class="LHC_spB HYJP_Blue">' + num[i] + '</span>';
                    } else {
                        kjhtml1 += '<span class="LHC_spB">' + num[i] + '</span>';
                    }
                } else if (lt == 4) {
                    if (i == 5 || i == 6) {
                        kjhtml1 += '<span class="LHC_spB HYJP_Blue">' + num[i] + '</span>';
                    } else {
                        kjhtml1 += '<span class="LHC_spB">' + num[i] + '</span>';
                    }
                } else if (lt == 65) {
                    var a = Number(num[0]);
                    var b = Number(num[1]);
                    var c = Number(num[2]);
                    var sum = a + b + c;
                    kjhtml1 += '<span class="LHC_spB">' + num[0] + '</span>';
                    kjhtml1 += '<span class="LHC_spB HYJP_PcHao ">+</span>';
                    kjhtml1 += '<span class="LHC_spB">' + num[1] + '</span>';
                    kjhtml1 += '<span class="LHC_spB HYJP_PcHao ">+</span>';
                    kjhtml1 += '<span class="LHC_spB">' + num[2] + '</span>';
                    kjhtml1 += '<span class="LHC_spB HYJP_PcHao ">=</span>';
                    kjhtml1 += '<span class="LHC_spB">' + sum + '</span>';
                    break;
                } else {
                    kjhtml1 += '<span class="LHC_spB">' + num[i] + '</span>';
                }




            }
            var time1 = timeStamp2String(item.LotteryNum.SubTime, "MM-dd HH:mm");


            var itemHtml = '<table class="Forecast_tableA" width="100%" bordercolor="#ccc" border="1"> <tr>' + ' <td align="center" valign="middle"> <p class="Forecast_Ptit">' + item.LotteryNum.Issue + '期</p>' + '    <p class="Forecast_Ptit">' + time1 + '</p> </td>' + ' <td colspan="2" align="left" valign="middle">';
            //if (item.lType == 5)
            //{
            //    itemHtml += '<div class="hdM_ddcon">' + kjhtml + '</div>';
            //}
            itemHtml += '<div class="Forecast_LHC">' + kjhtml1 + '</div>  </td></tr>' + bethtml + '   </table>';
            itemList.push(itemHtml);
        });
    }
    var html = itemList.join('');

    $(".Gues_Ctext").empty().html(html);
}



//在Jquery里格式化Date日期时间数据
function timeStamp2String(time, format) {
    var datetime = new Date(time);
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    if (format == "yyyy-MM-dd HH:mm:ss") {
        return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
    } else if (format == "yyyy-MM-dd") {
        return year + "-" + month + "-" + date;
    } else if (format == "MM-dd HH:mm:ss") {
        return month + "-" + date + " " + hour + ":" + minute + ":" + second;
    } else if (format == "MM-dd HH:mm") {
        return month + "-" + date + " " + hour + ":" + minute;
    }


}

//生肖
function get6caiSx(num, subtime) {
    var cainum;
    for (var i = 0; i < num.length; i++) {
        cainum += GetName(subtime, num[i]) + ",";
    }
    return cainum.substring(0, cainum.length - 1);

}


//六合彩数字转生肖
function getShengxiaoByDigit(digit) {
    var result = "鸡";


    if (digit == 11 || digit == 23 || digit == 35 || digit == 47) {
        result = "鼠";
    } else if (digit == 10 || digit == 22 || digit == 34 || digit == 46) {
        result = "牛";
    } else if (digit == 9 || digit == 21 || digit == 33 || digit == 45) {
        result = "虎";
    } else if (digit == 8 || digit == 20 || digit == 32 || digit == 44) {
        result = "兔";
    } else if (digit == 7 || digit == 19 || digit == 31 || digit == 43) {
        result = "龙";
    } else if (digit == 6 || digit == 18 || digit == 30 || digit == 42) {
        result = "蛇";
    } else if (digit == 5 || digit == 17 || digit == 29 || digit == 41) {
        result = "马";
    } else if (digit == 4 || digit == 16 || digit == 28 || digit == 40) {
        result = "羊";
    } else if (digit == 3 || digit == 15 || digit == 27 || digit == 39) {
        result = "猴";
    } else if (digit == 2 || digit == 14 || digit == 26 || digit == 38) {
        result = "鸡";
    } else if (digit == 1 || digit == 13 || digit == 25 || digit == 37 || digit == 49) {
        result = "狗";
    } else if (digit == 12 || digit == 24 || digit == 36 || digit == 48) {
        result = "猪";
    }

    return result;
}
//玩法标红

function getwanfa(playName) {
    var i = -1;
    var p0 = ["第一球五码", "第一球六码", "冠军五码", "第一球", "冠军", "第一名"];
    var p1 = ["第二球五码", "第二球六码", "第二名五码", "第二球", "亚军", "第二名", "亚军五码"];
    var p2 = ["第三球五码", "第三球六码", "第三名五码", "第三球", "第三名"];
    var p3 = ["第四球五码", "第四球六码", "第四名五码", "第四球", "第四名"];
    var p4 = ["第五球五码", "第五球六码", "第五名五码", "第五球", "第五名"];
    var p5 = ["第六球五码", "第六名五码", "第六球", "第六名"];
    var p6 = ["第七球五码", "第七名五码", "第七球", "第七名", "十码中特", "二十码中特", "四肖中特", "六肖中特", "蓝5码", "蓝8码"];
    var p7 = ["第八球五码", "第八名五码", "第八球", "第八名"];
    var p8 = ["第九球五码", "第九名五码", "第九球", "第九名"];
    var p9 = ["第十球五码", "第十名五码", "第十球", "第十名"];
    var p10 = ["红2胆", "红3胆"];
    var p11 = ["前区4码"];
    var p12 = ["后区3码"];
    if ($.inArray(playName, p0) != -1) {

        i = 0;
    } else if ($.inArray(playName, p1) != -1) {
        i = 1;
    } else if ($.inArray(playName, p2) != -1) {
        i = 2;
    } else if ($.inArray(playName, p3) != -1) {
        i = 3;
    } else if ($.inArray(playName, p4) != -1) {
        i = 4;
    } else if ($.inArray(playName, p5) != -1) {
        i = 5;
    } else if ($.inArray(playName, p6) != -1) {
        i = 6;
    } else if ($.inArray(playName, p7) != -1) {
        i = 7;
    } else if ($.inArray(playName, p8) != -1) {
        i = 8;
    } else if ($.inArray(playName, p9) != -1) {
        i = 9;
    } else if ($.inArray(playName, p10) != -1) {
        i = 10
    } else if ($.inArray(playName, p11) != -1) {
        i = 11;

    } else if ($.inArray(playName, p12) != -1) {
        i = 12;
    }
    return i;
}

//获取六合彩球的颜色
function getcolor(haoma) {
    var red = "01，02，07，08，12，13，18，19，23，24，29，30，34，35，40，45，46";
    var blue = "03，04，09，10，14，15，20，25，26，31，36，37，41，42，47，48";
    var green = "05，06，11，16，17，21，22，27，28，32，33，38，39，43，44，49";
    if (red.indexOf(haoma) >= 0) {
        return "red";
    } else if (blue.indexOf(haoma) >= 0) {
        return "blue";
    } else {
        return "green";
    }

}
var playUtil = {
    goLastPlay: function () {
        var playName = $(".SX_Ul li.current").attr("data-title") || "";
        var paytype = $(".zfC_XZ li.current").attr("data-idx");
        location.href = "/Plan/LastPlay/" + LType + "?uid=" + UserId + "&playName=" + playName + "&paytype=" + paytype;
    },
    giftCoin: function () {

        var playName = $(".SX_Ul li.current").attr("data-title") || "";
        location.href = "/Plan/Tip?uid=" + UserId + "&ltype=" + LType + "&playName=" + playName;
    },
    cancel: function () {
        $(".C8_TCpopupA").hide();
        $(".mask").hide();
    },
    confirm: function () {
        var uid = UserId;
        var ltype = LType;
        var paytype = $(".zfC_XZ li.current").attr("data-idx");
        var playName = $(".SX_Ul li.current").attr("data-title");

        if (!playName || playName.length < 1) {
            alertmsg("查看失败");
            return;
        }

        $.ajax({
            type: 'POST',
            data: {
                lType: ltype,
                uid: uid,
                id: recordId,
                coin: readCoin,
                paytype: paytype
            },
            url: '/Plan/ViewPlan',
            dataType: 'json',
            success: function (res) {

                if (res.IsSuccess) {
                    playUtil.goLastPlay();
                } else {
                    if (res.Code == 401) {
                        alertmsg(res.Message);
                    } else {
                        alertmsg(res.Message);
                    }
                }
            },
            error: function (res, xhr) {
                alertmsg("服务器繁忙");
            }
        });
    },
    view: function () {
        var uid = UserId;

        if (uid == MyUid) {
            playUtil.goLastPlay();

        } else {
            if (readCoin > 0 && !isRead) {

                //$(".C8_TCpopupA").show();
                $(".c8_TCpopupZf").show();
                $(".mask").show();
            } else {
                //this.confirm();
                playUtil.goLastPlay();
            }
        }

    }
}



function unorfollow(obj, follow_uid) {
    var text = $(obj).text();
    if (text == "关注") {
        $.post('/Personal/IFollow', {
            followed_userId: follow_uid
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                $(obj).text("已关注");
            }
        });
    } else {
        $(document).dialog({
            type: 'confirm',
            closeBtnShow: true,
            style: "ios",
            content: '确定不再关注此人?',
            onClickConfirmBtn: function () {
                $.post('/Personal/UnFollow', {
                    followed_userId: follow_uid
                }, function (data) {
                    if (data.Success != true) {
                        alertmsg(data.Msg);
                    } else {
                        $(obj).text("关注");
                    }
                });
            }
        });
    }
}