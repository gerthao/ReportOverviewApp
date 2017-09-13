'use strict';
var root = window.location.origin;
var countArray = [0, 0, 0];
var reportsArray = ["", "", ""];
var getCurrentTime = function () {
    $.get("/Home/TimeViewComponent", function (data) { $("#timeViewComponentContainer").html(data); });
};
var getUserLogs = function () {
    var link = root + "/Data/GetUserLogs/";
    try {
        $.ajax({
            url: link,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset-utf-8",
            success: function(data) {
                handleJsonUserLogs(data);
            },
            error: console.error("failed: " + link)
        });
    } catch (err) {
        console.error(err);
    }
};
var getReportCount = function () {
    var link = root + "/Data/GetDeadlines/";
    try {
        $.ajax({
            url: link,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset-utf-8",
            success: function(data) {
                deadlineCount(data);
            },
            error: console.error("failed: " + link)
        });
    } catch (err) {
        console.error(err);
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
    $('#TotalReportCount').html(data.length);
    var daily = data.filter(function (n) {
        if (n === null || n.reportDeadline === null) return false;
        return n.reportDeadline.substring(0, 19) === today
    });
    $('#todayReportCount').html(daily.length);
    var week = new Date();
    week.setDate(week.getDate() + 7);
    week = getDateTimeNow(week);
    var weekly = data.filter(function (n) {
        if (n === null || n.reportDeadline === null) return false;
        if (n.reportDeadline === null) return false;
        return n.reportDeadline.substring(0, 19) >= today && n.reportDeadline.substring(0, 19) <= week;
    });
    $('#weekReportCount').html(weekly.length);
    countArray[0] = data.length;
    countArray[2] = daily.length;
    countArray[1] = weekly.length;
    reportsArray[0] = createReportListGroup(data);
    reportsArray[1] = createReportListGroup(daily);
    reportsArray[2] = createReportListGroup(weekly);
    $('#widgetTabs li a').each(function (index, value) {
        if (value.className.indexOf("active") !== -1){
            handleReportCount(value);
            handleReportsList(value);
        }
    });
}
var createReportListGroup = function(jsonReportData){ 
    const limit = 30;
    var reportString = "";
    for (var i = 0; i < jsonReportData.length && i < limit; i++) {
        reportString += '<li class="list-group-item">' + jsonReportData[i].reportName + "</li>";
    }
    if (jsonReportData.length > limit) {
        reportString += '<li class="list-group-item bg-primary">' + (jsonReportData.length - limit) + ' More Reports' + '</li>';
    }
    reportString += "</ul>";
    return reportString;
};
var handleReportsList = function(htmlElement) {
    switch (htmlElement.id) {
        case "totalTab":
            $('#reportCard').html(reportsArray[0]);
            break;
        case "todayTab":
            $('#reportCard').html(reportsArray[1]);
            break;
        case "weeklyTab":
            $('#reportCard').html(reportsArray[2]);
            break;
        default:
            break;
    }
    if ($('#reportCard').text() === "") {
        $('#reportCard').html("<h5>No Reports To Display</h5>");
    }
}
var updateComponents = function() {
    getUserLogs();
    getReportCount();
}
$(document).ready(function () {
    updateComponents();
    $(function(){ window.setInterval(updateComponents, 5000); });
    $("#widgetTabs li a").on("click", function() {
        $(this).parent("li").parent("ul").children("li").each(function(index, value) {
            $(value.tagName + " a").removeClass("active");
        });
        $(this).addClass("active");
        handleReportCount(this);
        handleReportsList(this);
    });
});
function handleReportCount(htmlElement, reports) {
    var today = new Date();
    var nextWeek = new Date();
    nextWeek.setDate(nextWeek.getDate() + 7);
    switch (htmlElement.id) {
        case "totalTab":
            $("#reportCount").html(countArray[0]);
            $("#widgetTitle").html("Total Reports");
            $("#reportLink").html('<a href="../Reports/Index/">' +'Go to reports'+'</a>');
            break;
        case "todayTab":
            $("#reportCount").html(countArray[2]);
            $("#widgetTitle").html("Reports Due Today");
            $("#reportLink").html('<a href="../Reports/Index/?' + getHtmlDate('begin', today) + getHtmlDate('&end', today) + '">' + 'Go to reports' + '</a>');
            break;
        case "weeklyTab":
            $("#reportCount").html(countArray[1]);
            $("#widgetTitle").html("Reports Due Within One Week");
            $("#reportLink").html('<a href="../Reports/Index/?' + getHtmlDate('begin', today) + getHtmlDate('&end', nextWeek) + '">' + 'Go to reports' + '</a>');
            break;
        default:
            break;
    }
}
function getHtmlDate(name, date) {
    var dateString;
    var month = checkLessThanTen(date.getMonth() + 1);
    var day = checkLessThanTen(date.getDate());
    var year = date.getFullYear();
    dateString = month + '%2F' + day + '%2F' + year;
    return name + '=' + dateString;
}
function handleJsonUserLogs(data) {
    var body = '';
    if (data === null || data.length === 0) {
        body = '<tr><td class="col-sm-7">No Logs To Display</td>' +
            '<td class="col-sm-2"></td>' +
            '<td class="col-sm-3"></td></tr>';
    }
    else {
        data.sort(function(a, b){
            return b.timeStamp.localeCompare(a.timeStamp);
        });
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