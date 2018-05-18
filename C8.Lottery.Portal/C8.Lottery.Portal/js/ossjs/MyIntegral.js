$(function () {
    var pid = $(".hdNav_cai_lhs  .current").attr("data-index");
    GetList(pid);
    $(".hdNav_cai_lhs li").click(function () {
        var pid = $(this).attr("data-index");
        GetList(pid);
    });
    $(".hdNav_cai li").click(function () {
        var pid = $(this).attr("data-index");
        GetList(pid);
    });
});

function GetList(PId) {
    $.ajax({
        type: "GET",
        data: {
            PId: PId
        },
        async: false,
        url: "/Personal/GetMyIntegral",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                if (data.data.length == 0) {
                    $(".points_TInfo").html('<div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无相关数据~</p></div>');
                } else {
                    buildHtml(data.data);
                }
            } else {
                alertmsg("服务器繁忙");
            }
        }
    });
}

function buildHtml(data) {
    var itemList = [];
    var listDom = $(".points_TInfo");
    $.each(data, function (index, item) {
        var itemHtml = '<dl class="points_Dl">' + ' <dt><a href="javascript:;"><img src="/images/' + item.LotteryIcon + '.png"></a></dt>' + "<dd>" + '<p class="TIf_fen f-r">' + item.Score + "</p>" + '<h3 class="TIf_tit"><a href="javascript:;">' + item.Name + "</a></h3>" + "</dd>" + "</dl>";
        itemList.push(itemHtml);
    });
    listDom.html("");
    var html = itemList.join("");
    listDom.append(html);
}