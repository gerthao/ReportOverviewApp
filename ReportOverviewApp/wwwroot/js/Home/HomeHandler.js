function groupByDeadline(data) {
    return data.reduce(function (a, c) { (a[c.deadline] = a[c.deadline] || []).push(c); return a; }, {});
}
let dates = ['x'];
let deadlines = ['Deadlines'];
$.getJSON('api/Deadlines', function (data) {
    let grouping = groupByDeadline(data);
    Object.keys(grouping).forEach(function (k) {
        dates.push(new Date(k));
        deadlines.push(grouping[k].length);
    });
    let chart = c3.generate({
        bindto: '#chart',
        data: {
            x: 'x',
            columns: [
                dates,
                deadlines
            ]

        },
        bar: {
            width: {
                ratio: 0.25
            }
        },
        grid: {
            x: {
                show: true

            },
            y: {
                show: true

            }

        },
        zoom: {
            enabled: true
        },
        axis: {
            x: {
                type: 'timeseries',
                tick: {
                    format: '%Y-%m-%d'
                }
            }
        }
    });
});