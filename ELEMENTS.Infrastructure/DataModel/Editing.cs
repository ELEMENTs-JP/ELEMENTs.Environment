using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IEditInterfaceRepository
    {
        void Init();
        List<EditField> Fields { get; set; }
    }
    public class EditField
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColumnCSSClass { get; set; } = "col";
        public string Property { get; set; } = "Title";
        public EditFieldType Type { get; set; } = EditFieldType.TextBox;
        public EditFieldMode Mode { get; set; } = EditFieldMode.View;
    }

    public enum EditFieldType
    { 
        NULL = 0,

        Text = 1,
        TextBox = 10,
        TextArea = 11,
    }

    public enum EditFieldMode
    { 
        NULL = 0,

        Hidden = 1,
        View = 2,
        Edit = 3,
    }
}
