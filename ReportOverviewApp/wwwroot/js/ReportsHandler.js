
function EditReport(input) {
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
$(document).ready(function () {
    $("#ClearSearchForm").click(function () {
        //$("#SearchNameInput").val(null);
        $('input').val(null)
    });
    $("#beginDatepicker").datepicker();
    $("#endDatepicker").datepicker();
});

$(document).ready(function () {
    $("#ViewTableButton").click(function () {
        $("#ReportTable").toggleClass("table-condensed");
        //$("#ViewTableButtonIcon").toggleClass("glyphicon glyphicon-resize-full");
    });
});