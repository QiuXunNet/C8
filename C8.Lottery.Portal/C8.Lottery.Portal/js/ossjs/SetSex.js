$(function () {
    set();
});

function set() {
    $("#btnsub").click(function () {
        var selectvalue = $(".current").text().trim();
        var sexvalue;
        if (selectvalue == "男") {
            sexvalue = 0;
        } else {
            sexvalue = 1;
        }
        $.post("/Personal/EditUser", {
            value: sexvalue,
            type: 3
        }, function (data) {
            if (data.Success != true) {
                alertmsg(data.Msg);
            } else {
                location.href = "/Personal/Set";
            }
        });
    });
}