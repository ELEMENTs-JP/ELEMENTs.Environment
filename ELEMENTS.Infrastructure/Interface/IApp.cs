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
        List<IFeature> GetFeatures();
        List<IPage> GetPages();
        string Link { get; set; }
        string ColorCode { get; set; }
        AppType Type { get; set; }
        string Group { get; set; }
        bool IsActive { get; set; }
        string IconHTML { get; set; } 
    }
    public class BaseApp : IApp
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Base-App";
        public string Title { get; set; } = "Base App";
        public string Description { get; set; } = string.Empty;
        public string IconHTML { get; set; } = string.Empty;
        public List<IItemType> GetItemTypes()
        {
            List<IItemType> it = new List<IItemType>();
            return it;
        }
        public List<IFeature> GetFeatures()
        {
            List<IFeature> it = new List<IFeature>();
            return it;
        }
        public List<IPage> GetPages()
        {
            List<IPage> it = new List<IPage>();
            return it;
        }
        public string Link { get; set; } = string.Empty;
        public string ColorCode { get; set; } = "#cccccc";
        public AppType Type { get; set; } = AppType.App;
        public string Group { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public interface IFeature
    {
        string Title { get; set; }
        string Description { get; set; }
        string Link { get; set; }
        string IconHTML { get; set; }
        BoardType BoardType { get; set; }
        
        Guid GUID { get; set; }
        IItemType ItemType { get; set; }
        public IItemType GetItemsItemType();
    }

    public class BaseFeature : IFeature
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string IconHTML { get; set; } = string.Empty;
        
        /// <summary>
        /// Guid of the Item
        /// </summary>
        public Guid GUID { get; set; }
        public IItemType ItemType { get; set; }
        public virtual IItemType GetItemsItemType()
        {
            return null;
        }
        public BoardType BoardType { get; set; }
        
    }

    public interface IPage
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Link { get; set; }
        string IconHTML { get; set; }
        public IItemType GetItemType();
    }

    public class BasePage : IPage
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string IconHTML { get; set; } = string.Empty;
        public IItemType GetItemType()
        {
            return null;
        }
    }

    // ItemType 
    public enum ItemTypeTyp
    { 
        NULL = 0,
        System = 1, // Systemnavigation 
        Item = 2, // Listenansicht 
        File = 3, // Datei Ansicht
        Date = 4, // Kalender Ansicht
        Portfolio = 5, // Portfolio Ansicht
        Collection = 6, // Collection Ansicht 
        Internal = 7, // Interner ItemTye 
        Notice = 8,
        Table = 9,
        Related = 10, // Untergeordneter bzw. verknüpfter 
    }
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
        int Sorting { get; set; }
        bool InMenu { get; set; }
        string IconHTML { get; set; }


        // Relations 
        List<IItemType> GetParentItemTypes();
        List<IItemType> GetRelatedItemTypes();
        List<IItemType> GetParallelItemTypes();
        List<IItemType> GetChildItemTypes();
        List<IItemType> GetDefaultItemTypes();

        // Permissions
        List<IObjectPermission> GetPermissions();
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

        public int Sorting { get; set; } = 1;
        public bool InMenu { get; set; } = true;
        public string IconHTML { get; set; } = string.Empty;


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
        public virtual List<IItemType> GetDefaultItemTypes()
        {
            List<IItemType> itemtypes = new List<IItemType>();
            return itemtypes;
        }

        // Berechtigungen
        public virtual List<IObjectPermission> GetPermissions()
        {
            List<IObjectPermission> permissions = new List<IObjectPermission>();

            // Base Permissions
            permissions.Add(new ItemTypePermission() { PermissionID = "B-01", Permission = "Create", Description = "Ermöglicht das Erstellen von neuen Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-02", Permission = "Read", Description = "Ermöglicht das tabellarische Lesen und Suchen nach Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-03", Permission = "View", Description = "Ermöglicht das betrachten eines Datensatzes in der View." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-04", Permission = "Update", Description = "Ermöglicht das Editieren und Speichern eines Datensatzes." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-05", Permission = "Delete", Description = "Ermöglicht das Löschen eines Datensatzes." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-06", Permission = "Assign", Description = "Ermöglicht das Zuordnen von fremden Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = "B-07", Permission = "Remove", Description = "Ermöglicht das Wegordnen von fremden Datensätzen." });

            // Feature Permission
            permissions.Add(new FeaturePermission() { PermissionID = "F-0001", Permission = "Comments", Description = "Ermöglicht das Erstellen, Bearbeiten und Entfernen von eigenen Kommentaren." });
            permissions.Add(new FeaturePermission() { PermissionID = "F-0002", Permission = "Notes", Description = "Ermöglicht das Erstellen, Bearbeiten und Entfernen von eigenen Notizen." });
            permissions.Add(new FeaturePermission() { PermissionID = "F-0003", Permission = "Dateien", Description = "Ermöglicht das Hochladen, Entfernen, Bearbeiten, Suchen und betrachten sowie das Zu- und Wegordnen von Dateien." });
            permissions.Add(new FeaturePermission() { PermissionID = "F-0004", Permission = "Checklist", Description = "Ermöglicht das Erstellen, Bearbeiten und finalisieren von Checklisten." });
            permissions.Add(new FeaturePermission() { PermissionID = "F-0005", Permission = "Metadaten", Description = "Ermöglicht das Einsehen und Nutzen der Metadaten Informationen." });

            return permissions;
        }
    }

    public interface IObjectPermission
    {
        string PermissionID { get; set; }
        string Permission { get; set; }
        string Description { get; set; }
    }
    public class ItemTypePermission : IObjectPermission
    {
        public string PermissionID { get; set; }
        public string Permission { get; set; }
        public string Description { get; set; }
    }

    public class FeaturePermission : IObjectPermission
    {
        public string PermissionID { get; set; }
        public string Permission { get; set; }
        public string Description { get; set; }
    }

    // Board 
    public interface IBoardInterfaceRepository
    {
        bool IsInitialized { get; set; }
        ISqlDatabaseService Service { get; set; }

        IDTO CurrentBoard { get; set; }
        List<IDTO> Boards { get; set; }
        void Init();
        List<IDTO> Columns { get; set; }
        List<IDTO> Rows { get; set; }
        IItemType ItemType { get; set; }
        void AddRow(string title);
        void AddColumn(string title);
        void RemoveRow(IDTO row);
        void RemoveColumn(IDTO column);
    }

    public enum BoardType
    { 
        NULL = 0,

        Roadmap = 1,
        Backlog = 2,
        Kanban = 3,
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
        Link = 2,
        Percent = 3,
        Money = 4,
        Integer = 5,
        Decimal = 6,

        Date = 11,
        DateTime = 12,
        Time = 14,
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
        string ItemType { get; set; }
        string FilterProperty { get; set; }
        string FilterValue { get; set; }
    }

    public class EditField : IField
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColumnCSSClass { get; set; } = "col";
        public string Property { get; set; } = "Title";
        public EditFieldType Type { get; set; } = EditFieldType.TextBox;
        public EditFieldMode Mode { get; set; } = EditFieldMode.View;
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// Filter Property for Lookup
        /// </summary>
        public string FilterProperty { get; set; } = string.Empty; // Lookup 
        /// <summary>
        /// Filter Property Value for Lookup
        /// </summary>
        public string FilterValue { get; set; } = string.Empty; // Lookup 
    }

    public enum EditFieldType
    {
        NULL = 0,

        Text = 1,
        Divider = 2,

        TextBox = 10,
        TextArea = 11,

        MoneyBox = 21,
        IntegerBox = 22,
        PercentBox = 23,
        DecimalBox = 24,

        DateBox = 31,
        TimeBox = 32,
        DateTimeBox = 33,

        LookupItems = 41,
    }

    public enum EditFieldMode
    {
        NULL = 0,

        Hidden = 1,
        View = 2,
        Edit = 3,
    }

    public interface IAction
    {
        string ID { get; set; }
        string Title { get; set; }
        string Filter { get; set; }
        string Command { get; set; }
        string Link { get; set; }
    }

    public class MenuAction : IAction
    {
        public string ID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    
    }
}
