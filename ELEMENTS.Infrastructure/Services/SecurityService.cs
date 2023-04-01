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
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Infrastructure
{
    public interface ISecurityService
    {
        ClaimsPrincipal CurrentUser { get; set; }
        IDTO LoggedInUser { get; set; }
        IDTO SelectedPrincipal { get; set; }
        InformationNotificationService InfoNotifyService { get; }

        List<IDTO> Permissions { get; set; }
        bool ValidatePermission(string AppID, string ItemTypeID, string PermissionID);

    }

    public class SecurityService : ISecurityService, INotifyPropertyChanged
    {
        private readonly IWebHostEnvironment _environment;
        public SecurityService(IWebHostEnvironment environment)
        {
            _environment = environment;

            string contentRootPath = _environment.ContentRootPath;

            InfoNotifyService.Notification += InfoNotifyService_Notification;
        }

        private Task InfoNotifyService_Notification(InformationNotification arg)
        {
            // Check 
            if (arg.IsDone == true)
            { 
                return Task.FromResult<string>("DONE");
            }

            // Setting 
            if (arg.Event == "Update:Setting")
            {
                if (arg.Filter == "SecurityMode")
                {
                    SelectedPrincipal.Settings = new List<ISetting>();
                    SelectedPrincipal.Settings.Clear();
                    SelectedPrincipal.LoadSettings();

                    System.Diagnostics.Debug.WriteLine("Update:Setting: Filter: " + arg.Filter + " - Principal Settings Security Mode Updated");

                    arg.IsDone = true;
                }
            }

            return Task.FromResult<string>("OK");
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
            set
            {
                sp = value;
                OnPropertyChanged();
            }
        }

        IDTO liu = null;
        public IDTO LoggedInUser
        {
            get { return liu; }
            set
            {
                liu = value;
                OnPropertyChanged();
            }
        }


        private List<IDTO> _permissions = new List<IDTO>();
        public List<IDTO> Permissions
        {
            get { return _permissions; }
            set { _permissions = value; }
        }

        public bool ValidatePermission(string AppID, string ItemTypeID, string PermissionID)
        {
            // Check 
            #region Check
            if (SelectedPrincipal == null)
                return false;

            if (LoggedInUser == null)
                return false;

            if (string.IsNullOrEmpty(AppID))
                return false;

            if (string.IsNullOrEmpty(ItemTypeID))
                return false;

            if (string.IsNullOrEmpty(PermissionID))
                return false;

            if (Permissions == null)
                return false;
            #endregion

            // 1.) Search 4 Permission
            IDTO perm = Permissions.Where(se =>
                    se.MasterGUID == SelectedPrincipal.GUID &&
                    se.Title.Contains(AppID) &&
                    se.Title.Contains(ItemTypeID) &&
                    se.Title.Contains(PermissionID)).FirstOrDefault();

            // Permission available
            if (perm != null)
            {
                if (perm.Title.ToSecureString().ToLower().Contains("Allow".ToLower()))
                {
                    // Allowed
                    return true;
                }
                else if (perm.Title.ToSecureString().ToLower().Contains("Deny".ToLower()))
                {
                    // Allowed
                    return true;
                }
                else
                {
                    // else
                    return SecurityModeValidation();
                }
            }

            return SecurityModeValidation();
        }

        private bool SecurityModeValidation()
        {
            bool decision = false;

            // NO Permission available
            // -> check global Setting
            try
            {
                ISetting setting = SelectedPrincipal.GetSetting("SecurityMode", "Principal");
                if (setting != null)
                {
                    string mode = setting.Value.ToSecureString();
                    if (mode == "Optimistisch")
                    {
                        System.Diagnostics.Debug.WriteLine("Security Mode: " + mode);
                        decision = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Security Mode: " + mode);
                        decision = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                decision = false;
            }

            return decision;
        }
    
    }
}
