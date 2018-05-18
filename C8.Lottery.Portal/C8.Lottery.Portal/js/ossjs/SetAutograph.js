$(function () {
    set();
});

function set() {
    $("#btnsub").click(function () {
        var autograph = $("#autograph").val();
        if (autograph.trim().length > 20) {
            alertmsg("签名长度不能超过20");
            return false;
        }
        $.post("/Personal/EditUser", {
            value: autograph,
            type: 2
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                location.href = "/Personal/Set";
            }
        });
    });
}