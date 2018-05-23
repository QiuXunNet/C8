var lType = 1;
var time1;
var time2;
var time3;
var dateTime = "";

$(function () {
    lType = $('#lType').val();
    handtimeRunForOpen();
    if (time1 == null || time1 == undefined) {
        time1 = setInterval("handtimeRunForOpen()", 10000);
    }
    if (time3 == null || time3 == undefined) {
        time3 = setInterval("runTime()", 1000);
    }
});

function handtimeRunForOpen() {
    $.post('/Plan/GetCurrentIssueAndTime', { lType: lType }, function (data) {
        var arr = data.split('|');
        dateTime = arr[2];

        if (dateTime != "正在开奖") {
            clearInterval(time1);
            time1 = null;
            clearInterval(time2);
            time2 = null;
            if (time1 == null) {
                //time1 = setInterval("handtimeRunForOpen()", 10000);
                //time3 = setInterval("runTime()", 1000);
            }
        }
        else {
            clearInterval(time1);
            time1 = null;
            clearInterval(time2);
            time2 = null;
            clearInterval(time3);
            time3 = null;
            if (time2 == null) {
                //time2 = setInterval("handtimeRunForOpen()", 1000);
            }
            $('#openTime').html("正在开奖");
        }

        var curIssue = arr[0];
        $('#currentIssue').html(arr[0]);       //期号

        if (curIssue == '已封盘' || arr[1] == '已封盘' || dateTime == "正在开奖") {
            $('#fenpanTime').html('已封盘');
            $('#fengpan').show();               //底部的封盘遮罩
        }
        else {
            $('#fengpan').hide();               //底部的封盘遮罩
        }
    });
}

function runTime() {
    if (dateTime.length > 0) {
        var str = dateTime.replace(/&/g, ":");
        var arr = str.split(':');

        var date = new Date((new Date()).toLocaleDateString() + " " + (arr[0] % 24) + ":" + arr[1] + ":" + arr[2]);

        var t = date.getTime();
        t = t - 1000;
        date = new Date(t);

        var h = parseInt(arr[0] / 24) * 24 + date.getHours();
        h = h < 10 ? "0" + h : h;
        var m = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var s = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        //特殊处理  除8大彩外,高频彩倒计时不可能跨天
        if (lType >= 9 && h == 23) {
            $('#openTime').html("正在开奖");
        } else {
            if (Number(h) > 0) {
                $('#openTime').html('<t id="hour2">' + h + '</t>:<t id="minute2">' + m + '</t>:<t id="second2">' + s + '</t>');
            }
            else {
                $('#openTime').html('<t id="minute2">' + m + '</t>:<t id="second2">' + s + '</t>');
            }

            if (h == 0 && m == 0 && s == 30) {
                handtimeRunForOpen();
            }
        }

        dateTime = h + "&" + m + "&" + s;
    }
}