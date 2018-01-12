$('.input-group .input-group-append button').click(function () {
    $(this).parent().prev('input').val(new Date().toLocaleString());
});
function deleteReportDeadline() {
    $('button').prop('disabled', true);
    let formData = $('#editReportDeadlineForm').serializeArray().reduce(function (a, x) { a[x.name] = x.value; return a; }, {});
    let model = {
        __RequestVerificationToken: $("[name='__RequestVerificationToken']").val(),
        id: parseInt(formData.Id),
        deadline: formData.Deadline,
        runDate: formData.RunDate === "" ? null : formData.RunDate,
        approvalDate: formData.ApprovalDate === "" ? null : formData.ApprovalDate,
        sentDate: formData.SentDate === "" ? null : formData.SentDate,
        reportId: parseInt(formData.ReportId)
    };
    let request = $.ajax({
        type: 'DELETE',
        dataType: 'json',
        url: '/Reports/Deadlines/Delete/' + model.id,
        headers: {
            RequestVerificationToken: $("[name='__RequestVerificationToken']").val()
        },
        contentType: 'application/json; charset=utf-8'
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
    })
    request.fail(function (jqXHR, status, message) {
        $('#editReportDeadlineStatus').html('<div class="alert alert-danger ldt ldt-fade-in">' + status + ': ' + message + '\n' + JSON.stringify(model) + '</div>');
        $('#saveReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', false);
    })

}
function save() {
    $('button').prop('disabled', true);
    let formData = $('#editReportDeadlineForm').serializeArray().reduce(function (a, x) { a[x.name] = x.value; return a; }, {});
    let model = {
        __RequestVerificationToken: $("[name='__RequestVerificationToken']").val(),
        id: parseInt(formData.Id),
        deadline: formData.Deadline,
        runDate: formData.RunDate === "" ? null : formData.RunDate,
        approvalDate: formData.ApprovalDate === "" ? null : formData.ApprovalDate,
        sentDate: formData.SentDate === "" ? null : formData.SentDate,
        reportId: parseInt(formData.ReportId)
    };
    let request = $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Reports/Deadlines/Edit/',
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
    })
    request.fail(function (jqXHR, status, message) {
        $('#editReportDeadlineStatus').html('<div class="alert alert-danger ldt ldt-fade-in">' + status + ': ' + message + '\n' + JSON.stringify(model) + '</div>');
        $('#saveReportDeadline').find('i').removeClass('ld ld-heartbeat');
        $('button').prop('disabled', false);
    })
}
$('#deleteReportDeadline').click(function () {
    deleteReportDeadline();
});
$('#saveReportDeadline').click(function () {
    save();
});