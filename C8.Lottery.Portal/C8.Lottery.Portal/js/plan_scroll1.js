var mescroll;

$(function () {

    //设置滑动区域高度
    var h = $(document).height() - 290;
    $('.mscroll').height(h);


    //
    mescroll = new MeScroll("mescroll5", {
        down: {
            auto: false, //是否在初始化完毕之后自动执行下拉回调callback; 默认true
            callback: downCallback //下拉刷新的回调
        },
        up: {
            auto: true, //是否在初始化时以上拉加载的方式自动加载第一页数据; 默认false
            isBounce: false, //此处禁止ios回弹,解析(务必认真阅读,特别是最后一点): http://www.mescroll.com/qa.html#q10
            callback: upCallback, //上拉回调,此处可简写; 相当于 callback: function (page) { upCallback(page); }
            toTop: { //配置回到顶部按钮
                src: "/images/totop.png", //默认滚动到1000px显示,可配置offset修改
                //offset : 1000
            }
        }
    });


    //
    BindQiHao_NavLiClick();


});


function BindQiHao_NavLiClick() {
    $('.QiHao_Nav li').click(function () {

        var size = $(this).attr('v');

        mescroll.setPageSize(size);
        mescroll.setPageNum(1);
        mescroll.resetUpScroll(true);

        $('.QiHao_text').slideUp(300);
        $('.QiHao_tit').html('近' + size + '期');

    });
}



/*下拉刷新的回调 */
function downCallback(page) {

    //联网加载数据
    getListDataFromNet(1, 10, function (data) {
        //联网成功的回调,隐藏下拉刷新的状态
        mescroll.endSuccess();
        //设置列表数据
        setListData(page, data, false);
    }, function () {
        //联网失败的回调,隐藏下拉刷新的状态
        mescroll.endErr();
    });
}


/*上拉加载的回调 page = {num:1, size:10}; num:当前页 从1开始, size:每页数据条数 */
function upCallback(page) {

    //联网加载数据
    getListDataFromNet(page.num, page.size, function (curPageData) {

        //方法四 (不推荐),会存在一个小问题:比如列表共有20条数据,每页加载10条,共2页.如果只根据当前页的数据个数判断,则需翻到第三页才会知道无更多数据,如果传了hasNext,则翻到第二页即可显示无更多数据.
        mescroll.endSuccess(curPageData.length);

        //设置列表数据
        setListData(page, curPageData, true);
    }, function () {
        //联网失败的回调,隐藏下拉刷新和上拉加载的状态;
        mescroll.endErr();
    });
}






function getListDataFromNet(pageNum, pageSize, successCallback, errorCallback) {
    //延时一秒,模拟联网
    setTimeout(function () {
        try {
            $.post('/Plan/PlanData', { id: lType, pageIndex: pageNum, pageSize: pageSize }, function (data) {
                //联网成功的回调
                successCallback && successCallback(data);
            });

        } catch (e) {

            //联网失败的回调
            errorCallback && errorCallback();
        }
    }, 1000);
}



/*设置列表数据*/
function setListData(page, data, isAppend) {

    if (page.num == 1) {
        $('#planList').html(data);
    } else {
        $('#planList').append(data);
    }


}






