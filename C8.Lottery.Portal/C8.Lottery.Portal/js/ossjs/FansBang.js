var dropload;

$(function () {
    var initElement = $(".C8_bang li.current");
    initnews(initElement);
    MyRank(initElement);
    $(".C8_bang li").click(function () {
        $(".Ranking_content").children("div").css("display", "none");
        var id = $(this).attr("data-id");
        $("#bang_" + id).css("display", "block");
        if ($(".dropload-down").length > 0) {
            $(".dropload-down").remove();
        }
        $(".Rank_ftDL").empty();
        initnews($(this));
        MyRank($(this));
    });
});

function initnews(clickElement) {
    var id = clickElement.attr("data-id"), type = clickElement.attr("data-type");
    pageIndex = clickElement.attr("data-pageindex") || 1, pageSize = clickElement.attr("data-pagesize") || 100;
    if (!id) return;
    dropload = $("#bang_" + id).dropload({
        scrollArea: window,
        domDown: {
            domClass: "dropload-down",
            domRefresh: '<div class="dropload-refresh">↑上拉加载更多</div>',
            domLoad: '<div class="dropload-load"><span class="loading"></span>加载中...</div>',
            domNoData: '<div class="dropload-noData">暂无数据</div>'
        },
        loadDownFn: function (me) {
            $.ajax({
                type: "GET",
                url: "/Personal/FansBangList",
                data: {
                    typeId: id,
                    type: type,
                    pageIndex: pageIndex,
                    pageSize: pageSize
                },
                dataType: "html",
                success: function (data) {
                    if (data && data.length > 0) {
                        // 延迟0.1秒加载
                        setTimeout(function () {
                            $("#bang_" + id + " .dropload-down").before(data);
                            pageIndex++;
                            clickElement.attr("data-pageindex", pageIndex);
                            // 每次数据加载完，必须重置
                            dropload.resetload();
                        }, 100);
                    }
                },
                error: function (xhr, type) {
                    $(document).dialog({
                        type: "notice",
                        infoText: "服务器繁忙",
                        autoClose: 1500
                    });
                    // 即使加载出错，也得重置
                    dropload.resetload();
                }
            });
        }
    });
}

function MyRank(clickElement) {
    var id = clickElement.attr("data-id");
    var type = clickElement.attr("data-type");
    if (!id) return;
    $.get("/Personal/MyRank", {
        type: type
    }, function (data) {
        if (data.Success != true) {
            alertmsg(data.Msg);
        } else {
            if (data.data != null) {
                var text = "";
                if (data.data.Rank <= 0) {
                    text = "暂未上榜";
                } else {
                    text = data.data.Rank;
                }
                $(".Rank_ftDL").html("<dt><a href='javascript:;'><img src='" + (data.data.HeadPath == null ? "/images/default_avater.png" : data.data.HeadPath) + "'></a></dt> <dd> <p class='Ds_money1 Fs_money1 f-r'>" + data.data.Number + "</p> <div class='Rank_ftL'>" + "<h3><a href='javascript:;'>" + data.data.Name + "</a></h3>" + "<P>当前排名：<span id='myrank'>" + text + "</span></P> </div> </dd>");
            }
        }
    });
}