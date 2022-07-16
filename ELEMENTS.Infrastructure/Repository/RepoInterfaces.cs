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
        IItemType ItemType { get; set; }

        IItemType ReferenceItemType { get; set; } 
        QueryType DataQueryType { get; set; } 
        Guid ReferenceGUID { get; set; } 
    }

    public interface IItemsRepository
    {
        int TotalPageCount { get; set; }
        int QueryCount { get; set; }
        QueryType DataQueryType { get; set; }
        Guid ReferenceGUID { get; set; }
        ISQLiteService Service { get; set; }
        string Matchcode { get; set; }
        IItemType ItemType { get; set; }
        int PageSize { get; set; }
        int ItemCount { get; }
        int CurrentPage { get; set; }
        List<IDTO> Items { get; set; }
        List<IDTO> Load();
        List<IDTO> Search();
        IFactoryStatusInfo Delete(IDTO dto);
    }

    public interface IEditItemRepository
    {
        bool IsInitialized { get; set; }
        ISQLiteService Service { get; set; }
        IDTO DTO { get; set; }
        IDTO Init();
        Guid ItemGUID { get; set; }
        IItemType ItemType { get; set; }
        IFactoryStatusInfo DeleteItem();
    }

    public interface IUserInterfaceRepository
    {
        void Init();
        bool IsInitialized { get; set; }
        IItemType ItemType { get; set; }
        List<IField> Fields { get; set; }
        List<IColumn> Columns { get; set; }
    }
}
