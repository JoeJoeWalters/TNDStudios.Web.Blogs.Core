using Newtonsoft.Json;
using System;
using TNDStudios.Web.Blogs.Core.Attributes;
using TNDStudios.Web.Blogs.Core.Providers;

namespace TNDStudios.Web.Blogs.Core
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