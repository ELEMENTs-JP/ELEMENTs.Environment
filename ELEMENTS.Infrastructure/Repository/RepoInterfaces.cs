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
        int QueryCount { get; set; }
        ISQLiteService Service { get; set; }
        string Matchcode { get; set; }
        int PageSize { get; set; }
        int ItemCount { get; }
        int CurrentPage { get; set; }
        List<IDTO> Items { get; set; }
        List<IDTO> Load();
        List<IDTO> Search();
    }

    public interface IEditItemRepository
    {
        ISQLiteService Service { get; set; }
        IDTO DTO { get; set; }
        IDTO Init();
        Guid ItemGUID { get; set; }
    }
}
