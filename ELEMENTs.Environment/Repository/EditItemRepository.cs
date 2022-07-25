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
        public IItemType ItemType { get; set; }
        public Guid ItemGUID { get; set; } = Guid.Empty;
        public ISQLiteService Service { get; set; }
        public IDTO DTO { get; set; }
        public IDTO Init()
        {
            try
            {
                if (ItemType == null)
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
                    "ELEMENTs", this.ItemType.Name);
                info = Service.Factory.Delete(input);
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
            }

            return info;
        }

        public List<IDTO> ItemsByItemType(string ItemTypeName, string FilterProperty = "", string FilterValue = "")
        {
            try
            {
                if (ItemType != null)
                {
                    // Prepare 
                    IQueryParameter qp = QueryParameter.DefaultItemsQuery(
                        Service.Factory.MasterGUID, "ELEMENTs", ItemTypeName);
                    qp.PageSize = 999;
                    qp.CurrentPage = 1;

                    if (!string.IsNullOrEmpty(FilterProperty) && 
                        !string.IsNullOrEmpty(FilterValue))
                    {
                        FilterByClauseDTO filter = new FilterByClauseDTO();
                        filter.Property = FilterProperty;
                        filter.Value = FilterValue;
                        qp.JoinFilter.Add(filter);
                    }

                    // Query 
                    return Service.Factory.GetItems(qp);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL while ItemType Loading in DropDownList: " + ex.Message);
            }

            return new List<IDTO>();
        }
    }
}
