using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class PropertyDTO : IProperty
    {
        // Identify 
        public Guid GUID { get; set; }
        public Guid RelatedItemGUID { get; set; }
        public Guid MasterGUID { get; set; }
        public string Name { get; set; }
        public string AppType { get; set; }
        public string ItemType { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public string Typ { get; set; }
        public DateTime Date { get; set; }

        // Method 
        public static PropertyDTO GetDefault(Guid itemGUID, Guid relatedItemGUID,
            string name, Guid masterGUID, string appType, string itemType)
        {
            PropertyDTO dto = new PropertyDTO();

            try
            {
                // masterGUID 
                dto.GUID = itemGUID;
                dto.MasterGUID = masterGUID;
                dto.RelatedItemGUID = relatedItemGUID;

                // Title 
                dto.Name = name;
                dto.AppType = appType;
                dto.ItemType = itemType;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Erzeugen der Relation " + ex.Message);
            }

            return dto;
        }

        public bool Validate()
        {
            // Check 
            #region Check
            if (GUID == Guid.Empty)
            {
                throw new Exception("GUID not set");
            }

            if (RelatedItemGUID == Guid.Empty)
            {
                throw new Exception("Related GUID not set");
            }

            if (MasterGUID == Guid.Empty)
            {
                throw new Exception("masterGUID GUID not set");
            }

            if (string.IsNullOrEmpty(AppType) || AppType.ToSecureString() == "NULL")
            {
                throw new Exception("AppType not set");
            }

            if (string.IsNullOrEmpty(ItemType) || ItemType.ToSecureString() == "NULL")
            {
                throw new Exception("ItemType not set");
            }
            #endregion

            return true;
        }
    }

    public interface IProperty
    {
        // Identify 
        Guid GUID { get; set; }
        Guid MasterGUID { get; set; }
        Guid RelatedItemGUID { get; set; }

        // Typing 
        string AppType { get; set; }
        string ItemType { get; set; }

        // DataTyp 
        string DataType { get; set; }

        // Property 
        string Name { get; set; }

        // Values 
        string Value { get; set; }
        string Typ { get; set; }

        // Extension 
        DateTime Date { get; set; }

        // 
        bool Validate();
    }

}
