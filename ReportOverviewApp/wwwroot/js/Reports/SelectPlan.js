$(document).ready(function () {
    $('p.list-group-item.list-group-item-action').on('click', function () {
        let stateName = $(this).text();
        if ($(this).hasClass('bg-primary')) {
            $(this).removeClass('bg-primary');
            $.get('/Reports/GetSelectPlanList/', function (data) {
                history.replaceState(null, null, '/Reports/SelectPlan/');
                $('#selectPlanListViewComponent').hide(100, function () {
                    $('#selectPlanListViewComponent').html(data);
                });
                $('#selectPlanListViewComponent').show(333);

            });
            return;
        }
        $(this).addClass('bg-primary');
        $(this).siblings().removeClass('bg-primary');
        $.get('/Reports/GetSelectPlanList?State=' + stateName, function (data) {
            history.pushState(null, null, '/Reports/SelectPlan?State=' + stateName);
            $('#selectPlanListViewComponent').hide(100, function () {
                $('#selectPlanListViewComponent').html(data);
            });
            $('#selectPlanListViewComponent').show(333);

        });
    });
});