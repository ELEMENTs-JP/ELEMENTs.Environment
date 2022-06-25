using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class RUNer : IApp
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "RUNer";
        public string Description { get; set; } = "Task Management";
    }
}
