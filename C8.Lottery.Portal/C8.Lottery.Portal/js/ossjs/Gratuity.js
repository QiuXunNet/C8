$(function () {
    var pid = $(".hdNav_cai_lhs .current").attr("data-pid");
    var ltype = $("#group_" + pid + " .current").attr("data-ltype");
    var queryType = $(".C8_bang .current").attr("data-idx");
    getlist(queryType, 9, ltype);
    $(".hdNav_cai_lhs li").click(function () {
        $(".C8_bang").find("li").last().addClass("current").siblings().removeClass("current");
        var _this = $(this);
        var pid = _this.attr("data-pid");
        var ltype = $("#group_" + pid + " .current").attr("data-ltype");
        var queryType = $(".C8_bang .current").attr("data-idx");
        getlist(queryType, 9, ltype);
    });
    $(".hdNav_cai li").click(function () {
        $(".C8_bang").find("li").last().addClass("current").siblings().removeClass("current");
        var _this = $(this);
        var pid = _this.attr("data-pid");
        var ltype = $("#group_" + pid + " .current").attr("data-ltype");
        var queryType = $(".C8_bang .current").attr("data-idx");
        getlist(queryType, 9, ltype);
    });
    $(".GS_box li").on("click", function () {
        $(".C8_bang").find("li").last().addClass("current").siblings().removeClass("current");
        var pid = $(".hdNav_cai_lhs .current").attr("data-pid");
        var _this = $(this);
        var ltype = _this.attr("data-ltype");
        var queryType = $(".C8_bang .current").attr("data-idx");
        getlist(queryType, 9, ltype);
    });
    $(".C8_bang li").click(function () {
        var pid = $(".hdNav_cai_lhs .current").attr("data-pid");
        var ltype = $("#group_" + pid + " .current").attr("data-ltype");
        var queryType = $(this).attr("data-idx");
        getlist(queryType, 9, ltype);
    });
});

function getlist(queryType, RType, lType) {
    $.get("/List/GetRankMoneyList", {
        queryType: queryType,
        RType: RType,
        lType: lType
    }, function (data) {
        if (data.Success == true) {
            myrankHtml(data.data.MyRankMonyModel);
            if (data.data.RankMonyModelList.length == 0) {
                $(".Ranking_Cinfo").html('<div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无相关数据~</p></div>');
            } else {
                buildHtml(data.data.RankMonyModelList, queryType, lType);
            }
        } else {
            alertmsg(data.Msg);
        }
    });
}

function buildHtml(data, queryType, ltype) {
    console.log(data);
    var html = [];
    var html2 = [];
    var tb = '<table width="100%" border="0" class="Ranking_table1"> <tbody>';
    var tb2 = '<table width="100%" border="0" class="Ranking_table2"><tbody>';
    var tr = "";
    var tr2 = "";
    $.each(data, function (index, value) {
        if (queryType == "all") {
            if (value.Rank <= 3) {
                tr2 = "" + '<td width="33%" class="' + sortByRank(value.Rank) + '" align="center" valign="middle">' + ' <div class="Ranking_Blogotu2">' + ' <div class="Ranking_B2tu"> ' + '<a href="/Plan/PlayRecord/' + ltype + "?uid=" + value.UserId + '&type=1"><img src="' + (value.Avater == null ? "/images/default_avater.png" : value.Avater) + '"></a>' + " </div>" + ' <img src="' + gettop3img(value.Rank) + '" class="Ranking_PHtu">' + " </div>" + ' <h3 class="Ranking_name"><a href="javascript:;">' + value.NickName + "</a></h3>" + '<span class="Ds_money2">' + value.Money + "</span>" + "</td>";
                html2.push(tr2);
            } else {
                tr = '<tr><td align="center" valign="middle"> <p>' + value.Rank + "</p></td>" + '<td width="15%" align="center" valign="middle">' + ' <div class="Ranking_Blogotu">' + '  <a href="/Plan/PlayRecord/' + ltype + "?uid=" + value.UserId + '&type=1"><img src="' + (value.Avater == null ? "/images/default_avater.png" : value.Avater) + '"></a>  </div>' + '   </td> <td align="left" valign="middle">' + '   <p class="' + GetYsfont(value.Rank) + '"><a href="javascript:;">' + value.NickName + "</a></p>" + ' </td> <td width="25%" align="center" valign="middle"><span class="Ds_money">' + value.Money + "</span></td>" + " </tr> ";
            }
        } else {
            var tdrank = '<td align="center" valign="middle"> <p>' + value.Rank + "</p></td>";
            if (value.Rank <= 3) {
                tdrank = '<td width="10%" align="center" valign="middle"><img src="' + gettop3img(value.Rank) + '" class="Ranking_Btu"></td>';
            }
            tr = "<tr>" + tdrank + '<td width="15%" align="center" valign="middle">' + ' <div class="Ranking_Blogotu">' + '  <a href="/Plan/PlayRecord/' + ltype + "?uid=" + value.UserId + '&type=1"><img src="' + (value.Avater == null ? "/images/default_avater.png" : value.Avater) + '"></a>  </div>' + '   </td> <td align="left" valign="middle">' + '   <p class="' + GetYsfont(value.Rank) + '"><a href="javascript:;">' + value.NickName + "</a></p>" + ' </td> <td width="25%" align="center" valign="middle"><span class="Ds_money">' + value.Money + "</span></td>" + " </tr> ";
        }
        html.push(tr);
    });
    $(".Ranking_Cinfo").html("");
    if (queryType == "all") {
        $(".Ranking_Cinfo").append(tb2 + "<tr>" + html2.join("") + "</tr>" + "</tbody></table>" + tb + html.join("") + "</tbody></table>");
    } else {
        $(".Ranking_Cinfo").append(tb + html.join("") + "</tbody></table>");
    }
}

//我的排名
function myrankHtml(data) {
    $(".Rank_ftDL").html("");
    var text = "";
    if (data.Rank <= 0 || data.Rank > 100) {
        text = "暂未上榜";
    } else {
        text = data.Rank;
    }
    var html = "<dt>" + '<a href="javascript:;"><img src="' + (data.Avater == null ? "/images/default_avater.png" : data.Avater) + '"></a>' + " </dt>" + " <dd>" + '<p class="Ds_money1 f-r">' + data.Money + "</p>" + ' <div class="Rank_ftL">' + '  <h3><a href="javascript:;">' + data.NickName + "</a></h3>" + " <P>" + "  当前排名：<span>" + text + "</span>" + "</P>" + " </div>" + " </dd>";
    $(".Rank_ftDL").append(html);
}

//前3图片
function gettop3img(rank) {
    var imgsrc = "";
    switch (rank) {
        case 1:
            imgsrc = "/images/44_1.png";
            break;

        case 2:
            imgsrc = "/images/44_2.png";
            break;

        case 3:
            imgsrc = "/images/44_3.png";
            break;
    }
    return imgsrc;
}