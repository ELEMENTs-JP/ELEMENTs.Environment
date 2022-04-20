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
    public class tbl_MTD_Property : IProperty
    {
        #region Properties
        // Identify 
        [Key]
        [Required]
        public Guid GUID { get; set; }
        [Required]
        public Guid MasterGUID { get; set; }
        [Required]
        public Guid RelatedItemGUID { get; set; }

        // Typing 
        [Required]
        [MaxLength(50)]
        public string AppType { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemType { get; set; }

        // DataTyp 
        [Required]
        [MaxLength(50)]
        public string DataType { get; set; } // z.B.: DateTime, String, etc. 

        // Property 
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Values 
        [Required]
        [MaxLength(4000)]
        public string Value { get; set; } // anzuzeigender Wert (bei Password bspw. *****, etc.) 

        [Required]
        [MaxLength(50)]
        public string Typ { get; set; } // Gespeicherter Wert mit allen sonstigen Eigenschaften als JSON 

        // Extension 
        public DateTime Date { get; set; } // Anmerkungen zum Wert (z.B. ToolTip Informationen) 

        // Validation 
        public bool Validate()
        {
            if (GUID == Guid.Empty)
            {
                GUID = Guid.NewGuid();
            }
            if (RelatedItemGUID == Guid.Empty)
            { return false; }

            return true;
        }
        #endregion

        public static IProperty CreateTemplate(
          Guid itemGUID, Guid masterGUID, Guid relatedItemGUID,
          string propertyName, string propertyValue, string type = "Text")
        {
            IProperty inputProperty = new tbl_MTD_Property();
            inputProperty.GUID = itemGUID;
            inputProperty.MasterGUID = masterGUID;
            inputProperty.RelatedItemGUID = relatedItemGUID;
            inputProperty.Name = propertyName;
            inputProperty.Value = propertyValue;
            inputProperty.DataType = type.ToSecureString();
            return inputProperty;
        }

        // External 
        public static IProperty GetItem(IProperty input)
        {
            try
            {
                IProperty dbitem = null;

                // Validate 
                if (input.Validate() == false)
                {
                    
                }


                // Content 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    dbitem = (from query in context.tbl_MTD_Property
                              where query.Name == input.Name &&
                                    query.RelatedItemGUID == input.RelatedItemGUID &&
                                    query.MasterGUID == input.MasterGUID
                              select query).FirstOrDefault();
                }

                if (dbitem == null)
                {
                    dbitem = CreateItem(input);
                }

                return dbitem as IProperty;

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        public static IProperty SetItem(IProperty input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    
                }

                IProperty dbitem;

                // Content 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    dbitem = (from query in context.tbl_MTD_Property
                              where query.Name == input.Name &&
                                    query.RelatedItemGUID == input.RelatedItemGUID &&
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
                return dbitem as IProperty;

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        public static int DeleteItem(IProperty input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                    
                }

                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Property);
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
        private static IProperty CreateItem(IProperty input)
        {
            try
            {
                // Validate 
                if (input.Validate() == false)
                {
                  
                }

                // Factory 
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Property);
                string tbl = structure.Class;

                // Result 
                int noOfRows = 0;
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // RUN 
                    tbl_MTD_Property dbitem = new tbl_MTD_Property();
                    dbitem.GUID = input.GUID;
                    dbitem.MasterGUID = input.MasterGUID;
                    dbitem.RelatedItemGUID = input.RelatedItemGUID;
                    dbitem.Name = input.Name;
                    dbitem.Value = input.Value;
                    dbitem.Typ = "";
                    dbitem.DataType = "Text";
                    dbitem.AppType = (string.IsNullOrEmpty(input.AppType)) ? "Property" : input.AppType;
                    dbitem.ItemType = (string.IsNullOrEmpty(input.ItemType)) ? "Property" : input.ItemType;

                    context.tbl_MTD_Property.Add(dbitem);
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
        public static List<IProperty> GetItems(IQueryParameter fqp)
        {
            List<IProperty> result = new List<IProperty>();

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

                        List<IProperty> items = (from item in context.tbl_MTD_Property
                                                 where item.MasterGUID == fqp.MasterGUID
                                                 && item.RelatedItemGUID == fqp.ParentGUID
                                                 select item).Cast<IProperty>().ToList();
                        return items;
               
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        public static async Task<List<IProperty>> GetItemsAsync(IQueryParameter fqp)
        {
            List<IProperty> result = new List<IProperty>();

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
                  
                        List<IProperty> items = await (from item in context.tbl_MTD_Property
                                                       where item.MasterGUID == fqp.MasterGUID
                                                       && item.RelatedItemGUID == fqp.ParentGUID
                                                       select item).Cast<IProperty>().ToListAsync();
                        return items;
               
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
                FactoryStructure structure = FactoryStructure.GetClientMapping(ElementsEntityType.Property);
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
        public static int DeleteProperties(Guid MasterGUID, Guid RelatedItemGUID)
        {
            int count = 0;
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return count; }

                if (RelatedItemGUID == Guid.Empty)
                { return count; }

                // Context 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // Properties 
                    var props = (from item in context.tbl_MTD_Property
                                 where item.MasterGUID == MasterGUID
                                 && item.RelatedItemGUID == RelatedItemGUID
                                 select item);

                    count = props.Count();

                    if (count != 0)
                    {
                        context.tbl_MTD_Property.RemoveRange(props);
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
