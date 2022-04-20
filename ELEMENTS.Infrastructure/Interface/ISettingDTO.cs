using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface ISetting
    {
        // Identify 
        Guid GUID { get; set; }
        Guid MasterGUID { get; set; }

        string AppType { get; set; }
        string ItemType { get; set; }

        string ReferenceID { get; set; } // z.B. GUID of Principal, of User, of Item, or App, ItemType 
        string Level { get; set; } // {Principal; User; App; ItemType; Item; } 
        string Name { get; set; } // = Setting (Name of Setting) // Property 
        string Value { get; set; } // = Value of the Setting / Property 

        // 
        bool Validate();
    }

}
