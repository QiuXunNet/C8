$(function () {
    var dh = $(document.body).height() - $(".sion_header").height() - $(".hjc_header").height() - 10;
    $(".sion_conInfo").height(dh);
    var mescroll = new MeScroll("mescroll", {
        //上拉加载的配置项
        up: {
            callback: getListData,
            isBounce: false,
            noMoreSize: 1,
            empty: {
                icon: "/images/null.png",
                //图标,默认null
                tip: "暂无相关数据~"
            },
            page: {
                num: 0,
                //当前页 默认0,回调之前会加1; 即callback(page)会从1开始
                size: 30,
                //每页数据条数
                time: null
            },
            clearEmptyId: "dataList"
        }
    });
    /*初始化菜单*/
    var curNavIndex = 1;
    //收入明细
    $(".C8_Myyj li").click(function () {
        var i = $(this).attr("data-id");
        if (curNavIndex != i) {
            //更改列表条件
            curNavIndex = i;
            $(".C8_Myyj .current").removeClass("current");
            $(this).addClass("current");
            //重置列表数据
            mescroll.resetUpScroll();
            //隐藏回到顶部按钮
            mescroll.hideTopBtn();
        }
    });
    /*加载列表数据 */
    function getListData(page) {
        console.log(page);
        //联网加载数据
        var dataIndex = curNavIndex;
        getListDataFromNet(dataIndex, page.num, page.size, function (pageData, hasNextPage) {
            //console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            //设置列表数据
            mescroll.endSuccess(pageData.length, hasNextPage);
            buildHtml(pageData, dataIndex);
        }, function () {
            //隐藏下拉刷新和上拉加载的状态;
            mescroll.endErr();
        });
    }
    function getListDataFromNet(Type, pageIndex, pageSize, successCallback, errorCallback) {
        $.ajax({
            type: "GET",
            data: {
                Type: Type,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async: false,
            url: "/Personal/MyCommissionList",
            dataType: "json",
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
    var listDom = $("#dataList");
    $.each(data, function (index, item) {
        var moneyhtml = "<p>+" + formatCurrency(item.Money) + "</p>";
        var timehtml = " <p>" + item.UserName + " " + getDateTime(item.SubTime, 1) + " </p>";
        var h3html = "";
        if (curNavIndex == 2) {
            var text = "";
            if (item.State == 1) {
                text = "提现中";
            } else if (item.State == 2) {
                text = "提现失败";
            } else if (item.State == 3) {
                text = "已到账";
            }
            moneyhtml = '<p><strong class="sion_strH">' + text + "</strong></p>";
            timehtml = " <p>" + getDateTime(item.SubTime, 2) + "</p>";
        }
        if (curNavIndex == 1) {
            h3html = item.Issue + "期";
        } else {
            h3html = formatCurrency(item.Money);
        }
        var itemHtml = '<dl class="details_DL"> <dt><a href="javascript:;"><img src="' + item.LotteryIcon + '"></a></dt>' + '<dd>  <div class="details_ddRight">' + moneyhtml + "</div>" + '  <div class="details_ddLeft"> <h3><a href="javascript:;"><b>' + h3html + "</b></a></h3>" + timehtml + "  </div> </dd> </dl>";
        itemList.push(itemHtml);
    });
    var html = itemList.join("");
    listDom.append(html);
}

/* 货币格式化*/
function formatCurrency(num) {
    num = num.toString().replace(/\$|\,/g, "");
    if (isNaN(num)) num = "0";
    sign = num == (num = Math.abs(num));
    num = Math.floor(num * 100 + .50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10) cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) num = num.substring(0, num.length - (4 * i + 3)) + "," + num.substring(num.length - (4 * i + 3));
    return (sign ? "" : "-") + num + "." + cents;
}

//日期时间处理
function conver(s) {
    return s < 10 ? "0" + s : s;
}

/* 获取日期时间格式*/
function getDateTime(dateStr, type) {
    var date = eval("new " + dateStr.substr(1, dateStr.length - 2));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    if (type == 1) {
        return conver(month) + "-" + conver(day) + " " + conver(hh) + ":" + conver(mm) + ":" + conver(ss);
    } else {
        return conver(year) + "-" + conver(month) + "-" + conver(day) + " " + conver(hh) + ":" + conver(mm) + ":" + conver(ss);
    }
}