﻿$(function () {
    var id = $(".hdNav_cai_lhs .current").attr("data-id");
    var index = $("#ul_" + id + " .current").attr("data-type-id");
    var idx = $("#gul_" + id + " .current").attr("data-idx");
    var length = $(".wrapper-container .Grades").length;
    var dataname = "全部";
    var _ltype = index;
    $(".wrapper-container .Grades_" + index).show().siblings().hide();
    var dh = $(document.body).height() - $(".gaoshou").height() - $(".Grades_TopNav").height() - 90;
    $(".Grades_CBInfo").height(dh);
    var curNavIndex = "110";
    var mescrollArr = new Array(length);
    mescrollArr[curNavIndex] = initMescroll("mescroll_" + idx, "datalist_" + idx);
    $(".hdNav_cai_lhs li").on("click", function () {
        var _this = $(this);
        var _index = _this.attr("data-id");
        //$("#lottery_type ul_" + _index).hide();
        //var id = $(".hdNav_cai_lhs .current").attr("data-id");
        var id = _index;
        var gid = $("#ul_" + id + " .current").attr("data-type-id");
        var cid = $("#ul_" + id + " .current").attr("data-cid");
        if (gid == undefined) {
            $(".wrapper-container .Grades").hide();
        } else {
            $(".wrapper-container .Grades_" + id + gid).show().siblings().hide();
        }
        pager($(".wrapper-container .Grades_" + id + gid + " .current"));
    });
    $(".hdNav_cai li").on("click", function () {
        var _this = $(this);
        var _index = _this.attr("data-id");
        //$("#lottery_type ul_" + _index).hide();
        //id = $(".hdNav_cai_lhs .current").attr("data-id");
        var id = _index;
        var gid = $("#ul_" + id + " .current").attr("data-type-id");
        var cid = $("#ul_" + id + " .current").attr("data-cid");
        if (gid == undefined) {
            $(".wrapper-container .Grades").hide();
        } else {
            $(".wrapper-container .Grades_" + id + gid).show().siblings().hide();
        }
        pager($(".wrapper-container .Grades_" + id + gid + " .current"));
    });
    $(".GS_box li").on("click", function () {
        var _this = $(this);
        var _index = _this.attr("data-type-id");
        var cid = _this.attr("data-cid");
        _this.addClass("current").siblings().removeClass("current");
        $(".wrapper-container .Grades_" + cid + _index).show().siblings().hide();
        pager($(".wrapper-container .Grades_" + cid + _index + " .current"));
    });
    $(".wrapper-container li").on("click", function () {
        var _this = $(this);
        var _index = _this.attr("data-ltype");
        var cid = _this.attr("data-cid");
        _this.addClass("current").siblings().removeClass("current");
        $(".wrapper-container .Grades_" + cid + _index).show().siblings().hide();
        pager(_this);
    });
    function pager(obj) {
        var _this = $(obj), i = _this.attr("data-idx");
        _ltype = _this.attr("data-ltype");
        dataname = _this.attr("data-name");
        if (curNavIndex != i) {
            _this.addClass("current").siblings().removeClass("current");
            //隐藏当前列表和回到顶部按钮
            $("#mescroll_" + i).hide();
            mescrollArr[curNavIndex].hideTopBtn();
            //显示对应的列表
            $("#mescroll_" + i).show().siblings().hide();
            //取出菜单所对应的mescroll对象,如果未初始化则初始化
            if (mescrollArr[i] == null) {
                mescrollArr[i] = initMescroll("mescroll_" + i, "datalist_" + i);
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
    function getListDataFromNet(lType, PlayName, pageIndex, pageSize, successCallback, errorCallback) {
        $.ajax({
            type: "GET",
            data: {
                ltype: lType,
                PlayName: PlayName,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            async: false,
            url: "/Personal/GetMyBet",
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
    function initMescroll(mescrollId, clearEmptyId) {
        var mescroll = new MeScroll(mescrollId, {
            //上拉加载的配置项
            down: {
                use: false
            },
            up: {
                callback: getListData,
                isBounce: false,
                //此处禁止ios回弹
                noMoreSize: 5,
                empty: {
                    icon: "/images/null.png",
                    tip: "暂无相关数据~"
                },
                page: {
                    num: 0,
                    //当前页 默认0,回调之前会加1; 即callback(page)会从1开始
                    size: 30,
                    //每页数据条数
                    time: null
                },
                clearEmptyId: clearEmptyId
            }
        });
        return mescroll;
    }
    /*加载列表数据 */
    function getListData(page) {
        var ltype = _ltype;
        var palyname = dataname;
        //联网加载数据
        //记录当前联网的nav下标,防止快速切换时,联网回来curNavIndex已经改变的情况;
        var dataIndex = curNavIndex;
        getListDataFromNet(ltype, palyname, page.num, page.size, function (pageData, hasNextPage) {
            //console.log("dataIndex=" + dataIndex + ", curNavIndex=" + curNavIndex + ", page.num=" + page.num + ", page.size=" + page.size + ", pageData.length=" + pageData.length);
            mescrollArr[dataIndex].endSuccess(pageData.length, hasNextPage);
            //设置列表数据
            buildHtml(pageData, dataIndex, ltype);
        }, function () {
            //隐藏下拉刷新和上拉加载的状态;
            mescrollArr[dataIndex].endErr();
        });
    }
});

function buildHtml(data, curNavIndex, ltype) {
    if (curNavIndex == 0) {
        curNavIndex = curNavIndex + ltype;
    }
    var itemList = [];
    var listDom = $("#datalist_" + curNavIndex);
    $.each(data, function (index, item) {
        var kjhtml = "";
        var kjhtml1 = "";
        var num = item.LotteryNum.Num.split(",");
        var dttime = item.LotteryNum.SubTime;
        //针对6彩
        var bethtml = "";
        var cl1 = "hdM_ddcon ";
        if (item.BettingRecord.length > 0) {
            for (var j = 0; j < item.BettingRecord.length; j++) {
                var iswin = item.BettingRecord[j].WinState == 3 ? "中" : "不中";
                var cliswin = item.BettingRecord[j].WinState == 3 ? "Forecast_Pred" : "";
                var issym = item.BettingRecord[j].WinState == 3 ? "+" : "";
                var redwin = "";
                var mynum = item.BettingRecord[j].BetNum;
                if (item.BettingRecord[j].WinState == 3) {
                    var mynum = item.BettingRecord[j].BetNum;
                    var mynumsplit = item.BettingRecord[j].BetNum.split(",");
                    for (var x = 0; x < mynumsplit.length; x++) {
                        var z = getwanfa(item.BettingRecord[j].PlayName);
                        if (z == -1) {
                            var ltnum = item.LotteryNum.Num;
                            if (item.BettingRecord[j].PlayName == "平特一肖") {
                                // ltnum = getShengxiaoByDigit(item.LotteryNum.Num);
                                ltnum = get6caiSx(ltnum, dttime);
                                console.log(ltnum);
                            }
                            if (ltnum.indexOf(mynumsplit[x]) >= 0) {
                                mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                            }
                        } else {
                            var lonum = item.LotteryNum.Num.split(",");
                            if (item.BettingRecord[j].PlayName == "四肖中特" || item.BettingRecord[j].PlayName == "六肖中特") {
                                //lonum[z] = getShengxiaoByDigit(lonum[z]);
                                lonum[z] = GetName(dttime, lonum[z]);
                            }
                            if (z == 10) {
                                var lonum1 = lonum.slice(0, 6);
                                if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                    mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                }
                            } else if (z == 11) {
                                var lonum1 = lonum.slice(0, 5);
                                if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                    mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                }
                            } else if (z == 12) {
                                var lonum1 = lonum.slice(5);
                                if ($.inArray(mynumsplit[x], lonum1) != -1) {
                                    mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                }
                            } else {
                                if (lonum[z] == mynumsplit[x]) {
                                    console.log("中了");
                                    mynum = mynum.replace(mynumsplit[x], "<span class='For_Red'>" + mynumsplit[x] + "</span>");
                                }
                            }
                        }
                    }
                }
                bethtml += '<tr><td align="center" valign="middle" width="25%"><p>' + item.BettingRecord[j].PlayName + "</p> </td>" + ' <td align="center" valign="middle"  width="25%">  <p style="word-break:break-all; ">' + mynum + '<span class="For_Red"></span></p> </td>' + '<td align="center" valign="middle" width="25%"> <p class="' + cliswin + '">    <font class="P_TF">' + iswin + "</font><span>" + issym + "" + item.BettingRecord[j].Score + "</span></p>  </td> </tr>";
            }
        }
        kjhtml1 = "";
        for (var i = 0; i < num.length; i++) {
            var cl = "hdM_spssc HYJP_Red";
            //   console.log(item.BettingRecord[i].lType);
            if (ltype == 5) {
                cl1 = "Forecast_LHC";
                var color = getcolor(num[i]);
                if (color == "red") {
                    cl = "LHC_spA LHC_spAred";
                } else if (color == "green") {
                    cl = "LHC_spA LHC_spAgreen";
                } else {
                    cl = "LHC_spA LHC_spAblue";
                }
                //   kjhtml1 += '<span class="LHC_spA">' + num[i] + '</span>'
                if (i == num.length - 2) {
                    //kjhtml1 += '<span class="LHC_spA">' + getShengxiaoByDigit(num[i]) + '+</span>'
                    kjhtml1 += '<span class="LHC_spA">' + GetName(dttime, num[i]) + "+</span>";
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span><span>+</span>";
                } else {
                    //kjhtml1 += '<span class="LHC_spA">' + getShengxiaoByDigit(num[i]) + '</span>'
                    kjhtml1 += '<span class="LHC_spA">' + GetName(dttime, num[i]) + "</span>";
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                }
            } else if (ltype == 2) {
                if (i == 6) {
                    cl = "hdM_spssc HYJP_Blue";
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                } else {
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                }
            } else if (ltype == 8)
            {
                if (i == 7) {
                    cl = "hdM_spssc HYJP_Blue";
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                } else {
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                }
            } else if (ltype == 4) {
                if (i == 5 || i == 6) {
                    cl = "hdM_spssc HYJP_Blue";
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                } else {
                    kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
                }
            } else if (ltype == 63 || ltype == 64) {
                cl = "SaiChe_Text SaiChe_TA" + num[i];
                kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
            } else if (ltype == 65) {
                var a = Number(num[0]);
                var b = Number(num[1]);
                var c = Number(num[2]);
                var sum = a + b + c;
                kjhtml += '<span class="hdM_spssc HYJP_Red">' + num[0] + "</span>";
                kjhtml += '<span class="hdM_spssc HYJP_PcHao ">+</span>';
                kjhtml += '<span class="hdM_spssc HYJP_Red">' + num[1] + "</span>";
                kjhtml += '<span class="hdM_spssc HYJP_PcHao ">+</span>';
                kjhtml += '<span class="hdM_spssc HYJP_Red">' + num[2] + "</span>";
                kjhtml += '<span class="hdM_spssc HYJP_PcHao ">=</span>';
                kjhtml += '<span class="hdM_spssc HYJP_Red">' + sum + "</span>";
                break;
            } else {
                kjhtml += '<span class="' + cl + '">' + num[i] + "</span>";
            }
        }
        var itemHtml = '<table class="Forecast_tableA" width="100%" bordercolor="#ccc" border="1"> <tr>' + ' <td align="center" valign="middle"> <p class="Forecast_Ptit">' + item.LotteryNum.Issue + "期</p>" + '    <p class="Forecast_Ptit">' + item.LotteryNum.SubTime + "</p> </td>" + ' <td colspan="2" align="left" valign="middle"> <div class="' + cl1 + '">' + kjhtml + "</div>" + '<div class="Forecast_LHC">' + kjhtml1 + "</div>  </td></tr>" + bethtml + "   </table>";
        itemList.push(itemHtml);
    });
    var html = itemList.join("");
    listDom.append(html);
}

//生肖
function get6caiSx(num, subtime) {
    var cainum;
    for (var i = 0; i < num.length; i++) {
        cainum += GetName(subtime, num[i]) + ",";
    }
    return cainum.substring(0, cainum.length - 1);
}

//获取六合彩球的颜色
function getcolor(haoma) {
    var red = "01，02，07，08，12，13，18，19，23，24，29，30，34，35，40，45，46";
    var blue = "03，04，09，10，14，15，20，25，26，31，36，37，41，42，47，48";
    var green = "05，06，11，16，17，21，22，27，28，32，33，38，39，43，44，49";
    if (red.indexOf(haoma) >= 0) {
        return "red";
    } else if (blue.indexOf(haoma) >= 0) {
        return "blue";
    } else {
        return "green";
    }
}

//六合彩数字转生肖
function getShengxiaoByDigit(digit) {
    var result = "鸡";
    if (digit == 11 || digit == 23 || digit == 35 || digit == 47) {
        result = "鼠";
    } else if (digit == 10 || digit == 22 || digit == 34 || digit == 46) {
        result = "牛";
    } else if (digit == 9 || digit == 21 || digit == 33 || digit == 45) {
        result = "虎";
    } else if (digit == 8 || digit == 20 || digit == 32 || digit == 44) {
        result = "兔";
    } else if (digit == 7 || digit == 19 || digit == 31 || digit == 43) {
        result = "龙";
    } else if (digit == 6 || digit == 18 || digit == 30 || digit == 42) {
        result = "蛇";
    } else if (digit == 5 || digit == 17 || digit == 29 || digit == 41) {
        result = "马";
    } else if (digit == 4 || digit == 16 || digit == 28 || digit == 40) {
        result = "羊";
    } else if (digit == 3 || digit == 15 || digit == 27 || digit == 39) {
        result = "猴";
    } else if (digit == 2 || digit == 14 || digit == 26 || digit == 38) {
        result = "鸡";
    } else if (digit == 1 || digit == 13 || digit == 25 || digit == 37 || digit == 49) {
        result = "狗";
    } else if (digit == 12 || digit == 24 || digit == 36 || digit == 48) {
        result = "猪";
    }
    return result;
}

function getwanfa(playName) {
    var i = -1;
    var p0 = ["第一球五码", "第一球六码", "冠军五码", "第一球", "冠军", "第一名"];
    var p1 = ["第二球五码", "第二球六码", "第二名五码", "第二球", "亚军", "第二名", "亚军五码"];
    var p2 = ["第三球五码", "第三球六码", "第三名五码", "第三球", "第三名"];
    var p3 = ["第四球五码", "第四球六码", "第四名五码", "第四球", "第四名"];
    var p4 = ["第五球五码", "第五球六码", "第五名五码", "第五球", "第五名"];
    var p5 = ["第六球五码", "第六名五码", "第六球", "第六名"];
    var p6 = ["第七球五码", "第七名五码", "第七球", "第七名", "十码中特", "二十码中特", "四肖中特", "六肖中特", "蓝5码", "蓝8码"];
    var p7 = ["第八球五码", "第八名五码", "第八球", "第八名"];
    var p8 = ["第九球五码", "第九名五码", "第九球", "第九名"];
    var p9 = ["第十球五码", "第十名五码", "第十球", "第十名"];
    var p10 = ["红2胆", "红3胆"];
    var p11 = ["前区4码"];
    var p12 = ["后区3码"];
    if ($.inArray(playName, p0) != -1) {
        i = 0;
    } else if ($.inArray(playName, p1) != -1) {
        i = 1;
    } else if ($.inArray(playName, p2) != -1) {
        i = 2;
    } else if ($.inArray(playName, p3) != -1) {
        i = 3;
    } else if ($.inArray(playName, p4) != -1) {
        i = 4;
    } else if ($.inArray(playName, p5) != -1) {
        i = 5;
    } else if ($.inArray(playName, p6) != -1) {
        i = 6;
    } else if ($.inArray(playName, p7) != -1) {
        i = 7;
    } else if ($.inArray(playName, p8) != -1) {
        i = 8;
    } else if ($.inArray(playName, p9) != -1) {
        i = 9;
    } else if ($.inArray(playName, p10) != -1) {
        i = 10;
    } else if ($.inArray(playName, p11) != -1) {
        i = 11;
    } else if ($.inArray(playName, p12) != -1) {
        i = 12;
    }
    return i;
}