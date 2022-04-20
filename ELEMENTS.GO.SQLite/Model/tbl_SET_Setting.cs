using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
    public class tbl_SET_Setting : ISetting
    {
        #region Properties
        // Identify 
        [Key]
        [Required]
        public Guid GUID { get; set; }

        [Required]
        public Guid MasterGUID { get; set; }

        // Typing 
        [Required]
        [MaxLength(50)]
        public string AppType { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemType { get; set; }

        [Required]
        public string ReferenceID { get; set; } // z.B. GUID of Principal, of User, of Item, or App, ItemType 

        // Level 
        [Required]
        [MaxLength(50)]
        public string Level { get; set; } = string.Empty; // {Principal; User; App; ItemType; Item; } 

        // Setting // Property 
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // = Setting (Name of Setting) // Property 

        // Value 
        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = string.Empty; // = Value of the Setting / Property 
        #endregion

        // Validation 
        public bool Validate()
        {
            if (GUID == Guid.Empty)
            {
                GUID = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(ReferenceID))
            { return false; }

            return true;
        }

        // External 
        public static ISetting GetItem(ISetting input)
        {
            try
            {
                ISetting dbitem = null;

                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Get Item");
                }


                // Content 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    dbitem = (from query in context.tbl_SET_Setting
                              where query.Name == input.Name &&
                                    query.ReferenceID == input.ReferenceID &&
                                    query.MasterGUID == input.MasterGUID
                              select query).FirstOrDefault();
                }

                if (dbitem == null)
                {
                    dbitem = CreateItem(input);
                }

                return dbitem as ISetting;

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        public static ISetting SetItem(ISetting input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Get Item");
                }

                ISetting dbitem;

                // Content 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    dbitem = (from query in context.tbl_SET_Setting
                              where query.Name == input.Name &&
                                    query.ReferenceID == input.ReferenceID &&
                                    query.MasterGUID == input.MasterGUID
                              select query).FirstOrDefault();

                    if (dbitem != null)
                    {
                        // Set Setting 
                        dbitem.Value = input.Value;

                        // Update 
                        context.SaveChanges();
                    }
                }

                if (dbitem == null)
                {
                    dbitem = CreateItem(input);
                }

                // Return 
                return dbitem as ISetting;

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        public static int DeleteItem(ISetting input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Delete");
                }

                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Setting);
                string tbl = structure.Class;

                IInputDTO query = new InputDTO { ItemGUID = input.GUID, MasterGUID = input.MasterGUID };
                string sql = FactoryQuery.GetDeleteQuery(query, tbl);
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

        // Internal 
        private static ISetting CreateItem(ISetting input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    throw new Exception("Validierung des Input beim Create");
                }

                // Factory 
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Setting);
                string tbl = structure.Class;

                // Result 
                int noOfRows = 0;
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // RUN 
                    tbl_SET_Setting dbitem = new tbl_SET_Setting();
                    dbitem.AppType = input.AppType;
                    dbitem.ItemType = input.ItemType;
                    dbitem.GUID = input.GUID;
                    dbitem.MasterGUID = input.MasterGUID;
                    dbitem.ReferenceID = input.ReferenceID;
                    dbitem.Name = input.Name;
                    dbitem.Level = input.Level;
                    dbitem.Value = input.Value;
                    dbitem.AppType = (string.IsNullOrEmpty(input.AppType)) ? "Setting" : input.AppType;
                    dbitem.ItemType = (string.IsNullOrEmpty(input.ItemType)) ? "Setting" : input.ItemType;

                    context.tbl_SET_Setting.Add(dbitem);
                    context.SaveChanges();

                    noOfRows = 1;
                }

                if (noOfRows == 1)
                {
                    return GetItem(input);
                }
            }
            catch (Exception ex)
            {
             
            }

            return null;
        }
        public static List<ISetting> GetItems(QueryParameter fqp)
        {
            List<ISetting> result = new List<ISetting>();

            // Principal GUID 
            if (fqp.MasterGUID == Guid.Empty)
            { return result; }

            if (fqp.ParentGUID == Guid.Empty)
            { return result; }

            try
            {
                // Query 
                string query = FactoryQuery.GetItemsQuery(fqp);

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                 
                        List<ISetting> items = (from item in context.tbl_SET_Setting
                                                where item.MasterGUID == fqp.MasterGUID
                                                && item.ReferenceID == fqp.ParentGUID.ToString()
                                                select item).Cast<ISetting>().ToList();
                        return items;
             
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }
        public static async Task<List<ISetting>> GetItemsAsync(QueryParameter fqp)
        {
            List<ISetting> result = new List<ISetting>();

            // Principal GUID 
            if (fqp.MasterGUID == Guid.Empty)
            { return result; }

            if (fqp.ParentGUID == Guid.Empty)
            { return result; }

            try
            {
                // Query 
                string query = FactoryQuery.GetItemsQuery(fqp);

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                 
                        string referenceID = fqp.ParentGUID.ToSecureString();

                        List<ISetting> items = await (from item in context.tbl_SET_Setting
                                                      where item.MasterGUID == fqp.MasterGUID
                                                      && item.ReferenceID == referenceID
                                                      select item).Cast<ISetting>().ToListAsync();
                        return items;
                   
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }
        public static int GetAllItemsCount(IInputDTO input)
        {
            try
            {
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Setting);
                string tbl = structure.Class;

                string sql = FactoryQuery.GetCountQuery(input.MasterGUID, tbl, input.ItemType);
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    int count;
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = sql;
                            string result = command.ExecuteScalar().ToString();
                            int.TryParse(result, out count);
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

        public static ISetting CreateTemplate(
           Guid itemGUID, Guid masterGUID, string referenceID,
           string propertyName, string propertyValue)
        {
            ISetting input = new tbl_SET_Setting();
            input.Name = propertyName;
            input.Value = propertyValue;
            input.MasterGUID = masterGUID;
            input.ReferenceID = referenceID;
            input.GUID = itemGUID;

            return input;
        }

        public static int DeleteSettings(Guid MasterGUID, Guid RelatedItemGUID)
        {
            int count = 0;
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return count; }

                if (RelatedItemGUID == Guid.Empty)
                { return count; }

                string referenceID = RelatedItemGUID.ToSecureString();
                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Settings 
                    var sets = (from item in context.tbl_SET_Setting
                                where item.MasterGUID == MasterGUID
                                && item.ReferenceID == referenceID
                                select item);

                    count = sets.Count();

                    if (count != 0)
                    {
                        context.tbl_SET_Setting.RemoveRange(sets);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
             
            }

            return count;
        }
    }
}
