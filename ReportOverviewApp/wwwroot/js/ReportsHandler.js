

$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        $('input').val(null);
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
    $("#ViewTableButton").click(function () {
        $("#ReportTable").toggleClass("table-condensed");
    });
    $('#QuickFilter').keyup(function () {
    });
    //$('.checkbox').click(function () {
    //    var date = new Date();
    //    $(this).siblings("text").text((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
    //});

    
});