function getdown(type) {
    if (type == 2) {
        if (navigator.userAgent.indexOf("UCBrowser") > -1) {
            alert("不支持uc浏览器下载,建议使用系统自带浏览器");
            return;
        }
    }
    var url = getdata(type);
    window.location.href = url;
}

function getdata(ClientSource) {
    var url;
    $.ajax({
        type: "Get",
        url: "/Home/GetDown",
        data: "ClientSource=" + ClientSource,
        async: false,
        success: function (data) {
            if (data.Success == true) {
                if (data.data.length > 0) {
                    url = data.data[0].UpdateLink;
                }
            } else {
                alertmsg("暂无下载数据包");
            }
        }
    });
    return url;
}