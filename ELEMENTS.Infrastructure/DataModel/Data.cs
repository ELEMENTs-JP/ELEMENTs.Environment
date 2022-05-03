using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IDataImport
    {
        List<string> Rows { get; set; }

        void ReadData(string blankText);
    }
    public class DataImport : IDataImport
    {
        public string FileName { get; set; } = string.Empty;
        public List<string> Rows { get; set; } = new List<string>();

        public void ReadData(string blankText)
        {
            foreach (string row in blankText.SplitByLineBreak())
            {
                Rows.Add(row);
            }
        }
    }

    
}
