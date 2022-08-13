using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace ELEMENTS.Infrastructure
{
    public interface ISecurityService
    {
        ClaimsPrincipal CurrentUser { get; set; }
        IDTO LoggedInUser { get; set; }
        InformationNotificationService InfoNotifyService { get; }
    }

    public class SecurityService : ISecurityService
    {
        private readonly IWebHostEnvironment _environment;
        public SecurityService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;
        }

        public InformationNotificationService InfoNotifyService { get; } = new InformationNotificationService();
  
        public ClaimsPrincipal CurrentUser { get; set; }
        public IDTO LoggedInUser { get; set; }
    }


}
