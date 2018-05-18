$(function () {
    var location = newstypeid;
    getadlist(location);
});

function getadlist(location) {
    $.get("/News/GetAdvertisementListJson", {
        location: location,
        adtype: 2
    }, function (data) {
        if (data.Success == true) {
            inadHtml(data.data);
            console.log(data.data);
        } else {
            alert("Data Loaded: " + data.Msg);
        }
    });
}

function inadHtml(data) {
    var cityid = CityId;
    var city;
    var idx = -1;
    $.each(data, function (index, item) {
        if (item.RestrictedAreas != null && item.RestrictedAreas != "") {
            city = item.RestrictedAreas.split(",");
            idx = $.inArray(cityid.toString(), city);
        }
        if (item.RestrictedAreas == null || item.RestrictedAreas == "" || idx == -1) {
            if (!IsPC()) {
                if (item.Layer == 1) {
                    $(".listdetail").prepend(itemhtml(item));
                } else if (item.Layer == 2) {
                    $(".listtitle").after(itemhtml(item));
                } else if (item.Layer == 3) {
                    $(".slick-track").after(itemhtml(item));
                } else if (item.Layer == 4) {
                    $(".rec-gallery dt").after(itemhtml(item));
                } else if (item.Layer == 5) {
                    $(".rec-gallery").after(itemhtml(item));
                } else if (item.Layer == 6) { } else if (item.Layer == 7) { }
            }
        }
    });
}

function itemhtml(item) {
    var html = "";
    if (item.ThumbStyle == 0) {
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 1) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<div class="GG_B2Left f-l">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + "</div>" + '<div class="GG_B2Right">' + img + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 2) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '">';
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<div class="GG_B4Tu">' + img + "</div>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 3) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            for (var i = 0; i < item.ThumbList.length; i++) {
                img += '<li><img src="' + item.ThumbList[i] + '"></li>';
            }
        }
        html = '<div class="GG_Box GG_Box1">' + '<a href="' + item.TransferUrl + '">' + '<h3 class="GG_B1Tit">' + item.Title + "</h3>" + '<ul class="GG_B3Tu">' + img + '<div style="clear:both;"></div>' + "</ul>" + "</a>" + "</div>";
    } else if (item.ThumbStyle == 4) {
        var img = "";
        if (item.ThumbList != null && item.ThumbList.length > 0) {
            img = '<img src="' + item.ThumbList[0] + '"  class="GG_B5Tu">';
        }
        html = '  <div class="GG_Box GG_Box1 PD3">' + ' <a href="' + item.TransferUrl + '">' + img + "</a>" + "   </div>";
    }
    return html;
}