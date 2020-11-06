// Real-time Chart Example written by Simon Brunel
// https://www.bytefish.de/blog/realtime_charts_signalr_chartjs.html
// GitHub
// https://github.com/bytefish/SignalRSample
// JS simulation
// (Plunker: https://plnkr.co/edit/Imxwl9OQJuaMepLNy6ly?p=info)

'use strict';
var SensorTest = function (hubUrl) {
    var samples = 100;
    var speed = 250;
    var values = [];
    var labels = [];

    values.length = samples;
    labels.length = samples;
    // populate arrays
    values.fill(0);
    labels.fill(0);

    // report
    const tdTime = document.getElementById('sensor-time');
    const tdValue = document.getElementById('sensor-value');

    const chart = new Chart(document.getElementById("chart"),
        {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        data: values,
                        backgroundColor: 'rgba(255, 99, 132, 0.1)',
                        borderColor: 'rgb(255, 99, 132)',
                        borderWidth: 2,
                        lineTension: 0.25,
                        pointRadius: 0
                    }
                ]
            },
            options: {
                responsive: false,
                animation: {
                    duration: speed * 1.5,
                    easing: 'linear'
                },
                legend: false,
                scales: {
                    xAxes: [
                        {
                            display: false
                        }
                    ],
                    yAxes: [
                        {
                            ticks: {
                                max: 1,
                                min: -1
                            }
                        }
                    ]
                }
            }
        });

    setRealTime(hubUrl);

    function setRealTime(hubUrl) {
        const connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

        connection.on('Broadcast', function (sender, message) {
            values.push(message.value);
            values.shift();
            // msgic
            chart.update();

            // report data
            tdTime.innerHTML = _timeFormat(new Date(message.timestamp));
            tdValue.innerHTML = message.value;
        });

        connection.start().then(function () {
            console.log('Connection has started.');
        }).catch(function (err) {
            console.error(err.toString());
        });
    }



}