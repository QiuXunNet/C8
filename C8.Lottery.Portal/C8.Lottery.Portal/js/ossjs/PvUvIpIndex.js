﻿if (linkCode != "") {
    var ip = returnCitySN["cip"];
    var cookieUv = getCookie("WcbCookieUv" + linkCode);
    //alert(cookieUv);
    if (cookieUv == undefined || cookieUv == null || cookieUv == "") {
        setCookie("WcbCookieUv" + linkCode, "wc8");
        //发送请求保存 UV
        var data = {};
        data.type = "post";
        data.data = {
            linkCode: linkCode,
            type: "uv"
        };
        data.url = "/PvUvIp/AddUv";
        ajaxJSON(data);
        //  $.post("/PvUvIp/AddUv", );
    }
    var cookieIp = getCookie("WcbCookieIp" + linkCode);
    //alert(cookieIp);
    if (cookieIp == undefined || cookieIp == null || cookieIp == "" || cookieIp != ip) {
        setCookie("WcbCookieIp" + linkCode, ip);
        //发送请求保存 Ip  需要后台判断Ip是否相同
        var data = {};
        data.type = "post";
        data.data = {
            linkCode: linkCode,
            type: "ip",
            ip: ip
        };
        data.url = "/PvUvIp/AddIp";
        ajaxJSON(data);
        // $.post("/PvUvIp/AddIp", { linkCode: linkCode, type: "ip", ip: ip });
    }
    var data = {};
    data.type = "post";
    data.data = {
        linkCode: linkCode,
        type: "pv"
    };
    data.url = "/PvUvIp/AddPv";
    ajaxJSON(data);
    //  $.post("/PvUvIp/AddPv", { linkCode: linkCode, type: "pv" });
}
//添加cookie
function setCookie(c_name, value) {
    var exdate = new Date();
    var endTimeStr = exdate.getFullYear() + "/" + (exdate.getMonth() + 1) + "/" + exdate.getDate() + " 23:59:59";
    var endTime = new Date(endTimeStr);
    document.cookie = c_name + "=" + escape(value) + ";expires=" + endTime.toGMTString() + "; path=/";;
}
//获取cookie

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}
//发送异步请求

function ajaxJSON(params) {
    params.type = (params.type || 'GET').toUpperCase();
    params.data = params.data || {};
    var formatedParams = this.formateParams(params.data, params.cache);
    var xhr;
    //创建XMLHttpRequest对象
    if (window.XMLHttpRequest) {
        //非IE6
        xhr = new XMLHttpRequest();
    } else {
        xhr = new ActiveXObject('Microsoft.XMLHTTP');
    }
    //异步状态发生改变，接收响应数据
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (!!params.success) {
                if (typeof xhr.responseText == 'string') {
                    params.success(JSON.parse(xhr.responseText));
                } else {
                    params.success(xhr.responseText);
                }
            }
        } else {
            params.error && params.error(status);
        }
    }
    if (params.type == 'GET') {
        //连接服务器
        xhr.open('GET', (!!formatedParams ? params.url + '?' + formatedParams : params.url), true);
        //发送请求
        xhr.send();
    } else {
        //连接服务器
        xhr.open('POST', params.url, true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        //发送请求
        xhr.send(formatedParams);
    }
}

function formateParams(data, isCache) {
    var arr = [];
    for (var name in data) {
        arr.push(encodeURIComponent(name) + '=' + encodeURIComponent(data[name]));
    }
    if (isCache) {
        arr.push('v=' + (new Date()).getTime());
    }
    return arr.join('&');
}