var pageIndex = 1, pageSize = 30;

$(function () {
    $("#Gues1").parseEmoji();
    var dropload = $("#Gues2").dropload({
        scrollArea: window,
        domDown: {
            domClass: "dropload-down",
            domRefresh: "",
            domLoad: '<div class="dropload-load"><span class="loading"></span>加载中...</div>',
            domNoData: '<div style="height:auto;" class="dropload-noData"><img src="/images/null.png" alt="暂无数据" />暂无数据</div>'
        },
        loadDownFn: function (me) {
            $.ajax({
                type: "GET",
                url: "/News/LastComment",
                data: {
                    id: id,
                    type: ctype,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    refUid: RefUid
                },
                dataType: "json",
                success: function (res) {
                    if (res.Data.TotalCount < 1) {
                        me.noData();
                    }
                    if (res.IsSuccess && res.Data) {
                        buildHtml(res.Data);
                        if (res.Data.HasNextPage) {
                            pageIndex++;
                        } else {
                            me.lock();
                        }
                    } else {
                        alertmsg("服务器繁忙");
                    }
                    dropload.resetload();
                },
                error: function (xhr, type) {
                    alertmsg("服务器繁忙");
                    dropload.resetload();
                }
            });
        }
    });
});

function buildHtml(data) {
    var html = [];
    $.each(data.PageData, function (index, value) {
        var li = "<li>" + '<dl class="Guse_CDL">' + '<dt><a href="/Personal/UserCenter/' + value.UserId + '"><img src="' + (value.Avater.length <= 0 ? "/images/default_avater.png" : value.Avater) + '"></a></dt>' + "<dd>" + ' <div class="CDL_DDTop">' + '<h3><a href="/Personal/UserCenter/' + value.UserId + '">' + value.NickName + "</a></h3>" + "<p>" + value.SubTimeStr + "</p>" + " </div>" + '<div class="CDL_DDFont">' + " <p>" + value.Content + "</p>" + "</div>" + "</dd>" + "</dl>" + '<div class="comp_Dlk4">' + '<a href="javascript:;" class="comp_DZ">';
        if (value.CurrentUserLikes > 0) {
            li += '<i class="on" data-id="' + value.Id + '" data-ctype="2" data-commenttype="' + value.Type + '"><img src="/images/46_1.png" class="DZ_tu1"><img src="/images/46_2.png" class="DZ_tu2"></i>';
        } else {
            li += '<i data-id="' + value.Id + '" data-ctype="1" data-commenttype="' + value.Type + '"><img src="/images/46_1.png" class="DZ_tu1"><img src="/images/46_2.png" class="DZ_tu2"></i>';
        }
        li += "<span>" + (value.StarCount > 0 ? value.StarCount : "") + "</span>" + "</a>" + '<a href="/Comment/Index/' + value.Id + "?ctype=2&type=" + ctype + '" class="comp_HF"><img src="/images/46_3.png"></a>' + "</div>" + (value.ReplayCount > 0 ? '<div class="comp_replay"><a href="/News/CommentDetail/' + value.Id + "?type=" + value.Type + '">查看全部<span>' + value.ReplayCount + "</span>条回复>></a></div>" : "") + "</li>";
        li = $.stringParseEmoji(li);
        html.push(li);
    });
    $("#Gues2 .Gues_ul").append(html.join(""));
    $("#totalCount").html("（" + data.TotalCount + "）");
}