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