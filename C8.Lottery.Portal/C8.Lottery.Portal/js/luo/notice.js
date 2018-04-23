$(function () {

    var dh = $(document).height() -200;
    $(".details_box").height(dh);

    var curNavIndex = 0;
    var mescrollArr = new Array(3);
    mescrollArr[0]=initMescroll("mescroll0", "datalist0");

    $(".C8_JIlU li").click(function () {
        var _this = $(this),
            i = $(".C8_JIlU li").index(_this);

        if (curNavIndex != i) {
            _this.addClass("current").siblings().removeClass("current");
            //隐藏当前列表和回到顶部按钮
            $("#mescroll"+curNavIndex).hide();
            mescrollArr[curNavIndex].hideTopBtn();
            //显示对应的列表
            $("#mescroll"+i).show();
            //取出菜单所对应的mescroll对象,如果未初始化则初始化
            if(mescrollArr[i]==null){
                mescrollArr[i]=initMescroll("mescroll"+i, "datalist"+i);
            }else{
                //检查是否需要显示回到到顶按钮
                var curMescroll=mescrollArr[i];
                var curScrollTop=curMescroll.getScrollTop();
                if(curScrollTop>=curMescroll.optUp.toTop.offset){
                    curMescroll.showTopBtn();
                }else{
                    curMescroll.hideTopBtn();
                }
            }
            //更新标记
            curNavIndex=i;
        }

    });

    /*创建MeScroll对象*/
    function initMescroll(mescrollId,clearEmptyId){

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
                clearEmptyId: clearEmptyId
            }
        });
        return mescroll;
    }

    /*加载列表数据 */
    function getListData(page){
        //联网加载数据
        var dataIndex=curNavIndex; //记录当前联网的nav下标,防止快速切换时,联网回来curNavIndex已经改变的情况;
        getListDataFromNet(dataIndex, page.num, page.size, function(data,hasNextPage) {
            var pageData = data.PageData;
            var extraData = data.ExtraData;
            console.log("dataIndex="+dataIndex+", curNavIndex="+curNavIndex+", page.num="+page.num+", page.size="+page.size+", pageData.length="+pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);

            //设置列表数据
            buildHtml(pageData, dataIndex,extraData);
        }, function(){
            //隐藏下拉刷新和上拉加载的状态;
            mescrollArr[dataIndex].endErr();
        });
    }

    function getListDataFromNet(curNavIndex,pageIndex,pageSize,successCallback,errorCallback)
    {
        var url;
        if (curNavIndex == 0) {
            url = "/Personal/GetCommentNotice";
        } else if (curNavIndex == 1) {
            //查询系统消息
            url = "/Personal/GetSysMessage";
        } else {
            return;
        }
        $.ajax({
            type: 'GET',
            data: {
                uid: $("#userid").val()||0,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async:false,
            url: url,
            dataType: 'json',
            success: function(res){

                if (res.IsSuccess && res.Data) {
                    //回调
                    successCallback( res.Data,res.Data.HasNextPage);

                } else {
                    alertmsg("服务器繁忙");
                }
            },
            error: errorCallback
        });
    }

});

function buildHtml(data, curNavIndex, extraData) {
    console.log(data);
    //设置未读消息
    if (extraData ==null || extraData.Unread <= 0) {
        $("#liNav" + curNavIndex + " i").remove();
    } else {
        $("#liNav" + curNavIndex + " i").html(extraData.Unread);
    }
    var itemList = [];
    var listDom=$("#datalist"+curNavIndex);
    if (curNavIndex == 0) {

        $.each(data, function (index, item) {

            var itemHtml = '<div class="TID_Info">'
                + '<div class="TID_Ifk1">'
                + '	<div class="Ifk1_left f-l"><a href="javascript:;"><img src="' + item.FromAvater + '"></a></div>'
                + '	<div class="Ifk1_right">'
                + '		<h3><a href="/Comment/Index/'+item.ArticleId+'?ctype=2&type='+item.CommentType+'" class="f-r">回复</a><a href="javascript:;">' + item.FromNickName + '</a></h3>'
                + '		<p>' + item.SubTimeStr + '</p>'
                + '	</div>'
                + '</div>'
                + '<div class="TID_Ifk2">'
                + '	<div class="TID_Ifk2A">'
                + '		<p>' + item.Content + '</p>'
                + '	</div>';

            var parentComment = item.MyContent;
            var denamicType = item.CommentType == 1 ? "计划" : "资讯";
            if (parentComment!=null && parentComment.length>0) {
                itemHtml += '	<div class="TID_Ifk2B">'
                    + '		<p>我的评论：' + parentComment + '</p>'
                    + '	</div>'
                    + '</div>'
                    + '<div class="TID_Ifk3">'
                    + '	<P><font>发布于</font><span>' + item.LotteryTypeName + '</span><span>' + denamicType + '</span></P>'
                    + '</div>'
                    + '</div>';
            } else {

                itemHtml += '</div>'
                    + '<div class="TID_Ifk3">'
                    + '	<P><font>我发布于</font><span>' + item.LotteryTypeName + '</span><span>' + denamicType + '</span></P>'
                    + '</div>'
                    + '</div>';
            }
            itemHtml = $.stringParseEmoji(itemHtml);
            itemList.push(itemHtml);
        });
    } else if (curNavIndex == 1) {
        $.each(data, function (index, item) {
            //拼接系统消息
            var itemHtml = '<div class="TID_Info">'
                + '    <div class="TID_Ifk1">'
                + '        <div class="Ifk1_left f-l">'
                + '            <a href="javascript:;"><img src="/images/58.png"></a>'
                + '        </div>'
                + '        <div class="Ifk1_right">'
                + '            <h3>';
            if (item.Link && item.Link.length > 0) {
                itemHtml += '                <a href="' + item.Link + '">' + item.Title + '</a>';
            } else {
                itemHtml += '                <a href="javascript:;">' + item.Title + '</a>';
            }
            itemHtml+='            </h3>'
            +'            <p>'+ item.SubTimeStr+'</p>'
            +'        </div>'
            +'    </div>'
            +'    <div class="TID_Ifk2">'
            +'        <div class="TID_Ifk2A">'
            +'            <p>'+item.Content+'</p>'
            +'        </div>'
            +'    </div>'
            +'</div>';
            itemList.push(itemHtml);
        });
    }

    var html = itemList.join('');
    listDom.append(html);
}
