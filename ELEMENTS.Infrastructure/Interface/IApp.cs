using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IApp
    {
        Guid ID { get; set; }
        
        /// <summary>
        /// Internal Technical Name
        /// </summary>
        string Name { get; set; } 
        string Title { get; set; }
        string Description { get; set; }
        List<IItemType> GetItemTypes();
        List<IFeature> Features { get; set; }
        List<IPage> Pages { get; set; }
        string Link { get; set; }
        string ColorCode { get; set; }
        AppType Type { get; set; }
        string Group { get; set; }
        bool IsActive { get; set; }
    }
    public class BaseApp : IApp
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Base-App";
        public string Title { get; set; } = "Base App";
        public string Description { get; set; } = string.Empty;
        public List<IItemType> GetItemTypes()
        {
            List<IItemType> it = new List<IItemType>();
            return it;
        }
        public List<IFeature> Features { get; set; } = new List<IFeature>();
        public List<IPage> Pages { get; set; } = new List<IPage>();
        public string Link { get; set; } = string.Empty;
        public string ColorCode { get; set; } = "#cccccc";
        public AppType Type { get; set; } = AppType.App;
        public string Group { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public interface IFeature
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Link { get; set; }
    }

    public interface IPage
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Link { get; set; }
    }

    // ItemType 
    public interface IItemType
    {
        Guid ID { get; set; }
        /// <summary>
        /// Internal Technical Name
        /// </summary>
        string Name { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        ItemTypeTyp Typ { get; set; }
        List<IColumn> Columns { get; set; }
        List<IField> Fields { get; set; }
        string ColorCode { get; set; }
        string Group { get; set; } 

        // Relations 
        List<IItemType> GetParentItemTypes();
        List<IItemType> GetRelatedItemTypes();
        List<IItemType> GetParallelItemTypes();
        List<IItemType> GetChildItemTypes();
    }

    public enum ItemTypeTyp
    { 
        NULL = 0,
        System = 1, // Systemnavigation 
        Item = 2, // Basiert auf einem Item 
        File = 3, // basiert auf einer Datei
        Date = 4, // basiert auf einem Datum
        Related = 5, // Untergeordneter bzw. verknüpfter 
        Internal = 6, // Interner ItemTye 
    }

    public class ItemType : IItemType
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IColumn> Columns { get; set; } = new List<IColumn>();
        public List<IField> Fields { get; set; } = new List<IField>();
        public ItemTypeTyp Typ { get; set; } = ItemTypeTyp.Item;
        public string ColorCode { get; set; } = "#cccccc";
        public string Group { get; set; } = string.Empty;
        // Relations 
        public virtual List<IItemType> GetParentItemTypes()
        {
            List<IItemType> itemtypes = new List<IItemType>();
            return itemtypes;
        }
        public virtual List<IItemType> GetRelatedItemTypes()
        {
            List<IItemType> itemtypes = new List<IItemType>();
            return itemtypes;
        }
        public virtual List<IItemType> GetParallelItemTypes()
        {
            List<IItemType> itemtypes = new List<IItemType>();
            return itemtypes;
        }
        public virtual List<IItemType> GetChildItemTypes()
        {
            List<IItemType> itemtypes = new List<IItemType>();
            return itemtypes;
        }
    }


    // COLUMNS 
    public interface IColumn
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Property { get; set; }
        string ColumnCSSClass { get; set; }
        ColumnType Type { get; set; }
    }
    public class Column : IColumn
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ColumnCSSClass { get; set; } = "col";
        public string Property { get; set; }
        public ColumnType Type { get; set; }
    }

    public enum ColumnType
    {
        NULL = 0,
        Text = 1,
    }

    // Edit Field 
    public interface IField
    {
        string Title { get; set; }
        string Description { get; set; }
        string ColumnCSSClass { get; set; }
        string Property { get; set; }
        EditFieldType Type { get; set; }
        EditFieldMode Mode { get; set; }
    }

    public class EditField : IField
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColumnCSSClass { get; set; } = "col";
        public string Property { get; set; } = "Title";
        public EditFieldType Type { get; set; } = EditFieldType.TextBox;
        public EditFieldMode Mode { get; set; } = EditFieldMode.View;
    }

    public enum EditFieldType
    {
        NULL = 0,

        Text = 1,
        TextBox = 10,
        TextArea = 11,

        MoneyBox = 21,
        IntegerBox = 22,
        DecimalBox = 23,

        DateBox = 31,
        TimeBox = 32,
        DateTimeBox = 33,
    }

    public enum EditFieldMode
    {
        NULL = 0,

        Hidden = 1,
        View = 2,
        Edit = 3,
    }
}
