$(function () {
    login();
});

function login() {
    $("#btnlogin").click(function () {
        var mobile = $("#mobile").val();
        var password = $("#password").val();
        if (mobile.length == 0) {
            alertmsg("手机号不能为空");
            return false;
        }
        if (password.length == 0) {
            alertmsg("密码不能为空");
            return false;
        }
        $.post("/Home/Logins", {
            mobile: mobile,
            password: password
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                location.href = "/Personal/Index";
            }
        });
    });
}