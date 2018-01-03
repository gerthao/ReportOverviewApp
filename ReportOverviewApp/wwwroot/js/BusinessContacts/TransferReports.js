$(document).ready(function () {
    function loadItems(origin, destination) {
        $('#loading').html('<div>Loading...</div>');
        $.ajax({
            type: 'GET',
            url: '/Data/GetBusinessContactReports?id=' + origin,
            dataType: 'json',
            success: function (data) {
                let items = [];
                $.each(data, function (key, value) {
                    items.push('<option value="' + key + '">' + value + '</option>');
                });
                $(destination).html(items);
                $('#loading').html('Reports');
            },
            error: function () {
                $(destination).html('<option disabled>(Empty)</option>');
            }
        });
    }
    loadItems($('#owner').find(':selected').val(), '#ownerReports');
    loadItems($('#recipient').find(':selected').val(), '#recipientReports');
    checkDuplicate();

    function moveItems(origin, destination) {
        $(origin).find(':selected').prop('selected', false).appendTo(destination);
    }
    function moveAllItems(origin, destination) {
        $(origin).children().appendTo(destination);
    }
    function swapItems(first, second) {
        let items_1 = $(first).find(':selected');
        let items_2 = $(second).find(':selected');
        items_1.appendTo(second);
        items_2.appendTo(first);
    }
    function save(first, second, firstIds, secondIds) {
        let firstReportIds = [];
        let secondReportIds = [];
        $(firstIds).children().each(function () {
            firstReportIds.push(parseInt($(this).val()));
        });
        $(secondIds).children().each(function () {
            secondReportIds.push(parseInt($(this).val()));
        });

        let viewModel = {
            __RequestVerificationToken: $("[name='__RequestVerificationToken']").val(),
            first: parseInt($(first).val()),
            second: parseInt($(second).val()),
            firstReports: firstReportIds,
            secondReports: secondReportIds
        };
        let request = $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/BusinessContacts/TransferReports',
            headers: {
                RequestVerificationToken: $("[name='__RequestVerificationToken']").val()
            },
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(viewModel),
        });
        request.done(function (data) {
            $('#ownerReports').children().prop('selected', false);
            $('#recipientReports').children().prop('selected', false);
            if (data.success) {
                if (data.update) {
                    $('#status').html('<div class="alert alert-success ldt ldt-fade-in">Message: ' + data.message + '</div>');
                } else {
                    $('#status').html('<div class="alert alert-info ldt ldt-fade-in">Message: ' + data.message + '</div>');
                }
            }
            $('#save').find('i').removeClass('ld ld-heartbeat');
            $('button').prop('disabled', false);
        })
        request.fail(function (jqXHR, status, message) {
            $('#status').html('<div class="alert alert-danger ldt ldt-fade-in">' + status + ': ' + message + '</div>');
            $('#save').find('i').removeClass('ld ld-heartbeat');
            $('button').prop('disabled', false);
        })
    }
    function checkDuplicate() {
        if ($('#owner').find(':selected').val() === $('#recipient').find(':selected').val()) {
            $('button').prop('disabled', true);
        } else {
            $('button').prop('disabled', false);
        }
    }
    $('#owner').change(function () {
        loadItems($('#owner').find(':selected').val(), '#ownerReports');
        loadItems($('#recipient').find(':selected').val(), '#recipientReports');
        checkDuplicate();
    })
    $('#recipient').change(function () {
        loadItems($('#owner').find(':selected').val(), '#ownerReports');
        loadItems($('#recipient').find(':selected').val(), '#recipientReports');
        checkDuplicate();
    })
    $('#transferRight').click(function () {
        moveItems('#ownerReports', '#recipientReports');
    });
    $('#transferLeft').click(function () {
        moveItems('#recipientReports', '#ownerReports');
    });
    $('#transferSwap').click(function () {
        swapItems('#ownerReports', '#recipientReports');
    });
    $('#swapBusinessContacts').click(function () {
        swapItems('#owner', '#recipient');
    });
    $('#save').hover(function () {
        $(this).children(i).toggleClass('ld ld-heartbeat');
    });
    $('#save').click(function () {
        $(this).find('i').addClass('ld ld-heartbeat');
        $('button').prop('disabled', true);
        save('#owner', "#recipient", "#ownerReports", "#recipientReports");
    });

});