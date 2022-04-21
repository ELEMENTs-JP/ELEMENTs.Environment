let generalChartHeight = 285;
let generalleft = 15;
let generalright = 35;
let generaltop = 10;
let generalbottom = 10;

export function loadChart(divID, dotNetHelper)
{
    // Create Parameter 
    let jsonParameter = {
        "DIV": divID,
        "ItemType": "",
        "AppType": "",
        "DataParameter": "",
        "Parameter": "",
        "DataFilter": "",
        "Filter": "",
        "ChartType": "",
    };

    try {

        dotNetHelper.invokeMethodAsync('LoadChartData', jsonParameter).then(data => {

            onDonutChartJSSucess(data)
        });
    }
    catch (e)
    {
        console.log("FAIL: " + e);
    }
    
}

var chart;

function onDonutChartJSSucess(data) {

    try {
        var chartType = 'doughnut';
        var title = data.title.toString();
        var CutOutPercentage = 80;
        var legendePosition = 'right';
        var showLegende = false;

        // Farben 
        var colors = getChartColors();
        var legendTitle = '';

        // Arrays 
        var labelData = [];
        var dataset = [];

        try {
            for (var s = 0; s < data.series.length; s++) {
                var serie = data.series[s];
                if (serie === null)
                    continue;

                // Legende 
                legendTitle = serie.title;

                // Werte 
                for (var c = 0; c < serie.items.length; c++) {
                    try {
                        var key = serie.items[c].key;
                        var value = serie.items[c].value;

                        if (key === null)
                            key = '---';

                        // Data 
                        labelData.push(key);
                        dataset.push(value);
                    }
                    catch (e) {
                        console.log("FAIL: " + e);
                    }
                }
            }
        }
        catch (e) { console.log("FAIL: " + e); }


        // Get Canvas + Check Visibility 
        var canvas = null;

        try {
            canvas = document.getElementById(data.div);
            if (data.series.length === 0) {
                canvas.style.visibility = 'hidden';
                canvas.style.visibility = 'none';
            }
            canvas.height = generalChartHeight;
        }
        catch (e) {
            console.log("FAIL: " + e);
        }

        try {
            if (chart !== null && chart !== undefined) { chart.destroy(); }

            // Chart
            chart = new Chart(canvas, {
                type: chartType,
                data: {
                    labels: labelData,
                    datasets: [
                        {
                            label: legendTitle,
                            borderColor: '#fff',
                            borderWidth: 2,

                            pointBorderColor: colors,
                            pointBackgroundColor: colors,

                            backgroundColor: colors,

                            data: dataset
                        }
                    ]
                },
                options: {
                    color: colors,
                    drawBorder: false,
                    drawTicks: true,
                    cutoutPercentage: CutOutPercentage,
                    responsive: true,
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: generalleft,
                            right: generalright,
                            top: generaltop,
                            bottom: generalbottom
                        }
                    },
                    legend:
                    {
                        display: showLegende,
                        position: legendePosition,
                        fontSize: 8,
                        strokeStyle: '#fff',
                        title: {
                            text: title,
                            color: 'rgb(255, 99, 132)',
                        },
                        labels: {
                            color: 'rgb(255, 99, 132)'
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: title,
                            position: 'top'
                        }
                    },
                    scales: {
                        yAxes: [{
                            gridLines: {
                                drawBorder: true,
                                display: true,
                                drawTicks: true
                            },
                            ticks: {
                                display: true,
                                beginAtZero: true
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                drawBorder: true,
                                display: false
                            },
                            ticks: {
                                display: true,
                                beginAtZero: false,
                                fontSize: 9
                            }
                        }]
                    }

                }
            });
        }
        catch (e) {
            console.log("FAIL: " + e);
        }

    } catch (e) {
        console.log("FAIL: " + e);
    }

  
}

// Colors 
function getChartColors() {

    try {
        // viele starke Farben 
        var colors = [
            '#34A853', // green 
            '#ddd',
            '#e01256', // red 
            '#ccc',
            '#008CD8', // hellblau 
            '#aaa',
            '#AB117F', // deep pink 
            '#888',
            '#3B2F77', // violett 
            '#666',
            '#FCC200', // yellow 
            '#444',
            '#00A9A0', // türkis 
            '#222',
            '#E2007D', // rose 
            '#000',
            '#0085A4', // seeblau 
            '#E87A2C', // orange 
            '#DC0A15' // signal red 
        ];

        // shuffle(colors);
        return colors;
    } catch (e) {

        // viele starke Farben 
        var cathColors = [
            '#34A853', // green 
            '#eee',
            '#e01256', // red 
            '#ccc',
            '#008CD8', // hellblau 
            '#aaa',
            '#AB117F', // deep pink 
            '#888',
            '#3B2F77', // violett 
            '#666',
            '#FCC200', // yellow 
            '#444',
            '#00A9A0', // türkis 
            '#222',
            '#E2007D', // rose 
            '#000',
            '#0085A4', // seeblau 
            '#E87A2C', // orange 
            '#DC0A15' // signal red 
        ];

        // shuffle(colors);
        return cathColors;
    }
}
function getChartColorsMultiple(multiply) {

    try {

        var colors = [];

        for (var c = 0; c < multiply; c++) {
            colors.push('#32AA50');
            colors.push('#eee');
            colors.push('#DF1455');
            colors.push('#ccc');
            colors.push('#008CD7');
            colors.push('#aaa');
            colors.push('#AA1482');
            colors.push('#888');
            colors.push('#3C327D');
            colors.push('#666');
            //colors.push('#444');
            colors.push('#00AAAA');
            colors.push('#222');
            colors.push('#DC0082');
            //colors.push('#000');
            colors.push('#007DAF');
            colors.push('#E60F5A');
        }

        // shuffle(colors);
        return colors;
    } catch (e) {

        // viele starke Farben 
        var cathColors = [
            '#34A853', // green 
            '#eee',
            '#e01256', // red 
            '#ccc',
            '#008CD8', // hellblau 
            '#aaa',
            '#AB117F', // deep pink 
            '#888',
            '#3B2F77', // violett 
            '#666',
            '#FCC200', // yellow 
            '#444',
            '#00A9A0', // türkis 
            '#222',
            '#E2007D', // rose 
            '#000',
            '#0085A4', // seeblau 
            '#E87A2C', // orange 
            '#DC0A15' // signal red 
        ];

        // shuffle(colors);
        return cathColors;
    }
}
function getChartColorsShuffled() {

    try {

        // viele starke Farben 
        var colors = getChartColors();
        shuffle(colors);
        return colors;
    } catch (e) {
        // viele starke Farben 
        var theco = getChartColors();
        shuffle(theco);
        return theco;
    }
}
