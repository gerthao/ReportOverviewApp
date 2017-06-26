var current = 0;

function CreateReport(){
    $('#CreateReportModal').on('shown.bs.modal', function () {
        $('#ModalButton').focus()
    });
}

function EditReport(id) {
    current = id;
    $('#EditReportModal').on('shown.bs.modal', function () {
        $('#EditOption').focus()
    });
}

function GetCurrentReport() {
    return current;
}