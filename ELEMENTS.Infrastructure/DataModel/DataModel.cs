using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ELEMENTS.Infrastructure
{
    public class NavigationEntry
    {
        public NavigationEntry()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = "GENERATED-" + Guid.NewGuid().ToString();
            }
        }
        public string ID { get; set; }
        public int Position { get; set; } = 1;
        public string Link { get; set; } = "/";
        public string Title { get; set; } = "Home";
        public string Icon { get; set; } = "";
        public string Group { get; set; } = "";

        public List<NavigationEntry> Items { get; set; } = new List<NavigationEntry>();
    }
  
    public class AuthentificationFeedback
    {
        public string Name { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "FAIL";

        public bool AcceptAgreements { get; set; } = false;

    }
    public class InputSelectionNotification
    {
        public string Title { get; set; }
    }
    public class InputSelectionNotificationService
    {
        public event Func<InputSelectionNotification, Task> Notify;

        public void NotifySync(InputSelectionNotification notification)
        {
            if (Notify != null)
            {
                Notify.Invoke(notification);
            }
        }
    }

    public class Entry : IEntry
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Matchcode { get; set; } = string.Empty;
        public bool Checked { get; set; } = false;
    }
    public class Identify : Entry, IIdentify
    {
        public Guid GUID { get; set; } = Guid.Empty;
        public Guid MasterGUID { get; set; } = Guid.Empty;
        public string ID { get; set; } = string.Empty;
    }

    public class SystemType : Identify, ISystemType
    { 
        public string AppType { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string ItemState { get; set; } = string.Empty;
        public string PrivacyType { get; set; } = string.Empty;

    }
    public class MetaDateItem : SystemType, IMetaDateObject
    {
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public string Creator { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ModifiedBy { get; set; } = Guid.Empty;
        public string Modifier { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        
    }

    public class DataObject : MetaDateItem, IDataObject
    {
        // Metadata 
        public string Metadata { get; set; } = string.Empty;
      
        // Metadata 
        public IItemMetadataDTO GetMetadata()
        {
            IItemMetadataDTO metadata = new ItemMetadata();

            metadata = Helper.ToObjectFromJson(this.Metadata);

            return metadata;
        }
        public IItemMetadataDTO SetMetadata(IItemMetadataDTO metadata)
        {
            // Serialisierung JSON setzen 
            ItemMetadata mtd = metadata as ItemMetadata;
            Metadata = Helper.ToJsonString(mtd);

            IItemMetadataDTO gmtd = GetMetadata();
            if (gmtd == null)
            {
                throw new Exception("Metadaten konnten nicht geladen werden");
            }
            return gmtd;
        }
        public IItemMetadataDTO SetMetadataSave(IItemMetadataDTO metadata)
        {
            // Serialisierung JSON setzen 
            ItemMetadata mtd = metadata as ItemMetadata;
            Metadata = Helper.ToJsonString(mtd);

            // Konvertieren 
            IDTO item = this as IDTO;
            if (item != null)
            {
                // TODO: FIND SOLUTION with Factory 
                // In Datenbank speichern 
                // tbl_CON_Content.UpdateItem(item);
                throw new NotImplementedException();
            }

            IItemMetadataDTO gmtd = GetMetadata();
            if (gmtd == null)
            {
                throw new Exception("Metadaten konnten nicht geladen werden");
            }
            return gmtd;
        }
    }

    public class NavigateOject: DataObject, INavigateObject
    {
        [NotMapped]
        public string NavigateUrl { get; set; }
    }
    public class PropertyObject : NavigateOject, IPropertyObject
    {
        // Properties 
        [NotMapped]
        public List<IProperty> Properties { get; set; } = new List<IProperty>();
        public List<IProperty> LoadProperties()
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return Properties; }

                if (GUID == Guid.Empty)
                { return Properties; }

                // Context 
                //using (SQLiteContext context = SQLiteHelper.GetEntities())
                //{
                //    // Properties 
                //    Properties = (from item in context.tbl_MTD_Property
                //                  where item.MasterGUID == MasterGUID
                //                  && item.RelatedItemGUID == GUID
                //                  select item).Cast<IProperty>().ToList();
                //    return Properties;
                //}

            }
            catch (Exception ex)
            {

            }
            return new List<IProperty>();
        }
        public IProperty GetProperty(string Property)
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return null; }

                if (GUID == Guid.Empty)
                { return null; }

                // Name 
                string name = Property.ToSecureString();

                // Check for NULL 
                if (Properties == null)
                {
                    Properties = new List<IProperty>();
                }
                // Property 1.) RAM Query -> fastest as possible 
                if (Properties.Count == 0)
                {
                    // alle Properties laden 
                    LoadProperties();
                }

                // Property 2.) Full DB Query -> 1 TIME slow 
                IProperty _prop = Properties.Where(p => p.Name == name).FirstOrDefault();
                if (_prop == null)
                {
                    // LEERE Property erzeugen 
                    _prop = SetProperty(string.Empty, Property);
                }

                return _prop;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public IProperty SetProperty(string Value, string Property)
        {
            // Name 
            string name = Property.ToSecureString();

            // Check for NULL 
            if (Properties == null)
            {
                Properties = new List<IProperty>();
            }
            // Property 1.) RAM Query -> fastest as possible 
            if (Properties.Count == 0)
            {
                // alle Properties laden 
                LoadProperties();
            }

            // Property 1.) RAM Query -> fastest as possible 
            IProperty _prop = Properties.Where(p => p.Name == name).FirstOrDefault();

            // Wenn weiterhin NULL -> ERZEUGEN 
            if (_prop == null)
            {
                // Property erzeugen -> muss über ClientDatabaseContext geschehen 
                // Erzeugung darf nicht in einem Control entstehen 
                //_prop = new tbl_MTD_Property();
                //_prop.GUID = Guid.NewGuid();
                //_prop.Name = Property.ToSecureString();
                //_prop.RelatedItemGUID = GUID;
                //_prop.MasterGUID = MasterGUID;
                //_prop.DataType = "Text";
            }

            // Property 
            IProperty p = null;

            // Wenn NICHT NULL 
            if (_prop != null)
            {
                // Wert setzen 
                _prop.Value = Value.ToSecureString();

                // Update 
                // p = tbl_MTD_Property.SetItem(_prop);

                // Reload 
                LoadProperties();
            }

            // Return 
            return p;
        }
        public bool RemoveDeleteProperty(IProperty property)
        {
            // Delete 
            //int s = tbl_SET_Setting.DeleteItem(setting);

            // Reload 
            LoadProperties();

            // RETURN 
            // return (s == 1) ? true : false;
            return false;
        }
    }
    public class SettingObject : PropertyObject, ISettingObject
    {
        // Settings 
        [NotMapped]
        public List<ISetting> Settings { get; set; } = new List<ISetting>();
        public List<ISetting> LoadSettings()
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return Settings; }

                if (GUID == Guid.Empty)
                { return Settings; }

                // Context 
                //using (SQLiteContext context = SQLiteHelper.GetEntities())
                //{
                //    // Setting 
                //    Settings = (from item in context.tbl_SET_Setting
                //                where item.MasterGUID == MasterGUID
                //                && item.ReferenceID == GUID.ToString()
                //                select item).Cast<ISetting>().ToList();
                //    return Settings;
                //}

            }
            catch (Exception ex)
            {

            }
            return new List<ISetting>();
        }
        public ISetting GetSetting(string Setting, string Level)
        {
            try
            {
                // Principal GUID 
                if (MasterGUID == Guid.Empty)
                { return null; }

                if (GUID == Guid.Empty)
                { return null; }


                // Context 
                //using (SQLiteContext context = SQLiteHelper.GetEntities())
                //{
                //    // Properties 
                //    ISetting setting = (from item in context.tbl_SET_Setting
                //                        where item.MasterGUID == MasterGUID
                //                          && item.ReferenceID == GUID.ToSecureString()
                //                          && item.Level == Level.ToSecureString()
                //                          && item.Name == Setting.ToSecureString()
                //                        select item).Cast<ISetting>().FirstOrDefault();
                //    return setting;
                //}
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public ISetting SetSetting(string Value, string Setting, string Level)
        {
            // Setting erzeugen -> muss über ClientDatabaseContext geschehen 
            // Erzeugung darf nicht in einem Control entstehen 
            //tbl_SET_Setting set = new tbl_SET_Setting();
            //set.GUID = Guid.NewGuid();
            //set.Value = Value.ToSecureString();
            //set.Name = Setting; // Search Filter 1 
            //set.Level = Level; // Search Filter 2 
            //set.ReferenceID = GUID.ToSecureString(); // Search Filter 3 
            //set.MasterGUID = MasterGUID;

            //// Update 
            //ISetting s = tbl_SET_Setting.SetItem(set);

            // Reload 
            LoadSettings();

            // RETURN 
            return null;
        }
        public bool RemoveDeleteSetting(ISetting setting)
        {
            // Delete 
            //int s = tbl_SET_Setting.DeleteItem(setting);

            // Reload 
            LoadSettings();

            // RETURN 
            // return (s == 1) ? true : false;
            return false;
        }
    }

    public class DTO : SettingObject, IDTO
    {
        public bool Validate()
        {
            return true;
        }
    }
}
