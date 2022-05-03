using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTs
{
    public class CustomerRepository : IEditInterfaceRepository
    {
        public List<EditField> Fields { get; set; } = new List<EditField>();

        public CustomerRepository()
        {
            SetFields();
        }

        private void SetFields()
        {
            Fields = new List<EditField>();
            Fields.Clear();

            Fields.Add(new EditField() { Title = "ID", ColumnCSSClass = "col-4", 
                                        Description = "No of the entry.", 
                                        Type = EditFieldType.TextBox });
            
            Fields.Add(new EditField() { Title = "Title", ColumnCSSClass = "col-4", Type = EditFieldType.TextBox });
            Fields.Add(new EditField() { Title = "Position", ColumnCSSClass = "col-4", Type = EditFieldType.TextBox });

            Fields.Add(new EditField() { Title = "First Name", ColumnCSSClass = "col-6", Type = EditFieldType.TextBox });
            Fields.Add(new EditField() { Title = "Last Name", ColumnCSSClass = "col-6", Type = EditFieldType.TextBox });
            Fields.Add(new EditField() { Title = "Notes", ColumnCSSClass = "col-12", Type = EditFieldType.TextArea });
        }
    }
}
