$(function () {
    var winH = $(window).height();
    $("#main").height(winH - 60);
    getProcessingRecords();
    //滚动条滚动事件
    $("#main").bind("scroll", function () {
        if ($("#main")[0].scrollHeight - $("#main")[0].scrollTop == $("#main").height() && !isBottom) {
            getProcessingRecords();
        }
    });
});
//获取处理记录
function getProcessingRecords() {
    var id = 0;
    var lis = $("#main").find("li");
    if (lis.length > 0) {
        id = lis.last().attr("id");
    }
    $.post("/Talking/GetProcessingRecords", {
        id: id,
        roomId: roomId
    }, function (data) {
        if (data.Status == 1 && data.DataList.length > 0) {
            if (data.DataList.length < 20) isBottom = true;
            var currDate = "";
            var re_times = $(".re_time", "#main");
            if (re_times.length > 0) {
                currDate = re_times.last().attr("id");
            }
            $(data.DataList).each(function () {
                if (currDate == this.Date) {
                    $("#" + currDate).next().find("ul").append("<li id=" + this.Id + ">" + this.Message + "<span>" + this.Time + "</span></li>");
                } else {
                    currDate = this.Date
                    var html = "";
                    html += "<dl class='record'>";
                    html += "   <dt class='re_time' id ='" + this.Date + "'>" + this.Date + "</dt>";
                    html += "   <dd class='re_notes'>";
                    html += "       <ul>";
                    html += "           <li id=" + this.Id + ">" + this.Message + "<span>" + this.Time + "</span></li>";
                    html += "       </ul >";
                    html += "   </dd >";
                    html += "</dl >";
                    $("#main").append(html);
                }
            });
        }
    });
}