$('.input-group .input-group-append button').click(function () {
    $(this).parent().prev('input').val(formatDate(new Date()));
});
function deleteReportDeadline() {
    $('button').prop('disabled', true);
    let formData = $('#editReportDeadlineForm').serializeArray().reduce(function (a, x) { a[x.name] = x.value; return a; }, {});
    let model = {
        __RequestVerificationToken: $("[name='__RequestVerificationToken']").val(),
        id: parseInt(formData.Id),
        deadline: formatDate(new Date(formData.Deadline), 'date'),
        runDate: formData.RunDate === "" ? null : formatDate(new Date(formData.RunDate)),
        approvalDate: formData.ApprovalDate === "" ? null : formatDate(new Date(formData.RunDate)),
        sentDate: formData.SentDate === "" ? null : formatDate(new Date(formData.SentDate)),
        reportId: parseInt(formData.ReportId)
    };
    let request = $.ajax({
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(model),
        url: '/api/Deadlines/' + model.id,
        headers: {
            RequestVerificationToken: $("[name='__RequestVerificationToken']").val()
        }
        
    });
    request.done(function (data) {
        if (data.success) {
            if (data.update) {
                $('#editReportDeadlineStatus').html('<div class="alert alert-success ldt ldt-fade-in">Message: ' + data.message + '</div>');
            } else {
                $('#editReportDeadlineStatus').html('<div class="alert alert-info ldt ldt-fade-in">Message: ' + data.message + '</div>');
            }
        }
        $('#deleteReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', true);
        let saveDate = new Date(model.deadline);
        retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val(), convertDate(new Date(saveDate).toJSON()));
        $("#editReportDeadlineForm").find('input').prop('readonly', true);
        $("#editReportDeadlineForm").html('<div class="modal-header"><h5>Deleted</h5></div><div class="modal-body"></div>');
    });
    request.fail(function (jqXHR, status, message) {
        $('#editReportDeadlineStatus').html('<div class="alert alert-danger ldt ldt-fade-in">' + status + ': ' + message + '\n' + JSON.stringify(model) + '</div>');
        $('#saveReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', false);
    });
}
function formatDate(date, format) {
    if (date === undefined || date === null) {
        return null;
    }
    try {
        date = new Date(date);
    } catch (e) {
        console.error(e);
        return null;
    }
    if (typeof date !== typeof new Date()) {
        return null;
    }
    switch (format) {
        case 'date':
            return date.toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
        case 'time':
            return  date.toLocaleTimeString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
    }
    return date.toLocaleString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
}
function save() {
    $('button').prop('disabled', true);
    let formData = $('#editReportDeadlineForm').serializeArray().reduce(function (a, x) { a[x.name] = x.value; return a; }, {});
    let model = {
        __RequestVerificationToken: $("[name='__RequestVerificationToken']").val(),
        id: parseInt(formData.Id),
        deadline: formatDate(new Date(formData.Deadline), 'date'),
        runDate: formData.RunDate === "" ? null : formatDate(new Date(formData.RunDate)),
        approvalDate: formData.ApprovalDate === "" ? null : formatDate(new Date(formData.RunDate)),
        sentDate: formData.SentDate === "" ? null : formatDate(new Date(formData.SentDate)),
        reportId: parseInt(formData.ReportId)
    };
    let request = $.ajax({
        type: 'PUT',
        dataType: 'json',
        url: '/api/Deadlines/' + model.id,
        headers: {
            RequestVerificationToken: $("[name='__RequestVerificationToken']").val()
        },
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model)
    });
    request.done(function (data) {
        if (data.success) {
            if (data.update) {
                $('#editReportDeadlineStatus').html('<div class="alert alert-success ldt ldt-fade-in">Message: ' + data.message + '</div>');
            } else {
                $('#editReportDeadlineStatus').html('<div class="alert alert-info ldt ldt-fade-in">Message: ' + data.message + '</div>');
            }
        }
        $('#saveReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', false);
        let saveDate = new Date(model.deadline);
        retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val(), convertDate(new Date(saveDate).toJSON()));
    });
    request.fail(function (jqXHR, status, message) {
        $('#editReportDeadlineStatus').html('<div class="alert alert-danger ldt ldt-fade-in">' + status + ': ' + message + '\n' + JSON.stringify(model) + '</div>');
        $('#saveReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', false);
    });
}
$('#deleteReportDeadline').click(function () {
    $('#collapseDelete').collapse('show');
});
$('#confirmDelete').click(function () {
    deleteReportDeadline();
});
$('#unconfirmDelete').click(function () {
    $('#collapseDelete').collapse('hide');
});
$('#saveReportDeadline').click(function () {
    $('#collapseDelete').collapse('hide');
    save();
});