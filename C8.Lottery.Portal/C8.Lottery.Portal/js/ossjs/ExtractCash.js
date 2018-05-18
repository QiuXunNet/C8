var bankList = [
           { key: "中国工商银行", value: "ICBC" },
           { key: "中国银行", value: "BOC" },
           { key: "中国建设银行", value: "CCB" },
           { key: "中国农业银行", value: "ABC" },
           { key: "中国邮政储蓄银行", value: "PSBC" },
           { key: "中国光大银行", value: "CEB" },
           { key: "交通银行", value: "COMM" },
           { key: "招商银行", value: "CMB" },
           { key: "中国民生银行", value: "CMBC" },
           { key: "兴业银行", value: "CIB" },
           { key: "中信银行", value: "CITIC" },
           { key: "广发银行", value: "GDB" },
           { key: "华夏银行", value: "HXBANK" },
           { key: "浦发银行", value: "SPDB" },
           { key: "平安银行", value: "SPABANK" }
];

$(function () {
    for (var i in bankList) {
        if (bankList[i].key == bankName) {
            $("#bankImg").attr("src", "/images/Banklogo/" + bankList[i].value + ".png");
        }
    }
});

function extractCash() {
    var money = $.trim($("#txtNumber").val());

    var regex = /^\d+\.\d+$/;
    var b = regex.test(money);
    if (b) {
        alert("请输入整数金额");
        return;
    }

    if (money > MyCommission) {
        alert("您输入的提现金额大于可提现佣金");
        return;
    }
    if (money < MinExtractCash) {
        alert("您输入的提现金额小于最低提现金额");
        return;
    }

    $.post("/Amount/AddExtractCash", { backId: backId, money: money }, function (data) {
        if (data.Status == 2) {
            alert("您的实际佣金已变动,请刷新页面后重新提交");
        }
        if (data.Status == 1) {
            document.location.href = "/Amount/ExtractCashSuccess?money=" + money;
        }
        if (data.Status == 0) {
            alert("提现申请失败");
        }
    });
}

//全部提现
function all() {
    $("#txtNumber").val(MyCommission);
}