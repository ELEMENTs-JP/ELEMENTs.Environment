using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface IAddItemRepository
    {
        ISQLiteService Service { get; set; }
        string Value { get; set; }
        IDTO Create();
    }

    public interface IItemsRepository
    {
        ISQLiteService Service { get; set; }
        string Matchcode { get; set; }
        List<IDTO> Items { get; set; }
        List<IDTO> Load();
        List<IDTO> Search();
    }
}
