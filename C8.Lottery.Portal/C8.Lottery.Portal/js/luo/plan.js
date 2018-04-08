
$(function () {
    var coininput = $("coinVal");
    $(".Recharge_UL li").on("click", function () {
        var _this = $(this),
            val = _this.attr("data-val") || 0;
        _this.addClass("current").siblings().removeClass("current");
        coininput.val(val);
    });

    coininput.change(function() {
        var val = coininput.val;

        if (val == null || val.length<1) {
            //alertmsg("");
            return;
        }

        $(".Recharge_UL li").removeClass("current");
        $(".Recharge_UL li[data-val='" + val + "']").addClass("current");
    });
});