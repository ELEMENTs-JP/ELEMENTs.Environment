using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace tsp.DEFAULT
{
    public class SystemProductItemType : ItemType, IItemType
    {
        public SystemProductItemType()
        {
            this.GUID = Guid.NewGuid();
            this.ID = "IT-PROD-01";
            this.Title = "Product";
            this.Name = "Product";

            Init();
        }

        private void Init()
        {
            // Fields 
            this.Fields = new List<IField>();
            this.Fields.Add(new EditField()
            {
                Title = "ID",
                ColumnCSSClass = "col-3",
                Description = "No of the entry.",
                Type = EditFieldType.TextBox
            });

            this.Fields.Add(new EditField() { Title = "Title", ColumnCSSClass = "col-6", Mode = EditFieldMode.View, Type = EditFieldType.Text });
            this.Fields.Add(new EditField() { Title = "Kategorie", Property = "Kategorie", ColumnCSSClass = "col-6", Type = EditFieldType.TextBox });
            this.Fields.Add(new EditField() { Title = "Fortschritt", Property = "Progress", ColumnCSSClass = "col-6", Type = EditFieldType.ProgressBox });
            
            this.Fields.Add(new EditField() { Title = "Priorität", Property = "Priority", ColumnCSSClass = "col-6", Type = EditFieldType.Priority });
            this.Fields.Add(new EditField() { Title = "Status", Property = "Status", ColumnCSSClass = "col-6", Type = EditFieldType.Status });

            // Columns 
            this.Columns = new List<IColumn>();
            this.Columns.Add(new Column() { Title = "ID", Property = "ID", ColumnCSSClass = "col-1 pe-2", Type = ColumnType.Text });
            this.Columns.Add(new Column() { Title = "Title", Property = "Title", ColumnCSSClass = "col px-2", Type = ColumnType.Link });
            this.Columns.Add(new Column() { Title = "Kategorie", Property = "Kategorie", ColumnCSSClass = "col px-2", Type = ColumnType.Text });
            this.Columns.Add(new Column() { Title = "Fortschritt", Property = "Progress", ColumnCSSClass = "col px-2", Type = ColumnType.Progress });
            
            this.Columns.Add(new Column() { Title = "Priorität", Property = "Priority", ColumnCSSClass = "col px-2", Type = ColumnType.Priority});
            this.Columns.Add(new Column() { Title = "Status", Property = "Status", ColumnCSSClass = "col px-2", Type = ColumnType.Status });

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
