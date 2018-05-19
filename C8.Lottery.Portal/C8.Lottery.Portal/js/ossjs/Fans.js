var pageIndex = 1, pageSize = 30;

$(function () {
    var dropload = $(".fans_Box").dropload({
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
                url: "/Personal/MyFans",
                data: {
                    pageIndex: pageIndex,
                    pageSize: pageSize
                },
                dataType: "json",
                success: function (res) {
                    if (res.Data.TotalCount < 1) {
                        me.noData();
                    }
                    if (res.IsSuccess && res.Data) {
                        joinhtml(res.Data);
                        console.log(res.Data);
                        if (res.Data.HasNextPage) {
                            pageIndex++;
                        } else {
                            me.lock();
                            $(".dropload-down").remove();
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

function joinhtml(data) {
    var html = [];
    $.each(data.PageData, function (index, value) {
        console.log(value);
        var dl = "<dl class='details_DL'><dt><a href='/Personal/UserCenter/" + value.UserId + "'><img src='" + (value.Headpath.length <= 0 ? "/images/default_avater.png" : value.Headpath) + "'></a></dt>" + "<dd>" + "<div class='fans_ddRight f-r'  onclick='unorfollow(this," + value.UserId + ")'>" + (value.Isfollowed <= 0 ? "<p class='fans_Gz'>关注</p>" : "<p class='fans_Ygz'>已关注</p>") + "</div>" + "<div class='details_ddLeft'>" + "<h3><a href='/Personal/UserCenter/" + value.UserId + "'>" + value.Nickname + "</a></h3>" + "<p>" + value.Autograph + "</p>" + "</div> </dd> </dl>";
        html.push(dl);
    });
    $("#content").append(html.join(""));
}

function unorfollow(obj, follow_uid) {
    var text = $(obj).children("p").text();
    if (text == "关注") {
        $.post("/Personal/IFollow", {
            followed_userId: follow_uid
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                $(obj).children("p").remove();
                $(obj).append("<p class='fans_Ygz'>已关注</p>");
            }
        });
    } else {
        $(document).dialog({
            type: "confirm",
            closeBtnShow: true,
            style: "ios",
            content: "确定不再关注此人?",
            onClickConfirmBtn: function () {
                $.post("/Personal/UnFollow", {
                    followed_userId: follow_uid
                }, function (data) {
                    if (data.Success != true) {
                        alertmsg(data.Msg);
                    } else {
                        $(obj).children("p").remove();
                        $(obj).append("<p class='fans_Gz'>关注</p>");
                    }
                });
            }
        });
    }
}