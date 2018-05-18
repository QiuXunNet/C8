$(function () {
    $(".Recha_if2Dl").click(function () {
        $(".Recha_if2Dl dd").removeClass("current");
        $(this).find("dd").addClass("current");
        $("#currentImg").attr("src", ($(this).find("img:eq(0)").attr("src")));
        $("#currentText").text($(this).find("h3:eq(0)").text());
    });
    $(".Recharge_UL li").click(function () {
        $(this).parent().find("li").removeClass("current");
        $(this).addClass("current");
        $("#txtAmount").val($(this).attr("data-Amount"))
    });
    $("#txtAmount").keyup(function () {
        if ($(this).val().length > 0) {
            $(".Recharge_UL li").removeClass("current");
        }
    });
});

function play() {
    var amount = 0;
    if ($(".Recharge_UL .current").length > 0) {
        amount = $(".Recharge_UL .current:eq(0)").attr("data-Amount");
    } else {
        amount = $("#txtAmount").val();
    }
    if (amount == 0) {
        alert("请输入充值金额");
        return;
    }
    var regex = /^\d+\.\d+$/;
    var b = regex.test(amount);
    if (b) {
        alert("请输入整数金额");
        return;
    }
    var playType = $(".Recha_if2Dl .current:eq(0)").attr("data-value");
    if (playType == undefined || playType == null) {
        alert("请选择支付方式");
    }
    var obj = {};
    obj.amount = amount;
    if (playType == "1") {
        $.post("/Recharge/GetWxUrl", obj, function (data) {
            if (data == "0") {
                alert("操作失败,请刷新后重试");
            } else {
                document.location.href = data;
            }
        });
    }
    if (playType == "2") {
        $.post("/Recharge/GetZfbUrl", obj, function (data) {
            if (data == "0") {
                alert("操作失败,请刷新后重试");
            } else {
                $("#hideDiv").append(data);
            }
        });
    }
}