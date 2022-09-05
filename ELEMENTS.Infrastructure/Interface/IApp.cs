using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IApp
    {
        Guid GUID { get; set; } 
        string ID { get; set; } 
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
        public Guid GUID { get; set; } = Guid.NewGuid();
        public string ID { get; set; } = string.Empty;
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

        FeatureType FeatureType { get; set; }
        BoardType BoardType { get; set; }
        DashboardType DashboardType { get; set; }

        Guid GUID { get; set; }
        string ID { get; set; }
        IItemType ItemType { get; set; }
        public IItemType GetItemsItemType();
    }

    public class BaseFeature : IFeature
    {
        public string ID { get; set; } = string.Empty;
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

        public FeatureType FeatureType { get; set; } = FeatureType.NULL;
        public BoardType BoardType { get; set; } = BoardType.NULL;
        public DashboardType DashboardType { get; set; } = DashboardType.NULL;
        
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
        Guid GUID { get; set; }
        string ID { get; set; }
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

        // Settings 
        List<ISettingField> GetSettings();
    }

    public class ItemType : IItemType
    {
        public Guid GUID { get; set; } = Guid.NewGuid();
        public string ID { get; set; } = string.Empty;
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
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Create).ToSecureString(), Permission = PermissionType.Create.ToString(), Description = "Ermöglicht das Erstellen von neuen Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Read).ToSecureString(), Permission = PermissionType.Read.ToString(), Description = "Ermöglicht das tabellarische Lesen und Suchen nach Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.View).ToSecureString(), Permission = PermissionType.View.ToString(), Description = "Ermöglicht das betrachten eines Datensatzes in der View." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Update).ToSecureString(), Permission = PermissionType.Update.ToString(), Description = "Ermöglicht das Editieren und Speichern eines Datensatzes." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Delete).ToSecureString(), Permission = PermissionType.Delete.ToString(), Description = "Ermöglicht das Löschen eines Datensatzes." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Assign).ToSecureString(), Permission = PermissionType.Assign.ToString(), Description = "Ermöglicht das Zuordnen von fremden Datensätzen." });
            permissions.Add(new ItemTypePermission() { PermissionID = ((int)PermissionType.Remove).ToSecureString(), Permission = PermissionType.Remove.ToString(), Description = "Ermöglicht das Wegordnen von fremden Datensätzen." });

            // Feature Permission
            // permissions.Add(new FeaturePermission() { PermissionID = "FEA-BP-01", Permission = "Comments", Description = "Ermöglicht das Erstellen, Bearbeiten und Entfernen von eigenen Kommentaren." });
            // permissions.Add(new FeaturePermission() { PermissionID = "FEA-BP-02", Permission = "Notes", Description = "Ermöglicht das Erstellen, Bearbeiten und Entfernen von eigenen Notizen." });
            // permissions.Add(new FeaturePermission() { PermissionID = "FEA-BP-03", Permission = "Dateien", Description = "Ermöglicht das Hochladen, Entfernen, Bearbeiten, Suchen und betrachten sowie das Zu- und Wegordnen von Dateien." });
            // permissions.Add(new FeaturePermission() { PermissionID = "FEA-BP-04", Permission = "Checklist", Description = "Ermöglicht das Erstellen, Bearbeiten und finalisieren von Checklisten." });
            // permissions.Add(new FeaturePermission() { PermissionID = "FEA-BP-05", Permission = "Metadaten", Description = "Ermöglicht das Einsehen und Nutzen der Metadaten Informationen." });

            return permissions;
        }

        // Settings 
        public virtual List<ISettingField> GetSettings()
        {
            List<ISettingField> settings = new List<ISettingField>();

            //settings.Add(new ObjectSetting() { Name = "", Title = "", Description = "", DefaultValue = string.Empty });

            return settings;
        }
    }

    public enum PermissionType
    { 
        NULL = 0,
        Create = 100,
        Read = 200,
        View = 300,
        Update = 400,
        Delete = 500,
        Assign = 600,
        Remove = 700
    }
  

}
