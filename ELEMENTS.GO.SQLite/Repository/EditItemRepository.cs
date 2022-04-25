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
        public Guid ItemGUID { get; set; } = Guid.Empty;
        public ISQLiteService Service { get; set; }
        public IDTO DTO { get; set; }
        public IDTO Init()
        {
            try
            {
                // Prepare 
                IInputDTO input = InputDTO.GetItemTemplate(ItemGUID, Service.Factory.MasterGUID, "ELEMENTs", "Product");

                // Query 
                DTO = Service.Factory.GetItemByID(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return null;
        }

    }
}
