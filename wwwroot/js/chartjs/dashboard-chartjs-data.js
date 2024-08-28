window.chartColors = {
    black: 'rgb(0, 0, 0)',
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};


function GetJSON_Simple() {
    var resp = [];
    $.ajax({
        type: "GET",
        url: '../analysisajax/getMarks',
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

var jsondata = GetJSON_Simple();
//console.log(data[0].percentages);
const _labels = [];
for (const s of jsondata[0].subjects) {
    _labels.push(s);
}

const _series = [];
for (const s of jsondata[0].marks) {
    _series.push(s);
}

const _percentages = [];
for (const s of jsondata[0].percentages) {
    _percentages.push(s);
}


var chartColors = window.chartColors;
var color = Chart.helpers.color;

function legendLabelColors(alpha) {
    return [
        color(chartColors.red).alpha(alpha).rgbString(),
        color(chartColors.purple).alpha(alpha).rgbString(),
        color(chartColors.orange).alpha(alpha).rgbString(),
        color(chartColors.green).alpha(alpha).rgbString(),
        color(chartColors.blue).alpha(alpha).rgbString(),

        color(chartColors.grey).alpha(alpha).rgbString(),
    ];
}

var ctx = document.getElementById("chartjs_polar");

const data = {
    labels: _labels,
    datasets: [
        {
            //label: "Your Score: ", // for legend
            data: _series,
            backgroundColor: legendLabelColors(0.5),
            datalabels: {
                formatter: (value, context) => value,
                color: color(chartColors.black).alpha(0.5).rgbString()
            },
        },
        //{
        //    helper: true,
        //    data: [100, 100, 100, 100, 100, 100, 100, 100],
        //    backgroundColor: ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)", "rgba(54, 162, 88, 0.2)"],
        //    datalabels: {
        //        display: false
        //    }
        //}
    ],
};

Chart.register(ChartDataLabels);

new Chart(ctx, {
    type: "polarArea",
    data: data,
    //title: {
    //    display: true,
    //    text: "Polar Area Chart",
    //},
    animation: {
        animateRotate: false,
        animateScale: true,
    },

    options: {
        responsive: true,
        maintainAspectRatio: false,
        layout: {
            padding: 20
        },
        //scales: {
        //    r: {
        //        angleLines: {
        //            display: true
        //        },
        //        ticks: {
        //            display: false
        //        },
        //        pointLabels: {
        //            display: false
        //        }
        //    }
        //},

        scale: {
            //min: 0,
            //max: 80,
            ticks: {
                beginAtZero: true,
            },
            reverse: false,
        },
        plugins: {
            legend: {
                display: false
            },
            labels: {
                arc: true,
                fontColor: '#000',
                position: 'outside',
                fontSize: 14,
                render: (args) => args.label,
                fontColor: (args) => legendLabelColors(1)[args.index]
            }
        }
    }
});

const data2 = {
    labels: _labels,
    datasets: [
        {
            //label: "Your Score: ", // for legend
            data: _percentages,
            backgroundColor: legendLabelColors(0.2),
            borderColor: legendLabelColors(1),
            borderWidth: 1,
            barPercentage: 0.9,
            categoryPercentage: 0.9,
            datalabels: {
                display: false
            }
        },

        //{
        //    helper: true,
        //    data: [100, 100, 100, 100, 100, 100, 100, 100],
        //    backgroundColor: ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)", "rgba(54, 162, 88, 0.2)"],
        //    datalabels: {
        //        display: false
        //    }
        //}
    ],
};



var ctx2 = document.getElementById("chartjs_bar");

const backgroundBar = {
    id: 'backgroundBar',
    beforeDatasetsDraw(chart, args, pluginOptions) {
        //console.log(data2.labels.length);
        const { data, ctx, chartArea: { top, bottom, left, right, width, height }, scales: { x, y }
        } = chart;

        ctx.save();
        //console.log(width);
        const segment = width / data2.labels.length;
        const barWidth = segment * data2.datasets[0].barPercentage * data2.datasets[0].categoryPercentage;
        //console.log(segment);

        ctx.fillStyle = pluginOptions.barColor;
        for (let i = 0; i < data2.labels.length; i++) {
            ctx.fillRect(x.getPixelForValue(i) - barWidth / 2, top, barWidth, height);
        }
    }
};

new Chart(ctx2, {
    type: "bar",
    data: data2,
    //title: {
    //    display: true,
    //    text: "Polar Area Chart",
    //},
    animation: {
        animateRotate: false,
        animateScale: true,
    },

    plugins: [backgroundBar],

    options: {
        responsive: true,
        maintainAspectRatio: false,
        //layout: {
        //    padding: 20
        //},

        scales: {
            x: {
                display: false,
                grid: {
                    display: false
                },
                ticks: {
                    display: false
                }
            },
            y: {
                display: false,
                grid: {
                    display: false
                },
                ticks: {
                    display: false
                }
            }
        },

        scale: {
            min: 0,
            max: 100,
            ticks: {
                beginAtZero: true,
            },
            reverse: false,
        },

        plugins: {
            legend: {
                display: false
            },
            backgroundBar: {
                barColor: 'rgba(0,0,0,0.1)'
            },

            tooltip: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        return tooltipItem.raw + "%";
                    }
                }
            }
        }
    }
});
