using System;

namespace TNDStudios.Blogs.Attributes
{
    /// <summary>
    /// Attribute that is the marker for the blog controller (to identify the controllers 
    /// and which blog it belongs to)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BlogSetupAttribute : Attribute
    {
        public String BlogId { get; internal set; }
        public String Provider { get; internal set; }
        public String ProviderConnectionString { get; internal set; }

        /// <summary>
        /// Default Constructor with the blog ID to identify it, the data provider for this blog
        /// and the provider connection string if one is needed
        /// </summary>
        public BlogSetupAttribute(
            String blogId, 
            String provider, 
            String providerConnectionString = "")
        {
            BlogId = blogId;
            Provider = provider;
            ProviderConnectionString = providerConnectionString;
        }
    }
}
