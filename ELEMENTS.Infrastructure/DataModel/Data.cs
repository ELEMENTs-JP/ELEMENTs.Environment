using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IDataImport
    {
        List<DataRow> Rows { get; set; }

        void ReadData(string blankText);
    }
    public class DataImport : IDataImport
    {
        public string FileName { get; set; } = string.Empty;

        string header = string.Empty;
        public List<DataRow> Rows { get; set; } = new List<DataRow>();

        public void ReadData(string blankText)
        {
            List<string> rows = blankText.SplitByLineBreak().ToList();

            if (rows.Count >= 2)
            { 
                header = rows[0];

                for (int i = 1; i <= rows.Count() - 1; i++)
                {
                    DataRow dr = new DataRow { Row = rows[i].ToSecureString() };
                    dr.LoadColumns();
                    Rows.Add(dr);
                }
            }
        }
    }

    public class DataRow 
    {
        public DataRow()
        { }

        public string Row { get; set; } = string.Empty;

        public List<DataColumn> Columns { get; set; } = new List<DataColumn>();
        public void LoadColumns()
        {
            List<string> columns = Row.SplitByDefault().ToList();
            foreach (string column in columns)
            {
                Columns.Add(new DataColumn() { Value = column.RemoveAtStartEnd("\"", "\"") });
            }
        }
    }

    public class DataColumn
    {
        public DataColumn()
        { }
        public string Value { get; set; } = string.Empty;
    }

    
}
