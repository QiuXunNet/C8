var lType = 1;
var playName = '';
var betNum = '';
var betInfo = '';
var currentIssue = '';



$(function () {

    lType = $('#lType').val();      //初始化彩种类型

    BindconfirmBetCLick();          //确认投注


});


function BindconfirmBetCLick() {
    $('#confirmBet').click(function () {

        var betCount = parseInt($('.WF_ftTitle:visible span').html());

        if (betCount <= 0) {
            ShowTan('请选择号码');
        } else {

            betInfo = '';       //重置

            $('table:visible tr').each(function () {

                betNum = $(this).find('td:eq(1)').html();

                if (betNum != '') {
                    playName = $(this).find('td:eq(0)').find('h3').html();
                    if (playName == '二十码中特' || playName == '十码中特' || playName == '平五码' || playName == '五不中' || playName == '六不中') {

                        betNum = '';
                        $(this).find('td:eq(1)').find('span').each(function () {

                            betNum += $(this).html() + ',';
                        });

                        betNum = trimEnd(betNum);
                    }
                    betInfo += playName + "*" + betNum + "$";
                }
            });

            betInfo = trimEnd(betInfo);

            currentIssue = $('#currentIssue').html();

            //投注
            $.post('/Plan/Bet', { lType: lType, betInfo: betInfo, currentIssue: currentIssue }, function () {
                ShowTan('发帖成功');
            });

        }






    });
}
