

$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        $('input').val(null);
    });
    $('#frequencyList li a').click(function () {
        $('#frequencyInput').val($(this).html());
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
    $("#dueDate1").datepicker();
    $("#dueDate2").datepicker();
    $("#dueDate3").datepicker();
    $("#dueDate4").datepicker();
    $("#ViewTableButton").click(function () {
        $("#ReportTable").toggleClass("table-condensed");
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
    var root = window.location.origin;
    $(document).ready(
        function () {
            $('#ReportTable tr').on("click", function () {
                //var currentIndex = $(this).parent().children().index($(this)) + 1;
                var retrievedID = $(this).find(".ReportID").html();
                var link = root + "/Reports/JsonInfo/" + retrievedID;
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
            });
        }
    );
    function handleJSONReport(data) {
        if (data === null) {
            alert("there is no data");
        } else {
            var report = '';
            report = JSON.stringify(data[0]);
            $('#Tester').text(report);
            for (var i in data[0]) {
                $('#' + i).val(data[0][i]);
                if (i === "dateDone" && data[0][i] != null) {
                    $('#done').prop('checked', true);
                } if (i === 'dateClientNotified' && data[0][i] != null) {
                    $('#clientNotified').prop('checked', true);
                } if (i === 'dateSent' && data[0][i] != null) {
                    $('#sent').prop('checked', true);
                }
            }
        }
    }
});