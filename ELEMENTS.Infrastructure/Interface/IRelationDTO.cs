using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    // Relation 
    public class RelationDTO : IRelationDTO
    {
        #region Properties 
        // Identify 
        public Guid GUID { get; set; }
        public Guid MasterGUID { get; set; }

        // Parent // Child 
        public Guid ParentGUID { get; set; }
        public Guid ChildGUID { get; set; }

        // Type 
        public string RelationType { get; set; }
        public string ParentItemType { get; set; }
        public string ChildItemType { get; set; }

        // Date 
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        // Info 
        public string Comment { get; set; } = string.Empty;
        #endregion

        // ctr 
        public RelationDTO()
        {

        }

        // Method 
        public static RelationDTO CreateTemplate(Guid parent, string parentItemType, Guid child, string childItemType,
                    string relation, Guid masterGUID, DateTime? start = null, DateTime? ende = null)
        {
            RelationDTO dto = new RelationDTO();

            try
            {
                // Date 
                if (start == null)
                { start = DateTime.Now.Date; }
                if (ende == null)
                { ende = DateTime.Now.Date.AddMonths(3); }

                // masterGUID 
                dto.MasterGUID = masterGUID;

                // Parent 
                dto.ParentGUID = parent;
                dto.ParentItemType = parentItemType;

                // Child 
                dto.ChildGUID = child;
                dto.ChildItemType = childItemType;

                // Typ 
                dto.RelationType = relation;
                dto.From = start.Value;
                dto.To = ende.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Erzeugen der Relation " + ex.Message);
            }

            return dto;
        }

        // Methods 
        public bool Validate()
        {
            // Check 
            #region Check
            if (MasterGUID == Guid.Empty)
            {
                throw new Exception("masterGUID GUID not set");
            }

            if (ParentGUID == Guid.Empty)
            {
                throw new Exception("Parent GUID not set");
            }

            if (ChildGUID == Guid.Empty)
            {
                throw new Exception("Child GUID not set");
            }

            if (string.IsNullOrEmpty(ParentItemType) || ChildItemType.ToSecureString() == "NULL")
            {
                throw new Exception("Parent ItemType not set");
            }

            if (string.IsNullOrEmpty(ChildItemType) || ChildItemType.ToSecureString() == "NULL")
            {
                throw new Exception("Child ItemType not set");
            }

            if (string.IsNullOrEmpty(RelationType) || RelationType.ToSecureString() == "NULL")
            {
                throw new Exception("Relation Type not set");
            }
            #endregion

            return true;
        }
    }

    // IRelationDTO == tbl_TEC_Relation == Verknüpfungstabelle 
    public interface IRelationDTO
    {
        // Identify 
        //Guid GUID { get; set; }
        Guid MasterGUID { get; set; }

        // Identify 
        Guid ParentGUID { get; set; }
        Guid ChildGUID { get; set; }

        // Typ 
        string RelationType { get; set; }
        string ChildItemType { get; set; }
        string ParentItemType { get; set; }

        // Comment 
        string Comment { get; set; }

        // Date 
        DateTime From { get; set; }
        DateTime To { get; set; }

        // Helper 
        bool Validate();

    
    }
}
