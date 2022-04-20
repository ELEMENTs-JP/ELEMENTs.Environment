using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IEntry
    {
        string Title { get; set; }
        string Content { get; set; }
        string Matchcode { get; set; }
        bool Checked { get; set; }
    }
    public interface IIdentify : IEntry
    {
        Guid GUID { get; set; }
        Guid MasterGUID { get; set; }
        string ID { get; set; } // Z.B. Artikel Nr. 

    }
    public interface ISystemType : IIdentify
    {
        string AppType { get; set; }
        string ItemType { get; set; }
        string ItemState { get; set; }
        string PrivacyType { get; set; }

    }
    public interface IMetaDateObject : ISystemType
    {
        Guid CreatedBy { get; set; }
        string Creator { get; set; }
        DateTime CreatedAt { get; set; }

        Guid ModifiedBy { get; set; }
        string Modifier { get; set; }
        DateTime ModifiedAt { get; set; }
    }

    public interface IDataObject : IMetaDateObject
    {
        string Metadata { get; set; }

        // MetaData 
        IItemMetadataDTO GetMetadata();
        IItemMetadataDTO SetMetadata(IItemMetadataDTO metadata);
        IItemMetadataDTO SetMetadataSave(IItemMetadataDTO metadata);
    }

    public interface INavigateObject : IDataObject
    {
        [NotMapped]
        string NavigateUrl { get; set; }
    }

    public interface IPropertyObject : INavigateObject
    {
        List<IProperty> Properties { get; set; }
        List<IProperty> LoadProperties();
        IProperty GetProperty(string Property);
        IProperty SetProperty(string Value, string Property);
        bool RemoveDeleteProperty(IProperty property);
    }
    public interface ISettingObject : IPropertyObject
    {
        List<ISetting> Settings { get; set; }
        List<ISetting> LoadSettings();
        ISetting GetSetting(string Setting, string Level);
        ISetting SetSetting(string Value, string Setting, string Level);
        bool RemoveDeleteSetting(ISetting setting);
    }

    // IContentDTO == tbl_CON_Content == Datentabelle 
    public interface IDTO : ISettingObject
    {
        // 
        bool Validate();
    }
}
