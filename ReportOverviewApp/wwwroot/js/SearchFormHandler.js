$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        //$("#SearchNameInput").val(null);
        $('input').val(null)
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
});
