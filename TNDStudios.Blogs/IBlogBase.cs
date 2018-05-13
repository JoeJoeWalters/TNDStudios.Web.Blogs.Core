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
    }
}
