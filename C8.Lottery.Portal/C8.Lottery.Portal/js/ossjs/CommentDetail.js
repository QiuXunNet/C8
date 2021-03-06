﻿var pageIndex = 1, pageSize = 20;
var gues2 = $("#Gues2");

$(function () {
    $("#Gues1").parseEmoji();
    var dropload = gues2.dropload({
        scrollArea: window,
        domDown: {
            domClass: "dropload-down",
            domRefresh: "",
            domLoad: '<div class="dropload-load"><span class="loading"></span>加载中...</div>',
            domNoData: '<div class="dropload-noData"><img src="/images/null.png" alt="暂无数据" />暂无数据</div>'
        },
        loadDownFn: function (me) {
            $.ajax({
                type: "GET",
                url: "/News/LastReply",
                data: {
                    id: id,
                    type: ctype,
                    pageIndex: pageIndex,
                    pageSize: pageSize
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
        var li = "<li>" + '<dl class="Guse_CDL">' + '<dt><a href="/Personal/UserCenter/' + value.UserId + '"><img src="' + (value.Avater.length <= 0 ? "/images/default_avater.png" : value.Avater) + '"></a></dt>' + "<dd>" + ' <div class="CDL_DDTop">' + '<h3><a href="/Personal/UserCenter/' + value.UserId + '">' + value.NickName + "</a></h3>" + "<p>" + value.SubTimeStr + "</p>" + " </div>" + '<div class="CDL_DDFont">';
        var parentComment = value.ParentComment;
        if (parentComment) {
            li += "<p><span>" + value.Content + '</span>//<a href="/Personal/UserCenter/' + parentComment.UserId + '" class="include">@@' + parentComment.NickName + "：</a>" + parentComment.Content + "</p>";
        } else {
            li += " <p>" + value.Content + "</p>";
        }
        li += "</div>" + "</dd>" + "</dl>" + '<div class="comp_Dlk4">' + '<a href="javascript:;" class="comp_DZ">';
        if (value.CurrentUserLikes > 0) {
            li += '<i class="on" data-id="' + value.Id + '" data-ctype="2" data-commenttype="' + value.Type + '"><img src="/images/46_1.png" class="DZ_tu1"><img src="/images/46_2.png" class="DZ_tu2"></i>';
        } else {
            li += '<i data-id="' + value.Id + '" data-ctype="1" data-commenttype="' + value.Type + '"><img src="/images/46_1.png" class="DZ_tu1"><img src="/images/46_2.png" class="DZ_tu2"></i>';
        }
        li += "<span>" + (value.StarCount > 0 ? value.StarCount : "") + "</span>" + "</a>" + '<a href="/Comment/Index/' + value.Id + "?ctype=2&type=" + ctype + '" class="comp_HF"><img src="/images/46_3.png"></a>' + "</div>" + "</li>";
        html.push(li);
    });
    var lilistHtml = $.stringParseEmoji(html.join(""));
    $("#Gues2 .Gues_ul").append(lilistHtml);
    $("#totalCount").html("（" + data.TotalCount + "）");
}