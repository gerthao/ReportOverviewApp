'use strict';

let countArray = [0, 0, 0];
let reportsArray = ["", "", ""];

function getUserLogs() {
    //let link = "/Data/GetUserLogs/";
    //$.ajax({
    //    url: link,
    //    type: "GET",
    //    dataType: "json",
    //    contentType: "application/json; charset-utf-8",
    //    success: function(data) {
    //        handleJsonUserLogs(data);
    //    },
    //    error: console.error("failed: " + link)
    //});
    $.get("/Home/GetUserLogs", function (data) {
        $("#userLogListContainer").html(data);
    });
}
function getReportCount() {
    let link = "/Data/GetDeadlines/";
    $.ajax({
        url: link,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset-utf-8",
        success: function (data) {
            deadlineCount(data);
        },
        error: console.error("failed: " + link)
    });
}
function checkLessThanTen(number) {
    if (number < 10) {
        number = '0' + number;
    } return number;
}
function getDateTimeNow(date) {
    let dateString;
    let month = date.getMonth() + 1;
    let day = date.getDate();
    let year = date.getFullYear();
    dateString = year + '-' + checkLessThanTen(month) + '-' + checkLessThanTen(day) + 'T00:00:00';
    return dateString;
}
function getTimestamp(date) {
    let dateString;
    let month = checkLessThanTen(date.getMonth() + 1);
    let day = checkLessThanTen(date.getDate());
    let year = date.getFullYear();
    let hour = date.getHours();
    let minute = checkLessThanTen(date.getMinutes());
    let second = checkLessThanTen(date.getSeconds());
    let ampm;
    if (hour < 12) {
        ampm = "AM";
    } else {
        ampm = "PM";
        if (hour > 12) {
            hour = hour - 12;
        }
    }
    dateString = month + '/' + day + '/' + year + ' ' + checkLessThanTen(hour) + ':' + minute + ':' + second + ' ' + ampm;
    return dateString;
}
function deadlineCount(data) {
    let today = new Date();
    today = getDateTimeNow(today);
    $('#TotalReportCount').html(data.length);
    let daily = data.filter(function (n) {
        if (n === null || n.reportDeadline === null) return false;
        return n.reportDeadline.substring(0, 19) === today;
    });
    $('#todayReportCount').html(daily.length);
    let week = new Date();
    week.setDate(week.getDate() + 7);
    week = getDateTimeNow(week);
    let weekly = data.filter(function (n) {
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
        if (value.className.indexOf("active") !== -1) {
            handleReportCount(value);
            handleReportsList(value);
        }
    });
}
function createReportListGroup(jsonReportData){ 
    const limit = 30;
    let reportString = "";
    for (let i = 0; i < jsonReportData.length && i < limit; i++) {
        reportString += '<li class="list-group-item">' + jsonReportData[i].reportName + "</li>";
    }
    if (jsonReportData.length > limit) {
        reportString += '<li class="list-group-item"><strong>' + (jsonReportData.length - limit) + ' More Reports' + '</strong></li>';
    }
    reportString += "</ul>";
    return reportString;
}
function handleReportsList(htmlElement) {
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
        $('#reportCard').html("<h5>No reports to display</h5>");
    }
}
var updateComponents = function () {
    getUserLogs();
    getReportCount();
    setTimeout(updateComponents, 5000);
};
var getCurrentDateTime = function() {
    $.get("/Home/GetCurrentDateTime", function (data) {
        $("#currentDateTimeContainer").html(data);
    });
    //let link = "Home/GetCurrentDateTime";
    //$.ajax({
    //    url: link,
    //    dataType: "html",
    //    type: "GET",
    //    success: function (data) {
    //        $("#currentDateTimeContainer").html(data);
    //    },
    //    error: $("#currentDateTimeData").html("failed: " + link)
    //});
};
var updateDateTime = function () {
    getCurrentDateTime();
    setTimeout(updateDateTime, 1000);
};

$(document).ready(function () {
    //updateDateTime();
    updateComponents();
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
    let today = new Date();
    let nextWeek = new Date();
    nextWeek.setDate(nextWeek.getDate() + 7);
    switch (htmlElement.id) {
        case "totalTab":
            $("#reportCount").html(countArray[0]);
            $("#widgetTitle").html("Total Reports");
            $("#reportLink").html('<a class="btn btn-outline-primary" href="../Reports/Index/">' +'Go to reports'+'</a>');
            break;
        case "todayTab":
            $("#reportCount").html(countArray[2]);
            $("#widgetTitle").html("Reports Due Today");
            $("#reportLink").html('<a class="btn btn-outline-primary" href="../Reports/Index/?' + getHtmlDate('begin', today) + getHtmlDate('&end', today) + '">' + 'Go to reports' + '</a>');
            break;
        case "weeklyTab":
            $("#reportCount").html(countArray[1]);
            $("#widgetTitle").html("Reports Due Within One Week");
            $("#reportLink").html('<a class="btn btn-outline-primary" href="../Reports/Index/?' + getHtmlDate('begin', today) + getHtmlDate('&end', nextWeek) + '">' + 'Go to reports' + '</a>');
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
    let body = '';
    if (data === null || data.length === 0) {
        body =
            '<tr><td colspan="4">No logs to display</td></tr>';
    }
    else {
        data.sort(function(a, b){
            return b.timeStamp.localeCompare(a.timeStamp);
        });
        for (let i = 0; i < data.length; i++) {
            let temp = new Date(data[i]["timeStamp"]);
            temp = getTimestamp(temp);
            let msg = '<td>' + data[i]["message"] + '</td>';
            let changesMessage;
            if (data[i]["changes"] === null || data[i]["changes"] === undefined) {
                changesMessage = 'N/A';
            } else {
                changesMessage = '<button type="button" class="btn btn-sm btn-block btn-link fa fa-list-ul"></i>';
            }
            let changes = '<td class="text-center">' + changesMessage + '</td>';
            let usrID = '<td>' + data[i]["userID"] + '</td>';
            let tmStmp = '<td>' + temp + '</td>';
            body += '<tr>' + msg + tmStmp + usrID + changes + '</tr>';
        }
    }
    $('#userLogsBody').html(body);
}