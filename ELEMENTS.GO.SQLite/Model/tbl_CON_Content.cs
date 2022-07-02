using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
    // Fields 
    public partial class tbl_CON_Content : IDTO
    {
        // Fields 
        #region Properties
        // Identify 
        [Key]
        [Required]
        public Guid GUID { get; set; }

        [Required]
        public Guid MasterGUID { get; set; }

        // Identify 
        [MaxLength(50)]
        [Required]
        public string AppType { get; set; }

        [MaxLength(50)]
        [Required]
        public string ItemType { get; set; }

        public string ID { get; set; } = string.Empty;

        // Title // Matchcode 
        [MaxLength(500)]
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Matchcode { get; set; } = string.Empty;
        public bool Checked { get; set; } = false;

        // Created 
        public Guid CreatedBy { get; set; }

        [MaxLength(250)]
        public string Creator { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now.Date;

        // Modified 
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now.Date;

        [MaxLength(250)]
        public string Modifier { get; set; } = string.Empty;

        // strukturierte Metadatan (original json format)
        public string Metadata { get; set; } = string.Empty;

        // System Data 
        [MaxLength(50)]
        [Required]
        public string ItemState { get; set; }

        [MaxLength(50)]
        [Required]
        public string PrivacyType { get; set; }
        #endregion

        [NotMapped]
        public string NavigateUrl { get; set; } = string.Empty;

        // Methods 
        public bool Validate()
        {
            // Check 
            #region Check
            if (MasterGUID == Guid.Empty)
            {
                throw new Exception("Master GUID not set");
            }

            if (GUID == Guid.Empty)
            {
                throw new Exception("GUID not set");
            }

            if (string.IsNullOrEmpty(AppType))
            {
                throw new Exception("AppType not set");
            }

            if (string.IsNullOrEmpty(ItemType))
            {
                throw new Exception("ItemType not set");
            }
            #endregion

            return true;
        }
    }

    // Properties 
    public partial class tbl_CON_Content : IDTO
    {
        // Properties 
        [NotMapped]
        public List<IProperty> Properties { get; set; } = new List<IProperty>();
        public List<IProperty> LoadProperties()
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                {
                    Console.WriteLine("MasterGUID in LoadProperties is GUID Empty");
                    return Properties; }

                if (GUID == Guid.Empty)
                {
                    Console.WriteLine("GUID in LoadProperties is GUID Empty");
                    return Properties; }

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Properties 
                    Properties = (from item in context.tbl_MTD_Property
                                  where item.MasterGUID == MasterGUID
                                  && item.RelatedItemGUID == GUID
                                  select item).Cast<IProperty>().ToList();
                    return Properties;
                }

            }
            catch (Exception ex)
            {

            }
            return new List<IProperty>();
        }
        public IProperty GetProperty(string Property)
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                {
                    Console.WriteLine("MasterGUID in GetProperty is GUID Empty");
                    return null; 
                }

                if (GUID == Guid.Empty)
                {
                    Console.WriteLine("GUID in GetProperty is GUID Empty");
                    return null; 
                }

                // Name 
                string name = Property.ToSecureString();

                // Check for NULL 
                if (Properties == null)
                {
                    Properties = new List<IProperty>();
                }
                // Property 1.) RAM Query -> fastest as possible 
                if (Properties.Count == 0)
                {
                    // alle Properties laden 
                    LoadProperties();
                }

                // Property 2.) Full DB Query -> 1 TIME slow 
                IProperty _prop = Properties.Where(p => p.Name == name).FirstOrDefault();
                if (_prop == null)
                {
                    // LEERE Property erzeugen 
                    _prop = SetProperty(string.Empty, Property);
                }

                return _prop;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }

            return null;
        }
        public IProperty SetProperty(string Value, string Property)
        {
            // Name 
            string name = Property.ToSecureString();

            // Check for NULL 
            if (Properties == null)
            {
                Properties = new List<IProperty>();
            }
            // Property 1.) RAM Query -> fastest as possible 
            if (Properties.Count == 0)
            {
                // alle Properties laden 
                LoadProperties();
            }

            // Property 1.) RAM Query -> fastest as possible 
            IProperty _prop = Properties.Where(p => p.Name == name).FirstOrDefault();

            // Wenn weiterhin NULL -> ERZEUGEN 
            if (_prop == null)
            {
                // Property erzeugen -> muss über ClientDatabaseContext geschehen 
                // Erzeugung darf nicht in einem Control entstehen 
                _prop = new tbl_MTD_Property();
                _prop.GUID = Guid.NewGuid();
                _prop.Name = Property.ToSecureString();
                _prop.RelatedItemGUID = GUID;
                _prop.MasterGUID = MasterGUID;
                _prop.DataType = "Text";
            }

            // Property 
            IProperty p = null;

            // Wenn NICHT NULL 
            if (_prop != null)
            {
                // Wert setzen 
                _prop.Value = Value.ToSecureString();

                // Update 
                p = tbl_MTD_Property.SetItem(_prop);

                // Reload 
                LoadProperties();
            }

            // Return 
            return p;
        }
        public bool RemoveDeleteProperty(IProperty property)
        {
            int s = tbl_MTD_Property.DeleteItem(property);

            // Reload 
            LoadProperties();

            // RETURN 
            return (s == 1) ? true : false;
        }
    }

    // Settings 
    public partial class tbl_CON_Content : IDTO
    {
        // Settings 
        [NotMapped]
        public List<ISetting> Settings { get; set; } = new List<ISetting>();
        public List<ISetting> LoadSettings()
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return Settings; }

                if (GUID == Guid.Empty)
                { return Settings; }

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Setting 
                    Settings = (from item in context.tbl_SET_Setting
                                where item.MasterGUID == MasterGUID
                                && item.ReferenceID == GUID.ToString()
                                select item).Cast<ISetting>().ToList();
                    return Settings;
                }

            }
            catch (Exception ex)
            {

            }
            return new List<ISetting>();
        }
        public ISetting GetSetting(string Setting, string Level)
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return null; }

                if (GUID == Guid.Empty)
                { return null; }


                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Properties 
                    ISetting setting = (from item in context.tbl_SET_Setting
                                        where item.MasterGUID == MasterGUID
                                          && item.ReferenceID == GUID.ToSecureString()
                                          && item.Level == Level.ToSecureString()
                                          && item.Name == Setting.ToSecureString()
                                        select item).Cast<ISetting>().FirstOrDefault();
                    return setting;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public ISetting SetSetting(string Value, string Setting, string Level)
        {
            // Setting erzeugen -> muss über ClientDatabaseContext geschehen 
            // Erzeugung darf nicht in einem Control entstehen 
            tbl_SET_Setting set = new tbl_SET_Setting();
            set.GUID = Guid.NewGuid();
            set.Value = Value.ToSecureString();
            set.Name = Setting; // Search Filter 1 
            set.Level = Level; // Search Filter 2 
            set.ReferenceID = GUID.ToSecureString(); // Search Filter 3 
            set.MasterGUID = MasterGUID;

            // Update 
            ISetting s = tbl_SET_Setting.SetItem(set);

            // Reload 
            LoadSettings();

            // RETURN 
            return s;
        }
        public bool RemoveDeleteSetting(ISetting setting)
        {
            // Delete 
            int s = tbl_SET_Setting.DeleteItem(setting);

            // Reload 
            LoadSettings();

            // RETURN 
            return (s == 1) ? true : false;
        }
    }

    // Assign Remove 
    public partial class tbl_CON_Content : IDTO
    {
        // Assign 
        public void AssignAsChildToParent(Guid ParentItemGUID, string ParentItemType, string AssociationType)
        {
            try
            {
                if (ParentItemGUID == Guid.Empty ||
                    ParentItemType == "NULL")
                    return;

                if (string.IsNullOrEmpty(AssociationType))
                    return;

                RelationDTO rel = RelationDTO.CreateTemplate(
                    ParentItemGUID, ParentItemType.ToSecureString(),
                    GUID, ItemType,
                    AssociationType, MasterGUID);

                tbl_TEC_Relation.Assign(rel);
            }
            catch (Exception ex)
            {

            }
        }
        public void AssignAsParentToChild(Guid ChildItemGUID, string ChildItemType, string AssociationType)
        {
            try
            {
                if (ChildItemGUID == Guid.Empty ||
                        ChildItemType == "NULL")
                    return;

                if (string.IsNullOrEmpty(AssociationType))
                    return;

                RelationDTO rel = RelationDTO.CreateTemplate(
                    GUID, ItemType,
                    ChildItemGUID, ChildItemType.ToSecureString(),
                    AssociationType, MasterGUID);
                tbl_TEC_Relation.Assign(rel);
            }
            catch (Exception ex)
            {

            }

        }

        // Remove 
        public void RemoveItemFromParent(Guid ParentItemGUID, string ParentItemType, string AssociationType)
        {
            try
            {
                if (ParentItemGUID == Guid.Empty ||
                    ParentItemType == "NULL")
                    return;

                if (string.IsNullOrEmpty(AssociationType))
                    return;

                RelationDTO rel = RelationDTO.CreateTemplate(
                    ParentItemGUID, ParentItemType.ToSecureString(),
                    GUID, ItemType,
                    AssociationType, MasterGUID);

                tbl_TEC_Relation.Remove(rel);
            }
            catch (Exception ex)
            {

            }
        }
        public void RemoveItemFromChild(Guid ChildItemGUID, string ChildItemType, string AssociationType)
        {
            try
            {
                if (ChildItemGUID == Guid.Empty ||
                    ChildItemType == "NULL")
                    return;

                if (string.IsNullOrEmpty(AssociationType))
                    return;

                RelationDTO rel = RelationDTO.CreateTemplate(
                    GUID, ItemType,
                    ChildItemGUID, ChildItemType.ToSecureString(),
                    AssociationType, MasterGUID);

                tbl_TEC_Relation.Remove(rel);
            }
            catch (Exception ex)
            {

            }
        }
    }

    // Metadata 
    public partial class tbl_CON_Content : IDTO
    {
        // Metadata 
        public IItemMetadataDTO GetMetadata()
        {
            IItemMetadataDTO metadata = new ItemMetadata();

            metadata = ELEMENTS.Infrastructure.Helper.ToObjectFromJson(this.Metadata);

            return metadata;
        }
        public IItemMetadataDTO SetMetadata(IItemMetadataDTO metadata)
        {
            // Serialisierung JSON setzen 
            ItemMetadata mtd = metadata as ItemMetadata;

            Metadata = ELEMENTS.Infrastructure.Helper.ToJsonString(mtd);

            IItemMetadataDTO gmtd = GetMetadata();
            if (gmtd == null)
            {
                throw new Exception("Metadaten konnten nicht geladen werden");
            }
            return gmtd;
        }
        public IItemMetadataDTO SetMetadataSave(IItemMetadataDTO metadata)
        {
            // Serialisierung JSON setzen 
            ItemMetadata mtd = metadata as ItemMetadata;
            Metadata = ELEMENTS.Infrastructure.Helper.ToJsonString(mtd);

            // Konvertieren 
            IDTO item = this as IDTO;
            if (item != null)
            {
                // In Datenbank speichern 
                tbl_CON_Content.UpdateItem(item);
            }

            IItemMetadataDTO gmtd = GetMetadata();
            if (gmtd == null)
            {
                throw new Exception("Metadaten konnten nicht geladen werden");
            }
            return gmtd;
        }

    }


    // IFactory Methods 
    public partial class tbl_CON_Content : IDTO
    {
        // Create 
        public static IDTO CreateItem(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Create");
                }

                // Factory 
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                // Error 
                if (tbl == "null")
                {
                    throw new Exception("Tabelle wurde nicht identifiziert");
                }

                // INSERT // Generate the new Item ID 
                string id = GenerateID(input.ItemType);
                input.ID = id;

                // SQL 
                string sql = FactoryQuery.GetCreateQuery(input, tbl);

                // Result 
                int noOfRows = 0;
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // RUN 
                    noOfRows = context.Database.ExecuteSqlRaw(sql);
                }

                if (noOfRows == 0)
                {
                    throw new Exception("Der Eintrag konnte nicht erzeugt werden.");
                }
                if (noOfRows == 1)
                {
                    return GetItemByID(input);
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        // Update 
        public static IFactoryStatusInfo UpdateItem(IDTO input)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Update");
                }

                // Content 
                tbl_CON_Content item = input as tbl_CON_Content;
                if (item == null)
                {
                    info.Status = "FAIL";
                    info.Message = "Fehler bei der Konvertierung in das Datenbankitem";
                    return info;
                }

                // Update 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    var dbitem = (from query in context.tbl_CON_Content
                                  where query.GUID == item.GUID
                                  select query).FirstOrDefault();

                    if (dbitem != null)
                    {
                        // CONTENT 
                        dbitem = (tbl_CON_Content)Helper.MapPropertiesByReflection(item, dbitem);

                        // DATE 
                        dbitem.ModifiedAt = DateTime.Now;

                        if (string.IsNullOrEmpty(dbitem.ID))
                        {
                            // INSERT // Generate the new Item ID 
                            string id = GenerateID(dbitem.ItemType);
                            dbitem.ID = id;
                        }

                        // SAVE 
                        context.SaveChanges();

                        info.Status = "OK";
                        info.Message = "Update erfolgreich";
                    }
                }
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Fehler Update: " + ex.Message;
            }

            return info;
        }

        // Delete 
        public static int DeleteItem(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Delete");
                }

                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                string sql = FactoryQuery.GetDeleteQuery(input, tbl);
                int noOfRowDeleted = 0;

                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    noOfRowDeleted = context.Database.ExecuteSqlRaw(sql);
                }

                return noOfRowDeleted;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        // Item By ...
        public static IDTO GetItemByID(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Get Item");
                }

                string table = FactoryStructure.GetClientMapping(ElementsEntityType.Content).Class;
                string query = FactoryQuery.GetItemByGUIDQuery(input, table);

                IDTO dbitem = null;

                // Content -> Apps 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Query 
                    dbitem = context.tbl_CON_Content.FromSqlRaw(query).FirstOrDefault();
                }

                if (dbitem == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }

                return dbitem as IDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public static IDTO GetItemByTitle(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Get Item");
                }

                string table = FactoryStructure.GetClientMapping(ElementsEntityType.Content).Class;
                string query = FactoryQuery.GetItemByTitleQuery(input, table);

                IDTO dbitem = null;

                // Security 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Query 
                    dbitem = context.tbl_CON_Content.FromSqlRaw(query).FirstOrDefault();
                }

                if (dbitem == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }
                return dbitem as IDTO;

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        // Items By Parameter
        public static List<IDTO> GetItems(IQueryParameter fqp)
        {
            // Result 
            List<IDTO> result = new List<IDTO>();

            try
            {
                // Query 
                string query = FactoryQuery.GetItemsQuery(fqp);

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Query 
                    var items = context.tbl_CON_Content.FromSqlRaw(query);
                    result = items.Cast<IDTO>().ToList();
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        // Count 
        public static int GetAllItemsCount(IInputDTO input)
        {
            try
            {
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                string sql = string.Empty;

                // ItemGUID == User GUID für parametrisierte Count Abfrage  CreatedBy ModifiedBy 
                sql = FactoryQuery.GetCountQuery(input.MasterGUID, tbl, input.ItemType, input.ItemGUID);

                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    int count;
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = sql;
                            object result = command.ExecuteScalar();
                            count = Convert.ToInt32(result);
                        }
                    }
                    return count;
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }
    }

    public partial class tbl_CON_Content : IDTO
    {
        // Async 
        // Create 
        public static async Task<IDTO> CreateItemAsync(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Create");
                }

                // Factory 
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                // Error 
                if (tbl == "null")
                {
                    throw new Exception("Tabelle wurde nicht identifiziert");
                }

                // INSERT // Generate the new Item ID 
                string id = GenerateID(input.ItemType);
                input.ID = id;

                // SQL 
                string sql = FactoryQuery.GetCreateQuery(input, tbl);

                // Result 
                int noOfRows = 0;
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // RUN 
                    noOfRows = context.Database.ExecuteSqlRaw(sql);
                }

                if (noOfRows == 0)
                {
                    throw new Exception("Der Eintrag konnte nicht erzeugt werden.");
                }
                if (noOfRows == 1)
                {
                    return await GetItemByIDAsync(input);
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public static async Task<IDTO> UpdateItemAsync(IDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Update");
                }


                // Content 
                tbl_CON_Content item = input as tbl_CON_Content;
                if (item == null)
                { 
                    return null;
                }

                // Update 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    var dbitem = await (from query in context.tbl_CON_Content
                                        where query.GUID == item.GUID
                                        select query).FirstOrDefaultAsync();

                    if (dbitem != null)
                    {
                        // CONTENT 
                        dbitem = (tbl_CON_Content)Helper.MapPropertiesByReflection(item, dbitem);

                        // DATE 
                        dbitem.ModifiedAt = DateTime.Now;

                        if (string.IsNullOrEmpty(dbitem.ID))
                        {
                            // INSERT // Generate the new Item ID 
                            string id = GenerateID(dbitem.ItemType);
                            dbitem.ID = id;
                        }

                        // SAVE 
                        await context.SaveChangesAsync();
                        return dbitem as IDTO;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        public static async Task<IDTO> GetItemByIDAsync(IInputDTO input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Get Item");
                }

                string table = FactoryStructure.GetClientMapping(ElementsEntityType.Content).Class;
                string query = FactoryQuery.GetItemByGUIDQuery(input, table);

                IDTO dbitem = null;
                // Content 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Query 
                    dbitem = await context.tbl_CON_Content.FromSqlRaw(query).FirstOrDefaultAsync();
                }

                if (dbitem == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }

                return dbitem as IDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public static async Task<List<IDTO>> GetItemsAsync(IQueryParameter fqp)
        {
            // Result 
            List<IDTO> result = new List<IDTO>();

            // Query 
            string query = FactoryQuery.GetItemsQuery(fqp);

            try
            {
                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // CONTENT 
                    result = await context.tbl_CON_Content.FromSqlRaw(query).Cast<IDTO>().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        // Count 
        public static async Task<int> GetAllItemsCountAsync(IInputDTO input)
        {
            try
            {
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                string sql = string.Empty;
                sql = FactoryQuery.GetCountQuery(input.MasterGUID, tbl, input.ItemType);

                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    int count;
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = sql;
                            object result = await command.ExecuteScalarAsync();
                            count = Convert.ToInt32(result);
                        }
                    }
                    return count;
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }
        public static async Task<int> GetChildrenItemsCountAsync(IInputDTO input)
        {
            try
            {
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Content);
                string tbl = structure.Class;

                string sql = string.Empty;
                sql = FactoryQuery.GetChildrenCountQuery(input.MasterGUID, input.ItemGUID, tbl, input.ItemType);

                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    int count;
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = sql;
                            object result = await command.ExecuteScalarAsync();
                            count = Convert.ToInt32(result);
                        }
                    }
                    return count;
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

    }
    public partial class tbl_CON_Content : IDTO
    {

        // Helper 
        private static string GenerateID(string itemType)
        {
            string id = "" + GenerateShort(itemType) + "-";

            try
            {
                List<string> ids = new List<string>();

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Setting 
                    ids = (from item in context.tbl_CON_Content
                           where item.ItemType == itemType
                           && item.ID != null && item.ID != string.Empty
                           select item.ID).ToList();
                }
                int max = 0;
                foreach (string text in ids)
                {
                    int number = Helper.ExtractNumber(text);
                    if (number > max)
                    {
                        max = number;
                    }
                }

                return id + (max + 1);
            }
            catch (Exception ex)
            {

            }

            return id;
        }
        private static string GenerateShort(string itemType)
        {
            string result = string.Concat(itemType.Where(c => c >= 'A' && c <= 'Z'));

            if (result.Length == 2)
            {
                return result;
            }
            else if (result.Length > 2)
            {
                return result.Substring(0, 2);
            }
            else if (result.Length == 1)
            {
                return result + "Z";
            }
            else
            {
                return result + "AZ";
            }
        }
    }
}
