var dropload;
var pageIndex =  1,
   pageSize = 30;
$(function () {
    //newsManager.GetLastBetInfo();
    newsManager.LoadlTypesAndChannel();
    newsManager.calcNavScoll();
})

var newsManager = {
    GetLastBetInfo: function () {
        $.ajax({
            type: 'POST',
            url: '/News/GetLastBetInfo',
            data: { lType: $("#lType").val() },
            dataType: 'json',
            success: function (data) {

                if (data) {
                    var tempHtml = "";
                    $("#txtIssue").html(data.Issue);
                    $("#ballListDiv").html(data.NumHtml);
                }
            },
            error: function (xhr, type) {

                $(document).dialog({
                    type: 'notice',
                    infoText: '服务器繁忙',
                    autoClose: 1500
                });
            }
        });
    },
    LoadlTypesAndChannel: function () {
        $.ajax({
            type: 'POST',
            url: '/News/LoadlTypesAndChannel',
            data: { lType: $("#PlType").val(), clType: $("#lType").val() },
            dataType: 'json',
            success: function (data) {

                if (data && data.TypeList.length > 0) {
                    var tempHtml = "";
                    var currentlType = $("#PlType").val();
                    for (var i = 0; i < data.TypeList.length; i++) {
                        var entity = data.TypeList[i];
                        if (entity.TypeName == "6彩") {
                            if (entity.Id == currentlType) {
                                tempHtml += '<li class="current" data-id="' + entity.Id + '"><a href="/News/TypeList/' + entity.Id + '">' + entity.TypeName + '</a></li>';
                                continue;
                            }
                            tempHtml += '<li data-id="' + entity.Id + '"><a href="/News/TypeList/' + entity.Id + '">' + entity.TypeName + '</a></li>';
                        } else {
                            if (entity.Id == currentlType) {
                                tempHtml += '<li class="current" data-id="' + entity.Id + '"><a href="/News/NewIndex/' + entity.Id + '">' + entity.TypeName + '</a></li>';
                                continue;
                            }
                            tempHtml += '<li data-id="' + entity.Id + '"><a href="/News/NewIndex/' + entity.Id + '">' + entity.TypeName + '</a></li>';
                        }
                    }
                    $(".hdNav_cai").html(tempHtml);
                }
                if (data && data.ChannelList.length > 0) {
                    var tempHtml = "";
                    var currentChannelId = $("#ChannelId").val();
                    if (!currentChannelId || currentChannelId == "0") {
                        currentChannelId = data.ChannelList[0].Id;
                        $("#ChannelId").val(currentChannelId);
                    }
                    newsManager.initnews(currentChannelId);
                    newsManager.getadlist(currentChannelId);
                    for (var i = 0; i < data.ChannelList.length; i++) {
                        var item = data.ChannelList[i];
                        if (item.Id == currentChannelId) {
                            tempHtml += ' <li class="current" onclick="newsManager.ChangeChannel(' + item.Id + ')" data-id="' + item.Id + '" data-pageIndex="1" data-pageSize="30">';
                            tempHtml += '<a href="javascript:void(0)">' + item.TypeName + '</a>';
                            tempHtml += '</li>';
                        }
                        else {
                            tempHtml += ' <li onclick="newsManager.ChangeChannel(' + item.Id + ')" data-id="' + item.Id + '" data-pageIndex="1" data-pageSize="30">';
                            tempHtml += '<a href="javascript:void(0)">' + item.TypeName + '</a>';
                            tempHtml += ' </li>';
                        }
                    }
                    $(".info_hdNav_cai_lhs").html(tempHtml);
                    $(".info_hdNav_cai").html(tempHtml);
                }
                if (data && data.betDic) {
                    $("#txtIssue").html(data.betDic.Issue);
                    $("#ballListDiv").html(data.betDic.NumHtml);
                }
            },
            error: function (xhr, type) {

                $(document).dialog({
                    type: 'notice',
                    infoText: '服务器繁忙',
                    autoClose: 1500
                });
            }
        });
    },
    ChangeChannel: function (id) {
        $(".info_hdNav_cai_lhs li[data-id=" + id + "]").addClass("current").siblings().removeClass("current");
        $(".info_hdNav_cai li[data-id=" + id + "]").addClass("current").siblings().removeClass("current");
        pageIndex = 1;
        newsManager.initnews(id);
        newsManager.calcNavScoll();
    },
    calcNavScoll: function () {
        $(".info_hdNav_cai_lhs li").unbind("click");
        var index = $(".info_hdNav_cai_lhs li.current").index();
        if (index > 0) {
            var liposition = $(".info_hdNav_cai_lhs li.current").position().left;
            $(".info_hdNav_cai_lhs").scrollLeft(liposition);
        }
    },
    initnews: function (id) {
        //$("#news").html("<center>加载中......</center>");
        $.ajax({
            type: 'GET',
            url: '/News/NewsList',
            data: {
                typeId: id,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            dataType: 'html',
            success: function (data) {

                if (data && data.indexOf('div') > -1) {
                    if (data.indexOf('showLetter') > -1) {
                        $("#news").html(data);
                    }
                    else {
                        
                        if (pageIndex <= 1) {
                            data += '<div class="CK_Tiao" onclick="newsManager.showmore(' + id + ')"><a href="JavaScript:;"><span>查看下30条</span><img src="' + $("#osshost").val() + '/images/09.png"></a></div>';
                            $("#news").html(data);
                        } else {
                            $(data).insertBefore(".CK_Tiao");
                        }

                        var contentcount = $("<div>" + data + "</div>").find(".hjc_news_content").length;
                        if (contentcount < pageSize) {
                            $(".CK_Tiao").remove();
                        }
                    }
                } else {
                    $(".CK_Tiao").remove();
                }
            },
            error: function (xhr, type) {

                $(document).dialog({
                    type: 'notice',
                    infoText: '服务器繁忙',
                    autoClose: 1500
                });
            }
        });
    },
    showmore: function (id) {
        pageIndex++;
        newsManager.initnews(id);
    },
    getadlist: function (location) {
        $.get("/News/GetAdvertisementListJson",
                {
                    location: location,
                    adtype: 1
                },
               function (data) {
                   if (data.Success == true) {
                       newsManager.inadHtml(data.data);
                       //console.log(data.data);
                   } else {
                       $(document).dialog({
                           type: 'notice',
                           infoText: data.Msg,
                           autoClose: 1500
                       });
                       //alert("Data Loaded: " + data.Msg);
                   }

               });
    },
    inadHtml: function (data) {
        var cityid = $("#CityId").val();
        var city;
        var idx=-1;

        $.each(data, function (index, item){
            if(item.RestrictedAreas!=null && item.RestrictedAreas!="")
            {
                city=item.RestrictedAreas.split(',');
                idx=$.inArray(cityid.toString(),city);
            }

            if(item.RestrictedAreas==null ||item.RestrictedAreas=="" || idx==-1)
            {
                if(!IsPC())
                {
                    if(item.Layer==1)
                    {
                        $(".Plan_content").prepend(newsManager.itemhtml(item))
                    }else if(item.Layer==2)
                    {
                        $(".CZ_hdMain").after(newsManager.itemhtml(item))
                    }
                }

            }

        });
    },
    itemhtml: function (item) {
        var html = '';
        if (item.ThumbStyle == 0) {
            html = '<div class="GG_Box  GG_Box1 ">'
                      + '<a href="' + item.TransferUrl + '">'
                        + '<h3 class="GG_B1Tit">' + item.Title + '</h3>'
                       + ' <div class="newstime">'
                            + '<span>' + item.Company + '</span> <span>' + item.TimeStr + '</span>'
                          + '<span>' + item.CommentsNumber + '<i class="icon-comment"></i> </span>'
                       + '</div>'
                         + '</a>'
                            + '</div>';

        } else if (item.ThumbStyle == 1) {
            var img = '';
            if (item.ThumbList != null && item.ThumbList.length > 0) {
                img = '<img src="' + item.ThumbList[0] + '">';
            }


            html = '<div class="GG_Box GG_Box1">'
                    + '<a href="' + item.TransferUrl + '">'
                        + '<div class="GG_B2Left f-l">'
                            + '<h3 class="GG_B1Tit">' + item.Title + '</h3>'
                           + ' <div class="newstime">'
                            + '<span>' + item.Company + '</span> <span>' + item.TimeStr + '</span>'
                          + '<span>' + item.CommentsNumber + '<i class="icon-comment"></i> </span>'
                       + '</div>'
                       + '</div>'
                        + '<div class="GG_B2Right">' + img + '</div>'
                    + '</a>'
                    + '</div>';

        } else if (item.ThumbStyle == 2) {
            var img = '';
            if (item.ThumbList != null && item.ThumbList.length > 0) {
                img = '<img src="' + item.ThumbList[0] + '">';
            }
            html = '<div class="GG_Box GG_Box1">'
                        + '<a href="' + item.TransferUrl + '">'
                            + '<h3 class="GG_B1Tit">' + item.Title + '</h3>'
                           + '<div class="GG_B4Tu">' + img + '</div>'
                            + ' <div class="newstime">'
                                + '<span>' + item.Company + '</span> <span>' + item.TimeStr + '</span>'
                              + '<span>' + item.CommentsNumber + '<i class="icon-comment"></i> </span>'
                           + '</div>'
                        + '</a>'
                        + '</div>';

        } else if (item.ThumbStyle == 3) {
            var img = '';
            if (item.ThumbList != null && item.ThumbList.length > 0) {
                for (var i = 0; i < item.ThumbList.length; i++) {
                    img += '<li><img src="' + item.ThumbList[i] + '"></li>'
                }
            }

            html = '<div class="GG_Box GG_Box1">'
                        + '<a href="' + item.TransferUrl + '">'
                            + '<h3 class="GG_B1Tit">' + item.Title + '</h3>'
                            + '<ul class="GG_B3Tu">'
                                + img
                                + '<div style="clear:both;"></div>'
                            + '</ul>'

                             + ' <div class="newstime">'
                                + '<span>' + item.Company + '</span> <span>' + item.TimeStr + '</span>'
                              + '<span>' + item.CommentsNumber + '<i class="icon-comment"></i> </span>'
                           + '</div>'
                        + '</a>'
                        + '</div>';
        } else if (item.ThumbStyle == 4) {
            var img = '';
            if (item.ThumbList != null && item.ThumbList.length > 0) {
                img = '<img src="' + item.ThumbList[0] + '"  class="GG_B5Tu">';
            }
            html = '  <div class="GG_Box GG_Box1 PD3">'
           + ' <a href="' + item.TransferUrl + '">' + img + '</a>'
           + '   </div>';
        }


        return html;
    }
};