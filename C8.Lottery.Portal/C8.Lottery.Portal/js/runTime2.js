
var lType = 1;


$(function () {

    lType = $('#lType').val();

    setInterval("handtimeRunForOpen()", 1000);

});




//处理时间倒计时
function handtimeRunForOpen() {

    var d = new Date();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var sec = d.getSeconds();



    //判断是否已经停止投注
    //var nextIssue = $("#nextIssue").html();
    //if (nextIssue == null) {
    //    startAgain();
    //    return;
    //}

    if (lType == 3 || lType == 11) {      //带小时的
        timeRunForOpen2();
    }
    else {
        timeRunForOpen();
    }

}

//不带小时
function timeRunForOpen() {
    var second = $("#second2").html();

    if (second == undefined) {
        $.post('/Plan/GetOpenRemainingTime', { lType: lType }, function (data) {

            if (data != '正在开奖') {
                var arr3 = data.split('&');
                $('#openTime').html('<t id="minute2">' + arr3[1] + '</t>:<t id="second2">' + arr3[2] + '</t>');
            }
        });

        return;
    }

    //临时变量
    var sec = -1;
    var min = -1;
    //将显示秒数转换为数字
    if (second.substr(0, 1) == "0") {
        sec = parseInt(second.substr(1, 1));
    } else {
        sec = parseInt(second);
    }
    //秒数减一
    sec = sec - 1;

    //更新时间
    if (sec == 5 || sec == 35) {
        updateTime(lType);
    }

    if (sec < 10) {
        if (sec == -1) {
            //秒数用完了 取分钟数
            var minute = $("#minute2").html();
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
                    $("#minute2").html("0" + (min - 1));
                }
                else {
                    $("#minute2").html(min - 1);
                }
                $("#second2").html(59);
            }
            else {
                //分钟数用完 重置时间  ---------------延迟30秒

                var yc = 30000;
                if (lType % 2 == 0) {
                    yc = 5000;
                }

                setTimeout('resetTimeForOpen(' + lType + ')', yc);

                //时间用完后的处理
                timeOverForOpen();
            }
        }
        else {
            $("#second2").html("0" + (second - 1));
        }
    }
    else {
        $("#second2").html(second - 1);
    }
}


function timeOverForOpen() {
    $('#openTime').html('正在开奖');
}

//带小时的处理
function timeRunForOpen2() {
    var second = $("#second2").html();

    if (second == undefined) return;

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

    //更新时间
    if (sec == 5 || sec == 35) {
        updateTime(lType);
    }

    if (sec < 10) {
        if (sec == -1) {


            //秒数用完了 取分钟数
            var minute = $("#minute2").html();
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
                    $("#minute2").html("0" + (min - 1));
                } else {
                    $("#minute2").html(min - 1);
                }
                $("#second2").html(59);
            } else {


                //分钟数用完了 取小时数
                var hours = $("#hour2").html();

                if (hours != undefined) {
                    //将显示小时数转化为数字
                    if (hours.substr(0, 1) == "0") {
                        hour = parseInt(hours.substr(1, 1));
                    } else {
                        hour = parseInt(hours);
                    }
                }

                //判断小时数是否为0
                if (hours != undefined && hour - 1 > -1) {
                    //还有剩余小时
                    if (hour - 1 < 10) {
                        $("#hour2").html("0" + (hour - 1));
                    } else {
                        $("#hour2").html(hour - 1);
                    }
                    $("#minute2").html(59);
                    $("#second2").html(59);
                }
                else {
                    //小时数用完
                    //分钟数用完 重置时间
                    var yanchi = 60000;
                    if (lType == 11) {
                        yanchi = 60000;
                    }
                    setTimeout('resetTimeForOpen2();', yanchi);
                    //时间用完后的处理
                    timeOverForOpen();
                }
            }
        } else {
            $("#second2").html("0" + (second - 1));
        }
    } else {
        $("#second2").html(second - 1);
    }
}


//重置时间
function resetTimeForOpen(lType) {

    var d = new Date();
    var hour = d.getHours();

    //分钟数
    if (lType == 1) {
        if (hour >= 9 && hour < 22) {
            $('#openTime').html('<t id="minute2">09</t>:<t id="seconds">29</t>');
        }
        else {
            $('#openTime').html('<t id="minute2">04</t>:<t id="second2">29</t>');
        }
    }
    else if (lType == 7 || lType == 9) {
        $('#openTime').html('<t id="minute2">04</t>:<t id="second2">29</t>');
    }
    else if (lType == 17 || lType == 15 || lType == 13) {
        $('#openTime').html('<t id="minute2">09</t>:<t id="second2">59</t>');
    }
    else if (lType % 2 == 0) {
        $('#openTime').html('<t id="minute2">01</t>:<t id="second2">24</t>');
    }



    //秒数



}

function resetTimeForOpen2() {
    //$("#number").next().html('暂停下注');

    if (lType == 11) {
        $("#hour2").html('23');
        $("#minute2").html('58');
        $("#second2").html('59');
    }
    
}


//5s和35s的时候更新时间
function updateTimeForOpen(lType) {
    $.post("/Lottery/UpdateTime", { lType: lType }, function (data) {

        alert(159);

        if (data.length > 100) {
            location.reload();
        } else {
            var arr = data.split('&');
            if (arr[0] != "00") {
                $("#hour").html(arr[0]);
            }

            if ($("#nextIssue").html() != arr[3]) {
                $("#nextIssue").html(arr[3]);
            }
            $("#minute2").html(arr[1]);
            $("#second2").html(arr[2]);
        }
    });
}


function updateTime() {
    InitIssueAndTime();
}

function InitIssueAndTime() {

    $.post('/Plan/GetCurrentIssueAndTime', { lType: lType }, function (data) {

        HandIssueAndTime(data);
    });
}

function HandIssueAndTime(data) {

    var arr = data.split('|');

    $('#nextIssue').html(arr[0]);       //期号

    if (arr[1] == '已封盘') {
        $('#fenpanTime').html('已封盘');
        $('#fengpan').show();               //底部的封盘遮罩
    }
    else {

        $('#fengpan').hide();               //底部的封盘遮罩

        var arr2 = arr[1].split('&');

        if (lType == 3 || lType == 5 || lType == 11 || lType == 19) {
            $('#fenpanTime').html('<t id="hour">' + arr2[0] + '</t>:<t id="minute">' + arr2[1] + '</t>:<t id="second">' + arr2[2] + '</t>');
        }
        else {
            $('#fenpanTime').html('<t id="minute">' + arr2[1] + '</t>:<t id="second">' + arr2[2] + '</t>');
        }

    }

    if (arr[2] == '正在开奖') {
        $('#openTime').html('正在开奖');
    }
    else {
        var arr3 = arr[2].split('&');
        if (lType == 3 || lType == 5 || lType == 11 || lType == 19) {
            $('#openTime').html('<t id="hour2">' + arr3[0] + '</t>:<t id="minute2">' + arr3[1] + '</t>:<t id="second2">' + arr3[2] + '</t>');
        } else {
            $('#openTime').html('<t id="minute2">' + arr3[1] + '</t>:<t id="second2">' + arr3[2] + '</t>');
        }

    }
}