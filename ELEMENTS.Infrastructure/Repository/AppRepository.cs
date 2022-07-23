using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Infrastructure
{
    public interface IAppRepository
    {
        List<IApp> Apps { get; set; }
        IItemType GetItemTypeByName(string ItemType);
        IApp GetAppByItemTypeName(string ItemType);
        
        void Init();
    }
 
}
