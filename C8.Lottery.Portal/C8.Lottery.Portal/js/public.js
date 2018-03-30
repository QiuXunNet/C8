


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
