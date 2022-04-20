using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class ChartDTO
    {
        public string DIV { get; set; } = "";
        public string ItemType { get; set; } = "";
        public string AppType { get; set; } = "";
        public string Parameter { get; set; } = "";
        public string ChartType { get; set; } = "";
        public string Title { get; set; } = "";
        public List<ChartSeriesDTO> Series { get; set; } = new List<ChartSeriesDTO>();
    }
    public class ChartSeriesDTO
    {
        public string Title { get; set; } = "";
        public List<ChartItemDTO> Items { get; set; } = new List<ChartItemDTO>();
    }
    public class ChartItemDTO
    {
        public string Title { get; set; } = "";
        public string Key { get; set; } = "";
        public object Value { get; set; } = "";
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

    }
}
