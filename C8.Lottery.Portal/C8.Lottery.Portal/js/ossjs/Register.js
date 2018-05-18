$(function () {
    BindsendMsgClick();
    setInterval("runTime();", 1e3);
    reg();
});

function runTime() {
    var text = $("#sendMsg").text();
    if (text.indexOf("s") != -1) {
        text = text.substr(0, text.length - 1);
        var sec = parseInt(text);
        if (sec == 1) {
            $("#sendMsg").text("点击免费获取");
        } else {
            $("#sendMsg").text(sec - 1 + "s");
        }
    }
}

function BindsendMsgClick() {
    $("#sendMsg").click(function () {
        //$(this).hide();
        var text = $(this).text();
        if (text == "点击免费获取") {
            var mobile = $("#mobile").val();
            if (mobile != "") {
                $(this).text("60s");
                $.post("/Home/GetCode", {
                    mobile: mobile
                }, function (data) {
                    if (data != true) {
                        alertmsg("验证码获取失败");
                    }
                });
            } else {
                alertmsg("手机号不能为空");
            }
        }
    });
}

function reg() {
    $("#btnreg").click(function () {
        var mobile = $("#mobile").val();
        var password = $("#password").val();
        var name = $("#name").val();
        var vcode = $("#vcode").val();
     
        //var hid = $("#hid").val();
        var inviteid = vDataid;
        if (mobile.length == 0) {
            alertmsg("手机号不能为空");
            return false;
        }
        if (name.trim().length == 0) {
            alertmsg("昵称不能为空");
            return false;
        }
        if (password.length == 0) {
            alertmsg("密码不能为空");
            return false;
        } else if (password.length < 6 || password.length > 12) {
            alertmsg("密码长度为6-12位");
            return false;
        }
        if (vcode.length == 0) {
            alertmsg("验证码不能为空");
            return false;
        }
        $.post("/Home/Create", {
            mobile: mobile,
            name: name,
            password: password,
            vcode: vcode,
            inviteid: inviteid
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                location.href = "/Personal";
            }
        });
    });
}