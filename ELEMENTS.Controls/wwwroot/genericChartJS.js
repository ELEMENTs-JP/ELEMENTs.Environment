let generalChartHeight = 235;
let generalleft = 15;
let generalright = 35;
let generaltop = 10;
let generalbottom = 10;

import * as thePCKMOD from "./chartBase.js";

export function loadGenericChart(divID, charttyp, dotNetRef) {
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
        "ChartType": charttyp,
        "Category": ""
    };

    try {
        dotNetRef.invokeMethodAsync('LoadGenericChartData', jsonParameter)
            .then(data => {
                onGenericChartJSSucess(data)
            });

        // Assembly Name + Method 
        //DotNet.invokeMethodAsync('STRIDES.PieChart', 'LoadPieChartData', jsonParameter).then(data => {

        //    onPieChartJSSucess(data)
        //});
    }
    catch (e) {
        console.log("FAIL: " + e);
    }

}

function getChartType(enumChartType) {
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Line') { return 'line'; }
    if (enumChartType === 'Area') { return 'line'; }
    if (enumChartType === 'StepLine') { return 'line'; }
    if (enumChartType === 'HorizontalBar') { return 'bar'; }
    if (enumChartType === 'VerticalBar') { return 'bar'; }
    if (enumChartType === 'Pie') { return 'pie'; }
    if (enumChartType === 'Doughnut') { return 'doughnut'; }
    if (enumChartType === 'PolarArea') { return 'polarArea'; }
    if (enumChartType === 'Radar') { return 'radar'; }
    if (enumChartType === 'Progress') { return 'doughnut'; }

    return 'pie';
}
function getChartRadius(enumChartType) {
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Progress') { return 80; }
    if (enumChartType === 'Doughnut') { return 50; }

    return 0;
}
function getLegendPosition(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Line') { return 'top'; }
    if (enumChartType === 'Area') { return 'top'; }
    if (enumChartType === 'StepLine') { return 'top'; }
    if (enumChartType === 'HorizontalBar') { return 'top'; }
    if (enumChartType === 'VerticalBar') { return 'top'; }
    if (enumChartType === 'Pie') { return 'right'; }
    if (enumChartType === 'Doughnut') { return 'right'; }
    if (enumChartType === 'PolarArea') { return 'right'; }
    if (enumChartType === 'Radar') { return 'right'; }
    if (enumChartType === 'Progress') { return 'right'; }

    return 'right';
}
function getChartFillOption(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Area') { return true; }
    if (enumChartType === 'StepLine') { return true; }
    if (enumChartType === 'Radar') { return true; }

    return false;
}
function getChartFillColor(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Area') { return '#cccccc99'; }
    if (enumChartType === 'StepLine') { return '#cccccc99'; }
    if (enumChartType === 'Radar') { return '#cccccc99'; }

    return getChartColors();
}
function getBorderLineColor(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'Line') { return '#cccccc99'; }
    if (enumChartType === 'StepLine') { return '#cccccc99'; }
    if (enumChartType === 'Area') { return '#cccccc99'; }

    return '#ffffff';
}
function getChartIndexAxis(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'HorizontalBar') { return 'y'; }

    return 'x';
}
function getSteppedConfig(enumChartType) {
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'StepLine') { return true; }

    return false;
}
function getSteppedOption(enumChartType) {
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
    if (enumChartType === 'StepLine') { return 'middle'; }

    return '';
}
function getChartPointColors(enumChartType)
{
    // https://www.w3schools.com/js/js_graphics_chartjs.asp 
   
    return getChartColors();
}

function onGenericChartJSSucess(data) {

    try
    {
        let chartType = getChartType(data.chartType);
        let title = data.title.toString();
        let cutout = getChartRadius(data.chartType);
        let legendePosition = getLegendPosition(data.chartType);
        let showLegende = false;

        // Farben 
        let colors = getChartPointColors(data.chartType);
        let legendTitle = '';
        let fillChart = getChartFillOption(data.chartType);

        // Arrays 
        let labelData = [];
        let dataset = [];

        // Data 
        if (data.chartType === 'Progress') {
            try
            {
                // Data 
                let serie = data.series[0];
                let key = "Fortschritt";
                let value = serie.items[0].value;
                labelData.push(key);
                dataset.push(value);

                if (value < 100)
                {
                    labelData.push("offen");
                    dataset.push(100 - value);
                }
            }
            catch (e) { console.log("FAIL: " + e); }

        }
        else {
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
            catch (e) { console.log("FAIL: " + e); }
        }


        // Get Canvas + Check Visibility 
        let grapharea = null;
        let canvas = null;

        try {
            grapharea = document.getElementById(data.div).getContext("2d");
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

        // Package 
        let chartPCK = thePCKMOD.getPackageByID(data.div);
        if (chartPCK === null || chartPCK === undefined) {
            chartPCK = new thePCKMOD.tspChart();
            chartPCK.ID = data.div;
        }

        // Chart 
        let chart;
        if (chartPCK !== null && chartPCK !== undefined) {
            if (chartPCK.Chart !== null && chartPCK.Chart !== undefined) {
                chart = chartPCK.Chart;
            }
        }

        if (chart !== null && chart !== undefined) {
            try { chart.destroy(); } catch (eInner) { }
        }
        if (grapharea !== null && grapharea !== undefined) {
            try { grapharea.destroy(); } catch (eInner) { }
        }

        try {
            // Chart 
            chart = new Chart(grapharea, {
                type: chartType,
                data: {
                    labels: labelData,
                    datasets: [
                        {
                            label: legendTitle,
                            borderColor: getBorderLineColor(data.chartType),
                            borderWidth: 2,

                            pointBorderColor: colors,
                            pointBackgroundColor: colors,

                            backgroundColor: getChartFillColor(data.chartType),
                            fill: fillChart,
                            stepped: getSteppedOption(data.chartType),

                            data: dataset
                        }
                    ]
                },
                options: {
                    indexAxis: getChartIndexAxis(data.chartType),
                    color: colors,
                    drawBorder: false,
                    drawTicks: true,
                    cutoutPercentage: cutout,
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
                        align: 'center',
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

            // append 
            chartPCK.Chart = chart;

            if (chartPCK.Zustand === "Init") {
                chartPCK.Zustand = "Concurrent";
                thePCKMOD.allPackages.push(chartPCK);
            }

            // Update 
            chart.options.cutout = cutout;
            chart.options.plugins.legend.position = legendePosition;
            chart.update();
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
