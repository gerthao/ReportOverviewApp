var uri = 'Reports/Index';

function EditReport(input) {
    $(document).ready(
        function(){
            $('tr').click(function() {
                var currentIndex = $(this).parent().children().index($(this))+1;
                var report;
                try {
                    report = JSON.parse(input);
                } catch (err) {
                    report = err + "\n\n" + input;
                }
                $('#Tester').text(report);
            });
        }
    );
}

$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        //$("#SearchNameInput").val(null);
        $('input').val(null)
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
    $("#ViewTableButton").click(function () {
        $("#ReportTable").toggleClass("table-condensed");
        //$("#ViewTableButtonIcon").toggleClass("glyphicon glyphicon-resize-full");
    });
    //$('.checkbox').click(function () {
    //    var date = new Date();
    //    $(this).siblings("text").text((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
    //});
});