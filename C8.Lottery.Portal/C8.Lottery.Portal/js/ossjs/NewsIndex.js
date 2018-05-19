$(function () {
    $(".info_hdNav_cai_lhs li").unbind("click");
    calcNavScoll();
    initnews();
    getadlist(id);
});

function calcNavScoll() {
    var index = $(".info_hdNav_cai_lhs li.current").index();
    if (index > 0) {
        var liposition = $(".info_hdNav_cai_lhs li.current").position().left;
        $(".info_hdNav_cai_lhs").scrollLeft(liposition);
    }
}

function initnews() {
    dropload = $("#news_" + id).dropload({
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
                url: "/News/NewsList",
                data: {
                    typeId: id,
                    pageIndex: pageIndex,
                    pageSize: pageSize
                },
                dataType: "html",
                success: function (data) {
                    if (data && data.length > 0) {
                        // 延迟0.1秒加载
                        setTimeout(function () {
                            $("#news_" + id + " .dropload-down").before(data);
                            pageIndex++;
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

function getadlist(location) {
    $.get("/News/GetAdvertisementListJson", {
        location: location,
        adtype: 1
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
                    $(".CZ_hdMain").after(itemhtml(item));
                }
            }
        }
    });
}

function itemhtml(item) {
    var html = "";
    if (item.ThumbStyle == 0) {
        html = '<div class="GG_Box  GG_Box1 ">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + ' <div class="newstime">' + "<span>" + item.Company + "</span> <span>" + item.TimeStr + "</span>" + "<span>" + item.CommentsNumber + '<i class="icon-comment"></i> </span>' + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 1) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<div class="GG_B2Left f-l">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + ' <div class="newstime">' + "<span>" + item.Company + "</span> <span>" + item.TimeStr + "</span>" + "<span>" + item.CommentsNumber + '<i class="icon-comment"></i> </span>' + "</div>" + "</div>" + '<div class="GG_B2Right">' + img + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 2) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<div class="GG_B4Tu">' + img + "</div>" + ' <div class="newstime">' + "<span>" + item.Company + "</span> <span>" + item.TimeStr + "</span>" + "<span>" + item.CommentsNumber + '<i class="icon-comment"></i> </span>' + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 3) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            for (var i = 0; i < item.ThumbList.length; i++) {
                img += '<li><img src="' + item.ThumbList[i] + '"></li>';
            }
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<ul class="GG_B3Tu">' + img + '<div style="clear:both;"></div>' + "</ul>" + ' <div class="newstime">' + "<span>" + item.Company + "</span> <span>" + item.TimeStr + "</span>" + "<span>" + item.CommentsNumber + '<i class="icon-comment"></i> </span>' + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 4) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '"  class="GG_B5Tu">';
        }
        html = '  <div class="GG_Box GG_Box1 PD3">' + ' <a href="' + item.TransferUrl + '">' + img + "</a>" + "   </div>";
    }
    return html;
}