using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IEditInterfaceRepository
    {
        List<EditField> Fields { get; set; }
    }
    public class EditField
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColumnCSSClass { get; set; } = "col";
        public EditFieldType Type { get; set; } = EditFieldType.TextBox;

    }

    public enum EditFieldType
    { 
        NULL = 0,

        Text = 1,
        TextBox = 10,
        TextArea = 11,
    }
}
