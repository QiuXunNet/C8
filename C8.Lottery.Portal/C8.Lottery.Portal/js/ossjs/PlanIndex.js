$(function () {
    var cur = GetQueryString("cur");
    if (cur != null) {
        $(".C8_nav5 #g2").first().addClass("current").siblings().removeClass("current");
        $(".C8_nav5 #g2").click();
    }
    $(".hjc_back").click(function () {
        var ref = document.referrer;
        if (ref.length == 0) {
            $(this).attr('href', '/Home/Index');
        } else {
            $(this).attr('href', ref);
        }
    })
});

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}