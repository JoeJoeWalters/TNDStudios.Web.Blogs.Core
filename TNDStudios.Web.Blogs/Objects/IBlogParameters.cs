using System;
using TNDStudios.Web.Blogs.Core.Attributes;
using TNDStudios.Web.Blogs.Core.Providers;

namespace TNDStudios.Web.Blogs.Core
{
    public interface IBlogParameters
    {
        /// <summary>
        /// The Id for the blog (used to connect the controller to the blog)
        /// </summary>
        String Id { get; set; }

        /// <summary>
        /// The data provider for the blog
        /// </summary>
        IBlogDataProvider Provider { get; set; }

        /// <summary>
        /// The default SEO Settings for the blog
        /// </summary>
        BlogSEOSettings SEOSettings { get; set; }
    }
}
