using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TNDStudios.Blogs.Helpers
{
    /// <summary>
    /// General extension helpers for base classes
    /// </summary>
    public static class BaseClassExtensions
    {
        /// <summary>
        /// Get the description of an enumeration
        /// </summary>
        /// <typeparam name="T">The enumeration type needing the description attribute finding</typeparam>
        /// <param name="value">The enumeration item that needs resolving</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            // Get the enumeration type
            Type type = value.GetType();

            // If this is not an enumeration type
            if (!type.IsEnum)
                throw new CastObjectBlogException();

            // Get the member infor for the type
            MemberInfo[] memberInfo = type.GetMember(value.ToString());

            // Do we have any member info?
            if (memberInfo != null && memberInfo.Length > 0)
            {
                // Get all of the description attributes for this enum value
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                // Get the first attribute description value
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            // No attribute value so just return the name of the enum value
            return value.ToString();
        }
    }
}
