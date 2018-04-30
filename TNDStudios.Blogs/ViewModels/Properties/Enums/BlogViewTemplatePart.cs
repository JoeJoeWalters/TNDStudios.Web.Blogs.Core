using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Enumeration for the content parts for each of the display templates
    /// </summary>
    [DefaultValue(Unknown)]
    public enum BlogViewTemplatePart : Int32
    {
        Unknown = 0, // When the item cannot be found

        [EnumMember(Value = "header")]
        Index_Header = 101,

        [EnumMember(Value = "body")]
        Index_Body = 102,

        [EnumMember(Value = "footer")]
        Index_Footer = 103,

        [EnumMember(Value = "indexclearfix")]
        Index_Clearfix = 105,

        [EnumMember(Value = "indexclearfix-medium")]
        Index_Clearfix_Medium = 106,

        [EnumMember(Value = "indexclearfix-large")]
        Index_Clearfix_Large = 107,
        
        [EnumMember(Value = "item")]
        Blog_Item = 201,

        [EnumMember(Value = "edititem")]
        Blog_EditItem = 202
    }
}
