let generalChartHeight = 215;
let generalleft = 15;
let generalright = 35;
let generaltop = 10;
let generalbottom = 10;

export class tspChart
{
    #ID;
    #Chart;
    #Zustand; // Init; Concurrent;

    constructor() {
        this.ID = "";
        this.Chart = null;
        this.Zustand = "Init";
    }
    getID()
    {
        return this.#ID;
    }
    getChart()
    {
        return this.#Chart;
    }
    getZustand()
    {
        return this.#Zustand;
    }
}

export let allPackages = [];

export function getChartByID(id)
{
    try {
        for (const pck of allPackages) {
            if (pck.ID.toString() === id.toString()) {
                return pck.Chart;
            }
        }
    } catch (e) {
        console.log("FAIL: " + e);
    }
    return null;
}


export function getPackageByID(id)
{
    try {
        for (const pck of allPackages) {
            if (pck.ID.toString() === id.toString()) {
                return pck;
            }
        }
    } catch (e) {
        console.log("FAIL: " + e);
    }
    return null;
}

