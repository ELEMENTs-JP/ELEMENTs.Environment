using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
    // General 
    public partial class FactoryStructure
    {
        public string Namespace { get; set; }
        public string Class { get; set; }

        public static FactoryStructure GetClientMapping(ElementsEntityType et)
        {
            // Structure 
            FactoryStructure fs = new FactoryStructure();
            fs.Namespace = "ELEMENTS.Data.SQLite";
            fs.Class = string.Empty;

            switch (et)
            {
                case ElementsEntityType.Property:
                    {
                        fs.Class = "tbl_MTD_Property";
                        break;
                    }
                case ElementsEntityType.Setting:
                    {
                        fs.Class = "tbl_SET_Setting";
                        break;
                    }
                case ElementsEntityType.Relation:
                    {
                        fs.Class = "tbl_TEC_Relation";
                        break;
                    }
                case ElementsEntityType.Content:
                    {
                        fs.Class = "tbl_CON_Content";
                        break;
                    }
                default:
                    {
                        fs.Class = "tbl_CON_Content";
                        break;
                    }
            }

            return fs;
        }

    }

    // Content 
    public static partial class FactoryQuery
    {
        private static string tblShort = "s";

        // CREATE 
        public static string GetCreateQuery(IInputDTO input, string tbl)
        {
            string sql = string.Empty;


            // SQL 
            sql += "INSERT INTO " + tbl + " ([GUID], [MasterGUID], " +
                                            " [CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], " +
                                            " [ID], [Title], [Matchcode], " +
                                            " [AppType], [ItemType], " +
                                             " [Checked], [ItemState], [PrivacyType]) " +
                        " VALUES('" + input.ItemGUID + "' , " +
                                    " '" + input.MasterGUID + "' , " +

                                    " '" + input.UserGUID + "' , " +
                                    " " + DateTime.Now.ToSecureSqlValue(comparison: "") + " , " +
                                    " '" + input.UserGUID + "' , " +
                                    " " + DateTime.Now.ToSecureSqlValue(comparison: "") + " , " +

                                    " '" + input.ID.ToSecureString() + "' , " +
                                    " '" + input.Title.ToSecureString() + "' , " +
                                    " '" + input.Title.ToSecureString() + "' , " +

                                    " '" + input.AppType + "' , " +
                                    " '" + input.ItemType + "' , " +

                                    " 0, " +
                                    " 'Active', " +
                                    " 'PUBLIC' )";

            return sql;
        }
        
        // ITEMS
        public static string GetItemsQuery(IQueryParameter fqp)
        {
            // Check 
            fqp.Validate();

            // Query Table 
            string tbl = fqp.DatabaseTable.ToSecureString();

            try
            {
                // SQL 
                string sql = "";

                // Basic Query SELECT 
                #region Basic Query Select
                // SELECT + SUCHE 
                if ((fqp.TypeOfQuery == QueryType.List || fqp.TypeOfQuery == QueryType.Search) 
                    && fqp.ChildGUID == Guid.Empty && fqp.ParentGUID == Guid.Empty)
                {
                    sql += Select(tbl, fqp.MasterGUID);
                }

                // Children 
                if (fqp.TypeOfQuery == QueryType.ChildrenByParent && fqp.ParentGUID != Guid.Empty)
                {
                    // Association Type 
                    sql += SelectChildren(tbl, fqp.ParentGUID, fqp.MasterGUID, fqp.AssociationType);
                }
                // Parents 
                if (fqp.TypeOfQuery == QueryType.ParentsByChild && fqp.ChildGUID != Guid.Empty)
                {
                    // Association Type 
                    sql += SelectParents(tbl, fqp.ChildGUID, fqp.MasterGUID, fqp.AssociationType);
                }

                // Default Children 
                if (fqp.TypeOfQuery == QueryType.DefaultChildren && fqp.ParentGUID != Guid.Empty)
                {
                    // Association Type 
                    sql += SelectChildren(tbl, fqp.ParentGUID, fqp.MasterGUID, fqp.AssociationType);
                }
                // Related Items  
                if (fqp.TypeOfQuery == QueryType.RelatedItems && fqp.ParentGUID != Guid.Empty)
                {
                    // Association Type 
                    sql += SelectChildren(tbl, fqp.ParentGUID, fqp.MasterGUID, fqp.AssociationType);
                }
                // Parallel Items  
                if (fqp.TypeOfQuery == QueryType.ParallelItems && fqp.ParentGUID != Guid.Empty)
                {
                    // Association Type 
                    sql += SelectChildren(tbl, fqp.ParentGUID, fqp.MasterGUID, fqp.AssociationType);
                }

                // ME Personal Query Select 
                if (fqp.TypeOfQuery == QueryType.Personal)
                {
                    sql += SelectPersonal(tbl, fqp.UserGUID, fqp.MasterGUID);
                }

                // Creator Select 
                if (fqp.TypeOfQuery == QueryType.CreatedBy)
                {
                    sql += SelectCreatedByItems(tbl, fqp.UserGUID, fqp.MasterGUID);
                }

                // Modifyer Select 
                if (fqp.TypeOfQuery == QueryType.ModifiedBy)
                {
                    sql += SelectModifiedByItems(tbl, fqp.UserGUID, fqp.MasterGUID);
                }

                // JOIN 
                if (fqp.TypeOfQuery == QueryType.Join)
                {
                    // Query Table 
                    string jointbl = fqp.DatabaseTable.ToSecureString();
                    sql += SelectJOIN(tbl, fqp.QueryItemType, fqp.JoinFilter, fqp.MasterGUID, fqp.ParentGUID);
                }

                // NO-JOIN 
                if (fqp.TypeOfQuery == QueryType.NoJoin)
                {
                    // Query Table 
                    string jointbl = fqp.DatabaseTable.ToSecureString();
                    sql += SelectNoJOIN(tbl, fqp.QueryItemType, fqp.MasterGUID, fqp.ParentGUID);
                }

                // Property-JOIN 
                if (fqp.TypeOfQuery == QueryType.PropertyJoin)
                {
                    // Query Table 
                    string jointbl = fqp.DatabaseTable.ToSecureString();
                    sql += SelectPropertyJOIN(tbl, fqp.QueryItemType, fqp.JoinFilter, fqp.MasterGUID, fqp.ParentGUID);
                }



                // Parallel Items 
                //if (fqp.TypeOfQuery == QueryType.ParallelItemsByTag)
                //{
                //    if (fqp.ItemGUID == Guid.Empty)
                //        throw new Exception("ItemGUID (of item) not filled");

                //    if (fqp.QueryItemType.ToItemType() == ItemType.NULL)
                //        throw new Exception("ItemType not filled");

                //    sql = SelectParallelItemsByTag(fqp.QueryItemType.ToItemType(), fqp.ItemGUID);
                //}

                // --- END of specific Queries --- //
                #endregion


                #region FILTER
                // Filter Construction 
                sql += MultipleFilters(fqp.Filter);

                // Exclude Construction 
                sql += MultipleExcludes(fqp.Filter);
                #endregion

                // ItemTypes 
                #region ItemTypes
                // Multiple ItemTypes 
                sql += MultipleItemTypes(fqp.ItemTypes, fqp.QueryItemType, fqp.TypeOfQuery);

                sql += ItemTypesExcludes(fqp.ItemTypeExcludes, fqp.TypeOfQuery);
                #endregion

                // Archive 
                if (fqp.ItemState != "All")
                {
                    sql += " AND " + tblShort + ".ItemState = '" + fqp.ItemState.ToSecureString() + "' ";
                }

                // Search // Matchcode 
                if (!string.IsNullOrEmpty(fqp.Matchcode))
                {
                    sql += " AND " + tblShort + ".Title LIKE '%" + fqp.Matchcode + "%' ";
                }

                // Multiple Matchcodes 
                if (fqp.Matchcodes.Count >= 1)
                {
                    int mp = 1;  // Matchcode Position 
                    sql += " AND ( ";
                    foreach (string mc in fqp.Matchcodes)
                    {
                        sql += "  " + tblShort + ".Title LIKE '%" + mc + "%' ";

                        if (mp != fqp.Matchcodes.Count)
                        {
                            // NICHT das Ende erreicht 
                            sql += "  AND ";
                        }

                        mp++;
                    }
                    sql += " ) ";
                }

                // Metadata Match 
                if (!string.IsNullOrEmpty(fqp.MetadataMatch))
                {
                    sql += " AND " + tblShort + ".Metadata LIKE '%" + fqp.MetadataMatch + "%' ";
                }

                // Direction 
                #region Direction // Sortorder // ASC // DESC 
                string direction = (fqp.SortDirection == SortDirection.Ascending) ? " ASC " : " DESC ";

                //  COLLATE NOCASE  -> SQLITE 
                sql += " ORDER BY " + tblShort + "." + fqp.OrderByClause + " COLLATE NOCASE " + direction + " ";

                #endregion

                // Paging 
                #region Paging
                if (fqp.PageSize != 999) // Wenn 999 -> alle Einträge 
                {
                    int toSkipItems = (fqp.CurrentPage - 1) * fqp.PageSize;
                    toSkipItems = (toSkipItems < 0) ? 0 : toSkipItems;

                    // SQLITE 
                    sql += "LIMIT " + fqp.PageSize + " OFFSET " + toSkipItems + " ";
                }
                #endregion

                // NO Case Sensitive ??? 
                //sql += " COLLATE NOCASE ";

                // CLOSE 
                sql += " ; ";

                // RETURN 
                return sql;
            }
            catch (Exception ex)
            {
                return " SELECT " + tbl + ".* FROM " + tbl + "  WHERE " + tbl + ".MasterGUID = '" + fqp.MasterGUID + "';";
            }
        }
        
        // SINGLE
        public static string GetItemByGUIDQuery(IInputDTO input, string tbl)
        {
            string sql = string.Empty;

            // Content // Search // Security 
            sql += "SELECT * FROM  " + tbl + " WHERE " +
                " GUID = '" + input.ItemGUID + "' AND " +
                " MasterGUID = '" + input.MasterGUID + "' AND " +
                " AppType = '" + input.AppType + "' AND " +
                " ItemType = '" + input.ItemType + "'  ";

            return sql;
        }
        public static string GetItemDirectByGUIDWithoutParameter(IInputDTO input, string tbl)
        {
            string sql = string.Empty;

            // Content // Search // Security 
            sql += "SELECT * FROM  " + tbl + " WHERE " +
                " GUID = '" + input.ItemGUID + "' ";

            return sql;
        }
        public static string GetItemByTitleQuery(IInputDTO input, string tbl)
        {
            string sql = string.Empty;

            // Content // Search // Security !!! --- keine masterGUID Filterung --- !!! 
            sql += "SELECT * FROM  " + tbl + " WHERE " +
                " Title = '" + input.Title + "' AND " +
                " AppType = '" + input.AppType + "' AND " +
                " ItemType = '" + input.ItemType + "'  ";

            return sql;
        }
        public static string GetUserByMail(IInputDTO input, string tbl)
        {
            string query = string.Empty;

            // INFO: OLD 
            query = " SELECT DISTINCT " + tblShort + ".* FROM " + tbl + " AS " + tblShort + " ";

            // Normal Query 
            query += " INNER JOIN tbl_MTD_Property AS r " +
                        " ON (r.RelatedItemGUID = " + tblShort + ".GUID) " +
                        " WHERE s.ItemType = '" + input.ItemType + "' AND ";

            query += " ( r.Name = 'Mail' COLLATE NOCASE AND  r.Value = 'Value' COLLATE NOCASE ) ";

            return query;
        }
        // DELETE
        public static string GetDeleteQuery(IInputDTO input, string tbl)
        {
            return "DELETE FROM " + tbl + " WHERE GUID = '" + input.ItemGUID + "' AND MasterGUID = '" + input.MasterGUID + "'";
        }
        
        // COUNT
        public static string GetCountQuery(Guid MasterGUID, string tbl, string itemtype, Guid userGUID = new Guid())
        {
            // when called without parameters this will be true
            if (userGUID == Guid.Empty)
            {
                return "SELECT COUNT(Title) FROM " + tbl + " WHERE MasterGUID = '" + MasterGUID + "' AND ItemType = '" + itemtype + "' ";
            }
            else
            {
                string sql = "SELECT COUNT(Title) FROM " + tbl + " ";
                sql += " WHERE MasterGUID = '" + MasterGUID + "' AND ItemType = '" + itemtype + "' ";
                sql += " AND ( CreatedBy = '" + userGUID + "' OR ModifiedBy = '" + userGUID + "' ) ";
                return sql;
            }
        }
        public static string GetChildrenCountQuery(Guid MasterGUID, Guid parentGUID, string tbl, string childItemType)
        {
            string sql = "SELECT COUNT(Title) FROM tbl_CON_Content as s INNER JOIN tbl_TEC_Relation as r on(s.GUID = r.ChildGUID) ";
            sql += " WHERE r.ParentGUID = '" + parentGUID + "' ";
            sql += " AND s.MasterGUID = '" + MasterGUID + "'  ";
            sql += " AND s.ItemType = '" + childItemType + "' ";

            return sql;
        }
        public static string GetParentsCountQuery(Guid MasterGUID, Guid childGUID, string tbl, string parentItemType)
        {
            string sql = "SELECT COUNT(Title) FROM tbl_CON_Content as s INNER JOIN tbl_TEC_Relation as r on(s.GUID = r.ParentGUID) ";
            sql += " WHERE r.ChildGUID = '" + childGUID + "' ";
            sql += " AND s.MasterGUID = '" + MasterGUID + "'  ";
            sql += " AND s.ItemType = '" + parentItemType + "' ";

            return sql;
        }

        // SELECT 
        private static string Select(string table, Guid MasterGUID)
        {
            // Check 
            if (MasterGUID == Guid.Empty)
                throw new Exception("masterGUID GUID is empty");

            string query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " WHERE " + tblShort + ".MasterGUID = '" + MasterGUID.ToSecureString() + "' ";

            return query;
        }
        
        // SELECT Children + Parents
        private static string SelectChildren(string table, Guid ParentItemGUID, Guid MasterGUID, string associationType = "")
        {
            string query = string.Empty;

            query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " " +
                                        " INNER JOIN tbl_TEC_Relation AS r ON(r.ChildGUID = " + tblShort + ".GUID) " +
                                        " WHERE r.ParentGUID = '" + ParentItemGUID + "' COLLATE NOCASE " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            if (!string.IsNullOrEmpty(associationType))
            {
                query += " AND r.RelationType = '" + associationType + "' COLLATE NOCASE ";
            }

            return query;
        }
        private static string SelectParents(string table, Guid ChildItemGUID, Guid MasterGUID, string associationType = "")
        {
            string query = string.Empty;

            query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " " +
                                        " INNER JOIN tbl_TEC_Relation AS r " +
                                        " ON(r.ParentGUID = " + tblShort + ".GUID) " +
                                        " WHERE r.ChildGUID = '" + ChildItemGUID + "'  COLLATE NOCASE " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            if (!string.IsNullOrEmpty(associationType))
            {
                query += " AND r.RelationType = '" + associationType + "' COLLATE NOCASE ";
            }

            return query;
        }
        
        // SELECT JOIN
        private static string SelectPropertyJOIN(string table, string itemType, List<FilterByClauseDTO> filter, Guid MasterGUID, Guid ParentItemGUID = new Guid())
        {
            string query = string.Empty;

            // INFO: OLD 
            query = " SELECT DISTINCT " + tblShort + ".* FROM " + table + " AS " + tblShort + " ";

            // Normal Query 
            query += " INNER JOIN tbl_MTD_Property AS r " +
                        " ON (r.RelatedItemGUID = " + tblShort + ".GUID) " +
                        " WHERE s.ItemType = '" + itemType + "' AND ";

            query += " ( ";
            for (int i = 0; i <= filter.Count - 1; i++)
            {
                FilterByClauseDTO f = filter[i];
                query += " ( r.Name = '" + f.Property.ToSecureString() + "' COLLATE NOCASE AND  r.Value = '" + f.Value.ToSecureString() + "' COLLATE NOCASE ) ";
                if (i != filter.Count - 1)
                {
                    query += " OR ";
                }
            }

            query += " ) AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            return query;
        }
        private static string SelectJOIN(string table, string itemType, List<FilterByClauseDTO> filter, Guid MasterGUID, Guid ParentItemGUID = new Guid())
        {
            string query = string.Empty;

            // INFO: OLD 
            query = " SELECT DISTINCT " + tblShort + ".* FROM " + table + " AS " + tblShort + " ";

            // By Parent 
            query += " LEFT JOIN tbl_TEC_Relation AS p ON(p.ChildGUID = " + tblShort + ".GUID) ";

            // Normal Query 
            query += " INNER JOIN tbl_MTD_Property AS r " +
                        " ON (r.RelatedItemGUID = " + tblShort + ".GUID) " +
                        " WHERE s.ItemType = '" + itemType + "' AND ";

            // Parent 
            if (ParentItemGUID != Guid.Empty)
            {
                query += " p.ParentGUID = '" + ParentItemGUID + "' COLLATE NOCASE AND ";
            }

            query += " ( ";
            for (int i = 0; i <= filter.Count - 1; i++)
            {
                FilterByClauseDTO f = filter[i];
                query += " ( r.Name = '" + f.Property.ToSecureString() + "' COLLATE NOCASE AND  r.Value = '" + f.Value.ToSecureString() + "' COLLATE NOCASE ) ";
                if (i != filter.Count - 1)
                {
                    query += " OR ";
                }
            }

            query += " ) AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            // NO Parent 
            if (ParentItemGUID == Guid.Empty)
            {
                query += " AND p.ParentGUID IS NULL ";
            }

            return query;
        }
        private static string SelectNoJOIN(string table, string itemType, Guid MasterGUID, Guid ParentItemGUID = new Guid())
        {
            string query = string.Empty;

            // INFO: OLD 
            query = " SELECT DISTINCT " + tblShort + ".* FROM " + table + " AS " + tblShort + " ";

            // Parent 
            if (ParentItemGUID != Guid.Empty)
            {
                query += " INNER JOIN tbl_TEC_Relation AS p ON (p.ChildGUID = " + tblShort + ".GUID) ";
            }

            query += " LEFT JOIN tbl_MTD_Property AS r ON (r.RelatedItemGUID = " + tblShort + ".GUID) " +
                        " WHERE (r.RelatedItemGUID IS NULL AND s.ItemType = '" + itemType + "' ) " +
                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            // Parent 
            if (ParentItemGUID != Guid.Empty)
            {
                query += " AND p.ParentGUID = '" + ParentItemGUID + "' COLLATE NOCASE ";
            }

            // NO Parent 
            //if (ParentItemGUID == Guid.Empty)
            //{
            //    query += " AND p.ParentGUID IS NULL ";
            //}


            return query;
        }
        
        // SELECT Specific
        private static string SelectNewestItems(string table, Guid MasterGUID)
        {
            // Check 
            if (MasterGUID == Guid.Empty)
                throw new Exception("masterGUID GUID is empty");

            string query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " ";
            query += " WHERE " + tblShort + ".MasterGUID = '" + MasterGUID.ToSecureString() + "' ";

            return query;
        }
        private static string SelectPersonal(string table, Guid UserGUID, Guid MasterGUID)
        {
            string query = string.Empty;

            query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " " +
                                        " INNER JOIN tbl_TEC_Relation AS r " +
                                        " ON(r.ChildGUID = " + tblShort + ".GUID) " +
                                        " WHERE r.ParentGUID = '" + UserGUID + "' " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' ";

            return query;
        }
        private static string SelectCreatedByItems(string table, Guid UserGUID, Guid MasterGUID)
        {
            string query = string.Empty;

            query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " " +
                                        " WHERE " + tblShort + ".CreatedBy = '" + UserGUID + "' " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' ";

            return query;
        }
        private static string SelectModifiedByItems(string table, Guid UserGUID, Guid MasterGUID)
        {
            string query = string.Empty;

            query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " " +
                                        " WHERE " + tblShort + ".ModifiedBy = '" + UserGUID + "' " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' ";

            return query;
        }

        // ItemType 
        private static string MultipleItemTypes(List<string> itemTypes, string queryItemType, QueryType queryType)
        {
            string query = string.Empty;

            // Search -> keine ItemType Berücksichtigung
            if (queryType == QueryType.Search)
                return query;


            try
            {
                //// Normaler ItemType 
                //string redirect = queryItemType.ToItemType().RedirectByItemType().ToSecureString();
                //query += " AND ( ";
                //query += tblShort + ".ItemType = '" + redirect + "' ";
                //query += " ) ";

                // Default Query ItemType hinzufügen 
                if (!string.IsNullOrEmpty(queryItemType))
                { 
                    itemTypes.Add(queryItemType);
                }

                // zusätzliche ItemTypes 
                if (itemTypes.Count >= 1)
                {
                    query += " AND ( ";

                    // ISystemType Filter 
                    List<string> itemTypeFilter = new List<string>();

                    // Add ISystemType Filter 
                    foreach (string it in itemTypes)
                    {
                        // Check Redirect 
                        // string _it = it.ToItemType().RedirectByItemType().ToSecureString();

                        // add 
                        itemTypeFilter.Add(it);
                    }

                    // Durch ItemTypes durchgehen 
                    for (int i = 0; i < itemTypeFilter.Count; i++)
                    {
                        // Redirect holen -> es ist kein Problem dass der Redirect evtl. doppelt ist, da es eine OR Verknüpfung ist 
                        string redirect = itemTypeFilter[i].ToSecureString();
                        if (i == 0)
                        {
                            query += tblShort + ".ItemType = '" + redirect + "' ";
                        }
                        else
                        {
                            query += " OR " + tblShort + ".ItemType = '" + redirect + "' ";
                        }
                    }

                    query += " ) ";
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ItemTypes konnten nicht verarbeitet werden");
            }

            return query;

        }
        private static string ItemTypesExcludes(List<string> itemTypeExcludes, QueryType queryType)
        {
            string query = string.Empty;

            try
            {
                // ItemTypes 
                if (itemTypeExcludes.Count >= 1)
                {
                    query += " AND ( ";

                    // Durch ItemTypes durchgehen 
                    for (int i = 0; i < itemTypeExcludes.Count; i++)
                    {
                        // Redirect holen -> es ist kein Problem dass der Redirect evtl. doppelt ist, da es eine OR Verknüpfung ist 
                        string it = itemTypeExcludes[i].ToSecureString();
                        if (i == 0)
                        {
                            query += tblShort + ".ItemType != '" + it + "' ";
                        }
                        else
                        {
                            query += " AND " + tblShort + ".ItemType != '" + it + "' ";
                        }
                    }

                    query += " ) ";
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ItemTypes konnten nicht verarbeitet werden");
            }

            return query;

        }

        // Filters
        private static string MultipleFilters(List<FilterByClauseDTO> filters)
        {
            string query = string.Empty;

            try
            {
                if (filters.Count >= 1)
                {
                    for (int i = 0; i < filters.Count; i++)
                    {
                        FilterByClauseDTO fbc = filters[i];
                        if (fbc != null)
                        {
                            // Check for valid Filter 
                            if (!string.IsNullOrEmpty(fbc.Property) && fbc.Value != null)
                            {
                                query += " AND s." + fbc.Property + " " + " " + fbc.Comparison + " " + " " + fbc.Value.ToSecureSqlValue() + " ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Filter konnten nicht verarbeitet werden");
            }

            return query;
        }
        private static string MultipleExcludes(List<FilterByClauseDTO> excludes)
        {
            string query = string.Empty;

            try
            {
                if (excludes.Count >= 1)
                {
                    for (int i = 0; i < excludes.Count; i++)
                    {
                        FilterByClauseDTO fbc = excludes[i];
                        if (fbc != null)
                        {
                            // Check for valid Filter 
                            if (!string.IsNullOrEmpty(fbc.Property) && fbc.Value != null
                                && fbc.Comparison.ToLower().Contains("null"))
                            {
                                query += " AND s." + fbc.Property + " " + " is null ";
                            }
                            if (!string.IsNullOrEmpty(fbc.Property) && fbc.Value != null
                                && fbc.Comparison.ToLower().Contains("like"))
                            {
                                query += " AND s." + fbc.Property + " " + " not like " + " '%" + fbc.Value.ToSecureSqlValue() + "%' ";
                            }
                            if (!string.IsNullOrEmpty(fbc.Property) && fbc.Value != null
                                && fbc.Comparison.ToLower().Contains("not"))
                            {
                                query += " AND s." + fbc.Property + " " + " <> " + " '" + fbc.Value.ToSecureSqlValue() + "' ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Filter konnten nicht verarbeitet werden");
            }

            return query;
        }

    }

    internal static class ObsoleteQueries
    {
        // OBSOLETE 
        private static string tblShort = "s";
        // Query 

        private static string GetModifiedTodayCountQuery(Guid MasterGUID, string tbl, string itemtype)
        {
            string sql = "SELECT COUNT(Title) FROM " + tbl + " WHERE MasterGUID = '" + MasterGUID + "' AND ItemType = '" + itemtype + "' ";

            DateTime dt = DateTime.Now.Date;

            //// Date 
            //if (Helper.DatabaseTyp == tspDatabaseType.SQLServer)
            //{
            //    // SQL Server  
            //    sql += " AND ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            //}


            // SQLITE 
            sql += " AND ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            // sql += " AND ModifiedAt = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' ";


            return sql;
        }
        private static string SelectAppPermissions(Guid UserGUID, Guid MasterGUID)
        {
            string query = string.Empty;

            // TODOD: Permissions Query 
            query = " SELECT DISTINCT " + tblShort + ".* FROM tbl_SEC_Security AS " + tblShort + " " +
                                        " INNER JOIN tbl_TEC_Relation AS rP_UG ON(rP_UG.ChildGUID = " + tblShort + ".GUID) " +
                                        " INNER JOIN tbl_TEC_Relation AS rUG_U ON(rUG_U.ChildGUID = '" + UserGUID + "') " +
                                        " WHERE s.AppType = 'Security' AND (s.ItemType = 'Permission' OR  s.ItemType = 'License')  " +
                                        " AND " + tblShort + ".MasterGUID = '" + MasterGUID + "' COLLATE NOCASE ";

            return query;
        }
        private static string SelectChangedItemsToday(string table, Guid MasterGUID)
        {
            // Check 
            if (MasterGUID == Guid.Empty)
                throw new Exception("masterGUID GUID is empty");

            DateTime dt = DateTime.Now.Date;

            // Suche 
            string query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " WHERE " + tblShort + ".MasterGUID = '" + MasterGUID.ToSecureString() + "' ";

            // Date 
            //if (Helper.DatabaseTyp == tspDatabaseType.SQLServer)
            //{
            //    // SQL Server  
            //    query += " AND ( " + tblShort + ".ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            //    query += " OR " + tblShort + ".CreatedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ) ";
            //}


            // SQLITE 
            query += " AND ( " + tblShort + ".ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            query += " OR " + tblShort + ".CreatedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ) ";


            return query;
        }
        private static string SelectNewOrChangedComments(string table, Guid MasterGUID)
        {
            // Check 
            if (MasterGUID == Guid.Empty)
                throw new Exception("masterGUID GUID is empty");

            DateTime dt = DateTime.Now.Date;

            // Kommentare 
            string query = " SELECT " + tblShort + ".* FROM " + table + " AS " + tblShort + " WHERE " + tblShort + ".ItemType = 'Comment' AND " + tblShort + ".MasterGUID = '" + MasterGUID.ToSecureString() + "' ";

            // Date 
            //if (Helper.DatabaseTyp == tspDatabaseType.SQLServer)
            //{
            //    // SQL Server  
            //    query += " AND ( " + tblShort + ".ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            //    query += " OR " + tblShort + ".CreatedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ) ";
            //}

            // SQLITE 
            query += " AND ( " + tblShort + ".ModifiedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
            query += " OR " + tblShort + ".CreatedAt between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' AND '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ) ";


            return query;
        }
        private static string SelectItemsByTag(string matchcode)
        {
            string sql = string.Empty;

            sql += " SELECT DISTINCT s.* FROM tbl_CON_Content AS s ";

            sql += " LEFT JOIN tbl_TEC_Relation AS rC ON (s.GUID = rC.ChildGUID) ";
            sql += " LEFT JOIN tbl_CON_Content as tag ON (rC.ParentGUID = tag.GUID) ";
            sql += " LEFT JOIN tbl_TEC_Relation AS rT ON (tag.GUID = rT.ParentGUID) ";
            sql += " LEFT JOIN tbl_CON_Content as topic ON (rT.ChildGUID = topic.GUID) ";

            sql += " WHERE ( ";
            sql += " ( tag.Title LIKE '%" + matchcode + "%' ";
            sql += " OR topic.Title LIKE '%" + matchcode + "%' ";
            sql += " OR s.Title LIKE '%" + matchcode + "%' ) ";
            sql += " AND s.ItemType != 'Tag' ) ";

            return sql;
        }
        private static string SelectTopicsByTagByItem(Guid itemGUID)
        {
            string sql = string.Empty;

            sql += " SELECT DISTINCT s.* FROM tbl_CON_Content AS s ";

            sql += " INNER JOIN tbl_TEC_Relation AS rC ON (s.GUID = rC.ChildGUID) ";
            sql += " INNER JOIN tbl_CON_Content as tag ON (rC.ParentGUID = tag.GUID) ";
            sql += " INNER JOIN tbl_TEC_Relation AS rT ON (tag.GUID = rT.ParentGUID) ";
            sql += " INNER JOIN tbl_CON_Content as item ON (rT.ChildGUID = item.GUID) ";

            sql += " WHERE s.ItemType = 'Tag' AND s.Typ = 'Topic' AND item.GUID = '" + itemGUID.ToSecureString() + "' ";

            return sql;
        }
        private static string SelectCategoriesByTagByItem(Guid itemGUID)
        {
            string sql = string.Empty;

            sql += " SELECT DISTINCT s.* FROM tbl_CON_Content AS s ";

            sql += " INNER JOIN tbl_TEC_Relation AS rC ON (s.GUID = rC.ChildGUID) ";
            sql += " INNER JOIN tbl_CON_Content as tag ON (rC.ParentGUID = tag.GUID) ";
            sql += " INNER JOIN tbl_TEC_Relation AS rT ON (tag.GUID = rT.ParentGUID) ";
            sql += " INNER JOIN tbl_CON_Content as item ON (rT.ChildGUID = item.GUID) ";

            sql += " WHERE s.ItemType = 'Tag' AND s.Typ = 'Category' AND item.GUID = '" + itemGUID.ToSecureString() + "' ";

            return sql;
        }
        private static string SelectUseCasesByTagByItem(Guid itemGUID)
        {
            string sql = string.Empty;

            sql += " SELECT DISTINCT s.* FROM tbl_CON_Content AS s ";

            sql += " INNER JOIN tbl_TEC_Relation AS rC ON (s.GUID = rC.ChildGUID) ";
            sql += " INNER JOIN tbl_CON_Content as tag ON (rC.ParentGUID = tag.GUID) ";
            sql += " INNER JOIN tbl_TEC_Relation AS rT ON (tag.GUID = rT.ParentGUID) ";
            sql += " INNER JOIN tbl_CON_Content as item ON (rT.ChildGUID = item.GUID) ";

            sql += " WHERE s.ItemType = 'Tag' AND s.Typ = 'Theme' AND item.GUID = '" + itemGUID.ToSecureString() + "' ";

            return sql;
        }
    }

    // Property 
    public static partial class FactoryQuery
    {
        // Property 
        public static string GetPropertiesQuery(PropertyQueryParameter fpqp)
        {
            // Check 
            fpqp.Validate();

            // Query Table 
            string tbl = "tbl_MTD_Properties".ToSecureString();

            string query = "";

            // SELECT 
            query += " SELECT " + tbl + ".* FROM " + tbl + " ";

            // WHERE 
            query += " WHERE " + tbl + ".MasterGUID = '" + fpqp.MasterGUID.ToSecureString() + "' ";
            query += " AND   " + tbl + ".RelatedItemGUID = '" + fpqp.RelatedItemGUID.ToSecureString() + "' ";
            query += " AND   " + tbl + ".ItemType = '" + fpqp.QueryItemType.ToSecureString() + "' ";
            query += " AND   " + tbl + ".AppType = '" + fpqp.QueryAppType.ToSecureString() + "';";

            return query;
        }
        public static string GetCreatePropertyQuery(PropertyDTO input, string tbl = "tbl_MTD_Property")
        {
            // Check 
            input.Validate();

            // SQL 
            string sql = string.Empty;

            sql += "INSERT INTO " + tbl + " ([GUID], [MasterGUID], [RelatedItemGUID], " +
                                            " [AppType], [ItemType], " +
                                            " [DataType], [Name], " +
                                            " [Value], [Typ], [Date]) " +
                        " VALUES('" + input.GUID.ToSecureString() + "' , " +
                                    " '" + input.MasterGUID.ToSecureString() + "' , " +
                                    " '" + input.RelatedItemGUID.ToSecureString() + "' , " +

                                    " '" + input.AppType.ToSecureString() + "' , " +
                                    " '" + input.ItemType.ToSecureString() + "' , " +

                                    " '" + input.DataType.ToSecureString() + "' , " +
                                    " '" + input.Name.ToSecureString() + "' , " +

                                    " '" + input.Value.ToSecureString() + "', " +
                                    " '" + input.Typ.ToSecureString() + "', " +
                                    " '" + input.Date.ToSecureString() + "' )";

            return sql;
        }
        public static string GetUpdatePropertyQuery(PropertyDTO input, string tbl = "tbl_MTD_Property")
        {
            // Check 
            input.Validate();

            // SQL 
            string sql = string.Empty;

            sql += "UPDATE " + tbl + " SET [Value] = '" + input.Value + "', [Typ] = '" + input.Typ + "', [Date] = '" + input.Date.ToString() + "' " +
                                " WHERE  " + tbl + ".MasterGUID = '" + input.MasterGUID.ToSecureString() + "' " +
                                   " AND " + tbl + ".RelatedItemGUID =  '" + input.RelatedItemGUID.ToSecureString() + "' " +
                                   " AND " + tbl + ".GUID =  '" + input.GUID.ToSecureString() + "' " +
                                   " AND " + tbl + ".Name =  '" + input.Name.ToSecureString() + "' " +
                                   " AND " + tbl + ".AppType =  '" + input.AppType.ToSecureString() + "' " +
                                   " AND " + tbl + ".ItemType =  '" + input.ItemType.ToSecureString() + "' ;";

            return sql;
        }
        public static string GetDeleteAllPropertiesQuery(PropertyDTO input, string tbl = "tbl_MTD_Property")
        {
            // Check 
            input.Validate();

            // SQL 
            string sql = string.Empty;

            sql += "DELETE FROM " + tbl + " " +
                                " WHERE  " + tbl + ".MasterGUID = '" + input.MasterGUID.ToSecureString() + "' " +
                                   " AND " + tbl + ".RelatedItemGUID =  '" + input.RelatedItemGUID.ToSecureString() + "' " +
                                   " ;";

            return sql;
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
}
