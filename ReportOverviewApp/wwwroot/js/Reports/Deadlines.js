
    $('#viewButton').click(function () {
        $('#cards').toggleClass('card-columns');
    });
    $('.panel-collapse').on('hidden.bs.collapse', function () {
        $(this).prev('.card-header').children('.options').children('.toggleIcon').children('i').removeClass('fa-chevron-down').addClass('fa-chevron-up');

    });
    $('.panel-collapse').on('shown.bs.collapse', function () {
        $(this).prev('.card-header').children('.options').children('.toggleIcon').children('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');
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
        return data.reduce(function (a, c) { (a[c.deadline] = a[c.deadline] || []).push(c); return a; }, {});
    }
    function groupByPlan(data) {
        let grouping =
            data.reduce(function (a, c) {
                (a[c.report.plans.map(function (x) { return x.name; }).join(', ')] = a[c.report.plans.map(function (x) { return x.name; }).join(', ')] || []).push(c); return a;
            }, {});
        return Object.keys(grouping).sort(function (a, b) { return a > b; }).map(function (e) {
            let arr = {};
            arr[e] = grouping[e];
            return arr;
        });
    }
    $('#changeButton').click(function () {
        $('#status').children().fadeOut(200);
        retriveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val(), $('#selectPlan').find(':selected').val());
    });
    function retrieveReports(year, month, plan, lastEditedDate) {
        $('#loadingIcon').html('<i class="fas fa-cog fa-2x ld ld-spin"></i>');
        $('button').prop('disabled', true);
        let query = {
            year: year,
            month: month === '0' ? null : month,
            plan: plan === '' ? null : plan
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
                history.replaceState(null, null, queryUrl.replace(/ /g, '+'));
                if ($(data).length > 0) {
                    let groups = groupByDeadline(data);
                    for (let i in groups) {
                        let planGrouping = groupByPlan(groups[i]);
                        groups[i] = planGrouping;
                    }
                    let items = [];
                    $.each(groups, function (deadline, plans) {
                        items.push(buildCard(deadline, plans, lastEditedDate));
                    });
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
    function convertDate(inputFormat, delimiter) {
        if (typeof delimiter !== 'string') {
            delimiter = '-';
        }
        function pad(s) { return s < 10 ? '0' + s : s; }
        let date = new Date(inputFormat);
        return [pad(date.getMonth() + 1), pad(date.getDate()), date.getFullYear()].join(delimiter);
    }
    function buildCard(key, data, lastEditedDate) {
        let keyDate = new Date(key);
        let card = $('#cardTemplate').children().clone(true);
        $(card).find('.toggleIcon').attr('data-target', '#' + 'list' + convertDate(key, '-'));
        $(card).find('.toggleIcon').attr('aria-controls', 'list' + convertDate(key, '-'));
        $(card).find('.panel-collapse').attr('id', 'list' + convertDate(key, '-'));
        $(card).find('.panel-collapse').addClass('list-collapsable');
        $(card).find('.deadline').text(convertDate(key, '/'));
        $(card).find('.reportCount').text('Reports:  ' + data.reduce(function (a, c) { return parseInt(Object.values(c).map(function (x) { return x.length; })) + parseInt(a); }, 0));
        let completionCount = data.map(function (e) { return Object.values(e)[0]; }).map(function (f) { return f.map(function (c) { return c.isComplete ? 1 : 0; }).reduce(function (q, w) { return q + w; }); }).reduce(function (e, r) { return e + r; });
        //doesn't seem to work in IE11//
        let completionRate = completionCount / data.reduce(function (a, c) { return parseInt(Object.values(c).map(function (x) { return x.length; })) + parseInt(a);}, 0) * 100;
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
                let planDeadlineComplete = n.map(function (c) { return c.isComplete; }).reduce(function (q, w) { return q && w; });
                let planDeadlineStatusIcon = planDeadlineComplete ? '<span class="float-right"><i class="far fa-check-circle text-success"></i></span>' : '<span class="float-right"><i class="far fa-circle text-info"></i></span>'
                reports.push('<li class="list-group-item plan-item bg-dark text-light" style="text-align:center; opacity:0.9;"><strong><span class="float-left">' + (k + 1) + '.</span>' + (m === '' ? 'No Associated Plans Found' : m) + planDeadlineStatusIcon + '</strong></li>');
                $.each(n, function (o, p) {
                    reports.push(buildCardContent(p));
                });
            });
        });
        $(card).find('.card-contents').html(reports);
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
        let okay = data.map(function (e) { return Object.values(e)[0]; }).map(function (f) { return f.map(function (c) { return c.isComplete; }).reduce(function (q, w) { return q && w; }); }).reduce(function (e, r) { return e && r; });
        $(card).find('.deadline').text(convertDate(dateKey, '/'));
        if (convertDate(dateKey, '/') === convertDate(date, '/')) {
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
        retrieveReports($('#selectYear').val(), $('#selectMonth').find(':selected').val(), $('#selectPlan').find(':selected').val());
    }
    $('.card-search').on('keyup', function () {
        let value = $(this).val().toLowerCase();
        $(this).parent().parent().find('.report-item').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    initialize();




