using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// Enumeration to identify the fields for each content part (actual field names in the description attribute)
    /// </summary>
    [DefaultValue(Unknown)]
    public enum BlogViewTemplateField : Int32
    {
        Unknown = 0, // When the item cannot be found

        [Description("controllerurl")]
        Common_Controller_Url = 10101,

        [Description("editbutton")]
        Common_EditButton = 10102,

        [Description("items")]
        Index_Body_Items = 10201,

        [Description("clearfix")]
        Index_BlogItem_ClearFix = 10202,
        
        [Description("author")]
        BlogItem_Author = 20101,

        [Description("description")]
        BlogItem_Description = 20102,
        
        [Description("id")]
        BlogItem_Id = 20103,

        [Description("name")]
        BlogItem_Name = 20104,

        [Description("publisheddate")]
        BlogItem_PublishedDate = 20105,

        [Description("state")]
        BlogItem_State = 20106,

        [Description("updateddate")]
        BlogItem_UpdatedDate = 20107,

        [Description("content")]
        BlogItem_Content = 20108,

        [Description("tags")]
        BlogItem_Tags = 20109,

        [Description("seotags")]
        BlogItem_SEOTags = 20110,

        [Description("seourltitle")]
        BlogItem_SEOUrlTitle = 20111,

        [Description("editbutton")]
        BlogItem_EditButton = 20112,

        [Description("attachments")]
        Attachments = 30201,

        [Description("attachmentlist")]
        Attachment_List = 30301,

        [Description("attachmenttitle")]
        Attachment_Title = 30302,

        [Description("attachmenturl")]
        Attachment_Url = 30303,
    }
}
