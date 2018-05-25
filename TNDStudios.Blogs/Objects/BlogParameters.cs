using Newtonsoft.Json;
using System;
using TNDStudios.Blogs.Attributes;
using TNDStudios.Blogs.Providers;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// The initialising blog parameters
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogParameters : BlogBase, IBlogParameters
    {
        /// <summary>
        /// The Id for the blog (used to connect the controller to the blog)
        /// </summary>
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public String Id { get; set; }

        /// <summary>
        /// The data provider for this blog
        /// </summary>
        [JsonIgnore]
        [JsonProperty(PropertyName = "Provider", Required = Required.AllowNull)]
        public IBlogDataProvider Provider { get; set; }

        /// <summary>
        /// The default SEO Settings for the blog
        /// </summary>
        [JsonProperty(PropertyName = "SEO", Required = Required.Default)]
        public BlogSEOSettings SEOSettings { get; set; }
    }
}