let generalChartHeight = 285;
let generalleft = 15;
let generalright = 35;
let generaltop = 10;
let generalbottom = 10;

export function loadStepLineChart(divID, dotNetHelper) {
    // Create Parameter 
    let jsonParameter =
    {
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
            onStepLineChartJSSucess(data);
        });
    }
    catch (e) {
        console.log("FAIL: " + e);
    }

}



function onStepLineChartJSSucess(data) {

    try {
        let chartType = 'line';
        let title = data.title.toString();
        let CutOutPercentage = 80;
        let legendePosition = 'right';
        let showLegende = false;

        // Farben 
        let colors = getChartColors();
        let legendTitle = '';

        // Arrays 
        let labelData = [];
        let dataset = [];

        try {
            for (let s = 0; s < data.series.length; s++) {
                let serie = data.series[s];
                if (serie === null)
                    continue;

                // Legende 
                legendTitle = serie.title;

                // Werte 
                for (let c = 0; c < serie.items.length; c++) {
                    try {
                        let key = serie.items[c].key;
                        let value = serie.items[c].value;

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
        catch (ex) { console.log("FAIL: " + ex); }


        // Get Canvas + Check Visibility 
        let canvas = null;

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

        let chart;

        try {
            if (chart !== null && chart !== undefined) {
                try { chart.destroy(); } catch (eInner) { alert(eInner); }
            }

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

                            backgroundColor: '#cccccc99',
                            fill: true,
                            stepped: 'middle',

                            data: dataset
                        }
                    ]
                },
                options: {
                    color: '#555',
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
                            grid: {
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
                            grid: {
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
        let colors = [
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
        let cathColors = [
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

        let colors = [];

        for (let c = 0; c < multiply; c++) {
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
        let cathColors = [
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
        let colors = getChartColors();
        shuffle(colors);
        return colors;
    } catch (e) {
        // viele starke Farben 
        let theco = getChartColors();
        shuffle(theco);
        return theco;
    }
}
