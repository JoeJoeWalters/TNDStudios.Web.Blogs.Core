using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TNDStudios.Blogs.ViewModels
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

        [Description("items")]
        Index_Body_Items = 10201,

        [Description("clearfix")]
        Index_BlogItem_ClearFix = 10202,

        [Description("author")]
        Index_BlogItem_Author = 10301,

        [Description("description")]
        Index_BlogItem_Description = 10302,

        [Description("id")]
        Index_BlogItem_Id = 10303,

        [Description("name")]
        Index_BlogItem_Name = 10304,

        [Description("publisheddate")]
        Index_BlogItem_PublishedDate = 10305,

        [Description("state")]
        Index_BlogItem_State = 10306,

        [Description("updateddate")]
        Index_BlogItem_UpdatedDate = 10307,

        [Description("author")]
        Display_BlogItem_Author = 20301,

        [Description("description")]
        Display_BlogItem_Description = 20302,

        [Description("item")]
        Display_BlogHeader_Item = 20201,

        [Description("id")]
        Display_BlogItem_Id = 20303,

        [Description("name")]
        Display_BlogItem_Name = 20304,

        [Description("publisheddate")]
        Display_BlogItem_PublishedDate = 20305,

        [Description("state")]
        Display_BlogItem_State = 20306,

        [Description("updateddate")]
        Display_BlogItem_UpdatedDate = 20307,

        [Description("content")]
        Display_BlogItem_Content = 20308,

    }
}
