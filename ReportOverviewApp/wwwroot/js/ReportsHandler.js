

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
    $('#done').click(function () {
        if ($('#dateDone').val() === '' && $('#done:checked').val()) {
                var date = new Date();
                $('#dateDone').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                return;
        }
        if ($('#dateDone').val() !== '' && !$('#done:checked').val()) {
            $('#dateDone').val('');
        }
    });
    $('#clientNotified').click(function () {
        if ($('#dateClientNotified').val() === '' && $('#clientNotified:checked').val()) {
            var date = new Date();
            $('#dateClientNotified').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            return;
        } if ($('#dateClientNotifed').val() !== '' && !$('#clientNotified:checked').val()){
            $('#dateClientNotified').val('');
        }
    });
    $('#sent').click(function () {
        if ($('#dateSent').val() === '' && $('#sent:checked').val()) {
            var date = new Date();
            $('#dateSent').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            return;
        } if ($('#dateSent').val() !== '' && !$('#sent:checked').val()) {
            $('#dateSent').val('');
        }
    });
    $('#dateDone').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#done').prop('checked', true);
        } else {
            $('#done').prop('checked', false);
        }
    });
    $('#dateClientNotified').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#clientNotified').prop('checked', true);
        } else {
            $('#clientNotified').prop('checked', false);
        }
    });
    $('#dateSent').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#sent').prop('checked', true);
        } else {
            $('#sent').prop('checked', false);
        }
    });

    
});