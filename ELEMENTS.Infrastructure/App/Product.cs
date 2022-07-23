using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public class ProductItemType : ItemType, IItemType
    {
        public ProductItemType()
        {
            this.Title = "Product";
            this.Name = "Product";
            this.ID = Guid.NewGuid();

            Init();
        }

        private void Init()
        {
            // Fields 
            this.Fields = new List<IField>();
            this.Fields.Add(new EditField()
            {
                Title = "ID",
                ColumnCSSClass = "col-4",
                Description = "No of the entry.",
                Type = EditFieldType.TextBox
            });

            this.Fields.Add(new EditField() { Title = "Title", Property = "Title", ColumnCSSClass = "col-8", Type = EditFieldType.TextBox });

            // Columns 
            this.Columns = new List<IColumn>();
        }

        public override List<IItemType> GetChildItemTypes()
        {
            List<IItemType> its = new List<IItemType>();



            return its;
        }

        public override List<IItemType> GetParentItemTypes()
        {
            List<IItemType> its = new List<IItemType>();

            //its.Add(new PrincipalItemType());
            //its.Add(new RoleItemType());

            return its;
        }
    }
}
