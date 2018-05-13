using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TNDStudios.Blogs
{
    public abstract class BlogJsonBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogJsonBase()
        {
            // Set up the location of any additional data that might be captured
            AdditionalData = new Dictionary<string, JToken>();
        }

        /// <summary>
        /// Property bag of additional data as stated by Newtonsoft
        /// </summary>
        [XmlIgnore]
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
