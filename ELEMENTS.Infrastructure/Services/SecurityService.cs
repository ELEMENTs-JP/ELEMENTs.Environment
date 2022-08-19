using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        IDTO SelectedPrincipal { get; set; }
        InformationNotificationService InfoNotifyService { get; }
    }

    public class SecurityService : ISecurityService, INotifyPropertyChanged
    {
        private readonly IWebHostEnvironment _environment;
        public SecurityService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;
        }

        public InformationNotificationService InfoNotifyService { get; } = new InformationNotificationService();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL: Property Changed: " + ex.Message);
            }
        }
        public ClaimsPrincipal CurrentUser { get; set; }

        IDTO sp = null;
        public IDTO SelectedPrincipal 
        {
            get { return sp; }
            set { sp = value;
                OnPropertyChanged(); } 
        }

        IDTO liu = null;
        public IDTO LoggedInUser 
        {
            get { return liu; }
            set { liu = value; 
                OnPropertyChanged(); } 
        }
    }


}
