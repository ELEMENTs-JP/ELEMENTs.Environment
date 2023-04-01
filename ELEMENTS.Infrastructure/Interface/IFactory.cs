using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IFactory
    {
        // Administration 
        IFactoryStatusInfo CreateDatabase();
        IFactoryStatusInfo DeleteDatabase();
        IFactoryStatusInfo MigrateDatabase();
        string GetDatabaseVersion();
        void SetDatabasePath();
        Guid MasterGUID { get; set; }
        string MasterAppType { get; set; }

        // Factory 
        IDTO CreateDirect(string Title, string ItemType);
        IDTO Create(IInputDTO input);
        IDTO GetItemByID(IInputDTO input);
        IDTO GetItemDirectByGUID(IInputDTO input);
        IFactoryStatusInfo Update(IDTO dto);
        IFactoryStatusInfo Delete(IInputDTO input);
        List<IDTO> GetItems(IQueryParameter fqp);
        int GetItemsCount(IQueryParameter fqp);

        // Assign Remove 
        IFactoryStatusInfo AssignRelation(IRelationDTO dto);
        IFactoryStatusInfo RemoveRelation(IRelationDTO dto);
        IFactoryStatusInfo DeleteRelation(Guid ChildParentGUID, Guid masterGUID);
        IRelationDTO GetRelation(IRelationDTO dto);
    }

    public interface IFactoryStatusInfo
    {
        string Status { get; set; }
        string Message { get; set; }
        void Validate();
    }

    public interface IInputDTO
    {
        // Identify 
        Guid ItemGUID { get; set; }
        Guid UserGUID { get; set; }
        Guid MasterGUID { get; set; }
        string ID { get; set; }
        string Title { get; set; }
        string AppType { get; set; }
        string ItemType { get; set; }

        // Reference 
        string Reference { get; set; }

        bool CheckIfAlreadyExists { get; set; }
        bool Validate();
    }
    public class InputDTO : IInputDTO
    {
        // Identify 
        public Guid ItemGUID { get; set; }
        public Guid UserGUID { get; set; } = Guid.Empty;
        public Guid MasterGUID { get; set; } = Guid.Empty;
        public string ID { get; set; }
        public string Title { get; set; }
        public string AppType { get; set; }
        public string ItemType { get; set; }
        public string Reference { get; set; }
        public bool CheckIfAlreadyExists { get; set; } = false;

        // Method 
        public static IInputDTO CreateTemplate(
            Guid itemGUID, string title, Guid master,
            string appType, string itemType)
        {
            InputDTO dto = new InputDTO();

            try
            {
                // masterGUID 
                dto.ItemGUID = itemGUID;
                dto.MasterGUID = master;

                // Title 
                dto.Title = title;
                dto.AppType = appType;
                dto.ItemType = itemType;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Erzeugen der Relation " + ex.Message);
            }

            return dto;
        }

        public static IInputDTO GetItemTemplate(
          Guid itemGUID, Guid master,
          string appType, string itemType)
        {
            InputDTO dto = new InputDTO();

            try
            {
                // masterGUID 
                dto.ItemGUID = itemGUID;
                dto.MasterGUID = master;

                // Title 
                dto.Title = "";
                dto.AppType = appType;
                dto.ItemType = itemType;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Erzeugen der Relation " + ex.Message);
            }

            return dto;
        }

        public static IInputDTO SearchItemTemplate(
        string Title, string appType, string itemType)
        {
            InputDTO dto = new InputDTO();

            try
            {
                // Title 
                dto.ItemGUID = Guid.NewGuid();
                dto.MasterGUID = Guid.Empty;
                dto.Title = Title;
                dto.AppType = appType;
                dto.ItemType = itemType;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Erzeugen der Relation " + ex.Message);
            }

            return dto;
        }
        public bool Validate()
        {
            // Check 
            #region Check

            //if (MasterGUID == Guid.Empty)
            //{
            //    throw new Exception("Master GUID not set");
            //}

            if (ItemGUID == Guid.Empty)
            {
                throw new Exception("GUID not set");
            }

            if (string.IsNullOrEmpty(AppType) || AppType.ToSecureString() == "NULL")
            {
                throw new Exception("AppType not set");
            }

            if (string.IsNullOrEmpty(ItemType) || ItemType.ToSecureString() == "NULL")
            {
                throw new Exception("ItemType not set");
            }
            #endregion

            return true;
        }
    }
    
    // Query Parameter 
    public interface IQueryParameter
    {
        // General 
        Guid MasterGUID { get; set; }
        Guid UserGUID { get; set; }
        Guid ItemGUID { get; set; }

        // Types 
        string QueryItemType { get; set; }
        string QueryAppType { get; set; }

        // List 
        List<string> ItemTypes { get; set; }
        List<string> ItemTypeExcludes { get; set; }
        string ItemState { get; set; }

        // Matchcode 
        string Matchcode { get; set; }
        string MetadataMatch { get; set; }
        List<string> Matchcodes { get; set; }

        // Database Table 
        string DatabaseTable { get; }

        // Parent // Child 
        QueryType TypeOfQuery { get; set; }
        Guid ParentGUID { get; set; }
        Guid ChildGUID { get; set; }
        string AssociationType { get; set; }

        // Paging 
        int PageSize { get; set; }
        int CurrentPage { get; set; }

        // Filter 
        List<FilterByClauseDTO> Filter { get; set; }
        List<FilterByClauseDTO> JoinFilter { get; set; }
        List<FilterByClauseDTO> ExcludeFilter { get; set; }

        // Order // Sorting 
        string OrderByClause { get; set; }
        SortDirection SortDirection { get; set; }

        bool Validate();
    }

    public partial class QueryParameter : IQueryParameter
    {
        // Default Methods 
        public static QueryParameter DefaultItemsQuery(
            Guid MasterGUID, string AppType, string ItemType,
            QueryType queryType = QueryType.List, int currentPage = 1, int pageSize = 25,
            string orderByColumn = "Title",
            SortDirection sd = SortDirection.Ascending,
            string matchcode = "")
        {
            QueryParameter fqp = new QueryParameter();
            fqp.MasterGUID = MasterGUID;
            fqp.QueryAppType = AppType.ToSecureString();
            fqp.QueryItemType = ItemType.ToSecureString();
            fqp.CurrentPage = currentPage;
            fqp.PageSize = pageSize;
            fqp.OrderByClause = orderByColumn;
            fqp.SortDirection = sd;
            fqp.Matchcode = matchcode;
            fqp.TypeOfQuery = queryType;

            return fqp;
        }

        // General 
        public Guid MasterGUID { get; set; } = Guid.Empty;
        public Guid UserGUID { get; set; } = Guid.Empty;
        public Guid ItemGUID { get; set; } = Guid.Empty; // for Reverse Parent Search 

        // Types 
        public string QueryItemType { get; set; } = "NULL";
        public string QueryAppType { get; set; } = "NULL";

        // List 
        public List<string> ItemTypes { get; set; } = new List<string>();
        public List<string> ItemTypeExcludes { get; set; } = new List<string>();

        public string ItemState { get; set; } = "Active";

        // Matchcode 
        public string Matchcode { get; set; } = string.Empty;
        public string MetadataMatch { get; set; } = string.Empty;
        public List<string> Matchcodes { get; set; } = new List<string>();

        // Database Table 
        private string _databaseTable { get; set; } = "NULL";
        public string DatabaseTable { get { return _databaseTable; } }

        // Parent // Child 
        public QueryType TypeOfQuery { get; set; } = QueryType.List;
        public Guid ParentGUID { get; set; } = Guid.Empty;
        public Guid ChildGUID { get; set; } = Guid.Empty;
        public string AssociationType { get; set; } = string.Empty;

        // Paging 
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;

        // Filter 
        public List<FilterByClauseDTO> Filter { get; set; } = new List<FilterByClauseDTO>();
        public List<FilterByClauseDTO> JoinFilter { get; set; } = new List<FilterByClauseDTO>();
        public List<FilterByClauseDTO> ExcludeFilter { get; set; } = new List<FilterByClauseDTO>(); // Kriterien die NICHT angewendet werden sollen 

        // Order // Sorting 
        public string OrderByClause { get; set; } = "Title";
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public bool Validate()
        {
            // masterGUID 
            if (MasterGUID == Guid.Empty)
            {

                return false;
            }

            // ItemType 
            if (string.IsNullOrEmpty(QueryItemType))
            {
                // nur prüfen wenn es != Search ist 
                if (TypeOfQuery != QueryType.Search)
                {
                    return false;
                }
            }

            // Query Type 
            if (TypeOfQuery == QueryType.NULL)
            {

                return false;
            }

            // Database Table 
            _databaseTable = "tbl_CON_Content";

            if (Filter == null)
            {
                return false;
            }

            return true;
        }
    }
    public partial class PropertyQueryParameter
    {
        // General 
        public Guid MasterGUID = Guid.Empty;

        public Guid RelatedItemGUID = Guid.Empty;

        // Types 
        public string QueryItemType = "NULL";
        public string QueryAppType = "NULL";

        public bool Validate()
        {
            // masterGUID 
            if (MasterGUID == Guid.Empty)
                throw new Exception("masterGUID not set");

            // Related Item  
            if (RelatedItemGUID == Guid.Empty)
                throw new Exception("RelatedItem not set");

            // AppType 
            if (string.IsNullOrEmpty(QueryAppType))
                throw new Exception("QueryAppType not set");

            // ItemType 
            if (string.IsNullOrEmpty(QueryItemType))
                throw new Exception("QueryItemType not set");

            return true;
        }
    }

    // Table 
    public enum QueryType
    {
        // Default 
        NULL = 0,
        
        // 
        List = 1,
        ChildrenByParent = 2,
        ParentsByChild = 3,
        Search = 4,

        // 
        Personal = 11,
        Join = 12,
        PropertyJoin = 13,
        NoJoin = 14,

        // 
        DefaultChildren = 21,
        ParallelItems = 22,
        RelatedItems = 23,

        // 
        CreatedBy = 31,
        ModifiedBy = 32,
        NewestItems = 33,
        ParallelItemsByTag = 34,
    }
    // Filter 
    public class FilterByClauseDTO
    {
        private string _Property = "Title";
        public string Property
        {
            get { return _Property; }
            set { _Property = value; }
        }

        private string _Comparison = "=";
        public string Comparison
        {
            get { return _Comparison; }
            set { _Comparison = value; }
        }

        public object Value { get; set; }
        public string JoinItemType { get; set; }
    }

    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }

}
