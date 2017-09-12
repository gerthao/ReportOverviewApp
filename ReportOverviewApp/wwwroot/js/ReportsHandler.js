'use strict';

$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        $('input').val(null);
    });
    $('#frequencyList a').click(function () {
        $('#frequencyInput').val($(this).html());
    });
    $('#stateList a').click(function () {
        $('#stateInput').val($(this).html());
    });
    $('#planList li a').click(function () {
        $('#planInput').val($(this).html());
    });
    $('#beginDateInput').datepicker();
    $("#endDateInput").datepicker();    
    //$("#dueDate1").datepicker();
    //$("#dueDate2").datepicker();
    //$("#dueDate3").datepicker();
    //$("#dueDate4").datepicker();
    function checkLessThanTen(number) {
        if (number < 10) {
            number = '0' + number;
        } return number;
    }
    function getDateTimeNow() {
        var date = new Date();
        var dateString;
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var year = date.getFullYear();
        var hour = checkLessThanTen(date.getHours());
        var minute = checkLessThanTen(date.getHours());
        var second = checkLessThanTen(date.getSeconds());
        dateString = month + '/' + day + '/' + year + ' ' + hour + ':' + minute + ':' + second;
        return dateString;
    }
    $('#done').click(function () {
        if ($('#dateDone').val() === '' && $('#done:checked').val()) {
            $('#dateDone').val(getDateTimeNow());
            return;
        }
        if ($('#dateDone').val() !== '' && !$('#done:checked').val()) {
            $('#dateDone').val('');
        }
    });
    $('#clientNotified').click(function () {
        if ($('#dateClientNotified').val() === '' && $('#clientNotified:checked').val()) {
            $('#dateClientNotified').val(getDateTimeNow());
            return;
        } if ($('#dateClientNotifed').val() !== '' && !$('#clientNotified:checked').val()) {
            $('#dateClientNotified').val('');
        }
    });
    $('#sent').click(function () {
        if ($('#dateSent').val() === '' && $('#sent:checked').val()) {
            $('#dateSent').val(getDateTimeNow());
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
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
    });
    
    var root = window.location.origin;
    $('#reportTable tr td a').on("click", function () {
        var retrievedID = $(this).parent().parent().find(".ReportID").html();
        //var retrievedName = $(this).parent().parent().find(".ReportName");
        var link = root + "/Data/GetReport/" + retrievedID;
        try {
            $.ajax({
                url: link,
                type: "GET",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    handleJSONReport(data);
                },
                error: function () {
                    alert("failed: " + link);
                }
            });
        } catch (err) {
            alert(err);
        }
        updateFrequencyFields();
    });
    function handleJSONReport(data) {
        if (data === null) {
            alert("there is no data");
        } else {
            var report = '';
            report = JSON.stringify(data[0]);
            for (var i in data[0]) {
                $('#' + i).val(data[0][i]);
                if (i === "dateDone" && data[0][i] !== null) {
                    $('#done').prop('checked', true);
                } if (i === 'dateClientNotified' && data[0][i] !== null) {
                    $('#clientNotified').prop('checked', true);
                } if (i === 'dateSent' && data[0][i] !== null) {
                    $('#sent').prop('checked', true);
                }
            }
        }
    }
    function updateFrequencyFields() {
        switch ($('#frequency').val()) {
            case 'Weekly':
            case 'Biweekly':
            case 'Monthly':
                $('#dueDate1').parent().parent().hide();
                $('#dueDate2').parent().parent().hide();
                $('#dueDate3').parent().parent().hide();
                $('#dueDate4').parent().parent().hide();
                $('#daysAfterQuarter').parent().parent().hide();
                break;
            case 'Quarterly':
                $('#dueDate1').parent().parent().show();
                $('#dueDate2').parent().parent().show();
                $('#dueDate3').parent().parent().show();
                $('#dueDate4').parent().parent().show();
                $('#daysAfterQuarter').parent().parent().show();
                break;
            case 'Semiannual':
                $('#dueDate1').parent().parent().show();
                $('#dueDate2').parent().parent().show();
                $('#dueDate3').parent().parent().hide();
                $('#dueDate4').parent().parent().hide();
                $('#daysAfterQuarter').parent().parent().hide();
                break;
            case 'Annual':
                $('#dueDate1').parent().parent().show();
                $('#dueDate2').parent().parent().hide();
                $('#dueDate3').parent().parent().hide();
                $('#dueDate4').parent().parent().hide();
                $('#daysAfterQuarter').parent().parent().hide();
                break;
        }
    }
    updateFrequencyFields();
    $('#frequency').on('input', function () {
        updateFrequencyFields();
    });
});