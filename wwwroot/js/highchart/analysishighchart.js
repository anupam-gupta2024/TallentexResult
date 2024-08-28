'use strict';

function GetJSON_Simple() {
    var resp = [];
    $.ajax({
        type: "GET",
        url: '../analysisajax/getScore',
        async: false,
        contentType: "application/json",
        success: function (data) {
            resp.push(data);
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");
        }
    });
    return resp;
}

var data = GetJSON_Simple();
//console.log(data[0].percentages);
const _categories = [];
for (const s of data[0].subjects) {
    _categories.push(s);
}

const _your = [];
for (const s of data[0].your) {
    _your.push(parseInt(s));
}

const _topper = [];
for (const s of data[0].topper) {
    _topper.push(parseInt(s));
}

const _average = [];
for (const s of data[0].average) {
    _average.push(parseInt(s));
}


Highcharts.chart('container', {

    title: {
        text: 'Comparison of Test Score'
    },
    xAxis: {
        categories: _categories
    },
    yAxis: {
        title: { text: 'Marks' }
    },
    labels: {
        items: [{
            //                                                html: 'Total Score',
            style: {
                left: '50px',
                top: '18px',
                color: ( // theme
                    Highcharts.defaultOptions.title.style &&
                    Highcharts.defaultOptions.title.style.color
                ) || 'black'
            }
        }]
    },
    series: [{
        type: 'column',
        name: 'Your',
        data: _your,
        color: Highcharts.getOptions().colors[0], // Your Score color
        dataLabels: {
            enabled: true
        }
    }, {
        type: 'column',
        name: 'Topper',
        data: _topper,
        color: Highcharts.getOptions().colors[2], // Topper Score color
        dataLabels: {
            enabled: true
        }

    }, {
        type: 'spline',
        name: 'Average',
        data: _average,
        marker: {
            lineWidth: 2,
            lineColor: Highcharts.getOptions().colors[1],
            fillColor: 'white'
        },
        color: Highcharts.getOptions().colors[3],// Topper Score color
        dataLabels: {
            enabled: true
        }

    }]
});

/*************************************************************************************** */

function GetJSON_Difficulty() {
    var resp = [];
    $.ajax({
        type: "GET",
        url: '../analysisajax/getDifficulty',
        async: false,
        contentType: "application/json",
        success: function (data) {
            resp.push(data);
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");
        }
    });
    return resp;
}

var dataDifficulty = GetJSON_Difficulty();

Highcharts.chart('container1', {
    chart: {
        type: 'pie',
        options3d: {
            enabled: true,
            alpha: 45,
            beta: 0
        }
    },
    title: {
        text: 'Distribution based on Difficulty (Based on Candidate Response Stats)'
    },
    accessibility: {
        point: {
            valueSuffix: '%'
        }
    },
    tooltip: {
        pointFormat: '<b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            depth: 35,
            dataLabels: {
                enabled: true,
                pointFormat: '{point.name}: <b>{point.percentage:.1f}%</b>'
            }
        }
    },
    series: [{
        type: 'pie',

        data: [
            ['Difficult', parseInt(dataDifficulty[0].difficult)],
            ['Moderate', parseInt(dataDifficulty[0].moderate)],
            {
                name: 'Easy',
                y: parseInt(dataDifficulty[0].easy),
                sliced: true,
                selected: true
            }
        ]
    }]
});