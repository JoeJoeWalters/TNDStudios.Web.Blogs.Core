using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
