using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
  
    public class EditItemRepository : IEditItemRepository
    {
        public bool IsInitialized { get; set; } = false;
        public string ItemType { get; set; } = string.Empty;
        public Guid ItemGUID { get; set; } = Guid.Empty;
        public ISQLiteService Service { get; set; }
        public IDTO DTO { get; set; }
        public IDTO Init()
        {
            try
            {
                if (string.IsNullOrEmpty(ItemType))
                {
                    IsInitialized = false;
                    throw new Exception("ItemType not initialized");
                }
                if (ItemGUID == Guid.Empty)
                {
                    IsInitialized = false;
                    throw new Exception("Item GUID not initialized");
                }

                // Prepare 
                IInputDTO input = InputDTO.GetItemTemplate(ItemGUID, Service.Factory.MasterGUID, "ELEMENTs", "Product");

                // Query 
                DTO = Service.Factory.GetItemByID(input);

                if (DTO != null)
                {
                    IsInitialized = true;
                }

                return DTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return null;
        }
        public IFactoryStatusInfo DeleteItem()
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            if (DTO == null)
            {
                info.Status = "FAIL";
                info.Message = "DTO = NULL";
                return info;
            }

            try
            {
                // Delete 
                IInputDTO input = InputDTO.CreateTemplate(
                    DTO.GUID, DTO.Title,
                    Service.Factory.MasterGUID,
                    "ELEMENTs", this.ItemType);
                info = Service.Factory.Delete(input);
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
            }

            return info;
        }
    }
}
