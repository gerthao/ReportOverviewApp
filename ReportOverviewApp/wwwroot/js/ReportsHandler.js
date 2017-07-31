

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
    $('#CheckboxDone').click(function () {
        if ($('#InputDateDone').val() === '') {
            var date = new Date();
            $('#InputDateDone').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
        } else {
            $('#InputDateDone').val('');
        }
    });
    $('#CheckboxClientNotified').click(function () {
        if ($('#InputDateClientNotified').val() === '') {
            var date = new Date();
            $('#InputDateClientNotified').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
        } else {
            $('#InputDateClientNotified').val('');
        }
    });
    $('#CheckboxSent').click(function () {
        if ($('#InputDateSent').val() === '') {
            var date = new Date();
            $('#InputDateSent').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
        } else {
            $('#InputDateSent').val('');
        }
    });
    $('#InputDateDone').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#CheckboxDone').prop('checked', true);
        } else {
            $('#CheckboxDone').prop('checked', false);
        }
    });
    $('#InputDateClientNotified').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#CheckboxClientNotified').prop('checked', true);
        } else {
            $('#CheckboxClientNotified').prop('checked', false);
        }
    });
    $('#InputDateSent').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#CheckboxSent').prop('checked', true);
        } else {
            $('#CheckboxSent').prop('checked', false);
        }
    });

    
});