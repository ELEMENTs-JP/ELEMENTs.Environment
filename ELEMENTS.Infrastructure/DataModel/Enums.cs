using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public enum AppType
    { 
        NULL = 0,

        App = 1,
        Internal = 2,
        System = 3,
        Free = 4,
    }
    public enum ElementsEntityType
    {
        NULL = 0,

        Content = 1,
        Property = 2,
        Setting = 3,
        Relation = 4,
    }
    public enum Orientation
    {
        NULL = 0,
        Vertical = 1,
        Horizontal = 2,
    }
    public enum TabPosition
    {
        NULL = 0,
        Top = 1,
        Left = 2,
    }
    public enum LayoutType
    {
        NULL = 0,
        PageLayout = 1,
        ComponentLayout = 2,
        ControlLayout = 3,
    }
    public enum TeaserDirection { Left, Right };
}
