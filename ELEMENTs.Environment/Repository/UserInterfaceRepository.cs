using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public class EditInterfaceRepository : IEditInterfaceRepository
    {
        public bool IsInitialized { get; set; } = false;
        public List<IField> Fields { get; set; } = new List<IField>();
        public string ItemType { get; set; } = string.Empty;
        public EditInterfaceRepository()
        {
            
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(ItemType))
            {
                IsInitialized = false;
                return;
            }

            try
            {
                Fields = new List<IField>();
                Fields.Clear();

                Fields.Add(new EditField()
                {
                    Title = "ID",
                    ColumnCSSClass = "col-4",
                    Description = "No of the entry.",
                    Type = EditFieldType.TextBox
                });

                Fields.Add(new EditField() { Title = "Title", ColumnCSSClass = "col-4", Type = EditFieldType.TextBox });
                Fields.Add(new EditField() { Title = "Position", ColumnCSSClass = "col-4", Type = EditFieldType.TextBox });

                Fields.Add(new EditField() { Title = "First Name", ColumnCSSClass = "col-6", Type = EditFieldType.TextBox });
                Fields.Add(new EditField() { Title = "Last Name", ColumnCSSClass = "col-6", Type = EditFieldType.TextBox });
                Fields.Add(new EditField() { Title = "Notes", ColumnCSSClass = "col-12", Type = EditFieldType.TextArea });

                IsInitialized = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
