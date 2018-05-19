var dropload;

$(function () {
    var initElement = $(".C8_bang li.current");
    getList(initElement);

    $(".C8_bang li").click(function () {
        $(".Ranking_content").children("div").css("display", "none");
        var id = $(this).attr("data-id");
        $("#bang_" + id).css("display", "block");
        if ($(".dropload-down").length > 0) {
            $(".dropload-down").remove();
        }

        getList($(this));
    });
});

function getList(clickElement) {
    var id = clickElement.attr("data-id");
    var type = clickElement.attr("data-type");

    var div = $("#bang_" + id);

    if (!id) return;

    $.ajax({
        type: "post",
        url: "/Personal/FansBangList",
        data: { type: type },
        dataType: "json",
        success: function (data) {
            if (data.List != null && data.List.length > 0) {
                var html = '<table width="100%" border="0" class="Ranking_table1">';
                var headHtml = '<table width="100%" border="0" class="Ranking_table2"><tbody><tr>';
                $(data.List).each(function (i) {
                    this.HeadPath = this.HeadPath == null ? "/images/default_avater.png" : this.HeadPath;

                    //总榜头部
                    if (i < 3 && Number(id) == 4) {
                        headHtml += "<td width='33%' class='rank_33_" + this.Rank + "' align='center' valign='middle'>";
                        headHtml += "    <div class='Ranking_Blogotu2'>";
                        headHtml += "        <div class='Ranking_B2tu'><a href='/Personal/UserCenter/" + this.Followed_UserId + "'><img src='" + this.HeadPath + "'></a></div>";
                        headHtml += "            <img src='/images/44_" + this.Rank + ".png' class='Ranking_PHtu'>";
                        headHtml += "        </div>";
                        headHtml += "            <h3 class='Ranking_name'><a href='javascript:;'>" + this.Name + "</a></h3>";
                        headHtml += "            <span class='Ds_money2 Fs_money2'>" + this.Number + "</span>";
                        headHtml += "    </td>";
                    }
                    //其他榜头部
                    if (i < 3 && Number(id) != 4) {
                        html += "<tr>";
                        html += "    <td width='10%' align='center' valign='middle'><p><img src='/images/44_" + this.Rank + ".png' class='Ranking_Btu'></p></td>";
                        html += "    <td width='15%' align='center' valign='middle'><div class='Ranking_Blogotu'><a href='/Personal/UserCenter/" + this.Followed_UserId + "'><img src='" + this.HeadPath + "'></a></div></td>";
                        html += "    <td align='left' valign='middle'><p><a href='javascript:;'>" + this.Name + "</a></p></td>";
                        html += "    <td width='25%' align='center' valign='middle'><span class='Ds_money Fs_money'>" + this.Number + "</span></td>";
                        html += "</tr>";
                    }
                    if (i >= 3) {
                        html += "<tr>";
                        html += "    <td width='10%' align='center' valign='middle'><p>" + this.Rank + "</p></td>";
                        html += "    <td width='15%' align='center' valign='middle'><div class='Ranking_Blogotu'><a href='/Personal/UserCenter/" + this.Followed_UserId + "'><img src='" + this.HeadPath + "'></a></div></td>";
                        html += "    <td align='left' valign='middle'><p><a href='javascript:;'>" + this.Name + "</a></p></td>";
                        html += "    <td width='25%' align='center' valign='middle'><span class='Ds_money Fs_money'>" + this.Number + "</span></td>";
                        html += "</tr>";
                    }
                });
                html += "</table>"

                if (Number(id) == 4) {
                    headHtml += "</tr></tbody></table>";

                    html = headHtml + html;
                }

                div.empty().append(html);
            }
            else {
                div.empty().append('<div class="mescroll-empty"><img class="empty-icon" src="/images/null.png"><p class="empty-tip">暂无相关数据~</p></div>');
            }

            if (data.My != null) {
                var text = "";
                if (data.My.Rank <= 0) {
                    text = "暂未上榜";
                } else {
                    text = data.My.Rank;
                }
                $(".Rank_ftDL").empty().html("<dt><a href='javascript:;'><img src='" + (data.My.HeadPath == null ? "/images/default_avater.png" : data.My.HeadPath) + "'></a></dt> <dd> <p class='Ds_money1 Fs_money1 f-r'>" + data.My.Number + "</p> <div class='Rank_ftL'>" + "<h3><a href='javascript:;'>" + data.My.Name + "</a></h3>" + "<P>当前排名：<span id='myrank'>" + text + "</span></P> </div> </dd>");
            }
        }
    });
}

