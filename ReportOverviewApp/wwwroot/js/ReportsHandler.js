//Javascript file meant to handle the views for the Reports controller.//
'use strict';

$(document).ready(function () {
    $('#updateDeadlines').click(function () {
        $(this).text('Updating...');
    });
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
    $('#reportTable tr td #editReport').on("click", function () {
        let retrievedID = $(this).parent().parent().find(".ReportID").html();
        let action = $(this).attr('id');
        let link = root + "/Reports/EditReport?id=" + retrievedID;
        $('#editReportContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="fa fa-5x fa-refresh ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#editReportContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
    $('#reportTable tr td #editReportDeadline').on("click", function () {
        let retrievedID = $(this).parent().parent().find(".ReportID").html();
        let action = $(this).attr('id');
        let link = root + "/Reports/EditReportDeadline?id=" + retrievedID;
        $('#editReportDeadlineContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="fa fa-5x fa-refresh ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#editReportDeadlineContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
    $('#reportTable tr td #deleteReport').on("click", function () {
        let retrievedID = $(this).parent().parent().find(".ReportID").html();
        let action = $(this).attr('id');
        let link = root + "/Reports/DeleteReport?id=" + retrievedID;
        $('#deleteReportContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="fa fa-5x fa-refresh ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#deleteReportContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
});