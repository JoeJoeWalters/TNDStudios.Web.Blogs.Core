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

        [EnumMember(Value = "indexheader")]
        Index_Header = 101,

        [EnumMember(Value = "indexbody")]
        Index_Body = 102,

        [EnumMember(Value = "indexitem")]
        Index_BlogItem = 103,

        [EnumMember(Value = "indexfooter")]
        Index_Footer = 104,

        [EnumMember(Value = "indexclearfix")]
        Index_Clearfix = 105,

        [EnumMember(Value = "indexclearfix-medium")]
        Index_Clearfix_Medium = 106,

        [EnumMember(Value = "indexclearfix-large")]
        Index_Clearfix_Large = 107,

        [EnumMember(Value = "displayheader")]
        Display_Header = 201,

        [EnumMember(Value = "displaybody")]
        Display_Body = 202,

        [EnumMember(Value = "displayitem")]
        Display_Item = 203,

        [EnumMember(Value = "displayfooter")]
        Display_Footer = 204,

    }
}
