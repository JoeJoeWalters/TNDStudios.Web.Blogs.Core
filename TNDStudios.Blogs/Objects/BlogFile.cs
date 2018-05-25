using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Files that belong to a blog entry or the blog itself
    /// </summary>
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogFile
    {
        /// <summary>
        /// The identifier for the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public String Id { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Filename", Required = Required.Always)]
        public String Filename { get; set; }

        /// <summary>
        /// The title of the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Title", Required = Required.AllowNull)]
        public String Title { get; set; }

        /// <summary>
        /// Tags for the file (so that they can be used for SEO)
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Tags", Required = Required.AllowNull)]
        public List<String> Tags { get; set; }

        /// <summary>
        /// Content of the file if it needs to be passed or uploaded etc.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Byte[] Content { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogFile()
        {
            Filename = "";
            Title = "";
            Tags = new List<String>();
        }
    }
}
