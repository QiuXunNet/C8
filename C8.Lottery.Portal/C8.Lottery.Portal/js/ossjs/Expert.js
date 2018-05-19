$(function () {
    $(".C8_bang li").click(function () {
        var id = $(this).attr("data-idx");
        $(".Ranking_footer #" + id).show().siblings().hide();
    });
});