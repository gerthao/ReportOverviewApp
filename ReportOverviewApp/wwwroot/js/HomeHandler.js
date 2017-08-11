'use strict';
var root = window.location.origin;
setInterval(getUserLogs(), 5000);
//setInterval(function () { alert("what up?") }, 3000);
$(document).ready(function () {
    getUserLogs();
});

function getUserLogs() {
    var link = root + "/Home/GetUserLogs/";
    try {
        $.ajax({
            url: link,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset-utf-8",
            success: function (data) {
                handleJSON(data);
            },
            error: function () {
                alert("failed: " + link);
            }
        });
    } catch (err) {
        alert(err);
    }
}
function handleJSON(data) {
    var body = '';
    if (data === null) {
        body = '<tr class="col-sm-12"><td class="col-sm-12">No Logs To Display</td></tr>';
    }
    else {
        //var logs = '';
        //logs = JSON.stringify(data[0]);
        for (var i = 0; i < data.length; i++) {
            var msg = '<td class="col-sm-7">' + data[i]["message"] + '</td>';
            var usrID = '<td class="col-sm-2">' + data[i]["userID"] + '</td>';
            var tmStmp = '<td class="col-sm-3">' + data[i]["timeStamp"] + '</td>';
            body += '<tr>';
            body += (msg + tmStmp + usrID);
            body += '</tr>';
        }
    }
    $('#userLogsBody').html(body);
}