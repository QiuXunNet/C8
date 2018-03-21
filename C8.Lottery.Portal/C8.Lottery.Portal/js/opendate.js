var CP = {
    ev: { click: "ontouchstart" in window ? "touchend" : "click", mup: "ontouchstart" in window ? "touchend" : "mouseup", mmove: "ontouchstart" in window ? "touchmove" : "mousemove", mdown: "ontouchstart" in window ? "touchstart" : "mousedown" },
    kj: { id: "#kj-box", timec: 2, timer: null, ntime: 0, runNext: !1, ckNext: !1, showTime: function() { this.ntime < 1 || this.ntime < 1800 && !this.ckNext && !this.runNext && (this.ckNext = !0, this.loadData()) } },
    rq: {
        id: ".rq-box",
        wk: ["日", "一", "二", "三", "四", "五", "六"],
        startYear: 3e3,
        startMonth: 13,
        maxYear: 0,
        maxMonth: 0,
        nowYear: null,
        nowMonth: null,
        nowDate: null,
        nextQiYear: 0,
        nextQiMonth: 0,
        nextQiDate: 0,
        nextQiWeek: 0,
        isTodayKa: !1,
        nextQiTimeSce: 0,
        options: [],
        init: function() {
            var t = new Date;
            this.nowYear = t.getFullYear(), this.nowMonth = t.getMonth() + 1, this.nowDate = t.getDate();
            var e = t.getHours(),
                s = !1;
            for (var i in _djson) {
                i < this.startYear && (this.startYear = i), i > this.maxYear && (this.maxYear = i);
                for (var a in _djson[i])
                    if (this.options.push({ title: i + "年" + (a > 9 ? a : "0" + a) + "月", date: i + "-" + a }), !(this.nextQiYear > 0))
                        for (var n in _djson[i][a])
                            if (this.nowYear != i || this.nowMonth != a || this.nowDate != _djson[i][a][n].d) { if (s && 1 == _djson[i][a][n].k) { this.nextQiDate = _djson[i][a][n].d, this.nextQiWeek = _djson[i][a][n].w, this.nextQiMonth = a, this.nextQiYear = i; break } } else {
                                if (1 == _djson[i][a][n].k && (this.isTodayKa = !0, e < 22)) { this.nextQiDate = _djson[i][a][n].d, this.nextQiWeek = _djson[i][a][n].w, this.nextQiMonth = a, this.nextQiYear = i; break }
                                s = !0
                            }
            }
            for (var a in _djson[this.startYear]) a < this.startMonth && (this.startMonth = a);
            for (var a in _djson[this.maxYear]) a > this.maxMonth && (this.maxMonth = a);
            this.createTable(this.nowYear, this.nowMonth), this.moveTime()
        },
        select: function(t) {
            var e = $(t).val().split("-");
            this.createTable(e[0], parseInt(e[1]))
        },
        prev: function() {
            var t, e, s = $(".rq-select").attr("nmon").split("-"),
                i = parseInt(s[0]),
                a = parseInt(s[1]);
            e = a - 1, e < 1 ? (t = i - 1, e = 12) : t = i, t < this.startYear && (t = this.startYear), t == this.startYear && e < this.startMonth && (e = this.startMonth), this.createTable(t, e)
        },
        next: function() {
            var t, e, s = $(".rq-select").attr("nmon").split("-"),
                i = parseInt(s[0]),
                a = parseInt(s[1]);
            e = a + 1, e > 12 ? (t = i + 1, e = 1) : t = i, t > this.maxYear && (t = this.maxYear), t == this.maxYear && e > this.maxMonth && (e = this.maxMonth), this.createTable(t, e)
        },
        createTable: function(t, e) {
            $(this.id).empty();
            var s = "";
            s += '<div class="rq-show"></div>', s += '<div class="rq-memu"><div class="rq-select" nmon="' + t + "-" + e + '">' + t + "年" + e + "月</div>", s += '<div class="rq-memu-left" on' + CP.ev.click + '="CP.rq.prev()"></div><div class="rq-memu-right" on' + CP.ev.click + '="CP.rq.next()"></div></div>', s += '<table class="nav-table rq-table"><tbody><tr>';
            for (var i in this.wk) s += "<td>" + this.wk[i] + "</td>";
            s += "</tr>";
            var a = _djson[t][e],
                n = !1;
            1 == e ? _djson[t - 1] && _djson[t - 1][12] && (n = _djson[t - 1][12]) : _djson[t][e - 1] && (n = _djson[t][e - 1]);
            var r = !1;
            12 == e ? _djson[t + 1] && _djson[t + 1][1] && (r = _djson[t + 1][1]) : _djson[t][e + 1] && (r = _djson[t][e + 1]);
            for (var o, d, h, l, c, v, m = a.length, i = 0, u = 0; i < 6 && !(u >= m); i++) {
                for (s += "<tr>", o = 0, d = 0; o < 7; o++) 0 == i && o < a[0].w ? n ? (h = n.length - a[0].w + o, s += '<td class="rq-day"><div class="rq-tnum" style="color:#dedede;">' + n[h].d + '</div><div  style="color:#dedede;" class="rq-tdnum">' + n[h].td + "</div></td>") : s += "<td></td>" : u >= m ? r ? (s += '<td class="rq-day"><div class="rq-tnum" style="color:#dedede;">' + r[d].d + '</div><div class="rq-tdnum" style="color:#dedede;">' + r[d].td + "</div></td>", d++) : s += "<td></td>" : (c = 1 == a[u].k ? 1 : 0, v = t == this.nowYear && e == this.nowMonth && a[u].d == this.nowDate, l = "", l = v ? 1 == c ? " rq-cur-g cur-day" : " rq-cur-n cur-day" : 1 == c ? " rq-kjday" : "", s += '<td class="rq-day" on' + CP.ev.click + '="CP.rq.setDay(' + t + "," + e + "," + a[u].d + ',this)"><div class="rq-tnum' + l + '" ifk="' + c + '">' + a[u].d + '</div><div class="rq-tdnum">' + a[u].td + "</div></td>", u++);
                s += "</tr>"
            }
            s += "</tbody></table>", $(this.id).html(s), $(".rq-select").val(t + "-" + e), this.setDay(this.nowYear, this.nowMonth, this.nowDate)
        },
        setDay: function(t, e, s, i) {
            if (i) {
                var a = $(".cur-day")[0];
                $(a).removeClass("cur-day"), $(a).removeClass("rq-kjday"), $(a).removeClass("rq-cur-g"), $(a).removeClass("rq-cur-n"), "1" == $(a).attr("ifk") && $(a).addClass("rq-kjday");
                var n = $(i).find(".rq-tnum")[0];
                $(n).addClass("cur-day"), $(n).removeClass("rq-kjday"), $(n).removeClass("rq-cur-g"), $(a).removeClass("rq-cur-n"), "1" == $(n).attr("ifk") ? $(n).addClass("rq-cur-g") : $(n).addClass("rq-cur-n")
            }
            $(".rq-show").empty();
            var r = _djson[t][e][s - 1],
                o = '<div class="rq-show-left">';
            o += '<div class="ltxt">' + t + "年" + (e < 10 ? "0" + e : e) + "月" + (s < 10 ? "0" + s : s) + "日</div>", o += '<div class="ltxt">' + this.week(r.w) + "</div>", o += '<div class="lnb">' + s + "</div>", 1 == r.k && (o += '<div class="ltxt" style="font-weight:bold;color:red;">【开奖日】</div>'), o += '<div class="ltxt">' + r.tm + r.td + "</div>", o += '<div class="ltxt">' + r.py + " 【" + r.sx + "年】</div>", o += '<div class="ltxt">' + r.pm + " " + r.pd + "</div>", o += '</div><div class="rq-show-right">', o += '<div class="rtxt"><em class="yi">宜</em>' + r.yi.replace(/\./g, "、") + "</div>", o += '<div class="rtxt"><em class="ji">忌</em>' + r.ji.replace(/\./g, "、") + "</div>", o += '<div class="rq-nextq">', o += '<div class="ltxt">下期开奖</div>', o += '<div class="ltxt" style="font-weight:bold;color:red;">' + this.nextQiYear + "年" + (this.nextQiMonth < 10 ? "0" + this.nextQiMonth : this.nextQiMonth) + "月" + (this.nextQiDate < 10 ? "0" + this.nextQiDate : this.nextQiDate) + "日</div>", o += '<div class="ltxt" style="font-weight:bold;color:red;">' + this.week(this.nextQiWeek) + " 21:30</div>", o += '<div class="rq-show-time"><em class="stime hour">00</em><em class="md">:</em><em class="stime minutes">00</em><em class="md">:</em><em class="stime second">00</em></div>', o += "</div>", o += "</div>", $(".rq-show").html(o)
        },
        week: function(t) { return ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][t] },
        moveTime: function() {
            var t = Date.UTC(this.nextQiYear, this.nextQiMonth - 1, this.nextQiDate, 21, 30, 0),
                e = new Date,
                s = Date.UTC(e.getFullYear(), e.getMonth(), e.getDate(), e.getHours(), e.getMinutes(), e.getSeconds());
            this.nextQiTimeSce = Math.ceil((t - s) / 1e3), setInterval(function() { CP.rq.runTime() }, 1e3)
        },
        runTime: function() {
            if (this.nextQiTimeSce > 0) {
                var t = CP.timeToStr(this.nextQiTimeSce--);
                $(".rq-show-time").length > 0 && ($(".hour").html(t.h), $(".minutes").html(t.i), $(".second").html(t.s))
            }
        }
    },
    timeToStr: function(t) {
        for (var e = i = s = 0;;) {
            if (t < 60) { s = t; break }
            if (s = t % 60, (t = (t - s) / 60) < 60) { i = t; break }
            i = t % 60, e = (t - i) / 60;
            break
        }
        var a = {};
        return a.h = e < 10 ? "0" + e : "" + e, a.i = i < 10 ? "0" + i : "" + i, a.s = s < 10 ? "0" + s : "" + s, a
    }
};
$(document).ready(function() { $.ajax({ type: "get", url: "/js/opendate.json", async: !0 }), "undefined" != typeof _djson && CP.rq.init() });