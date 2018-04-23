using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace TNDStudios.Blogs.Helpers
{
    /// <summary>
    /// Better implementation of a tolerant enum converter for Newtonsoft Json
    /// </summary>
    public class TolerantEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// Override of the base Json reader with an error trap to handle default handling
        /// </summary>
        /// <param name="reader">Passthrough</param>
        /// <param name="objectType">Passthrough</param>
        /// <param name="existingValue">Passthrough</param>
        /// <param name="serializer">Passthrough</param>
        /// <returns>The enum value</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                // Try and do the transformation with the base implementation
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch(Exception ex)
            {
                // A default not defined for this enum? Do some extra work ..
                if (existingValue == null)
                {
                    // No default, go try and get the first value
                    object returnValue = GetFirstEnumValue(objectType); 

                    // Still could not resolve a value so throw the origional error
                    if (returnValue == null)
                        throw ex;

                    // Made it to the end, return the value found
                    return returnValue;
                }
                else
                    return existingValue; // We have a default value, just return that ..
            }
        }

        /// <summary>
        /// Get the first value of the enumeration (used for when an existing default value hasn't been defined)
        /// </summary>
        /// <param name="enumType">The type of enum proposed</param>
        /// <returns>The first enum value</returns>
        private object GetFirstEnumValue(Type enumType)
        {
            try
            {
                // Attempt to get the first of the values of this enum
                return Enum.GetValues(enumType).GetValue(0);
            }
            catch
            {
                // Throw the error away and return nothing (which will be handled)
                return null;
            }
        }
    }
}
