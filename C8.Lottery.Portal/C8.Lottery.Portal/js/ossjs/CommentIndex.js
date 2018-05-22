$(function () {
    $("#editor").initEmoji()
    $("#editor").focus();
});

function publish() {
    var content = $.trim($("#editor").val());
    if (content.length <= 0)
    {
        alertmsg("请输入内容")
        return;
    }
    $.post("/Comment/Publish", {
        id: Id,
        ctype: OperateType,
        type: CommentType,
        content: content,
        refUid: RefUserId
    }, function (result) {
        if (result) {
            if (result.Code === 401) {
                location.href = "/Home/Login"
            }
            if (result.IsSuccess) {
                alertmsg("发表成功");
                location.href = document.referrer
            } else {
                alertmsg(result.Message)
            }
        } else {
            alertmsg("服务器繁忙")
        }
    })
}