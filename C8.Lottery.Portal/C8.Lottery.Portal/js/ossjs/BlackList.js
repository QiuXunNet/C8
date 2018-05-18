var isBottom = false;
$(function () {
    var winH = $(window).height();
    $(".wrapper").height(winH - 50);
    getBlackList();
    //滚动条滚动事件
    $(".wrapper").bind("scroll", function () {
        if ($(".wrapper")[0].scrollHeight - $(".wrapper")[0].scrollTop == $(".wrapper").height() && !isBottom) {
            getBlackList();
        }
    });
});
//获取处理记录
function getBlackList() {
    var id = 0;
    var lis = $(".ul_blacklist").find("li");
    if (lis.length > 0) {
        id = lis.last().attr("id");
    }
    $.post("/Talking/GetBlackList", {
        id: id,
        roomId: @ViewBag.RoomId
    }, function (data) {
        if (data.Status == 1 && data.DataList.length > 0) {
            if (data.DataList.length < 20) isBottom = true;
            var html = "";
            $(data.DataList).each(function () {
                html += "<li id=" + this.Id + ">";
                html += "   <img src=" + (this.PhotoImg == null ? "/images/default_avater.png" : this.PhotoImg) + " />";
                html += "   <span>" + this.UserName + "</span>";
                html += "   <input type='button' onclick = 'removeBlackList(this)' data-UserName ='" + this.UserName + "' data-UserId='" + this.UserId + "' data-RoomId='" + this.RoomId + "' value='解禁' />";
                html += "</li>";
            });
            $(".ul_blacklist").append(html);
        }
    });
}

function removeBlackList(t) {
    var userId = $(t).attr("data-UserId");
    var roomId = $(t).attr("data-RoomId");
    var userName = $(t).attr("data-UserName");
    var obj = {};
    obj.userId = userId;
    obj.roomId = roomId;
    obj.userName = userName;
    $.post("/Talking/RemoveBlackList", obj, function (data) {
        if (data.Status == 1) {
            alert("解黑成功");
            $(t).closest("li").remove();
        }
    })
}