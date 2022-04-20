using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS
{
    public class OptionItem
    {
        public string ID { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool isSelected { get; set; } = false;
    }
}
