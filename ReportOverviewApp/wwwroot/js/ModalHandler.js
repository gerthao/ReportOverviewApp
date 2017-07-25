//var current = 0;

//function CreateReport(){
//    $('#CreateReportModal').on('shown.bs.modal', function () {
//        $('#ModalButton').focus()
//    });
//}

//function EditReport(cur){
//    $('body').on('click', 'a', function(event) {
//        current = $(this).closest('tr').index();
//        $('#Tester').val = current;
//    });
//};
var report;

function EditReport(input){
    $(document).ready(
        function(){
            $('tr').click(function() {
                var currentIndex = $(this).parent().children().index($(this));
                //var report = JSON.parse(input);
                $('#Tester').text(currentIndex);
            });
        }
    );
}

$('#Tester').click(function(){
    $('#Tester').text("Hello world.");
});