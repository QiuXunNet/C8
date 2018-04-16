var curNavIndex = 0;
var playType = 0;
var playName = '';
var queryType = 1;
$(function () {
    var h = $(window).height() - 177;
    $('#mescroll0,#mescroll1').height(h);

    var mescroll = new MeScroll("mescroll0", {
        down: {
            auto: false,
            callback: downCallback
        },
        up: {
            callback: getListData,
            isBounce: false, //此处禁止ios回弹
            noMoreSize: 5,
            empty: {
                icon: "/images/null.png",
                tip: "暂无高手推荐~",
                btntext: "去看看免费专家 >", //按钮,默认""
                btnClick: function () {
                    $(".C8_nav5 li:eq(2)").trigger("click");
                    //alertmsg("点击免费专家切换");
                }
            },
            clearEmptyId: "datalist0",
            toTop: {
                src: "/images/totop.png",
                offset: 1000
            },
            page: {
                num: 0,
                size: 20,
                time: null
            }
        }
    });
    var mescrol2;
    setTimeout(function () {
        queryType = 2;
        mescrol2 = new MeScroll("mescroll1", {
            down: {
                auto: false,
                callback: downCallback
            },
            up: {
                callback: getListData,
                isBounce: false, //此处禁止ios回弹
                noMoreSize: 5,
                empty: {
                    icon: "/images/null.png",
                    tip: "暂无免费专家~",
                    btntext: "去看看高手推荐 >",
                    btnClick: function () {
                        $(".C8_nav5 li:eq(1)").trigger("click");
                        //alertmsg("点击高手推荐切换");
                    }
                },
                clearEmptyId: "datalist1",
                toTop: {
                    src: "/images/totop.png",
                    offset: 1000
                },
                page: {
                    num: 0,
                    size: 20,
                    time: null
                }
            }
        });
    }, 2000);

    $(".GS_nav li").click(function () {
        var _this = $(this),
            i = _this.attr("data-index");
        queryType = $("#curNavIndex").val();

        if (!queryType || queryType < 1) return;

        if (playType != i) {
            playType = i;
            _this.addClass("current").siblings().removeClass("current");

            if (queryType == 1) {
                mescroll.resetUpScroll();
                //隐藏回到顶部按钮
                mescroll.hideTopBtn();
            } else if (queryType == 2) {

                mescrol2.resetUpScroll();
                //隐藏回到顶部按钮
                mescrol2.hideTopBtn();
            }
        }
    });

    /*加载列表数据 */
    function getListData(page) {

        getListDataFromNet(playType, queryType, page.num, page.size, function (pageData, hasNextPage) {
            console.log("playType=" + playType + ",queryType=" + queryType + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);

            if (queryType == 1) {
                mescroll.endSuccess(pageData.length, hasNextPage);
                //设置高手推荐列表数据
                buildHtml(pageData, 0, page.num);
            } else if (queryType == 2) {
                mescrol2.endSuccess(pageData.length, hasNextPage);
                //设置高手推荐列表数据
                buildHtml(pageData, 1, page.num);
            }

        }, function () {
            //隐藏下拉刷新和上拉加载的状态;
            if (queryType == 1) {
                mescroll.endErr();
            } else if (queryType == 2) {
                mescrol2.endErr();
            }
        });
    }

    /*下拉刷新*/
    function downCallback() {
        getListDataFromNet(playType, queryType, 1, 20, function (pageData, hasNextPage) {
            if (queryType == 1) {
                mescroll.endSuccess(pageData.length, hasNextPage);
                //设置高手推荐列表数据
                buildHtml(pageData, 0, 1);
            } else if (queryType == 2) {
                mescrol2.endSuccess(pageData.length, hasNextPage);
                //设置高手推荐列表数据
                buildHtml(pageData, 1, 1);
            }
        }, function () {
            //隐藏下拉刷新和上拉加载的状态;
            if (queryType == 1) {
                mescroll.endErr();
            } else if (queryType == 2) {
                mescrol2.endErr();
            }
        });
    }

    function getListDataFromNet(playType, queryType, pageIndex, pageSize, successCallback, errorCallback) {
        var url = "/Plan/ExpertList";
        $.ajax({
            type: 'GET',
            data: {
                lType: $("#lType").val(),
                playName: $("#gsnav li[data-index='" + playType + "']").attr("data-title"),
                type: queryType,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            url: url,
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

function buildHtml(data, tabtype,pageIndex) {

    var itemList = [];
    var listDom = $("#datalist" + tabtype);

    $.each(data, function (index, item) {
        var itemHtml = '<div class="CIFB_K">'
            + '<div class="CIFB_KL f-l">'
            + '<div class="KL_number f-l">';
        if (item.RowNumber <= 3) {
            itemHtml += '<div class="KL_nbqs">'
                + '<img src="/images/44_' + item.RowNumber + '.png">'
                + '</div>';
        } else {
            itemHtml += '<span class="KL_nbsp">' + item.RowNumber + '</span>';
        }
        itemHtml += '</div>'
            + '<div class="KL_port f-l">'
            + '<div class="port_tu">'
            + '<a href="/Plan/PlayRecord/' + item.lType + '?uid=' + item.UserId + '"><img src="' + (item.Avater.length < 1 ? "/images/default_avater.png" : item.Avater) + '" /></a>'
            + '</div>';
        if (item.RowNumber <= 3) {
            itemHtml += '<i class="port_sf"><img src="/images/66.png"></i>';
        }

        itemHtml += '</div>'
            + '            <div class="KL_font">'
            + '                <h3 class="KL_fh3On">'
            + '                    <a href="/Plan/PlayRecord/' + item.lType + '?uid=' + item.UserId + '">' + item.Name + '</a>'
            + '                </h3>'
            //+ '                <p>1000次查看</p>'
            + '            </div>'
            + '        </div>'
            + '        <div class="CIFB_KR">'
            + '            <div class="KR_L f-l">'
            + '                <h3>' + item.HitRateDesc + '</h3>'
            + '                <p>最大连中' + item.MaxWin + '期</p>'
            + '            </div>'
            + '            <div class="KR_R">'
            + '                <h3>'
            + '                    <span>' + item.PlayTotalScore + '</span></i>'
            + '                </h3>'
            + '                <p>上期' + (item.LastWin ? "中" : "未中") + '</p>'
            + '            </div>'
            + '        </div>'
            + '    </div>';

        itemList.push(itemHtml);
    });

    var html = itemList.join('');
    if (pageIndex == 1) {
        listDom.html(html);
    } else {
        listDom.append(html);
    }
}