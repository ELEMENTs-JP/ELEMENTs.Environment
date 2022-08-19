using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Data.SQLite
{
   
    public partial class SQLiteFactory : IFactory
    {
        // Administration 
        public void SetDatabasePath()
        {
            SQLiteContext.FullFilePath = SQLiteContext.FullDirectory + SQLiteContext.DbFileName;
        }
        public IFactoryStatusInfo CreateDatabase()
        {
            IFactoryStatusInfo info = SQLiteHelper.EnsureDatabaseCreated();
            info.Validate();
            return info;
        }
        public IFactoryStatusInfo DeleteDatabase()
        {
            IFactoryStatusInfo info = SQLiteHelper.DeleteDatabase();
            info.Validate();
            return info;
        }
        public IFactoryStatusInfo MigrateDatabase()
        {
            IFactoryStatusInfo info = SQLiteHelper.MigrateDatabase();
            info.Validate();
            return info;
        }

        public string GetDatabaseVersion()
        {
            return SQLiteHelper.GetMigrationVersion();
        }
        public Guid MasterGUID { get; set; } = Guid.Empty;
        public string MasterAppType { get; set; } = "ELEMENTs";
    }

    public partial class SQLiteFactory : IFactory
    {
        // Content 
        public IDTO CreateDirect(string Title, string ItemType)
        {
            try
            {
                Guid itemGUID = Guid.NewGuid();
                IInputDTO input = InputDTO.CreateTemplate(
                    itemGUID, Title, this.MasterGUID, this.MasterAppType, ItemType);
                
                // Check 
                if (input.Validate() == false)
                {
                    throw new Exception("Die Validierung schlug fehl");
                }

                IDTO dto = null;

                // Prüfung auf Existinz 
                if (input.CheckIfAlreadyExists == true)
                {
                    // Bei Titel holen 
                    dto = tbl_CON_Content.GetItemByTitle(input);

                    // wenn NICHT NULL -> wenn EXISTIERT 
                    if (dto != null)
                    {
                        // Wenn geprüft werden soll,
                        // dann wird existierendes zurückgegeben 
                        return dto;
                    }
                }

                // DTO 
                dto = tbl_CON_Content.CreateItem(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht erzeugt werden.");
                }

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IDTO Create(IInputDTO input)
        {
            try
            {
                // Check 
                if (input.Validate() == false)
                {
                    throw new Exception("Die Validierung schlug fehl");
                }

                IDTO dto = null;

                // Prüfung auf Existinz 
                if (input.CheckIfAlreadyExists == true)
                {
                    // Bei Titel holen 
                    dto = tbl_CON_Content.GetItemByTitle(input);

                    // wenn NICHT NULL -> wenn EXISTIERT 
                    if (dto != null)
                    {
                        // Wenn geprüft werden soll,
                        // dann wird existierendes zurückgegeben 
                        return dto;
                    }
                }

                // DTO 
                dto = tbl_CON_Content.CreateItem(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht erzeugt werden.");
                }

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IFactoryStatusInfo Delete(IInputDTO input)
        {

            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            // Input 
            if (input.Validate() == false)
            {
                info.Status = "FAIL";
                info.Message = "Validierung schlug fehl";
                return info;
            }

            try
            {
                // Base Class (new BASE METHOD) 
                int affectedItems = tbl_CON_Content.DeleteItem(input);

                if (affectedItems == 0)
                {
                    info.Status = "FAIL";
                    info.Message = "Kein Item betroffen";
                    return info;
                }

                // Properties 
                if (affectedItems != 0)
                {
                    // Properties löschen 
                    int props = tbl_MTD_Property.DeleteProperties(input.MasterGUID, input.ItemGUID);
                }

                // Settings 
                if (affectedItems != 0)
                {
                    // Settings löschen 
                    int sets = tbl_SET_Setting.DeleteSettings(input.MasterGUID, input.ItemGUID);
                }

                // Relation 
                if (affectedItems != 0)
                {
                    // Alle sonstigen Relationen löschen 
                    tbl_TEC_Relation.Delete(input.ItemGUID, input.MasterGUID);
                }

                info.Status = "OK";
                info.Message = "Ausführung erfolgreich";
                return info;
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Fehler bei der Ausführung: " + ex.Message;
                return info;
            }

        }
        public IFactoryStatusInfo Update(IDTO dto)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            try
            {
                // Update 
                dto.ModifiedAt = DateTime.Now;
                tbl_CON_Content.UpdateItem(dto);

                info.Status = "OK";
                info.Message = "Update erfolgreich";
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Fehler beim Update: " + ex.Message;
            }

            return info;
        }
        public IDTO GetItemByID(IInputDTO input)
        {
            try
            {
                // Check 
                if (input.Validate() == false)
                    return null;

                // DTO 
                IDTO dto = tbl_CON_Content.GetItemByID(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }
                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IDTO GetItemDirectByGUID(IInputDTO input)
        {
            try
            {
                // Check 
                if (input.Validate() == false)
                    return null;

                // DTO 
                IDTO dto = tbl_CON_Content.GetItemDirectByGUID(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }
                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<IDTO> GetItems(IQueryParameter fqp)
        {
            List<IDTO> allItems = new List<IDTO>();

            try
            {
                // Check 
                if (fqp.Validate() == false)
                { return allItems; }

                allItems = tbl_CON_Content.GetItems(fqp);
                return allItems;
            }
            catch (Exception ex)
            {
                return new List<IDTO>();
            }
        }
        public int GetItemsCount(IQueryParameter fqp)
        {
            try
            {
                InputDTO input = new InputDTO
                {
                    MasterGUID = fqp.MasterGUID,
                    ItemType = fqp.QueryItemType,
                    ItemGUID = fqp.UserGUID,
                    UserGUID = fqp.UserGUID,
                };

                // List 
                if (fqp.TypeOfQuery == QueryType.List)
                {
                    return tbl_CON_Content.GetAllItemsCount(input);
                }

                // Children 
                else if (fqp.TypeOfQuery == QueryType.ChildrenByParent ||
                        fqp.TypeOfQuery == QueryType.ParallelItems ||
                        fqp.TypeOfQuery == QueryType.DefaultChildren)
                {
                    input.ItemGUID = fqp.ParentGUID;
                    return tbl_CON_Content.GetChildrenItemsCount(input);
                }

                // Parents 
                else if (fqp.TypeOfQuery == QueryType.ParentsByChild)
                {
                    input.ItemGUID = fqp.ChildGUID;
                    return tbl_CON_Content.GetParentsItemsCount(input);
                }

                // Other 
                else
                {
                    return tbl_CON_Content.GetAllItemsCount(input);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return 0;
            }
        }
    }

    public partial class SQLiteFactory : IFactory
    {
        // Assign Remove 
        public IFactoryStatusInfo AssignRelation(IRelationDTO dto)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            try
            {
                if (dto.Validate() == false)
                {
                    info.Status = "FAIL";
                    info.Message = "Validation vailed";
                    return info;
                }

                tbl_TEC_Relation.Assign(dto);
                return info;
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
                return info;
            }
        }
        public IFactoryStatusInfo RemoveRelation(IRelationDTO dto)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            try
            {
                if (dto.Validate() == false)
                {
                    info.Status = "FAIL";
                    info.Message = "Validation vailed";
                    return info;
                }

                tbl_TEC_Relation.Remove(dto);
                return info;
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
                return info;
            }
        }
        public IFactoryStatusInfo DeleteRelation(Guid ChildParentGUID, Guid masterGUID)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            try
            {
                tbl_TEC_Relation.Delete(ChildParentGUID, masterGUID);
                return info;
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
                return info;
            }
        }
        public IRelationDTO GetRelation(IRelationDTO dto)
        {
            try
            {
                if (dto.Validate() == false)
                    return null;

                IRelationDTO relation = tbl_TEC_Relation.GetRelation(dto);
                return relation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public partial class SQLiteFactory : IFactory
    {
        public static IDTO GetUserByMail(string Mail)
        {
            try
            {
                // Check 
                if (string.IsNullOrEmpty(Mail))
                    return null;

                // DTO 
                IDTO dto = tbl_CON_Content.GetUserByMail(Mail);
                if (dto == null)
                {
                    throw new Exception("Der User konnte nicht geladen werden.");
                }
                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public partial class SQLiteFactory : IFactory
    {
        // Metadata 
    }

    public partial class SQLiteFactory : IFactory
    {
        // Properties 
    }

    public partial class SQLiteFactory : IFactory
    {
        // Settings 
    }

    public partial class SQLiteFactory : IFactory
    {
        // Content -> Async ??? 
        private static async Task<IDTO> CreateAsync(IInputDTO input)
        {
            try
            {
                // Check 
                if (input.Validate() == false)
                    return null;

                // DTO 
                IDTO dto = await tbl_CON_Content.CreateItemAsync(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht erzeugt werden.");
                }

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static async void UpdateAsync(IDTO dto)
        {
            try
            {
                // Update 
                dto.ModifiedAt = DateTime.Now;
                tbl_CON_Content.UpdateItemAsync(dto);
            }
            catch (Exception ex)
            {

            }
        }
        private static async Task<IDTO> GetItemByIDAsync(IInputDTO input)
        {
            try
            {
                // Check 
                if (input.Validate() == false)
                    return null;

                // DTO 
                IDTO dto = await tbl_CON_Content.GetItemByIDAsync(input);
                if (dto == null)
                {
                    throw new Exception("Das Item konnte nicht geladen werden.");
                }
                return dto;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        private static async Task<List<IDTO>> GetItemsAsync(IQueryParameter fqp)
        {
            List<IDTO> allItems = new List<IDTO>();

            try
            {
                // Check 
                if (fqp.Validate() == false)
                { return allItems; }

                allItems = await tbl_CON_Content.GetItemsAsync(fqp);
            }
            catch (Exception ex)
            {

            }

            return allItems;
        }
        private static async Task<int> GetItemsCountAsync(IQueryParameter fqp)
        {
            try
            {

                InputDTO input = new InputDTO
                {
                    MasterGUID = fqp.MasterGUID,
                    ItemType = fqp.QueryItemType,
                };

                if (fqp.ParentGUID != Guid.Empty)
                {
                    input.ItemGUID = fqp.ParentGUID;
                }

                return await tbl_CON_Content.GetAllItemsCountAsync(input);
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        private static async Task<int> GetChildrenItemsCountAsync(IQueryParameter fqp)
        {
            try
            {
                InputDTO input = new InputDTO
                {
                    MasterGUID = fqp.MasterGUID,
                    ItemType = fqp.QueryItemType,
                    ItemGUID = (fqp.ParentGUID != Guid.Empty) ? fqp.ParentGUID : fqp.ItemGUID,
                };

                return await tbl_CON_Content.GetChildrenItemsCountAsync(input);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
