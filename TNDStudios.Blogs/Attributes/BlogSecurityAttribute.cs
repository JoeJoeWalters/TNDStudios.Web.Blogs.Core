using System;

namespace TNDStudios.Web.Blogs.Core.Attributes
{
    /// <summary>
    /// Attribute that tells the system what level of blog security it should have
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class BlogSecurityAttribute : Attribute
    {
        /// <summary>
        /// The level of security for the blog method
        /// </summary>
        public BlogPermission Permission { get; set; }

        /// <summary>
        /// Setup to tell the system what level of security it should have
        /// </summary>
        public BlogSecurityAttribute(BlogPermission permission)
        {
            this.Permission = permission;
        }
    }
}
