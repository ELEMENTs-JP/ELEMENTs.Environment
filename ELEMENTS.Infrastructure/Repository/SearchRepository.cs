using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface ISearchRepository
    {
        string Matchcode { get; set; }
        string FilterValue { get; set; }
        IList<IDTO> Items { get; set; }
        IList<IDTO> Store { get; set; }
        IList<string> Filter { get; set; }
        void SearchItems();
        void FilterValues(KeyValuePair<string, bool> filter);
        void Init();
    }
   
}
