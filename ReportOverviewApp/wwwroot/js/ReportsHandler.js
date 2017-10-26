//Javascript file meant to handle the views for the Reports controller.//
'use strict';

$(document).ready(function () {
    $('#updateDeadlines').click(function () {
        $(this).text('Updating...');
    })
    $("#clearSearchForm").click(function () {
        $('input').val(null);
    });
    $('#frequencyList a').click(function () {
        $('#frequencyInput').val($(this).html());
    });
    $('#stateList a').click(function () {
        $('#stateInput').val($(this).html());
    });
    $('#planList a').click(function () {
        $('#planInput').val($(this).html());
    });
    $('#sourceDepartmentList a').click(function () {
        $('#sourceDepartmentInput').val($(this).html());
    });
    $('#businessContactList a').click(function () {
        $('#businessContactInput').val($(this).html());
    });
    $('#businessOwnerList a').click(function () {
        $('#businessOwnerInput').val($(this).html());
    });
    if (navigator.userAgent.indexOf('Trident/') !== -1) {
        $(".input-datepicker").datepicker();
    }
    function checkLessThanTen(number) {
        if (number < 10) {
            number = '0' + number;
        } return number;
    }
    function getDateTimeNow() {
        let date = new Date();
        let dateString;
        let month = date.getMonth() + 1;
        let day = date.getDate();
        let year = date.getFullYear();
        let hour = checkLessThanTen(date.getHours());
        let minute = checkLessThanTen(date.getMinutes());
        let second = checkLessThanTen(date.getSeconds());
        dateString = month + '/' + day + '/' + year + ' ' + hour + ':' + minute + ':' + second;
        return dateString;
    }
    //not implemented yet//
    function handleCheckboxDateTimeOnClick(checkboxInput, dateInput) {
        $(checkboxInput).click(function () {
            if ($(dateInput).val() === '' && $(checkboxInput + ':checked').val()) {
                $(dateInput).val(getDateTimeNow());
                return;
            }
            if ($(dateInput).val() !== '' && !$(checkboxInput + ':checked').val()) {
                $(dateInput).val('');
            }
        });
    }
    //not implemented yet//
    function handleCheckboxDateTimeOnInput(checkboxInput, dateInput) {
        $(dateInput).on('input', function (e) {
            if ($(this).val() !== '') {
                $(checkboxInput).prop('checked', true);
            } else {
                $(checkboxInput).prop('checked', false);
            }
        });
    }
    $('#editReport_done').click(function () {
        if ($('#editReport_dateDone').val() === '' && $('#editReport_done:checked').val()) {
            $('#editReport_dateDone').val(getDateTimeNow());
            return;
        }
        if ($('#editReport_dateDone').val() !== '' && !$('#editReport_done:checked').val()) {
            $('#editReport_dateDone').val('');
        }
    });
    $('#editReport_clientNotified').click(function () {
        if ($('#editReport_dateClientNotified').val() === '' && $('#editReport_clientNotified:checked').val()) {
            $('#editReport_dateClientNotified').val(getDateTimeNow());
            return;
        } if ($('#editReport_dateClientNotifed').val() !== '' && !$('#editReport_clientNotified:checked').val()) {
            $('#editReport_dateClientNotified').val('');
        }
    });
    $('#editReport_sent').click(function () {
        if ($('#editReport_dateSent').val() === '' && $('#editReport_sent:checked').val()) {
            $('#editReport_dateSent').val(getDateTimeNow());
            return;
        } if ($('#editReport_dateSent').val() !== '' && !$('#editReport_sent:checked').val()) {
            $('#editReport_dateSent').val('');
        }
    });
    $('#edit_dateDone').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#editReport_done').prop('checked', true);
        } else {
            $('#editReport_done').prop('checked', false);
        }
    });
    $('#editReport_dateClientNotified').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#editReport_clientNotified').prop('checked', true);
        } else {
            $('#editReport_clientNotified').prop('checked', false);
        }
    });
    $('#editReport_dateSent').on('input', function (e) {
        if ($(this).val() !== '') {
            $('#editReport_sent').prop('checked', true);
        } else {
            $('#editReport_sent').prop('checked', false);
        }
    });
    $('.modal').on('hidden.bs.modal', function () {
        let findForm = $(this).find('form');
        if (findForm[0] === null || findForm[0] === undefined) {
            return;
        }
        findForm[0].reset();
    });

    let root = window.location.origin;
    //$('#reportTable tr td a').on("click", function () {
    //    let retrievedID = $(this).parent().parent().find(".ReportID").html();
    //    let action = $(this).attr('id');
    //    let link = root + "/Data/GetReport/" + retrievedID;
    //    try {
    //        $.ajax({
    //            url: link,
    //            type: "GET",
    //            dataType: 'json',
    //            contentType: 'application/json; charset=utf-8',
    //            success: function (data) {
    //                handleJSONReport(data, action);
    //            },
    //            error: function () {
    //                console.error("failed: " + link);
    //            }
    //        });
    //    } catch (err) {
    //        console.error(err);
    //    }
    //    updateFrequencyFields();
    //});
    $('#reportTable tr td a').on("click", function () {
        let retrievedID = $(this).parent().parent().find(".ReportID").html();
        let action = $(this).attr('id');
        let link = root + "/Reports/EditReport?id=" + retrievedID;
        $.get(link, function (data) {

            $('#editReportContainer').html(data);
        })
    });
    function handleJSONReport(data, action) {
        if (data === null) {
            console.log("there is no data");
        } else {
            for (let i in data[0]) {
                if (action === 'editReport') {
                    $('#' + action + '_' + i).val(data[0][i]);
                    if (i === "dateDone" && data[0][i] !== null) {
                        $('#' + action + '_done').prop('checked', true);
                    } if (i === 'dateClientNotified' && data[0][i] !== null) {
                        $('#' + action + '_clientNotified').prop('checked', true);
                    } if (i === 'dateSent' && data[0][i] !== null) {
                        $('#' + action + '_sent').prop('checked', true);
                    }
                }
                if (action === 'deleteReport') {
                    if (i === 'id') {
                        $('#' + action + '_' + i).val(data[0][i]);
                    } else {
                        $('#' + action + '_' + i).html(data[0][i]);
                    }
                    
                }
            }
        }
    }
    function updateFrequencyFields() {
        switch ($('#edit_frequency').val()) {
            case 'Weekly':
            case 'Biweekly':
            case 'Monthly':
                $('#editReport_dueDate1').parent().parent().hide();
                $('#editReport_dueDate2').parent().parent().hide();
                $('#editReport_dueDate3').parent().parent().hide();
                $('#editReport_dueDate4').parent().parent().hide();
                $('#editReport_daysAfterQuarter').parent().parent().hide();
                break;
            case 'Quarterly':
                $('#editReport_dueDate1').parent().parent().show();
                $('#editReport_dueDate2').parent().parent().show();
                $('#editReport_dueDate3').parent().parent().show();
                $('#editReport_dueDate4').parent().parent().show();
                $('#editReport_daysAfterQuarter').parent().parent().show();
                break;
            case 'Semiannual':
                $('#editReport_dueDate1').parent().parent().show();
                $('#editReport_dueDate2').parent().parent().show();
                $('#editReport_dueDate3').parent().parent().hide();
                $('#editReport_dueDate4').parent().parent().hide();
                $('#editReport_daysAfterQuarter').parent().parent().hide();
                break;
            case 'Annual':
                $('#editReport_dueDate1').parent().parent().show();
                $('#editReport_dueDate2').parent().parent().hide();
                $('#editReport_dueDate3').parent().parent().hide();
                $('#editReport_dueDate4').parent().parent().hide();
                $('#editReport_daysAfterQuarter').parent().parent().hide();
                break;
        }
    }
    updateFrequencyFields();
    $('#frequency').on('input', function () {
        updateFrequencyFields();
    });
});