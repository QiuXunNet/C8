var qiu;
var qiuClone;

var lType = 9;

var lhc_red = ["01", "02", "07", "08", "12", "13", "18", "19", "23", "24", "29", "30", "34", "35", "40", "45", "46"];
var lhc_blue = ["03", "04", "09", "10", "14", "15", "20", "25", "26", "31", "36", "37", "41", "42", "47", "48"];
var lhc_green = ["05", "06", "11", "16", "17", "21", "22", "27", "28", "32", "33", "38", "39", "43", "44", "49"];

var lottery = {
    fc3d: { qName: "福彩3D", ltype: 1, fc3d_q1: { playName: "第一球", num: [], numLength: 5, ds: "", dx: "" }, fc3d_q2: { playName: "第二球", num: [], numLength: 5, ds: "", dx: "" }, fc3d_q3: { playName: "第三球", num: [], numLength: 5, ds: "", dx: "" }, fc3d_q4: { playName: "杀一码", num: [], numLength: 1 }, fc3d_q5: { playName: "杀二码", num: [], numLength: 2 }, fc3d_q6: { playName: "胆一码", num: [], numLength: 1 }, fc3d_q7: { playName: "胆二码", num: [], numLength: 2 }, fc3d_q8: { playName: "胆三码", num: [], numLength: 3 } },
    xsq: { qName: "双色球", ltype: 2, xsq_q1: { playName: "红3胆", num: [], numLength: 3 }, xsq_q2: { playName: "杀3红", num: [], numLength: 3 }, xsq_q3: { playName: "蓝单双", ds: "" }, xsq_q4: { playName: "蓝大小", ds: "" }, xsq_q5: { playName: "龙头单双", ds: "" }, xsq_q6: { playName: "凤尾单双", ds: "" }, xsq_q7: { playName: "蓝球8码", num: [], numLength: 8 }, xsq_q8: { playName: "红2胆", num: [], numLength: 2 }, xsq_q9: { playName: "蓝五码", num: [], numLength: 5 } },
    qxc: { qName: "七星彩", ltype: 3, qxc_q1: { playName: "第一球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q2: { playName: "第二球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q3: { playName: "第三球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q4: { playName: "第四球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q5: { playName: "第五球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q6: { playName: "第六球", num: [], numLength: 5, ds: "", dx: "" }, qxc_q7: { playName: "第七球", num: [], numLength: 5, ds: "", dx: "" } },
    dlt: { qName: "大乐透", ltype: 4, dlt_q1: { playName: "前区4码", num: [], numLength: 4 }, dlt_q2: { playName: "前区杀4码", num: [], numLength: 4 }, dlt_q3: { playName: "前区龙头单双", ds: "" }, dlt_q4: { playName: "前区凤尾单双", ds: "" }, dlt_q5: { playName: "后区3码", num: [], numLength: 3 }, dlt_q6: { playName: "后区杀3码", num: [], numLength: 3 }, dlt_q7: { playName: "后区龙头单双", ds: "" }, dlt_q8: { playName: "后区凤尾单双", ds: "" } },
    lhc: { qName: "六合彩", ltype: 5, lhc_q1: { ds: "" }, lhc_q2: { dx: "" }, lhc_q3: { dx: "" }, lhc_q4: { ds: "" }, lhc_q5: { color: "" }, lhc_q6: { animal: [], animalLength: 1 }, lhc_q7: { animal: [], animalLength: 4 }, lhc_q8: { animal: [], animalLength: 6 }, lhc_q9: { num: [], numLength: 20 }, lhc_q10: { num: [], numLength: 10 }, lhc_q11: { num: [], numLength: 5 }, lhc_q12: { num: [], numLength: 5 }, lhc_q13: { num: [], numLength: 6 } },
    pl3: { qName: "排列3", ltype: 6, pl3_q1: { playName: "第一球", num: [], numLength: 5, ds: "", dx: "" }, pl3_q2: { playName: "第二球", num: [], numLength: 5, ds: "", dx: "" }, pl3_q3: { playName: "第三球", num: [], numLength: 5, ds: "", dx: "" }, pl3_q4: { playName: "杀一码", num: [], numLength: 1 }, pl3_q5: { playName: "杀二码", num: [], numLength: 2 }, pl3_q6: { playName: "胆一码", num: [], numLength: 1 }, pl3_q7: { playName: "胆二码", num: [], numLength: 2 }, pl3_q8: { playName: "胆三码", num: [], numLength: 3 } },
    pl5: { qName: "排列5", ltype: 7, pl5_q1: { playName: "第一球", num: [], numLength: 5, ds: "", dx: "" }, pl5_q2: { playName: "第二球", num: [], numLength: 5, ds: "", dx: "" }, pl5_q3: { playName: "第三球", num: [], numLength: 5, ds: "", dx: "" }, pl5_q4: { playName: "第四球", num: [], numLength: 5, ds: "", dx: "" }, pl5_q5: { playName: "第五球", num: [], numLength: 5, ds: "", dx: "" } },
    qlc: { qName: "七乐彩", ltype: 8, qlc_q1: { playName: "第一球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q2: { playName: "第二球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q3: { playName: "第三球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q4: { playName: "第四球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q5: { playName: "第五球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q6: { playName: "第六球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q7: { playName: "第七球", num: [], numLength: 5, ds: "", dx: "" }, qlc_q8: { playName: "胆一码", num: [], numLength: 1 }, qlc_q9: { playName: "胆二码", num: [], numLength: 2 }, qlc_q10: { playName: "胆三码", num: [], numLength: 3 }, qlc_q11: { playName: "杀三码", num: [], numLength: 3 }, qlc_q12: { playName: "杀五码", num: [], numLength: 5 } },
    cqssc: { qName: "重庆时时彩", ltype: 9, cqssc_q1: { num: [], numLength: 5, ds: "", dx: "" }, cqssc_q2: { num: [], numLength: 5, ds: "", dx: "" }, cqssc_q3: { num: [], numLength: 5, ds: "", dx: "" }, cqssc_q4: { num: [], numLength: 5, ds: "", dx: "" }, cqssc_q5: { num: [], numLength: 5, ds: "", dx: "" }, cqssc_total: { ds: "", dx: "" } },
    gd11x5: { qName: "广东11选5", ltype: 10, gd11x5_q1: { num: [], numLength: 5, ds: "", dx: "" }, gd11x5_q2: { num: [], numLength: 5, ds: "", dx: "" }, gd11x5_q3: { num: [], numLength: 5, ds: "", dx: "" }, gd11x5_q4: { num: [], numLength: 5, ds: "", dx: "" }, gd11x5_q5: { num: [], numLength: 5, ds: "", dx: "" }, gd11x5_total: { ds: "", dx: "" }, gd11x5_longhu: { lh: "" } },
    k3: { qName: "快三", ltype: 11, numCount: 6, k3_q1: { playName: "和值", ds: "", dx: "" }, k3_q2: { playName: "独胆", num: [], numLength: 1 }, k3_q3: { playName: "双胆", num: [], numLength: 2 } },
    gdkl10f: { qName: "广东快乐十分", ltype: 12, numCount: 2, gdkl10f_q1: { playName: "第一球  ", ds: "", dx: "", lastdan: "", totaldx: "", lh: "", direction: "", zhongfabai: "" }, gdkl10f_q2: { playName: "第二球", ds: "", dx: "", lastds: "", totaldx: "", lh: "", direction: "", zhongfabai: "" } },
    kl12: { qName: "快乐十二", ltype: 13, kl12_q1: { num: [], numLength: 6, ds: "", dx: "" }, kl12_q2: { num: [], numLength: 6, ds: "", dx: "" }, kl12_q3: { num: [], numLength: 6, ds: "", dx: "" }, kl12_q4: { num: [], numLength: 6, ds: "", dx: "" }, kl12_q5: { num: [], numLength: 6, ds: "", dx: "" }, kl12_total: { ds: "", dx: "" }, kl12_longhu: { lh: "" } },
    pk10: { qName: "北京赛车", ltype: 14, pk10_q1: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, pk10_q2: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, pk10_q3: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, pk10_q4: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, pk10_q5: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, pk10_q6: { num: [], numLength: 5, ds: "", dx: "", }, pk10_q7: { num: [], numLength: 5, ds: "", dx: "" }, pk10_q8: { num: [], numLength: 5, ds: "", dx: "" }, pk10_q9: { num: [], numLength: 5, ds: "", dx: "" }, pk10_q10: { num: [], numLength: 5, ds: "", dx: "" } },
    xyft: { qName: "幸运飞艇", ltype: 15, xyft_q1: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, xyft_q2: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, xyft_q3: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, xyft_q4: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, xyft_q5: { num: [], numLength: 5, ds: "", dx: "", lh: "" }, xyft_q6: { num: [], numLength: 5, ds: "", dx: "", }, xyft_q7: { num: [], numLength: 5, ds: "", dx: "" }, xyft_q8: { num: [], numLength: 5, ds: "", dx: "" }, xyft_q9: { num: [], numLength: 5, ds: "", dx: "" }, xyft_q10: { num: [], numLength: 5, ds: "", dx: "" } },
    xync: { qName: "幸运农场", ltype: 17, numCount: 2, xync_q1: { playName: "第一球", ds: "", dx: "", lastdan: "", totaldx: "", lh: "", direction: "", zhongfabai: "" }, xync_q2: { playName: "第二球", ds: "", dx: "", lastds: "", totaldx: "", lh: "", direction: "", zhongfabai: "" } }
}

/**
 * 初始化彩种对象
 * @param {彩种id} ltype 
 */
function initLottery(ltype) {
    switch (ltype) {
        case 1:
            qiu = lottery.fc3d;
            break;
        case 2:
            qiu = lottery.xsq;
            break;
        case 3:
            qiu = lottery.qxc;
            break;
        case 4:
            qiu = lottery.dlt;
            break;
        case 5:
            qiu = lottery.lhc;
            break;
        case 6:
            qiu = lottery.pl3;
            break;
        case 7:
            qiu = lottery.pl5;
            break;
        case 8:
            qiu = lottery.qlc;
            break;
        case 9:
            qiu = lottery.cqssc;
            break;
        case 10:
            qiu = lottery.gd11x5;
            break;
        case 11:
            qiu = lottery.k3;
            break;
        case 12:
            qiu = lottery.gdkl10f;
            break;
        case 13:
            qiu = lottery.kl12;
            break;
        case 14:
            qiu = lottery.pk10;
            break;
        case 15:
            qiu = lottery.xyft;
            break;
        case 16:
            break;
        case 17:
            qiu = lottery.xync;
            break;
    }

    $(".betContainer").empty();
    $(".WanFa_footer .WF_ftTitle span").html(0);
}
/**
 * 将对象obj1复制到obj2
 * @param {被复制对象} obj1 
 * @param {复制对象} obj2 
 */
function copy(obj1, obj2) {
    var obj2 = obj2 || {};
    for (var name in obj1) {
        if (typeof obj1[name] === "object") {
            obj2[name] = (obj1[name].constructor === Array) ? [] : {};
            copy(obj1[name], obj2[name]);
        } else {
            obj2[name] = obj1[name];
        }
    }
    return obj2;
}

/**
 * 判断两个对象是否相等
 * @param {对象a} a 
 * @param {对象b} b 
 */
function isEqual(a, b) {
    //判断a和b全等
    if (a === b) {
        //判断是否为0和-0
        return a !== 0 || 1 / a === 1 / b;
    }
    //判断是否为null和undefined
    if (a === null || b === null) {
        return a === b;
    }
    //接下来判断a和b的数据类型
    var classNameA = toString.call(a),
        classNameB = toString.call(b);
    //如果数据类型不相等，则返回false
    if (classNameA !== classNameB) {
        return false;
    }
    //如果数据类型相等，再根据不同数据类型分别判断
    switch (classNameA) {
        case '[object RegExp]':
        case '[object String]':
            return '' + a === '' + b;
        case '[object Number]':
            if (+a !== +a) {
                return +b !== +b;
            }
            return +a === 0 ? 1 / +a === 1 / b : +a === +b;
        case '[object Date]':
        case '[object Boolean]':
            return +a === +b;
    }
    //如果是对象类型
    if (classNameA == '[object Object]') {
        //获取a和b的属性长度
        var propsA = Object.getOwnPropertyNames(a),
            propsB = Object.getOwnPropertyNames(b);
        if (propsA.length != propsB.length) {
            return false;
        }
        for (var i = 0; i < propsA.length; i++) {
            var propName = propsA[i];
            //如果对应属性对应值不相等，则返回false
            if (a[propName] !== b[propName]) {
                return false;
            }
        }
        return true;
    }
    //如果是数组类型
    if (classNameA == '[object Array]') {
        if (a.toString() == b.toString()) {
            return true;
        }
        return false;
    }
}

/**
 * 比较数组中元素大小（转换成数字）
 * @param {比较值1} value1 
 * @param {比较值2} value2 
 */
function arrayElementCompare(value1, value2) {
    var intValue1 = parseInt(value1),
        intValue2 = parseInt(value2);
    if (intValue1 < intValue2) {
        return -1;
    } else if (intValue1 > intValue2) {
        return 1;
    } else {
        return 0;
    }
}


/**
 * 打开弹窗
 * @param {球号Id} qNum 
 * @param {弹窗类型} type 
 * @param {彩种编码} ltype
 */
function openModule(qNum, type, ltype) {

    try {
        //initLottery(ltype);
        qiuClone = {};
        qiuClone = copy(qiu, qiuClone);
    } catch (e) {

    }

    var popup = $("#" + type);
    popup.find("span.current").removeClass("current");
    popup.attr("data-id", qNum);
    popup.attr("data-type", type);
    popup.attr("data-ltype", ltype);
    var selectedQiu = qiuClone[ltype][qNum];
    if (type == "popup_10_dan_da" || type == "popup_11_dan_da" || type == "popup_12_dan_da" || type == "popup_1_10_dan_da") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.ds) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else if (curQiu.textContent == selectedQiu.dx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else {
                    if (selectedQiu.num && selectedQiu.num.length > 0) {
                        for (var i = 0; i < selectedQiu.num.length; i++) {
                            if (curQiu.textContent == selectedQiu.num[i]) {
                                curQiu.setAttribute("class", "C8_Qiu current");
                            }
                        }
                    }
                }
            }
        }
        popup.show();
    } else if (type == "popup_dan_da") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.ds) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else if (curQiu.textContent == selectedQiu.dx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        popup.show();
    } else if (type == "popup_dan" || type == "popup_daxiao") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.ds) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else if (curQiu.textContent == selectedQiu.dx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        popup.show();
    } else if (type == "popup_color") {


        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.color) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        popup.show();
    } else if (type == "popup_animal") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (selectedQiu.animal && selectedQiu.animal.length > 0) {
                    for (var i = 0; i < selectedQiu.animal.length; i++) {
                        if (curQiu.textContent == selectedQiu.animal[i]) {
                            curQiu.setAttribute("class", "C8_Qiu current");
                        }
                    }
                }
            }
        }
        popup.show();
    } else if (type == "popup_10") {
        //limitnum
        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (selectedQiu.num && selectedQiu.num.length > 0) {
                    for (var i = 0; i < selectedQiu.num.length; i++) {
                        if (curQiu.textContent == selectedQiu.num[i]) {
                            curQiu.setAttribute("class", "C8_Qiu current");
                        }
                    }
                }
            }
        }
        popup.show();
    } else if (type == "popup_num6" || type == "popup_num12" || type == "popup_num35" || type == "popup_num33" || type == "popup_num16" || type == "popup_num49" || type == "popup_num30") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (selectedQiu.num && selectedQiu.num.length > 0) {
                    for (var i = 0; i < selectedQiu.num.length; i++) {
                        if (curQiu.textContent == selectedQiu.num[i]) {
                            curQiu.setAttribute("class", "C8_Qiu current");
                        }
                    }
                }
            }
        }
        popup.show();
    } else if (type == "popup_lh") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.lh) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        popup.show();
    } else if (type == "popup_dan_da_lh") {

        var qiuSpan = popup.find("span.C8_Qiu");

        if (qiuSpan && qiuSpan.length > 0) {
            for (var j = 0; j < qiuSpan.length; j++) {
                var curQiu = qiuSpan[j];
                if (curQiu.textContent == selectedQiu.ds) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else if (curQiu.textContent == selectedQiu.dx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else if (curQiu.textContent == selectedQiu.lh) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                } else {
                    if (selectedQiu.num && selectedQiu.num.length > 0) {
                        for (var i = 0; i < selectedQiu.num.length; i++) {
                            if (curQiu.textContent == selectedQiu.num[i]) {
                                curQiu.setAttribute("class", "C8_Qiu current");
                            }
                        }
                    }
                }
            }
        }
        popup.show();
    } else if (type == "popup_kl10") {

        var danSpan = popup.find(".danContainer span.C8_Qiu");
        if (danSpan && danSpan.length > 0) {
            for (var j = 0; j < danSpan.length; j++) {
                var curQiu = danSpan[j];
                if (curQiu.textContent == selectedQiu.ds) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }

        var daxiaoSpan = popup.find(".daxiaoContainer span.C8_Qiu");
        if (daxiaoSpan && daxiaoSpan.length > 0) {
            for (var j = 0; j < daxiaoSpan.length; j++) {
                var curQiu = daxiaoSpan[j];
                if (curQiu.textContent == selectedQiu.dx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }

        var lastDanSpan = popup.find(".lastdanContainer span.C8_Qiu");
        if (lastDanSpan && lastDanSpan.length > 0) {
            for (var j = 0; j < lastDanSpan.length; j++) {
                var curQiu = lastDanSpan[j];
                if (curQiu.textContent == selectedQiu.lastdan) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        var totaldaxiaoSpan = popup.find(".totaldaxiaoContainer span.C8_Qiu");
        if (totaldaxiaoSpan && totaldaxiaoSpan.length > 0) {
            for (var j = 0; j < totaldaxiaoSpan.length; j++) {
                var curQiu = totaldaxiaoSpan[j];
                if (curQiu.textContent == selectedQiu.totaldx) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }


        var sxSpan = popup.find(".sxContainer span.C8_Qiu");
        if (sxSpan && sxSpan.length > 0) {
            for (var j = 0; j < sxSpan.length; j++) {
                var curQiu = sxSpan[j];
                if (curQiu.textContent == selectedQiu.lh) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }

        var directionSpan = popup.find(".directionContainer span.C8_Qiu");
        if (directionSpan && directionSpan.length > 0) {
            for (var j = 0; j < directionSpan.length; j++) {
                var curQiu = directionSpan[j];
                if (curQiu.textContent == selectedQiu.direction) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }

        var zhongfabaiSpan = popup.find(".zhongfabaiContainer span.C8_Qiu");
        if (zhongfabaiSpan && zhongfabaiSpan.length > 0) {
            for (var j = 0; j < zhongfabaiSpan.length; j++) {
                var curQiu = zhongfabaiSpan[j];
                if (curQiu.textContent == selectedQiu.zhongfabai) {
                    curQiu.setAttribute("class", "C8_Qiu current");
                }
            }
        }
        popup.show();
    }
    $(".mask").show();
}

/*清空 */
function empty() {
    qiu = copy(lottery, qiu);
    $(".betContainer").empty();
    $(".WanFa_footer .WF_ftTitle span").html(0);
}

/**
 * 计算单个球下注数量
 * @param {当前下注结果} str 
 */
function calcBuyCount(str) {
    var arr = str.split('|');
    arr = removeEmpty(arr);
    return arr.length;
}
/**
 * 移除数组中的空值
 * @param {数组对象} arrayObject 
 */
function removeEmpty(arrayObject) {
    for (var i = 0; i < arrayObject.length; i++) {
        if (arrayObject[i] == "" || typeof (arrayObject[i]) == "undefined") {
            arrayObject.splice(i, 1);
            i = i - 1;
        }
    }
    return arrayObject;
}

/**
 * 计算注数
 */
function calc() {

    var count = 0;

    if (qiuClone == 'undefined') return count;

    if (lType == 1) {
        count = commonCalc(count, "fc3d", 1, 8);
    }
    else if (lType == 2) {
        count = commonCalc(count, "xsq", 1, 9);
    }
    else if (lType == 3) {
        count = commonCalc(count, "qxc", 1, 7);
    }
    else if (lType == 4) {
        count = commonCalc(count, "dlt", 1, 8);
    }
    else if (lType == 5) {
        count = commonCalc(count, "lhc", 1, 8);
        count = calcLhcSpecial(count, "lhc", 9, 13);
    }
    else if (lType == 6) {
        count = commonCalc(count, "pl3", 1, 8);
    }
    else if (lType == 7) {
        count = commonCalc(count, "pl5", 1, 5);
    }
    else if (lType == 8) {
        count = commonCalc(count, "qlc", 1, 12);
    }
    else if (lType >= 9 && lType < 15) {
        count = commonCalc(count, "cqssc", 1, 5);

        if (qiuClone.cqssc.cqssc_total && qiuClone.cqssc.cqssc_total.html) {
            count += calcBuyCount(qiuClone.cqssc.cqssc_total.html);
        }
    }
    else if (lType >= 15 && lType < 38) {
        count = commonCalc(count, "gd11x5", 1, 5);
        if (qiuClone.gd11x5.gd11x5_total && qiuClone.gd11x5.gd11x5_total.html) {
            count += calcBuyCount(qiuClone.gd11x5.gd11x5_total.html);
        }
        if (qiuClone.gd11x5.gd11x5_longhu && qiuClone.gd11x5.gd11x5_longhu.html) {
            count += calcBuyCount(qiuClone.gd11x5.gd11x5_longhu.html);
        }
    }
    else if ((lType >= 38 && lType < 51) || lType == 65) {
        count = commonCalc(count, "k3", 1, 3);
    }
    else if (lType >= 51 && lType < 60) {
        count = commonCalc(count, "gdkl10f", 1, 2);
    }
    else if (lType >= 60 && lType < 63) {
        count = commonCalc(count, "kl12", 1, 5);

        if (qiuClone.kl12.kl12_total && qiuClone.kl12.kl12_total.html) {
            count += calcBuyCount(qiuClone.kl12.kl12_total.html);
        }
        if (qiuClone.kl12.kl12_longhu && qiuClone.kl12.kl12_longhu.html) {
            count += calcBuyCount(qiuClone.kl12.kl12_longhu.html);
        }
    }
    else if (lType == 63) {

        count = commonCalc(count, "pk10", 1, 10);
    }
    else if (lType == 64) {
        count = commonCalc(count, "xyft", 1, 10);
    }


    return count;
}

/**
 * 计算六合彩特殊玩法
 * @param {传入当前注数} count 
 * @param {彩种编码} ltypeCode 
 * @param {起始球号} minNum 
 * @param {截至球号} maxNum 
 */
function calcLhcSpecial(count, ltypeCode, minNum, maxNum) {
    count = count || 0;
    if (minNum && maxNum && minNum <= maxNum) {
        for (var i = minNum; i <= maxNum; i++) {
            var curQ = qiuClone[ltypeCode][ltypeCode + "_" + "q" + i];
            if (curQ && curQ.html && $.trim(curQ.html).length > 0) {
                count += 1;
            }
        }
    }
    return count;
}

/**
 * 计算每个球的下注数量
 * @param {传入当前注数} count 
 * @param {彩种编码} maxNum 
 * @param {起始球号} minNum 
 * @param {截至球号} maxNum 
 */
function commonCalc(count, ltypeCode, minNum, maxNum) {
    count = count || 0;
    if (minNum && maxNum && minNum <= maxNum) {
        for (var i = minNum; i <= maxNum; i++) {
            var curQ = qiuClone[ltypeCode][ltypeCode + "_" + "q" + i];
            if (curQ && curQ.html) {
                count += calcBuyCount(curQ.html);
            }
        }
    }
    return count;
}


$(function () {

    qiu = copy(lottery, qiu);
    $(".betContainer").empty();

    /**
     * 大小号码选择事件
     */
    $(".C8_TCpopupA .daxiaoContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");

        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["dx"] = "";
            _this.removeClass("current");
        } else {
            qiuClone[ltype][qiuNum]["dx"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 单双号码球点击事件
     */
    $(".C8_TCpopupA .danContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["ds"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["ds"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 龙虎号码球点击事件
     */
    $(".C8_TCpopupA .sxContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["lh"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["lh"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 尾单号码球点击事件
     */
    $(".C8_TCpopupA .lastdanContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["lastdan"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["lastdan"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 和小号码球点击事件
     */
    $(".C8_TCpopupA .totaldaxiaoContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["totaldx"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["totaldx"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 方位号码球点击事件
     */
    $(".C8_TCpopupA .directionContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["direction"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["direction"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 中发白号码球点击事件
     */
    $(".C8_TCpopupA .zhongfabaiContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["zhongfabai"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["zhongfabai"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 波色号码球点击事件
     */
    $(".C8_TCpopupA .colorContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = _this.parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");
        if (_this.hasClass("current")) {
            qiuClone[ltype][qiuNum]["color"] = "";
            _this.removeClass("current")
        } else {
            qiuClone[ltype][qiuNum]["color"] = val;
            _this.addClass("current").siblings().removeClass("current");
        }
    });
    /**
     * 生肖号码球点击事件
     */
    $(".C8_TCpopupA .animalContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = $(this).parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");

        if (_this.hasClass("current")) {
            var _this = $(this),
                val = _this.text(),
                qiuNum = module.attr("data-id");
            var index = qiuClone[ltype][qiuNum].animal.indexOf(val);
            if (index > -1) {
                qiuClone[ltype][qiuNum].animal.splice(index, 1);
                _this.removeClass("current");
            }
        } else {
            var curQiu = qiuClone[ltype][qiuNum],
                num = curQiu.animal;
            if (num.length < curQiu.animalLength) {
                num.push(val);
                qiuClone[ltype][qiuNum].animal = num;
                _this.addClass("current");
            } else {
                $(document).dialog({
                    type: 'notice',
                    infoText: '只能选' + curQiu.animalLength + '个号码',
                    autoClose: 2000
                });
                //num.splice(0,1,val);
            }
        }
    });
    /**
     * 数字号码球点击事件
     */
    $(".C8_TCpopupA .numContainer .C8_Qiu").click(function () {
        var _this = $(this),
            module = $(this).parentsUntil(".C8_TCpopupA").parent(),
            val = _this.text(),
            qiuNum = module.attr("data-id"),
            ltype = module.attr("data-ltype");

        // var moduleId = module.attr("id");
        // if (moduleId == "popup_dan_da_lh") {
        //     val = parseInt(val);
        // }
        //

        if (_this.hasClass("current")) {
            var _this = $(this),
                val = _this.text(),
                qiuNum = module.attr("data-id");
            var index = qiuClone[ltype][qiuNum].num.indexOf(val);
            if (index > -1) {
                qiuClone[ltype][qiuNum].num.splice(index, 1);
                _this.removeClass("current");
            }
        } else {
            var curQiu = qiuClone[ltype][qiuNum],
                num = curQiu.num;
            if (num.length < curQiu.numLength) {
                num.push(val);
                qiuClone[ltype][qiuNum].num = num;
                _this.addClass("current");
            } else {
                $(document).dialog({
                    type: 'notice',
                    infoText: '只能选' + curQiu.numLength + '个号码',
                    autoClose: 2000
                });
                //num.splice(0,1,val);
            }
        }
    });

    /**
     *弹窗确认按钮点击事件 
     */
    $(".C8_TCpopupA .confirmButton").click(function () {
        var module = $(this).parentsUntil(".C8_TCpopupA").parent();
        var dataid = module.attr("data-id"),
            datatype = module.attr("data-type"),
            ltype = module.attr("data-ltype"),
            _currentNum = $("#" + dataid);

        var selectedQiu = qiuClone[ltype][dataid];
        var selectedResult,
            isClose = false,
            msg = "";
        if (datatype == "popup_10_dan_da" || datatype == "popup_11_dan_da" || datatype == "popup_12_dan_da" || datatype == "popup_1_10_dan_da") {
            var numLen = selectedQiu.num.length;

            if (numLen == 0 || numLen == selectedQiu.numLength) {
                var num = selectedQiu.num.sort(arrayElementCompare).join(",");
                selectedResult = num + "|" + (selectedQiu.ds || "") + "|" + (selectedQiu.dx || "");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.numLength + '个号码';
            }
        } else if (datatype == "popup_dan_da") {
            selectedResult = (selectedQiu.ds || "") + "|" + (selectedQiu.dx || "");
            isClose = true;
        } else if (datatype == "popup_10") {
            var numLen = selectedQiu.num.length;
            if (numLen == 0 || numLen == selectedQiu.numLength) {
                selectedResult = selectedQiu.num.sort(arrayElementCompare).join(",");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.numLength + '个号码';
            }
        } else if (datatype == "popup_num6" || datatype == "popup_num12" || datatype == "popup_num35" || datatype == "popup_num33" || datatype == "popup_num16" || datatype == "popup_num30") {
            var numLen = selectedQiu.num.length;
            if (numLen == 0 || numLen == selectedQiu.numLength) {
                selectedResult = selectedQiu.num.sort(arrayElementCompare).join(",");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.numLength + '个号码';
            }
        } else if (datatype == "popup_animal") {
            var numLen = selectedQiu.animal.length;
            if (numLen == 0 || numLen == selectedQiu.animalLength) {
                selectedResult = selectedQiu.animal.sort(arrayElementCompare).join(",");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.animalLength + '个号码';
            }
        } else if (datatype == "popup_num49") {
            var numLen = selectedQiu.num.length;
            if (numLen == 0 || numLen == selectedQiu.numLength) {
                var htmlArray = [];
                var numArray = selectedQiu.num.sort();
                for (var i = 0; i < numLen; i++) {
                    var selectedNum = numArray[i];
                    if (selectedNum) {
                        if ($.inArray(selectedNum, lhc_red) > -1) {
                            htmlArray.push("<span class=\"WF_TBAsp WF_TBAspred\">" + selectedNum + "</span>");
                        } else if ($.inArray(selectedNum, lhc_blue) > -1) {
                            htmlArray.push("<span class=\"WF_TBAsp WF_TBAspblue\">" + selectedNum + "</span>");
                        } else if ($.inArray(selectedNum, lhc_green) > -1) {

                            htmlArray.push("<span class=\"WF_TBAsp WF_TBAspgreen\">" + selectedNum + "</span>");
                        }
                    }
                }

                selectedResult = htmlArray.join(" ");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.numLength + '个号码';
            }
        } else if (datatype == "popup_dan_da") {
            selectedResult = (selectedQiu.ds || "") + "|" + (selectedQiu.dx || "");
            isClose = true;
        } else if (datatype == "popup_daxiao") {
            selectedResult = (selectedQiu.dx || "");
            isClose = true;
        } else if (datatype == "popup_dan") {
            selectedResult = (selectedQiu.ds || "");
            isClose = true;
        } else if (datatype == "popup_color") {
            selectedResult = (selectedQiu.color + "波" || "");
            isClose = true;
        } else if (datatype == "popup_lh") {
            selectedResult = (selectedQiu.lh || "");
            isClose = true;
        } else if (datatype == "popup_dan_da_lh") {
            var numLen = selectedQiu.num.length;

            if (numLen == 0 || numLen == selectedQiu.numLength) {
                var num = selectedQiu.num.sort(arrayElementCompare).join(",");
                selectedResult = num + "|" + (selectedQiu.ds || "") + "|" + (selectedQiu.dx || "") + "|" + (selectedQiu.lh || "");
                isClose = true;
            } else {
                msg = '请选择' + selectedQiu.numLength + '个号码';
            }
        } else if (datatype == "popup_kl10") {

            selectedResult = (selectedQiu.ds || "") + "|" + (selectedQiu.dx || "") + "|" + (selectedQiu.lastdan || "") + "|" + (selectedQiu.totaldx || "") + "|" + (selectedQiu.lh || "") +
                "|" + (selectedQiu.direction || "") + "|" + (selectedQiu.zhongfabai || "");
            isClose = true;
        }

        if (isClose) {
            selectedQiu.html = selectedResult;
            var arr = selectedResult.split('|');
            arr = removeEmpty(arr);
            if (arr.length < 1) {
                $(document).dialog({
                    type: 'notice',
                    infoText: '请选择',
                    autoClose: 1500
                });
                return;
            }

            _currentNum.html(selectedResult);
            

            var quantity = calc(); //计算各个彩种注数
            if (typeof (quantity) != 'undefined') {
                $(".WanFa_footer .WF_ftTitle span").html(quantity);
            }

            module.hide();
            qiu = qiuClone;
            $(".mask").hide();
            // $(document).dialog({
            //     type: 'notice',
            //     infoText: '当前下注：' + quantity,
            //     autoClose: 1500
            // });
        } else {
            $(document).dialog({
                type: 'notice',
                infoText: msg,
                autoClose: 1500
            });
        }
    });

    /**
     * 弹窗取消按钮点击事件
     */
    $(".C8_TCpopupA .cancelButton").click(function () {
        $(this).parentsUntil(".C8_TCpopupA").parent().hide();
        $(".mask").hide();
    });
});