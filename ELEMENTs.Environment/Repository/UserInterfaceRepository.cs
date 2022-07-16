using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public class UserInterfaceRepository : IUserInterfaceRepository
    {
        public bool IsInitialized { get; set; } = false;
        public List<IField> Fields { get; set; } = new List<IField>();
        public List<IColumn> Columns { get; set; } = new List<IColumn>();
        public IItemType ItemType { get; set; } 
        public UserInterfaceRepository()
        {
            
        }

        public void Init()
        {
            if (ItemType == null)
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
                Fields.Add(new EditField() { Title = "Position", Property = "Position", ColumnCSSClass = "col-4", Type = EditFieldType.TextBox });

                Fields.Add(new EditField() { Title = "Notes", ColumnCSSClass = "col-12", Type = EditFieldType.TextArea });

                IsInitialized = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
