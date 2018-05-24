var url = $("#socketUrl").val() + "/LhcHandler.ashx";
var ws;

$(function () {
    //ws = new WebSocket(url);
    ////链接成功回调
    //ws.onopen = function () {
    //    wsOnopen();
    //};
    ////收到消息回调
    //ws.onmessage = function (evt) {
    //    wsOnmessage(evt);
    //};
    ////链接失败回调
    //ws.onerror = function (evt) { };
    ////链接关闭回调
    //ws.onclose = function () {
    //    wsOnclose();
    //};
});

//链接成功回调
function wsOnopen() { }

//收到消息回调
function wsOnmessage(evt) {
    var msg = evt.data;
    var data = eval("(" + msg + ")");
    if (data == undefined || data == null) return;
    switch (data.MsgTypeChild) {
        case 11:
            //开始命令
            break;

        case 12:
            //结束命令
            ws.close();
            //结束后，主动断开链接
            break;

        case 21:
            //消息
            showBall(data);
            break;

        case 31:
            //心跳包
            break;
    }
}

//开始开奖
function startLottery() {
    if (ws == undefined || ws == null || ws.readyState != WebSocket.OPEN) {
        ws = new WebSocket(url);
        //链接成功回调
        ws.onopen = function () {
            wsOnopen();
        };
        //收到消息回调
        ws.onmessage = function (evt) {
            wsOnmessage(evt);
        };
        //链接失败回调
        ws.onerror = function (evt) { };
        //链接关闭回调
        ws.onclose = function () {
            wsOnclose();
        };
    }
}

//链接关闭回调
function wsOnclose() { }

//显示球
function showBall(data) {
    if (data.IsPeriod == 1) {
        // 如果是期数消息
        $("#txtIssue").text(data.Content);
        $("#ballListDiv").empty();
    } else {
        if (data.IsTM == 1) {
            //如果是特码
            $("#ballListDiv").append('<span class="Mif_jgC">+</span>&nbsp;');
        }

        var ballNo = data.BallNo; 
        var ballList = $(".hdM_spliu", "#ballListDiv");

        var num = data.Content.length < 2 ? "0" + data.Content : data.Content;

        if (ballList.eq(ballNo - 1).length == 1) {
            ballList.eq(ballNo - 1).removeClass().addClass("hdM_spliu " + getColor(num)).text(num);
        }
        else {
            $("#ballListDiv").append('<span class="hdM_spliu ' + getColor(num) + '">' + num + "</span>&nbsp;");
        }
    }
}