using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface IAddItemRepository
    {
        string AppType { get; set; }
        string ItemType { get; set; }
        Guid MasterGUID { get; set; }
        string Value { get; set; }
        IDTO Create();
    }
    public class AddItemRepository : IAddItemRepository
    {
        public string AppType { get; set; } = "Default";
        public string ItemType { get; set; } = "Default";
        public Guid MasterGUID { get; set; }
        public string Value { get; set; }
        public IDTO Create()
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                {
                    return null;
                }

                IFactory factory = new SQLiteFactory();

                SQLiteContext.DbFileName = "ELEMENTs.db";
                factory.SetDatabasePath();

                Guid itemGUID = Guid.NewGuid();
                IInputDTO input = InputDTO.CreateTemplate(itemGUID, Value, MasterGUID, AppType, ItemType);
                IDTO dto = factory.Create(input);
                if (dto == null)
                {
                    return null;
                }
                if (dto != null)
                {
                    return dto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return null;
        }
    }
}
