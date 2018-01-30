﻿$('#viewButton').click(function () {
      
        $('#cards').toggleClass('card-columns');

    
});
$('.panel-collapse').on('hidden.bs.collapse', function () {
    $(this).prev('.card-header').children('.options').children('.toggleIcon').children('i').removeClass('fa-chevron-down').addClass('fa-chevron-up');
    //$(this).parent('div.card').appendTo('#cards');
    //if ($('#selectedCard').html()){
    //    $('#selectedCard').css('padding-bottom', '0rem');
    //}
   
});
$('.panel-collapse').on('shown.bs.collapse', function () {
    $(this).prev('.card-header').children('.options').children('.toggleIcon').children('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');
    //$(this).parent('div.card').appendTo('#selectedCard');
    //$('#selectedCard').css('padding-bottom', '1.2rem');
});
$('#showAllButton').click(function () { $('.list-collapsable').collapse('show'); });
$('#hideAllButton').click(function () { $('.list-collapsable').collapse('hide'); });

//$('.markIncomplete').click(function () {
//    unmarkAll($(this).parent().prev('.deadline').text());
//});
//$('.markAll').click(function () {
//    let icon = $(this).find('i').attr('class');
//    let date = $(this).parent().prev('.deadline').text();
//    if (icon === 'fas fa-check-circle') {
//        markAll(date);
//    } else if (icon === 'fas fa-minus-circle') {
//        unmarkAll(date);
//    } else return false;
//    return true;
//});
//function markAll(date) {
//    $.ajax({
//        type: 'POST',
//        url: '/api/Deadlines/MarkAsComplete/' + String(formatDate(new Date(date)), 'date') + '/true',
//        data: JSON.stringify(formatDate(new Date(date), 'date')),
//        contentType: 'application/json; charset=UTF-8',
//        dataType: 'json',
//        success: function (data) {
//            let alertMsg = $('#alertSuccessTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(data.message);
//            $('#status').html(alertMsg);
//            retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val());
//        },
//        error: function (a, b, c) {
//            let alertMsg = $('#alertDangerTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(c);
//            $('#status').html(alertMsg);
//        }
//    });
//}
//function formatDate(date, format) {
//    if (date === undefined || date === null) {
//        return null;
//    }
//    try {
//        date = new Date(date);
//    } catch (e) {
//        console.error(e);
//        return null;
//    }
//    if (typeof date !== typeof (new Date())) {
//        return null;
//    }
//    switch (format) {
//        case 'date':
//            return date.toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
//        case 'time':
//            return date.toLocaleTimeString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
//    }
//    return date.toLocaleString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
//}
//function unmarkAll(date) {
//    $.ajax({
//        type: 'POST',
//        url: '/api/Deadlines/MarkAsComplete/' + String(formatDate(new Date(date)), 'date') + '/false',
//        data: JSON.stringify(formatDate(new Date(date), 'date')),
//        dataType: 'json',
//        contentType: 'application/json; charset=UTF-8',
//        success: function (data) {
//            let alertMsg = $('#alertSuccessTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(data.message);
//            $('#status').html(alertMsg);
//            retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val());
//        },
//        error: function (a, b, c) {
//            let alertMsg = $('#alertDangerTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(c);
//            $('#status').html(alertMsg);
//        }
//    });
//}
//function deleteAll(date) {
//    $.ajax({
//        type: 'DELETE',
//        url: '/api/Deadlines/DeleteAll',
//        data: JSON.stringify(formatDate(new Date(date))),
//        contentType: 'application/json',
//        dataType: 'json',
//        success: function (data) {
//            let alertMsg = $('#alertWarningTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(data.message);
//            $('#status').html(alertMsg);
//            retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val());
//        },
//        error: function (a, b, c) {
//            let alertMsg = $('#alertDangerTemplate').children().clone(true);
//            $(alertMsg).find('.alertMessage').html(c);
//            $('#status').html(alertMsg);
//        }
//    });
//}
//$('.deleteAll').click(function () {
//    deleteAll($(this).parent().prev('.deadline').text());
//});
function groupByDeadline(data) {
    return data.reduce((a, c) => { (a[c.deadline] = a[c.deadline] || []).push(c); return a; }, {});
}
function groupByPlan(data) {
    let qwer =
        data.reduce((a, c) => {
            (a[c.report.plans.map(x => x.name).reduce((b, d) => b + d, '')] = a[c.report.plans.map(x => x.name).reduce((b, d) => b + d, '')] || []).push(c); return a;
        }, {});
    return Object.keys(qwer).map(function (e) {
        let arr = {};
        arr[e] = qwer[e];
        return arr;
    });
}
$('#changeButton').click(function () {
    $('#status').children().fadeOut(200);
    retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val());
});
function retriveReports(year, month, lastEditedDate) {
    $('#loadingIcon').html('<i class="fas fa-cog fa-2x ld ld-spin"></i>');
    $('button').prop('disabled', true);
    let query = {
        year: year,
        month: month
    };
    $.ajax({
        type: 'GET',
        url: '/api/Deadlines',
        data: query,
        dataType: 'json',
        success: function (data) {
            let queryString = '';
            let queryUrl = '/Reports/Deadlines';
            let mapping = Object.keys(query).map(function (key) {
                if (query[key] !== null)
                    return key + '=' + query[key];
                else return null;
            });
            queryString = mapping.reduce(function (a, c) {
                if (a === null) return c;
                if (c === null) return a;
                return a + '&' + c;
            });
            if (queryString !== '') {
                queryUrl += '?' + queryString;
            }
            history.replaceState(null, null, queryUrl);
            if ($(data).length > 0) {
                //let groups = groupByDeadline(data);
                //);
                let groups = groupByDeadline(data);
                //let groups = Object.values(groupData).reduce((a, c) => { (a[(c.map(e => e.deadline)[0])] = a[(c.map(e => e.deadline)[0])] || []).push(groupByPlan(c)); return a; }, {});
                for (let i in groups) {
                    let qwer = groupByPlan(groups[i]);
                    groups[i] = qwer;
                }
                let items = [];
                $.each(groups, function (deadline, plans) {
                    console.log(plans);
                    items.push(buildCard(deadline, plans, lastEditedDate));
                });
                //console.log(items);
                //$(groups).each(function (i, e) {
                //    $.each(e, function (j, f) {
                //        items.push(buildCard(j, f, lastEditedDate));
                //    });
                //});
                $('#loadingIcon').find('i').fadeOut(200, function () {
                    
                    $('#cards').html(items);

                    
                });
            } else {
                $('#loadingIcon').find('i').fadeOut(200, function () {
                    $('#status').html('<div class="alert alert-warning">Message: No deadlines found</div>');
                    $('#cards').children().hide();
                });
            }
            $('button').prop('disabled', false);
        },
        error: function (a, b, c) {
            $('#loadingIcon').find('i').fadeOut(200, function () {
                $('#status').html('<div class="alert alert-danger">' + c + '</div>');
                $('#cards').children().hide();
            });
            $('button').prop('disabled', false);
        }
    });

}
function convertDate(inputFormat) {
    function pad(s) { return s < 10 ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
}
function buildCard(key, data, lastEditedDate) {
    let keyDate = new Date(key);
    let card = $('#cardTemplate').children().clone(true);
    $(card).find('.toggleIcon').attr('data-target', '#' + 'list' + convertDate(key).replace('/', '-').replace('/', '-'));
    $(card).find('.toggleIcon').attr('aria-controls', 'list' + convertDate(key).replace('/', '-').replace('/', '-'));
    $(card).find('.panel-collapse').attr('id', 'list' + convertDate(key).replace('/', '-').replace('/', '-'));
    $(card).find('.panel-collapse').addClass('list-collapsable');
    $(card).find('.deadline').text(convertDate(key));
    //console.log(data.reduce((a, c) => parseInt(Object.values(c).map(x => x.length)) + parseInt(a), 0));
    $(card).find('.reportCount').text('Reports:  ' + data.reduce((a, c) => parseInt(Object.values(c).map(x => x.length)) + parseInt(a), 0));
    //console.log(data.map(e => Object.values(e)[0]).map(f => f.map(c => c.isComplete ? 1 : 0).reduce((q, w) => q + w)).reduce((e, r) => e + r));
    //let completionMap = data.map(rd => rd.isComplete ? 1 : 0);
    let completionCount = data.map(e => Object.values(e)[0]).map(f => f.map(c => c.isComplete ? 1 : 0).reduce((q, w) => q + w)).reduce((e, r) => e + r);
    let completionRate = completionCount / data.reduce((a, c) => parseInt(Object.values(c).map(x => x.length)) + parseInt(a), 0) * 100;
    $(card).find('.completionCount').text('Completed:  ' + completionCount);
    $(card).find('.completionRate').text('Completion Rate:  ' + String(completionRate).substring(0, 5) + '%');
    $(card).find('.progress-bar').css('width', completionRate + '%').attr('aria-valuenow', completionRate).attr('aria-valuemin', 0).attr('aria-valuemax', 100);
    if (convertDate(new Date(key)) === lastEditedDate) {
        $(card).find('.panel-collapse').addClass('show');
        $(card).find('.toggleIcon').find('i').removeClass('fas fa-chevron-up').addClass('fas fa-chevron-down');
    }
    let reports = [];
    $.each(data, function (k, l) {
        
        $.each(l, function (m, n) {
            reports.push('<li class="list-group-item" style="text-align:center;"><strong><span class="float-left">[' + (k+1) + ']</span>' +  (m === '' ? 'No Associated Plans Found' : m) + '</strong></li>');
            $.each(n, function (o, p) {
                
                reports.push(buildCardContent(p));
            });
        });
    });
    $(card).find('.card-contents').html(reports);
    console.log(data);
    setCardStatus(card, key, data);
    return card;
}
function buildCardContent(reportDeadline) {
    let content = $('#cardItemListTemplate').children().clone(true);
    $(content).find('.reportName').text(reportDeadline.report.name);
    setIcon($(content).find('.ran'), reportDeadline.hasRun);
    setIcon($(content).find('.approved'), reportDeadline.isApproved);
    setIcon($(content).find('.sent'), reportDeadline.isSent);
    $(content).find('.frequency').text(reportDeadline.report.frequency);
    setRouteId($(content).find('.editReportDeadlineLink'), reportDeadline.id);
    setRouteId($(content).find('.editReportLink'), reportDeadline.report.id);
    setRouteId($(content).find('.viewReportLink'), reportDeadline.report.id);
    setRouteId($(content).find('.deleteReportLink'), reportDeadline.report.id);
    return content;
}
function groupBy(items, key) {
    return items.reduce(
        function (acc, x) { (acc[x[key]] = acc[x[key]] || []).push(x); return acc; }, {});
}
function setRouteId(link, id) {
    let url = $(link).attr('href');
    $(link).attr('href', url + '/' + id);

}
function setCardStatus(card, key, data) {
    let date = new Date();
    let dateKey = new Date(key);
    //let okay = true;
    //$.each(data, function (k, l) { okay = okay && checkMissing(l); });
    let okay = data.map(e => Object.values(e)[0]).map(f => f.map(c => c.isComplete).reduce((q, w) => q && w)).reduce((e, r) => e && r);
    $(card).find('.deadline').text(convertDate(dateKey));
    if (convertDate(dateKey) === convertDate(date)) {
        $(card).find('.deadline').append(' (Today)');
        if (!okay) {
            $(card).find('.card-header').addClass('text-dark');
            $(card).addClass('bg-warning');
        } else {
            $(card).find('.card-header').addClass('text-light');
            $(card).find('.card-footer').addClass('text-light');
            $(card).addClass('bg-success');
            //$(card).find('.markAll').find('i').removeClass('fas fa-check-circle').addClass('fas fa-minus-circle');

        }
        $(card).addClass('bg-warning');
        $(card).find('.card-header').addClass('text-dark');
    } else if (dateKey < date) {
        if (!okay) {
            $(card).find('.card-header').addClass('text-light');
            $(card).find('.card-footer').addClass('text-light');
            $(card).addClass('bg-danger');
        } else {
            $(card).find('.card-header').addClass('text-light');
            $(card).find('.card-footer').addClass('text-light');
            $(card).addClass('bg-success');
            //$(card).find('.markAll').find('i').removeClass('fas fa-check-circle').addClass('fas fa-minus-circle');

        }
    }
    else {
        if (okay) {
            $(card).find('.card-header').addClass('text-light');
            $(card).find('.card-footer').addClass('text-light');
            $(card).addClass('bg-success');
            //$(card).find('.markAll').find('i').removeClass('fas fa-check-circle').addClass('fas fa-minus-circle');

        }
    }

}
function checkMissing(report) {
    return report.hasRun && report.isApproved && report.isSent;
}
function setIcon(icon, data) {
    if (data) {
        $(icon).html('<i class="far fa-check-circle text-success"></i>');
    } else {
        $(icon).html('<i class="far fa-circle text-info"></i>');
    }
}
function initialize() {
    retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val());
}
initialize();