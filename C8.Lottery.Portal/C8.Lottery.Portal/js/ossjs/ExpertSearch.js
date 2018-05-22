$(function () {
    $(".ttIf2_Dlicon").on("click", function () {
        var uid = $(this).attr("data-uid");
    
        DeleteHistory(uid, lType);
    });
    $(".ttIf2_QK").on("click", function () {
 
        DeleteHistory(0, lType);
    });
    $(".Search_BLdel").on("click", function () {
        $("#NickName").val('');
    });
});

function Search(lType) {
    var NickName = $("#NickName").val();
    if (NickName.trim().length <= 0) {
        alertmsg("请输入专家名称")
        return false;
    }
    $.get("/Plan/ExpertSearchList", {
        lType: lType,
        NickName: NickName
    }, function (data) {
        console.log(data);
        if (data.Success == true) {
            $(".ttIf1_tit").remove();
            if (data.data.length > 0) {
                buildHtml(data.data);
            } else {
                $(".ttIf1_con").html('<div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无相关数据~</p></div>');
            }
        } else {
            alertmsg(data.Msg);
        }
    });
}

function buildHtml(data) {
    console.log(data);
    var html = [];
    var li = '';
    var text = '';
    $.each(data, function (index, value) {
        console.log(value.PlayName);
        if (value.isFollow > 0) {
            text = '已关注';
        } else {
            text = '关注';
        }
        li = '<li data-uid="' + value.UserId + '"><div class="ttIf1_cL f-l"><a href="javascript:;" onclick="InsertHotSearch(\'' + value.PlayName + '\',' + value.UserId + ',' + lType + ')"><img src="' + (value.Avater == '' ? '/images/default_avater.png' : value.Avater) + '"></a></div>' + '  <a href="javascript:;"  onclick="javascript:unorfollow(this,' + value.UserId + ');" class="ttIf1_cGz f-r">' + text + '</a>' + '    <div class="ttIf1_cR" onclick="InsertHotSearch(\'' + value.PlayName + '\',' + value.UserId + ',' + lType + ')"  data-uid="' + value.UserId + '"><a href="javascript:;">' + value.Name + '</a></div>' + ' </li>'
        html.push(li);
    });
    $(".ttIf1_con").empty();
    $(".ttIf1_con").append(html.join(''));
}

function InsertHotSearch(P, UserId, lType) {
    $.post("/Plan/InsertHotSearch", {
        uid: UserId,
        lType: lType
    }, function (data) {
        if (data.Success == true) {
            //window.location.href = "/Plan/PlayRecord/" + lType + "?uid=" + UserId + "&type=1"
            window.location.href = "/Plan/PlayRecord/" + lType + "?uid=" + UserId + "&p=" + P + "";
        } else {
            alertmsg(data.Msg);
        }
    });
}

function unorfollow(obj, follow_uid) {
    var text = $(obj).text();
    if (text == "关注") {
        $.post('/Personal/IFollow', {
            followed_userId: follow_uid
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                $(obj).text("已关注");
            }
        });
    } else {
        $(document).dialog({
            type: 'confirm',
            closeBtnShow: true,
            style: "ios",
            content: '确定不再关注此人?',
            onClickConfirmBtn: function () {
                $.post('/Personal/UnFollow', {
                    followed_userId: follow_uid
                }, function (data) {
                    if (data.Success != true) {
                        alertmsg(data.Msg);
                    } else {
                        $(obj).text("关注");
                    }
                });
            }
        });
    }
}

function DeleteHistory(uid, lType) {
    $.get("/Plan/DeleteHistory", {
        uid: uid,
        lType: lType
    }, function (data) {
        if (data.Success == true) {
            if (uid > 0) {
                $("#d_" + uid).remove();
            } else {
                $(".Search_ttInfo2").children("dl").remove();
            }
        } else {
            alertmsg(data.Msg);
        }
    });
}