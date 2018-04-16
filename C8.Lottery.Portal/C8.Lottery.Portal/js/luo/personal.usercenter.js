
$(function() {
    var h = $(window).height() - 320;
    $('.mescroll').height(h);

    var curNavIndex = 0;
    var mescrollArr = new Array(3);
    mescrollArr[0] = initMescroll("mescroll0", "datalist0");

    $(".C8_Trzy li").click(function() {
        var _this = $(this),
            i = _this.attr("data-index");

        if (curNavIndex != i) {
            _this.addClass("current").siblings().removeClass("current");
            //隐藏当前列表和回到顶部按钮
            $("#mescroll" + curNavIndex).hide();
            mescrollArr[curNavIndex].hideTopBtn();
            //显示对应的列表
            $("#mescroll" + i).show();
            //取出菜单所对应的mescroll对象,如果未初始化则初始化
            if (mescrollArr[i] == null) {
                mescrollArr[i] = initMescroll("mescroll" + i, "datalist" + i);
            } else {
                //检查是否需要显示回到到顶按钮
                var curMescroll = mescrollArr[i];
                var curScrollTop = curMescroll.getScrollTop();
                if (curScrollTop >= curMescroll.optUp.toTop.offset) {
                    curMescroll.showTopBtn();
                } else {
                    curMescroll.hideTopBtn();
                }
            }
            //更新标记
            curNavIndex = i;
        }

    });

    /*创建MeScroll对象*/
    function initMescroll(mescrollId, clearEmptyId) {

        var mescroll = new MeScroll(mescrollId, {
            //上拉加载的配置项
            down: {
                use:false
            },
            up: {
                callback: getListData,
                isBounce: false, //此处禁止ios回弹
                noMoreSize: 1,
                empty: {
                    icon: "/images/null.png",
                    tip: "暂无相关数据~",
                    //btntext: "去逛逛 >", //按钮,默认""
                    //btnClick: function() {
                    //    alertmsg("去逛逛");
                    //}
                },
                clearEmptyId: clearEmptyId
            }
        });
        return mescroll;
    }

    /*加载列表数据 */
    function getListData(page) {
        //联网加载数据
        var dataIndex = curNavIndex; //记录当前联网的nav下标,防止快速切换时,联网回来curNavIndex已经改变的情况;
        getListDataFromNet(dataIndex, page.num, page.size, function(pageData, hasNextPage) {
            console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);

            //设置列表数据
            buildHtml(pageData, dataIndex, page.num);
        }, function() {
            //隐藏下拉刷新和上拉加载的状态;
            mescrollArr[dataIndex].endErr();
        });
    }
    /*下拉刷新*/
    function downCallback() {
        var dataIndex = curNavIndex;
        getListDataFromNet(dataIndex, 1, 10, function(pageData, hasNextPage) {
            //console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);

            //设置列表数据
            buildHtml(pageData, dataIndex,1);
        }, function() {
            //隐藏下拉刷新和上拉加载的状态;
            mescrollArr[dataIndex].endErr();
        });
    }

    function getListDataFromNet(curNavIndex, pageIndex, pageSize, successCallback, errorCallback) {
        var url;
        if (curNavIndex == 0) {
            url = "/Personal/GetPlan";
        } else if (curNavIndex == 1) {
            url = "/Personal/GetDenamic";
        } else if (curNavIndex == 2) {
            url = "/Personal/GetVisitRecord";
        } else {
            return;
        }
        $.ajax({
            type: 'GET',
            data: {
                uid: $("#userid").val(),
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async:false,
            url: url,
            dataType: 'json',
            success: function(res){

                if (res.IsSuccess && res.Data) {
                    //回调
                    successCallback( res.Data.PageData,res.Data.HasNextPage);

                } else {
                    alertmsg("服务器繁忙");
                }
            },
            error: errorCallback
        });
    }

});

function buildHtml(data, tabtype,pageIndex) {

    var itemList = [];
    var listDom=$("#datalist"+tabtype);
    if (tabtype == 0) {
        $.each(data, function (index, item) {
            var itemHtml = '<dl class="Other_TIfDL">'
                + '<dt><a href="javascript:;"><img src="/images/' + item.LogoIndex + '.png"></a></dt>'
                + '<dd>';
            if (item.WinState == 1) {
                itemHtml += '<div class="Other_TIR"><p><strong class="sion_strH">未开奖</strong></p></div>';
            } else if (item.WinState == 2) {
                itemHtml += '<div class="Other_TIR"><p>已开奖</p></div>';
            }
            itemHtml += '<div class="Other_TIddL">'
                + '<h3><a href="javascript:;">' + item.LotteryTypeName + '</a></h3>'
                + '<p>第' + item.Issue + '期  ' + item.TimeStr + '</p>'
                + '</div>'
                + '</dd>'
                + '</dl>';

            itemList.push(itemHtml);
        });
    } else if (tabtype == 1) {
        $.each(data, function (index, item) {
            var itemHtml = '<div class="TID_Info">'
                + '<div class="TID_Ifk1">'
                + '	<div class="Ifk1_left f-l"><a href="javascript:;"><img src="' + item.Avater + '"></a></div>'
                + '	<div class="Ifk1_right">'
                + '		<h3><a href="javascript:;">' + item.NickName + '</a></h3>'
                + '		<p>' + item.SubTimeStr + '</p>'
                + '	</div>'
                + '</div>'
                + '<div class="TID_Ifk2">'
                + '	<div class="TID_Ifk2A">'
                + '		<p>' + item.Content + '</p>'
                + '	</div>';

            var parentComment = item.ParentComment;
            var denamicType = item.Type == 1 ? "计划" : "资讯";
            if (parentComment) {
                itemHtml += '	<div class="TID_Ifk2B">'
                    + '		<p>' + parentComment.NickName + '：' + parentComment.Content + '</p>'
                    + '	</div>'
                    + '</div>'
                    + '<div class="TID_Ifk3">'
                    + '	<P><font>' + parentComment.NickName + '发布于</font><span>' + item.LotteryTypeName + '</span><span>' + denamicType + '</span></P>'
                    + '</div>'
                    + '</div>';
            } else {

                itemHtml += '</div>'
                    + '<div class="TID_Ifk3">'
                    + '	<P><font>他发布于</font><span>' + item.LotteryTypeName + '</span><span>' + denamicType + '</span></P>'
                    + '</div>'
                    + '</div>';
            }
            itemHtml = $.stringParseEmoji(itemHtml);
            itemList.push(itemHtml);
        });
    } else if (tabtype == 2) {
        $.each(data, function (index, item) {
            var itemHtml = '<dl class="Other_TIfDL">'
            + '    <dt><a href="javascript:;"><img src="' + item.Avater + '" alt=""></a></dt>'
            + '    <dd>'
            + '        <div class="Other_TIR"><p><strong class="sion_strH">访问了他' + item.ModuleName + '</strong></p></div>'
            + '        <div class="Other_TIddL">'
            + '            <h3><a href="javascript:;">' + item.NickName + '</a></h3>'
            + '            <p>' + item.VisitTimeStr + '</p>'
            + '        </div>'
            + '    </dd>'
            + '</dl>';
            itemList.push(itemHtml);
        });
    }

    var html = itemList.join('');
    if (pageIndex == 1) {
        listDom.html(html);
    } else {
        listDom.append(html);
    }
}

function follow(userid) {
    var _this = $("#follow"),
        type = _this.attr("data-type"),
        url = type === "1" ? "/Personal/UnFollow" : "/Personal/IFollow";
    $.post(url, { followed_userId: userid }, function (data) {

        if (!data.Success) {
            alertmsg(data.Msg);
        } else {
            if (type === "1") {
                $("#follow").attr("data-type", "0").html("关注");
            } else {

                $("#follow").attr("data-type", "1").html("已关注");
            }
        }
    });
}