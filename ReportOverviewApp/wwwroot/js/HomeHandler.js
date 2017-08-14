'use strict';
var root = window.location.origin;
var getCurrentTime = function () {
    $.get("/Home/TimeViewComponent", function (data) { $("#timeViewComponentContainer").html(data); });
};
var getUserLogs = function () {
    var link = root + "/Home/GetUserLogs/";
    try {
        $.ajax({
            url: link,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset-utf-8",
            success: function (data) {
                handleJsonUserLogs(data);
            },
            error: function () {
                alert("failed: " + link);
            }
        });
    } catch (err) {
        alert(err);
    }
};
var getReportCount = function () {
    var link = root + "/Reports/GetDeadlines/";
    try {
        $.ajax({
            url: link,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset-utf-8",
            success: function (data) {
                deadlineCount(data);
            },
            error: function () {
                alert("failed: " + link);
            }
        });
    } catch (err) {
        alert(err);
    }
}
function checkLessThanTen(number) {
    if (number < 10) {
        number = '0' + number;
    } return number;
}
function getDateTimeNow(date) {
    var dateString;
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var year = date.getFullYear();
    dateString = year + '-' + checkLessThanTen(month) + '-' + checkLessThanTen(day) + 'T00:00:00';
    return dateString;
}
function getTimestamp(date) {
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
var deadlineCount = function (data) {
    var today = new Date();
    today = getDateTimeNow(today);
    $('#totalReportCount').html(data.length);
    var daily = data.filter(function (n) {
        if (n == null) return false;
        return n.substring(0, 19) == today;
    });
    $('#todayReportCount').html(daily.length);
    var week = new Date();
    week.setDate(week.getDate() + 7);
    week = getDateTimeNow(week);
    var weekly = data.filter(function (n) {
        //alert(n + ", " + today + ", " + week);
        //alert(n >= today && n <= week);
        if (n == null) return false;
        return n.substring(0, 19) >= today && n.substring(0, 19) <= week;
    });
    $('#weekReportCount').html(weekly.length);
}
var updateComponents = function () {
    getUserLogs();
    getReportCount();
}
$(document).ready(function () {
    updateComponents();
    $(function () { window.setInterval(updateComponents, 5000); });
});
function handleJsonUserLogs(data) {
    var body = '';
    if (data === null || data.length === 0) {
        body = '<tr class="col-sm-12"><td class="col-sm-12">No Logs To Display</td></tr>';
    }
    else {
        data.sort(function (a, b) {
            return b.timeStamp.localeCompare(a.timeStamp);
        })
        for (var i = 0; i < data.length; i++) {
            var temp = new Date(data[i]["timeStamp"]);
            temp = getTimestamp(temp);
            var msg = '<td class="col-sm-7">' + data[i]["message"] + '</td>';
            var usrID = '<td class="col-sm-2">' + data[i]["userID"] + '</td>';
            var tmStmp = '<td class="col-sm-3">' + temp + '</td>';
            body += ('<tr>' + msg + tmStmp + usrID + '</tr>');
        }
    }
    $('#userLogsBody').html(body);
}