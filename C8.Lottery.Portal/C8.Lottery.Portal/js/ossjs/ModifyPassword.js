$(function () {
    modiflypwd();
});

function modiflypwd() {
    $("#btnconfirm").click(function () {
        var oldpwd = $("#oldpwd").val();
        var newpwd = $("#newpwd").val();
        var cfnewpwd = $("#cfnewpwd").val();
        if (oldpwd.length == 0) {
            alertmsg("旧密码不能为空");
            return false;
        }
        if (newpwd.length == 0) {
            alertmsg("新密码不能为空");
            return false;
        } else if (newpwd.length < 6 || newpwd.length > 12) {
            alertmsg("密码长度为6-12位");
            return false;
        }
        if (cfnewpwd.length == 0) {
            alertmsg("确认密码不能为空");
            return false;
        } else {
            if (cfnewpwd != newpwd) {
                alertmsg("两次密码输入不一致、请重新输入");
                return false;
            }
        }
        $.post("/Personal/ModifyPWD", {
            oldpwd: oldpwd,
            newpwd: newpwd
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                alertmsg("修改成功");
            }
        });
    });
}