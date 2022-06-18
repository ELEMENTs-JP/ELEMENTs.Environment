using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class ItemMetadata : IItemMetadataDTO
    {
        public Guid ItemGUID { get; set; } = Guid.Empty;
        public string Language { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Metadata { get; set; } = string.Empty;
    }

    public interface IItemMetadataDTO
    {
        Guid ItemGUID { get; set; } 
        string Language { get; set; } 
        string Version { get; set; }
        string Comments { get; set; }
        string Tags { get; set; }
        string Metadata { get; set; }
    }
}
