using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public class DEFAULT : BaseApp, IApp
    {
        public DEFAULT()
        {
            ID = Guid.NewGuid();
            Title = "DEFAULT";
            Name = "DEFAULT";
            Description = "Default app";
            Group = "App";

        }
        public List<IItemType> GetItemTypes()
        {
            List<IItemType>  ItemTypes = new List<IItemType>();
            ItemTypes.Clear();

            // Add ItemTypes 
            ItemTypes.Add(new ProductItemType());
            
            return ItemTypes;
        }
   
    }
}
