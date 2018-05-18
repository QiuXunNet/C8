var shareData = {
    title: "邀请注册",
    desc: "万彩吧，助你壕梦成真！！新用户注册有惊喜",
    // 如果是微信该link的域名必须要在微信后台配置的安全域名之内的。
    link: "http://" + host,
    icon: "http://" + img,
    // 不要过于依赖以下两个回调，很多浏览器是不支持的
    success: function () {
        alert("success");
    },
    fail: function () {
        alert("fail");
    }
};

nativeShare.setShareData(shareData);

function call(command) {
    try {
        nativeShare.call(command);
    } catch (err) {
        // 如果不支持，你可以在这里做降级处理
        alert(err.message);
    }
}

function setTitle(title) {
    nativeShare.setShareData({
        title: title
    });
}