using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{

    public interface ISQLiteService
    {
        Guid MasterGUID { get; set; }
        IFactoryStatusInfo CreateDatabase(string name);
    }

   
}
