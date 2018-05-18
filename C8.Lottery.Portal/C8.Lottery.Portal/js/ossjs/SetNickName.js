$(function () {
    set();
});

function set() {
    $("#btnsub").click(function () {
        var name = $("#name").val();
        if (name.trim().length == 0) {
            alertmsg("昵称不能为空");
            return false;
        }
        if (name.trim().length > 8) {
            alertmsg("昵称长度不能超过8");
            return false;
        }
        $.post("/Personal/EditUser", {
            value: name,
            type: 1
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                location.href = "/Personal/Set";
            }
        });
    });
}