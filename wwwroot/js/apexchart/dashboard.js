function GetJSON_Simple() {
    var resp = [];
    $.ajax({
        type: "GET",
        url: '../analysisajax/getPercent',
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

class ApexChart extends React.Component {

    constructor(props) {
        super(props);
        var data = GetJSON_Simple();
        //console.log(data[0].percentages);
        const _labels = [];
        for (const s of data[0].subjects) {
            _labels.push(s);
        }

        const _series = [];
        for (const s of data[0].percentages) {
            _series.push(s);
        }

        //console.log(marks);

        this.state = {

            series: _series,
            options: {
                chart: {
                    height: 400,
                    type: 'radialBar',
                },
                plotOptions: {
                    radialBar: {
                        offsetY: 0,
                        startAngle: 0,
                        endAngle: 270,
                        hollow: {
                            margin: 5,
                            size: '30%',
                            background: 'transparent',
                            image: undefined,
                        },
                        dataLabels: {
                            name: {
                                show: false,
                            },
                            value: {
                                show: false,
                            }
                        },
                        barLabels: {
                            enabled: true,
                            useSeriesColors: true,
                            offsetX: -8,
                            fontSize: "12px",
                            formatter: function (seriesName, opts) {
                                return seriesName + ":  " + opts.w.globals.series[opts.seriesIndex] + "%"
                            },
                        },
                    }
                },
                //colors: ['#7ECB78', '#F59F4A', '#1ab7ea', '#0084ff', '#39539E', '#0077B5'],
                labels: _labels,
                responsive: [{
                    breakpoint: 480,
                    options: {
                        legend: {
                            show: false
                        }
                    }
                }]
            },


        };
    }

    render() {
        return (
            <div>
                <div id="chart">
                    <ReactApexChart options={this.state.options} series={this.state.series} type="radialBar" height={400} />
                </div>
                <div id="html-dist"></div>
            </div>
        );
    }
}

const domContainer = document.querySelector('#app');
ReactDOM.render(React.createElement(ApexChart), domContainer);