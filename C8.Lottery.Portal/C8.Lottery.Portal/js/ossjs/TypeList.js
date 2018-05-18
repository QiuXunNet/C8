$(function () {
    var location = -1;
    getadlist(location);
    //if ("ViewBag.time" == "正在开奖") {*
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
});

//广告位
function getadlist(location) {
    $.get("/News/GetAdvertisementListJson", {
        location: location,
        adtype: 3
    }, function (data) {
        if (data.Success == true) {
            inadHtml(data.data);
            console.log(data.data);
        } else {
            alert("Data Loaded: " + data.Msg);
        }
    });
}

function inadHtml(data) {
    var cityid = CityId;
    var city;
    var idx = -1;
    $.each(data, function (index, item) {
        if (item.RestrictedAreas != null && item.RestrictedAreas != "") {
            city = item.RestrictedAreas.split(",");
            idx = $.inArray(cityid.toString(), city);
        }
        if (item.RestrictedAreas == null || item.RestrictedAreas == "" || idx == -1) {
            if (!IsPC()) {
                if (item.Layer == 1) {
                    $(".Plan_content").prepend(itemhtml(item));
                } else if (item.Layer == 2) {
                    $(".Plan_content").after(itemhtml(item));
                } else if (item.Layer == 3) {
                    $(".info_LHC_nav").after(itemhtml(item));
                } else if (item.Layer == 4) {
                    $(".col4").after(itemhtml(item));
                } else if (item.Layer == 5) {
                    $(".col3").after(itemhtml(item));
                } else if (item.Layer == 6) { } else if (item.Layer == 7) { }
            }
        }
    });
}

function itemhtml(item) {
    var html = "";
    if (item.ThumbStyle == 0) {
        html = '<div class="GG_Box  GG_Box1 GG_Box2">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 1) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<div class="GG_B2Left f-l">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + "</div>" + '<div class="GG_B2Right">' + img + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 2) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<div class="GG_B4Tu">' + img + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 3) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            for (var i = 0; i < item.ThumbList.length; i++) {
                img += '<li><img src="' + item.ThumbList[i] + '"></li>';
            }
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<ul class="GG_B3Tu">' + img + '<div style="clear:both;"></div>' + "</ul>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 4) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '"  class="GG_B5Tu">';
        }
        html = '  <div class="GG_Box GG_Box1 PD3">' + ' <a href="' + item.TransferUrl + '">' + img + "</a>" + "   </div>";
    }
    return html;
}

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
        $("#ballListDiv,#shengXiaoListDiv").empty();
    } else {
        if (data.IsTM == 1) {
            //如果是特码
            $("#ballListDiv,#shengXiaoListDiv").append('<span class="LHC_spAjia">+</span>&nbsp;');
        }
        var num = data.Content.length < 2 ? "0" + data.Content : data.Content;
        $("#ballListDiv").append('<span class="LHC_spA ' + getColor(num) + '">' + num + "</span>&nbsp;");
        console.log(data.SubTime);
        $("#shengXiaoListDiv").append('<span class="LHC_spA">' + getShengXiao(data.Content) + "</span>&nbsp;");
    }
}