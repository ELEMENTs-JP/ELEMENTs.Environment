using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
    public class tbl_TEC_Relation : IRelationDTO
    {
        // https://docs.microsoft.com/de-de/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt 

        // Identify 
        // ITEM GUID 

        [Required]
        public Guid MasterGUID { get; set; }

        [Required]
        public string AppType { get; set; } = string.Empty;

        // Parent 
        [Required]
        public Guid ParentGUID { get; set; }
        public string ParentItemType { get; set; } = string.Empty;

        // Child 
        [Required]
        public Guid ChildGUID { get; set; }
        public string ChildItemType { get; set; } = string.Empty;

        // Type 
        [Required]
        [MaxLength(50)]
        public string RelationType { get; set; } = "Relation";

        // Date 
        public DateTime From { get; set; } = DateTime.Now.Date;
        public DateTime To { get; set; } = DateTime.Now.Date;

        // Kommentar 
        [MaxLength(4000)]
        public string Comment { get; set; } = string.Empty;

        // Methods 
        public bool Validate()
        {
            // Check 
            #region Check
            if (MasterGUID == Guid.Empty)
            {
                throw new Exception("Master GUID not set");
            }

            if (ParentGUID == Guid.Empty)
            {
                throw new Exception("GUID not set");
            }

            if (ChildGUID == Guid.Empty)
            {
                throw new Exception("GUID not set");
            }

            if (string.IsNullOrEmpty(AppType))
            {
                throw new Exception("AppType not set");
            }

            if (string.IsNullOrEmpty(ParentItemType))
            {
                throw new Exception("ParentItemType not set");
            }
            if (string.IsNullOrEmpty(ChildItemType))
            {
                throw new Exception("ChildItemType not set");
            }
            if (string.IsNullOrEmpty(RelationType))
            {
                throw new Exception("RelationType not set");
            }
            #endregion

            return true;
        }

        // Assign // Remove 
        #region Assign // Remove
        internal static void Assign(IRelationDTO dto)
        {
            try
            {
                // check 
                if (dto.Validate() == false)
                    return;

                // Load 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // check 
                    var result = (from query in context.tbl_TEC_Relation
                                  where query.ParentGUID == dto.ParentGUID
                                  && query.ChildGUID == dto.ChildGUID
                                  && query.RelationType == dto.RelationType
                                  select query).FirstOrDefault();

                    // Exists already 
                    if (result != null)
                        return;

                    // NULL -> NEW -> CREATE 
                    tbl_TEC_Relation item = new tbl_TEC_Relation();

                    // GUIDs 
                    item.MasterGUID = dto.MasterGUID;
                    item.ParentGUID = dto.ParentGUID;
                    item.ChildGUID = dto.ChildGUID;

                    // Types 
                    item.ChildItemType = dto.ChildItemType;
                    item.ParentItemType = dto.ParentItemType;

                    // Association 
                    item.RelationType = (string.IsNullOrEmpty(dto.RelationType) ? "---" : dto.RelationType);
                    item.Comment = dto.Comment;

                    // Dates 
                    item.From = dto.From;
                    item.To = dto.To;

                    // Add 
                    context.tbl_TEC_Relation.Add(item);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler bei der Zuordnung " + ex.Message);
                // BaseHelper.LogEx(ex);
            }
        }
        internal static void Remove(IRelationDTO dto)
        {
            try
            {
                // check 
                if (dto.Validate() == false)
                    return;

                // Load 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    var result = (from query in context.tbl_TEC_Relation
                                  where query.ChildGUID == dto.ChildGUID &&
                                        query.ParentGUID == dto.ParentGUID &&
                                        query.MasterGUID == dto.MasterGUID &&
                                        query.RelationType == dto.RelationType
                                  select query).FirstOrDefault();

                    if (result == null)
                        return;

                    context.tbl_TEC_Relation.Remove(result);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // BaseHelper.LogEx(ex);
            }

        }
        internal static void Delete(Guid ChildParentGUID, Guid masterGUID)
        {
            try
            {
                // check 
                if (ChildParentGUID == Guid.Empty ||
                    masterGUID == Guid.Empty)
                    return;

                // Load 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    var result = (from query in context.tbl_TEC_Relation
                                  where (query.ParentGUID == ChildParentGUID ||
                                        query.ChildGUID == ChildParentGUID) &&
                                        query.MasterGUID == masterGUID
                                  select query);

                    if (result == null)
                        return;

                    context.tbl_TEC_Relation.RemoveRange(result);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        internal static IRelationDTO GetRelation(IRelationDTO dto)
        {
            try
            {
                // check 
                if (dto.Validate() == false)
                    return null;

                // Load 
                using (SQLiteContext context = SQLiteHelper.GetEntities())
                {
                    // check 
                    var result = (from query in context.tbl_TEC_Relation
                                  where query.ParentGUID == dto.ParentGUID
                                  && query.ChildGUID == dto.ChildGUID
                                  && query.RelationType == dto.RelationType
                                  select query).FirstOrDefault();

                    // Exists == TRUE 
                    if (result != null)
                    {
                        IRelationDTO rel = result as IRelationDTO;
                        if (rel != null)
                        {
                            return rel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler bei der Zuordnung " + ex.Message);
                // BaseHelper.LogEx(ex);
            }

            return null;
        }
        
        #endregion
    }

}
