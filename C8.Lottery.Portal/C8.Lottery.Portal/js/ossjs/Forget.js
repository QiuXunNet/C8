$(function () {
    Validate();
    setInterval("runTime();", 1e3);
    BindsendMsgClick();
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

//验证
function Validate() {
    $("#btnNext").click(function () {
        var mobile = $("#mobile").val();
        var vcode = $("#vcode").val();
        if (mobile.length == 0) {
            alertmsg("手机号不能为空");
            return false;
        }
        if (vcode.length == 0) {
            alertmsg("验证码不能为空");
            return false;
        }
        if (mobile != "") {
            $.post("/Home/Validate", {
                mobile: mobile,
                vcode: vcode
            }, function (data) {
                if (data.Success != true) {
                    alertmsg(data.Msg);
                } else {
                    location.href = "/Home/SetPassword";
                }
            });
        }
    });
}