﻿
$(function () {


    /*右上角*/
    //$(".QiHao_tit").bind("click", function () {
    //    $(this).siblings(".QiHao_text").toggle();
    //});

   

    BindqueryDateLiClick();         //历史记录 日期选择

});

//function BindqueryDateLiClick() {
//    $('#queryDate li').bind('click', function() { 
//        var date = $(this).html();

//        $.post('/Record/OpenRecord', { date: date }, function (data) {
//            $('body').html(data);
//        });

//    });
//}


function BindqueryDateLiClick() {
    $('#queryDate').bind('change', function () {
        //var date = $(this).find("option:selected").text();
        var date = $("option:selected", this).attr("data-Date");
       
        $.get('/Record/OpenRecord', { date: date }, function (data) {
            $('body').html(data);
         
         
        });

    });
}