using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IPermissionConfigurationRepository
    {
        event PropertyChangedEventHandler PropertyChanged;
        ISqlDatabaseService Service { get; set; }

        // Security Object
        IDTO UserGroup { get; set; }
        string ItemType { get; set; }
        // Apps 
        IApp CurrentApp { get; set; }
        List<IApp> Apps { get; set; }

        // ItemType
        IItemType CurrentItemType { get; set; }
        List<IItemType> ItemTypes { get; set; }

        // Permissions
        List<IObjectPermission> Permissions { get; set; }
        List<IDTO> AllPermissionAssignments { get; set; }

        void Assign(IObjectPermission iop, string Right);
        void Remove(IObjectPermission iop);
        List<IDTO> Load();

    }



    // Settings 
    public interface ISettingField
    {
        string Title { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        string Group { get; set; }
        string DefaultValue { get; set; }
        SettingScope Scope { get; set; }
        string ColumnCSSClass { get; set; }
        EditFieldType Type { get; set; }
        EditFieldMode Mode { get; set; }
        FieldOrientation Orientation { get; set; }
        List<OptionItem> OptionItems { get; set; }
    }

    public class ObjectSetting : ISettingField
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public string Group { get; set; }
        public SettingScope Scope { get; set; }
        public string ColumnCSSClass { get; set; } = "col";
        public EditFieldType Type { get; set; } = EditFieldType.TextBox;
        public EditFieldMode Mode { get; set; } = EditFieldMode.View;
        public FieldOrientation Orientation { get; set; } = FieldOrientation.Vertical;
        public List<OptionItem> OptionItems { get; set; } = new List<OptionItem>();
    }

    public enum SettingScope
    {
        NULL = 0,
        Principal = 1,
        User = 2,
        Application = 3,
        ItemType = 4,
        Item = 5,
    }

    // Permissions 
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

    public enum FeatureType
    { 
        NULL = 0,

        Dashboard = 1,
        Board = 2,
    }
    public enum BoardType
    {
        NULL = 0,

        Roadmap = 1,
        Backlog = 2,
        Kanban = 3,
    }
    public enum DashboardType
    {
        NULL = 0,

        SystemDashboard = 11,
        
        AppDashboard = 21,
        
        UserDashboard = 31,
        
        GeneralDashboard = 41,
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
        string RelatedItemType { get; set; }
    }
    public class Column : IColumn
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ColumnCSSClass { get; set; } = "col";
        public string Property { get; set; }
        public ColumnType Type { get; set; }
        public string RelatedItemType { get; set; }
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
        Progress = 7,

        Date = 11,
        DateTime = 12,
        Time = 14,
        FromTo = 15,

        UserImage = 21,
        
        Parents = 31,
        Children = 32,
        //Related = 33,
        //Parallels = 34,

        Priority = 41,
        Status = 42,
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
        ProgressBox = 25,

        DateBox = 31,
        TimeBox = 32,
        DateTimeBox = 33,

        LookupItems = 41,
        OptionItems = 42,
        PersonPicker = 43,
    
        CheckBox = 51,
        ToggleSwitch = 52,

        Status = 61,
        Priority = 62,

    }

    public enum EditFieldMode
    {
        NULL = 0,

        Hidden = 1,
        View = 2,
        Edit = 3,
    }
    public enum FieldOrientation
    { 
        NULL = 0,
        Vertical = 1,
        Horizontal = 2,
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
    public class FactoryStatusInfo : IFactoryStatusInfo
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public void Validate()
        {
            if (Status != "OK")
            {
                Console.WriteLine("FAIL: " + Message);
            }
        }
    }

}

