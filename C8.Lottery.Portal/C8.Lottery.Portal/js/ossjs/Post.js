$(function () {
    var lType = $('#lType').val();
    var target;
    var targetName;
    $.post('/Plan/AlreadyPostData', {
        id: lType
    }, function (data) {
        initBetting(qiu, data);
        for (var i = 0; i < data.length; i++) {
            var d = data[i];
            var playName = d.BigPlayName;
            var betNum = d.BetNum;
            if (d.lType == 5) {
                if (d.BigPlayName == "二十码中特" || d.BigPlayName == "十码中特" || d.BigPlayName == "平五码" || d.BigPlayName == "五不中" || d.BigPlayName == "六不中") {
                    var betNumArr = betNum.split(',');
                    var betNumResArr = [];
                    $.each(betNumArr, function (index, item) {
                        var num = GetColorByLhc(item);
                        betNumResArr.push(num);
                    });
                    betNum = betNumResArr.join(' ');
                }
            }
            targetName = $('.WanFa_TableA h3:contains(' + playName + ')');
            $.each(targetName, function (index, item) {
                var curItem = $(item);
                var itemName = curItem.html();
                if (itemName != playName) return true;
                target = curItem.parent().next();
                var html = target.html();
                if (html == '') {
                    target.html(betNum);
                } else {
                    target.html(html + '|' + betNum);
                }
            });
        }
    });
});