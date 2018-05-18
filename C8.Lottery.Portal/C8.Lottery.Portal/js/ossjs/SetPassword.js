$(function () {
    set();
});

function set() {
    $("#btnsub").click(function () {
        var password1 = $("#password1").val();
        var password2 = $("#password2").val();
        if (password1.length == 0) {
            alertmsg("密码不能为空");
            return false;
        } else if (password1.length < 6 || password1.length > 12) {
            alertmsg("密码长度为6-12位");
            return false;
        }
        if (password2.length == 0) {
            alertmsg("确认密码不不能为空");
            return false;
        }
        if (password2 != password1) {
            alertmsg("两次密码不一致，请重新输入");
            return false;
        }
        $.post("/Home/SetPw", {
            password: password2
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                alertmsg("重置成功");
                setTimeout("location.href='/Home/Login';", 1500);
            }
        });
    });
}