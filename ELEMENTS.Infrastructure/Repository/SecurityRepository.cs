using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface ISecurityRepository
    {
        bool ValidatePermission(PermissionType Permission);
    }
}
