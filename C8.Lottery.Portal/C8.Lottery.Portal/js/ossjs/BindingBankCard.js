var backName = "";
var backAccount = "";

$(function () {
    if ($("#txtBankAccount").val().length > 0) {
        getBackType($("#txtBankAccount").val());
    }
    $("#txtBankAccount").keyup(function () {
        if ($.trim($(this).val()).length >= 14) {
            getBackType($.trim($(this).val()));
        }
    });
    $("#txtBankAccount").blur(function () {
        if ($.trim($(this).val()).length >= 14) {
            getBackType($.trim($(this).val()));
        }
    });
});

//根据银行卡号，获取银行卡所属银行
function getBackType(no) {
    var obj = {};
    $.ajax({
        url: "/Amount/CheckBankAccount?BankAccount=" + no,
        type: "get",
        success: function (data) {
            obj = eval("(" + data + ")");
            if (obj.validated && obj.cardType == "DC") {
                var backDate = getBackNameOrImg(obj.bank);
                if (backDate.backName != "") {
                    var div = $(".Withd_DL");
                    div.show();
                    div.find(".backImg").attr("src", backDate.backImg);
                    div.find(".backName").text(backDate.backName);
                    div.find(".backAccount").text(obj.key.substr(obj.key.length - 4, 4));

                    backName = backDate.backName;
                    backAccount = obj.key;
                }
                else {
                    backName = "";
                    backAccount = "";
                    $(".Withd_DL").hide();
                }
            }
            else {
                backName = "";
                backAccount = "";
                $(".Withd_DL").hide();
            }
        }

    })
}

function successCallback(result, methodName) {
    alert(1);
    var html = '<ul>';
    for (var i = 0; i < result.length; i++) {
        html += '<li>' + result[i] + '</li>';
    }
    html += '</ul>';
    document.getElementById('divCustomers').innerHTML = html;
}


function add() {
    if (backAccount == "" || backName == "") {
        alert("请输入正确的银行卡账号");
        return;
    }
    var trueName = $.trim($("#txtUserName").val());
    if (trueName.length == 0) {
        alert("请输入姓名");
        return;
    }

    if (confirm("请核对你的银行是否无误")) {
        $.post("/Amount/AddBankCard", { BankAccount: backAccount, BankName: backName, trueName: trueName }, function (data) {
            if (data.Status == 1) {
                alert("绑定成功");
                document.location.href = "/Personal/MyCommission"
            }
            else {
                alert("绑定失败");
            }
        });
    }
}
function getBackNameOrImg(bank) {
    var obj = {};
    switch (bank) {
        case "ICBC":
            obj.backName = "中国工商银行";
            break;
        case "BOC":
            obj.backName = "中国银行";
            break;
        case "CCB":
            obj.backName = "中国建设银行";
            break;
        case "ABC":
            obj.backName = "中国农业银行";
            break;
        case "PSBC":
            obj.backName = "中国邮政储蓄银行";
            break;
        case "CEB":
            obj.backName = "中国光大银行";
            break;
        case "COMM": //DDD
            obj.backName = "交通银行";
            break;
        case "CMB":
            obj.backName = "招商银行";
            break;
        case "CMBC":
            obj.backName = "中国民生银行";
            break;
        case "CIB":
            obj.backName = "兴业银行";
            break;
        case "CITIC"://DDD
            obj.backName = "中信银行";
            break;
        case "GDB"://ddd
            obj.backName = "广发银行";
            break;
        case "HXBANK"://ddd
            obj.backName = "华夏银行";
            break;
        case "SPDB":
            obj.backName = "浦发银行";
            break;
        case "SPABANK":
            obj.backName = "平安银行";
            break;
        default:
            obj.backName = "";
            break;
    }
    obj.backImg = webHost + "/images/Banklogo/" + bank + ".png";
    return obj;
}


