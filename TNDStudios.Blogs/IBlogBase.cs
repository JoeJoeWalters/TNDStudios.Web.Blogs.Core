using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Interface for the base class 
    /// </summary>
    public interface IBlogBase
    {
        /// <summary>
        /// Additional data left over when Json Casting cannot map the properties
        /// </summary>
        Dictionary<string, JToken> AdditionalData { get; set; }

        /// <summary>
        /// Cast the item to an Xml String
        /// </summary>
        /// <returns>The object transformed as an Xml string</returns>
        String ToXmlString();

        /// <summary>
        /// Convert an incoming string to a given object type of IBlogBase 
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="data">The data string to convert</param>
        /// <returns></returns>
        T FromXmlString<T>(String data) where T: IBlogBase, new();
    }
}
