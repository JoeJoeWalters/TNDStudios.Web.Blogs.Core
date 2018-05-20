using System;
using TNDStudios.Blogs.Attributes;
using TNDStudios.Blogs.Providers;

namespace TNDStudios.Blogs
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
