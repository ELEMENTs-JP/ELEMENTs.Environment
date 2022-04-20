using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public class FactoryStatusInfo : IFactoryStatusInfo
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public void Validate()
        {
            if (Status != "OK")
            {
                throw new Exception("Fehler: " + Message);
            }
        }
    }

}

