using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TNDStudios.Web.Blogs.Core
{
    /// <summary>
    /// The options for the state of the blog entry
    /// </summary>
    public enum BlogHeaderState : Int32
    {
        Deleted = 0,
        Unpublished = 1,
        Published = 2
    }

    /// <summary>
    /// Implementaton of the blog header based on a domain object with an Int64 unique id
    /// </summary>
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogHeader: BlogBase, IBlogHeader
    {
        /// <summary>
        /// Header / Blog Item Id (Always need a header id if serialising etc.)
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public String Id { get; set; }

        /// <summary>
        /// The state of the blog item
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "State", Required = Required.Default)]
        public BlogHeaderState State { get; set; }

        /// <summary>
        /// Name of the blog entry
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Name", Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Short description of the blog entry
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Description", Required = Required.Default)]
        public String Description { get; set; }

        /// <summary>
        /// Associated tags of the blog entry
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Tags")]
        public List<String> Tags { get; set; }

        /// <summary>
        /// Associated SEO tags of the blog entry
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "SEOTags")]
        public List<String> SEOTags { get; set; }

        /// <summary>
        /// Who authored the blog entry
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Author", Required = Required.Always)]
        public String Author { get; set; }

        /// <summary>
        /// When was the blog entry published
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "PublishedDate", Required = Required.Default)]
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        /// When was the blog entry updated
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "UpdatedDate", Required = Required.Always)]
        public DateTime UpdatedDate { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogHeader()
        {
            Id = null; // No Id for newly created item
            State = BlogHeaderState.Unpublished; // Unpublished by default
            Name = ""; // No name by default
            Description = ""; // No description by default
            Tags = new List<String>(); // No tags by default
            SEOTags = new List<String>(); // The SEO tags for the item
            Author = ""; // No author by default
        }
    }
}