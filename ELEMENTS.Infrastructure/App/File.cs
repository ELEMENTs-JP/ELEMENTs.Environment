using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace tsp.DEFAULT
{
    public class SystemFileItemType : ItemType, IItemType
    {
        public SystemFileItemType()
        {
            this.GUID = Guid.NewGuid();
            this.ID = "IT-FI-01";
            this.Title = "File";
            this.Name = "File";
            this.Typ = ItemTypeTyp.File;

            Init();
        }
        private void Init()
        {
            this.Fields = new List<IField>();
            this.Fields.Add(new EditField() { Title = "Title", ColumnCSSClass = "col-9", Mode = EditFieldMode.View, Type = EditFieldType.Text });
            this.Fields.Add(new EditField() { Title = "Kategorie", Property = "Kategorie", ColumnCSSClass = "col-3", Type = EditFieldType.TextBox });

            // File Properties 
            this.Fields.Add(new EditField() { Title = "ID", Property = "FileGUID", ColumnCSSClass = "col-3", Mode = EditFieldMode.View, Type = EditFieldType.Text });
            this.Fields.Add(new EditField() { Title = "Name", Property = "FileName", ColumnCSSClass = "col-6", Mode = EditFieldMode.View, Type = EditFieldType.Text });
            this.Fields.Add(new EditField() { Title = "Größe", Property = "FileSize", ColumnCSSClass = "col-3", Mode = EditFieldMode.View, Type = EditFieldType.Text });

            // Columns 
            #region Columns
            this.Columns = new List<IColumn>();
            this.Columns.Add(new Column() { Title = "ID", Property = "ID", ColumnCSSClass = "col-1 pe-2", Type = ColumnType.Text });
            this.Columns.Add(new Column() { Title = "Title", Property = "Title", ColumnCSSClass = "col px-2", Type = ColumnType.Link });
            #endregion
        }
    }
}
