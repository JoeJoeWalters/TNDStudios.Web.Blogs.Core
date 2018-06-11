using System;
using System.ComponentModel;
using System.Reflection;

namespace TNDStudios.Blogs.Helpers
{
    /// <summary>
    /// General extension helpers for base classes 
    /// (Has to be static by it's nature as an extension)
    /// </summary>
    public static class BaseClassExtensions
    {
        /// <summary>
        /// Get the description of an enumeration
        /// </summary>
        /// <typeparam name="T">The enumeration type needing the description attribute finding</typeparam>
        /// <param name="value">The enumeration item that needs resolving</param>
        /// <returns></returns>
        public static String GetDescription(this Enum value)
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

        /// <summary>
        /// Use the description attribute on the enumeration to match the enum value to a given string
        /// </summary>
        /// <typeparam name="T">The type of the enumeration</typeparam>
        /// <param name="description">The description pattern that is to be matched</param>
        /// <returns></returns>
        public static object GetValueFromDescription<T>(this String description)
        {
            // What is the type of the enumeration?
            var type = typeof(T);

            // Is this actually a type? If not then throw
            if (!type.IsEnum)
                throw new CastObjectBlogException();

            // Loop the fields in the enumeration
            foreach (var field in type.GetFields())
            {
                // Get the set of custom attributes where it's a description type
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                // Did it cast correctly?
                if (attribute != null)
                {
                    // If the description matches?
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    // If this is not a description attribute, does the name match instead?
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            // No matches so throw an error
            throw new ArgumentException("Enum could not match a description attribute",
                $"Enum could not match a description attribute '{description}'");
        }

        /// <summary>
        /// Get the formatted custom date for a given nullable datetime
        /// </summary>
        /// <param name="value">The nullable datetime to be converted</param>
        /// <param name="format">The format for the datetime to be converted</param>
        /// <returns></returns>
        public static String ToCustomDate(this DateTime? value, String format, String defaultValue = null)
            => value.HasValue ? ToCustomDate(value.Value, format) : (defaultValue ?? "");

        /// <summary>
        /// Get the formatted custom date for a given non-nullable datetime
        /// </summary>
        /// <param name="value">The datetime to be converted</param>
        /// <param name="format">The format for the datetime to be converted</param>
        /// <returns></returns>
        public static String ToCustomDate(this DateTime value, String format)
            => value.ToString(format); // Simple for now, but could be more complex later
    }
}
