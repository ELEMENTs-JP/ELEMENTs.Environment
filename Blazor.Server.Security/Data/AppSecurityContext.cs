using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Security.Data
{
    // TODO: Security - Security Data Context
    public class AppSecurityContext : IdentityDbContext
    {
        // TODO: Security - Security Data Context
        public AppSecurityContext(DbContextOptions<AppSecurityContext> options) 
            : base(options)
        { 
        } 
    }
}
