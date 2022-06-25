using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class FILEster : IApp
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "FILEster";
        public string Description { get; set; } = "File Management";
    }
}
