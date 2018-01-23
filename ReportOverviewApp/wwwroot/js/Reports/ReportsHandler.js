//Javascript file meant to handle the views for the Reports controller.//
'use strict';

$(document).ready(function () {
    $('#updateDeadlines').click(function () {
        $(this).find('i').addClass('ld ld-spin');
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
    $('.modal').on('hidden.bs.modal', function () {
        let findForm = $(this).find('form');
        if (findForm[0] === null || findForm[0] === undefined) {
            return;
        }
        findForm[0].reset();
        
    });
    var stateObj = { foo: "bar" };
    $('.editReportLink').on("click", function () {
        let link = $(this).attr('href');
        $('#editReportContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="fas fa-5x fa-cog ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#editReportContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
    $('.editReportDeadlineLink').on("click", function () {
        let link = $(this).attr('href');
        $('#editReportDeadlineContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="loading-icon fa fa-5x fa-cog ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#editReportDeadlineContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
    $('.deleteReportLink').on("click", function () {
        let link = $(this).attr('href');
        $('#deleteReportContainer').html('<div class="col-md-12" style="text-align: center; padding-top: 50%; padding-bottom: 50%; position: absolute;"><i class="loading-icon fa fa-5x fa-cog ld ld-spin"></i></div>');
        $.get(link, function (data) {
            $('#deleteReportContainer').fadeOut(500, function () {
                $(this).html(data);
                $(this).fadeIn(500);
            });
        });
    });
});