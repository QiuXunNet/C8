
var coininput;
var tipUtil = {
    gift: function () {
        var planId = $("#planId").val() || 0;

        if (planId < 1) {
            alertmsg("打赏失败，请刷新重试");
            return;
        }

        var coin = coininput.val() || 0;

        if (coin > 10000000) {
            alertmsg("超过最大打赏值");
            return;
        }

        if (coin.match(/^[1-9]{1}[0-9]*$/)) {
            $.ajax({
                type: 'POST',
                data: {
                    id: planId,
                    coin: coin
                },
                url: '/Plan/GiftCoin',
                dataType: 'json',
                success: function (res) {
                    if (res.Code == 401) {
                        location.href = "Home/Login";
                    } else {
                        alertmsg(res.Message);
                    }
                },
                error: function (res, xhr) {
                    alertmsg("服务器繁忙");
                }
            });

        } else {
            alertmsg("打赏金币不正确");
        }
    },
    goback: function () {
        var ref = document.referrer;
        if (ref == null || ref == "")
            ref = "/Home/Index";

        location.href = ref;
    }
};

$(function () {
    coininput = $("#coinVal");
    //指定打赏金额li点击事件
    $(".Recharge_UL li").on("click", function () {
        var _this = $(this),
            val = _this.attr("data-val") || 0;
        _this.addClass("current").siblings().removeClass("current");
        coininput.val(val);
    });
    //金币输入事件
    coininput.on("input", function () {
        var val = coininput.val();

        if (val != "undefined" && val != null && val != "") {
            if (val.match(/^[1-9]{1}[0-9]*$/)) {

                $(".Recharge_UL li").removeClass("current");
                $(".Recharge_UL li[data-val='" + val + "']").addClass("current");
            } else {
                //coininput.val();
                alertmsg("输入有误");
            }
        }


    });
    //打赏按钮点击事件
    $("#btnGift").on("click", function() {
        tipUtil.gift();
    });
});