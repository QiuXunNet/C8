


//小提示框
function ShowTan(text) {

    $(this).dialog({
        type: 'notice',
        infoText: text,
        autoClose: 2000
    });
}


//去尾
function trimEnd(str) {
    return str.substr(0, str.length - 1);
}

/**
 * 重定向
 * @param {Url} url 
 * @returns {} 
 */
function redirect(url) {
    location.href = url;
}

/**
 * 隐藏自定义Header
 * @returns {} 
 */
function hideHeader() {

    var platform = getUrlParam('pl');

    if (typeof (platform) != 'undefined' && platform == 1) {
        $('.hjc_header').hide();
        $('.Rule_box').css({ 'margin-top': '0px' });
    }

}

/**
 * 获取请求参数
 * @param {参数名称} name 
 */
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}