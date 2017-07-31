

$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        $('input').val(null);
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
    $("#DueDate1").datepicker();
    $("#DueDate2").datepicker();
    $("#DueDate3").datepicker();
    $("#DueDate4").datepicker();
    $("#ViewTableButton").click(function () {
        $("#ReportTable").toggleClass("table-condensed");
    });
    $('#QuickFilter').keyup(function () {
    });
    $('#CheckboxDone').click(function () {
        if ($('#InputDateDone').val() === '' && $('#CheckboxDone:checked').val()) {
                var date = new Date();
                $('#InputDateDone').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                return;
        }
        if ($('#InputDateDone').val() !== '' && !$('#CheckboxDone:checked').val()) {
            $('#InputDateDone').val('');
        }
    });
    $('#CheckboxClientNotified').click(function () {
        if ($('#InputDateClientNotified').val() === '' && $('#CheckboxClientNotified:checked').val()) {
            var date = new Date();
            $('#InputDateClientNotified').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            return;
        } if ($('#InputDateClientNotifed').val() !== '' && !$('#CheckboxClientNotified:checked').val()){
            $('#InputDateClientNotified').val('');
        }
    });
    $('#CheckboxSent').click(function () {
        if ($('#InputDateSent').val() === '' && $('#CheckboxSent:checked').val()) {
            var date = new Date();
            $('#InputDateSent').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            return;
        } if ($('#InputDateSent').val() !== '' && !$('#CheckboxSent:checked').val()) {
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