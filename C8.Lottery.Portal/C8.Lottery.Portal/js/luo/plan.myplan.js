
$(function () {
    var dh = $(window).height() - 100;
    $(".mescroll").height(dh);

    var curNavIndex = 0;
    var mescrollArr = new Array(3);
    mescrollArr[0] = initMescroll("mescroll0", "datalist0");

    $(".C8_nav3 li").click(function () {
        var _this = $(this),
            i = _this.attr("data-index");

        if (curNavIndex != i) {
            _this.addClass("current").siblings().removeClass("current");
            //隐藏当前列表和回到顶部按钮
            $("#mescroll" + curNavIndex).hide();
            mescrollArr[curNavIndex].hideTopBtn();
            //显示对应的列表
            $("#mescroll" + i).show().siblings().hide();
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
    $(".hdNav_cai li").click(function () {
        var _this = $(this),
            i = _this.attr("data-index");

        if (curNavIndex != i) {
            _this.addClass("current").siblings().removeClass("current");
            //隐藏当前列表和回到顶部按钮
            $("#mescroll" + curNavIndex).hide();
            mescrollArr[curNavIndex].hideTopBtn();
            //显示对应的列表
            $("#mescroll" + i).show().siblings().hide();
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
                callback: downCallback,
                auto: false
            },
            up: {
                callback: getListData,
                isBounce: false, //此处禁止ios回弹
                noMoreSize: 1,
                empty: {
                    icon: "/images/null.png",
                    tip: "暂无计划~",
                    //btntext: "去逛逛 >", //按钮,默认""
                    //btnClick: function() {
                    //    alertmsg("去逛逛");
                    //}
                },
                page: {
                    size: 10
                },
                clearEmptyId: clearEmptyId
            }
        });
        return mescroll;
    }

    /*加载列表数据 */
    function getListData(page) {

        var dataIndex = curNavIndex;
        getListDataFromNet(dataIndex, page.num, page.size, function (pageData, hasNextPage) {
            console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);

            buildHtml(pageData, dataIndex);
        }, function () {
            mescrollArr[dataIndex].endErr();
        });
    }
    /*下拉刷新*/
    function downCallback() {
        var dataIndex = curNavIndex;
        getListDataFromNet(dataIndex, 1, 10, function (pageData, hasNextPage) {
            //console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);

            buildHtml(pageData, dataIndex, 1);
        }, function () {
            mescrollArr[dataIndex].endErr();
        });
    }

    function getListDataFromNet(curNavIndex, pageIndex, pageSize, successCallback, errorCallback) {
        $.ajax({
            type: 'GET',
            data: {
                uid: $("#userid").val(),
                ltype: $("#ltype").val(),
                winState: curNavIndex,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            url: '/Personal/GetPlan',
            dataType: 'json',
            success: function (res) {

                if (res.IsSuccess && res.Data) {
                    //回调
                    successCallback(res.Data.PageData, res.Data.HasNextPage);

                    //alert($("#datalist0").height());

                } else {
                    alertmsg("服务器繁忙");
                }
            },
            error: errorCallback
        });
    }

});

function buildHtml(data, curNavIndex, pageindex) {

    var itemList = [];
    var listDom = $("#datalist" + curNavIndex);

    $.each(data, function (index, item) {
        var itemHtml = '<dl class="details_DL">'
            + '    <dt><a href="javascript:;"><img src="/images/' + item.LogoIndex + '.png"></a></dt>'
            + '    <dd>'
            + '    	<div class="details_ddRight">';
        if (item.WinState == 1) {
            itemHtml += '        	<a href="/Plan/Post/' + item.lType + '" class="det_bj">编辑</a>'
                + '            <h3 class="det_wkj">未开奖</h3>';
        } else if (item.WinState == 2) {
            itemHtml += '<p>已开奖</p>';
        }
        itemHtml += '        </div>'
        + '        <div class="details_ddLeft">'
        + '            <h3><a href="javascript:;">' + item.LotteryTypeName + '</a></h3>'
        + '			 <p>第' + item.Issue + '期 </p>'
        + '        </div>'
        + '    </dd>'
        + '</dl>';

        itemList.push(itemHtml);
    });

    var html = itemList.join('');
    if (pageindex == 1) {
        listDom.html(html);
    } else {
        listDom.append(html);
    }
}
