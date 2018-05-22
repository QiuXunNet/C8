$(function () {
    var curNavIndex = 1;
    var mescrollArr = new Array(17);
    var dh = $(document).height() - 180;
    $(".guessing_content").height(dh);
    mescrollArr[curNavIndex] = initMescroll("mescroll1", "datalist1");

    function pager(obj) {
        var _this = $(obj),
            i = _this.attr("data-index");
        console.log(i);
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
    }
    $(".hdNav_cai_lhs li").click(function () {
        pager($(this));
    });
    $(".hdNav_cai li").click(function () {
        pager($(this));
    });

    function initMescroll(mescrollId, clearEmptyId) {
        var mescroll = new MeScroll(mescrollId, {
            //上拉加载的配置项
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
                page: {
                    num: 0, //当前页 默认0,回调之前会加1; 即callback(page)会从1开始
                    size: 30, //每页数据条数
                    time: null //加载第一页数据服务器返回的时间; 防止用户翻页时,后台新增了数据从而导致下一页数据重复;
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
        getListDataFromNet(dataIndex, page.num, page.size, function (pageData, hasNextPage) {
            //console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);
            //设置列表数据
            buildHtml(pageData, dataIndex);
        }, function () {
            //隐藏下拉刷新和上拉加载的状态;
            mescrollArr[dataIndex].endErr();
        });
    }

    function getListDataFromNet(ltype, pageIndex, pageSize, successCallback, errorCallback) {
        $.ajax({
            type: 'GET',
            data: {
                PId: ltype,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async: false,
            url: '/Personal/GetBet',
            dataType: 'json',
            success: function (res) {
                if (res.IsSuccess && res.Data) {
                    //回调
                    successCallback(res.Data.PageData, res.Data.HasNextPage);
                } else {
                    alertmsg("服务器繁忙");
                }
            },
            error: errorCallback
        });
    }
});

function buildHtml(data, curNavIndex) {
    var itemList = [];
    var listDom = $("#datalist" + curNavIndex);
    $.each(data, function (index, item) {
        var itemHtml = '<dl class="guess_Dl"> <dt><a href="javascript:;"><img src="' + item.LotteryIcon + '"></a></a></dt>' + '<dd><a href="/Plan/Post/' + item.lType + '"> <span class="guess_icon_right"></span>  <div class="guess_Dlleft"> <h3>' + item.Name + '</h3>  <p>' + item.Score + '</p>   </div>' + ' </a></dd> </dl>';
        itemList.push(itemHtml);
    });
    var html = itemList.join('');
    listDom.append(html);
}