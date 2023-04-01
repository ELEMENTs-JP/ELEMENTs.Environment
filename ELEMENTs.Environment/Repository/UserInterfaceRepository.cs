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
        public string Group { get; set; } = string.Empty;
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

                if (this.ItemType != null)
                {
                    foreach (IField field in this.ItemType.Fields)
                    {
                        Fields.Add(field);
                    }
                }
                else
                {
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
                }

                IsInitialized = true;
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class SettingInterfaceRepository : ISettingInterfaceRepository
    {
        public bool IsInitialized { get; set; } = false;
        public List<ISettingField> Fields { get; set; } = new List<ISettingField>();
        public IItemType ItemType { get; set; }
        public string Group { get; set; } = string.Empty;
        public SettingInterfaceRepository()
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
                Fields = new List<ISettingField>();
                Fields.Clear();

                if (ItemType != null)
                {
                    foreach (ISettingField field in ItemType.GetSettings())
                    {
                        Fields.Add(field);
                    }
                }

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
            }
        }
    }
}
