$(function ()
{
    var id = $(".C8_bang li.current").attr("data-idx");
    $(".Ranking_footer #" + id).show().siblings().hide();
    $(".C8_bang li").click(function () {
        var id = $(this).attr("data-idx");
        $(".Ranking_footer #" + id).show().siblings().hide();
    });

    $(".hdNav_cai_lhs li").click(function () {
        var id = $(this).attr("data-id");
        var url = $("#" + id + " li").first().find("a").attr("href");
        location.href = url;
      
    });
    $(".hdNav_cai li").click(function () {
        var id = $(this).attr("data-id");
        var url = $("#" + id + " li").first().find("a").attr("href");
        location.href = url;

    });

});