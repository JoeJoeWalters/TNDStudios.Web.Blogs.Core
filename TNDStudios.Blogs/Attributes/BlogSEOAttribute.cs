using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs.Attributes
{
    /// <summary>
    /// Definition of the SEO parts for the blog (such as the default author, descripiton etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BlogSEOAttribute : Attribute
    {
        /// <summary>
        /// The SEO Settings shared so the blog can pick it up easily in the same class form
        /// </summary>
        public BlogSEOSettings SEOSettings { get; }

        /// <summary>
        /// Default Constructor with the blog ID to identify it, the data provider for this blog
        /// and the provider connection string if one is needed
        /// </summary>
        public BlogSEOAttribute(
            String author,
            String title,
            String description,
            String tags)
        {
            SEOSettings = new BlogSEOSettings()
            {
                Author = author ?? "",
                Title = title ?? "",
                Description = description ?? "",
                Tags = tags ?? ""
            };
        }
    }

    /// <summary>
    /// Used to store the SEO settings for a given blog
    /// </summary>
    public class BlogSEOSettings
    {
        public String Author { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Tags { get; set; }

        public BlogSEOSettings()
        {
            Author = "";
            Title = "";
            Description = "";
            Tags = "";
        }
    }

}
