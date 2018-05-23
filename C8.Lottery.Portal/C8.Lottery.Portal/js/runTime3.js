var t1;
var t2;
var t3;

$(function () {
    t1 = setInterval("handtimeRun()", 1000);
    //getTime()
   // t2 = setInterval("getTime()", 2*60*1000);
});


function getTime() {
    var ltypes = "";

    $.ajax({
        url: '/Home/GetRemainOpenTime',
        type: "post",
        data: {},
        async:false,
        success: function (data) {
            var arr = eval("(" + data + ")");
            var lty
            $(arr).each(function () {
                if (this.value == "正在开奖") {
                    ltypes = this.lType + "!";
                }
                $('p[lType=' + this.lType + ']').find('span').eq(1).html(this.value);
            });
        }
    });

    for (var i in ltypes.split('!')) {
        if (i != "") {
            setTimeout(function () { getTimeBylType(i) }, 1000 * 32);
        }
    }    
}


function getTimeBylType(lType) {
    $.post('/Home/GetRemainOpenTimeByType', { lType: lType }, function (data) {
        $('p[lType=' + lType + ']').find('span').eq(1).html(data);
    });
}

//处理时间倒计时
function handtimeRun() {

    var d = new Date();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var sec = d.getSeconds();

    for (var i = 1; i < 66; i++) {
        if (i != 11 && i != 53) {
            timeRun(i);
        }
    }
}


//带小时的处理
function timeRun(cid) {

    var secondSpan = $('p[lType=' + cid + ']').find(".second");
    var minuteSpan = $('p[lType=' + cid + ']').find(".minute");
    var hourSpan = $('p[lType=' + cid + ']').find(".hour");
    var openSpan = $('p[lType=' + cid + ']').find('span').eq(1);

    var second = secondSpan.html();

    if (second == undefined)
        return;

    //临时变量
    var sec = -1;
    var min = -1;
    var hour = -1;
    //将显示秒数转换为数字
    if (second.substr(0, 1) == "0") {
        sec = parseInt(second.substr(1, 1));
    } else {
        sec = parseInt(second);
    }
    //秒数减一
    sec = sec - 1;
 
    if (sec < 10) {
        if (sec == -1) {
            //秒数用完了 取分钟数
            var minute = minuteSpan.html();   //$("#minute").html();
            //将显示分钟数转化为数字
            if (minute.substr(0, 1) == "0") {
                min = parseInt(minute.substr(1, 1));
            } else {
                min = parseInt(minute);
            }

            //判断分钟数是否为0
            if (min - 1 > -1) {
                //还有剩余分钟
                if (min - 1 < 10) {
                    minuteSpan.html("0" + (min - 1));
                }
                else {
                    minuteSpan.html(min - 1);
                }
                secondSpan.html(59);
            } else {
                //分钟数用完了 取小时数
                var hours = hourSpan.html();
                //将显示小时数转化为数字
                if (hours.substr(0, 1) == "0") {
                    hour = parseInt(hours.substr(1, 1));
                } else {
                    hour = parseInt(hours);
                }

                //判断小时数是否为0
                if (hour - 1 > -1) {
                    //还有剩余小时
                    if (hour - 1 < 10) {
                        hourSpan.html("0" + (hour - 1));
                    } else {
                        hourSpan.html(hour - 1);
                    }

                    minuteSpan.html(59);
                    secondSpan.html(59);
                }
                else {
                    openSpan.html('正在开奖');

                    setTimeout(function () { getTimeBylType(cid) }, 1000 * 32);
                }
            }
        }
        else {
            secondSpan.html("0" + (second - 1));
        }
    }
    else {
        secondSpan.html(second - 1);
    }
}