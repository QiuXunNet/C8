﻿$(function () {
    var curNavIndex = 1;
    var mescrollArr = new Array(3);
    mescrollArr[curNavIndex] = initMescroll("mescroll1", "dataList1");

    function pager(obj) {
        var _this = $(obj),
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
                mescrollArr[i] = initMescroll("mescroll" + i, "dataList" + i);
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
    $(".C8_JIlU li").click(function () {
        var bang = $(this).index();
        $(".details_box .details_info1").eq(bang).show().siblings().hide();
        pager($(this));
    });

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
                    tip: "暂无交易记录~",
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

    function getListDataFromNet(Type, pageIndex, pageSize, successCallback, errorCallback) {
        $.ajax({
            type: 'GET',
            data: {
                Type: Type,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async: false,
            url: '/Personal/RecordList',
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
/* 获取日期时间格式*/
function getDateTime(dateStr, type) {
    var date = eval('new ' + dateStr.substr(1, dateStr.length - 2));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    if (type == 2) {
        return month + "-" + day + " " + hh + ":" + mm + ":" + ss;
    } else {
        return year + "-" + month + "-" + day + " " + hh + ":" + mm + ":" + ss;
    }
}

function buildHtml(data, curNavIndex) {
    console.log(data);
    var itemList = [];
    var listDom = $("#dataList" + curNavIndex);
    $.each(data, function (index, item) {
        var moneyhtml = '<p>+' + item.Money + '</p></div>';
        var timehtml = '<p>' + getDateTime(item.SubTime, curNavIndex) + '</p>';
        var text = "";
        if (curNavIndex == 2) {
            text = item.UserName;
            moneyhtml = '<p>' + (item.Type == 3 ? "点阅" : "打赏") + ' -' + item.Money + '</p></div>';
            timehtml = '<p>' + item.Issue + '期' + getDateTime(item.SubTime, curNavIndex) + '</p>'
        } else if (curNavIndex == 1) {
            if (item.PayType == 1) {
                text = "微信充值";
            } else if (item.PayType == 2) {
                text = "支付宝充值";
            } else if (item.PayType == 3) {
                text = "银联充值";
            }
        } else if (curNavIndex == 3) {
            if (item.OrderId == 100) {
                text = "充值任务奖励";
            } else if (item.OrderId == 105) {
                text = "邀请成功注册奖励";
            }
        }
        var itemHtml = '<dl class="details_DL">' + '<dt><a href="javascript:;"><img src="' + item.LotteryIcon + '"></a></dt><dd>' + '<div class="details_ddRight">' + moneyhtml + '</div>' + '<div class="details_ddLeft">' + '<h3><a href="javascript:;"><b>' + text + '</b></a></h3>' + timehtml + '</div> </dd></dl>'
        itemList.push(itemHtml);
    });
    var html = itemList.join('');
    listDom.append(html);
}