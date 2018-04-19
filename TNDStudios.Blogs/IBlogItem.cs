using System;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Interface for a blog item type
    /// </summary>
    public interface IBlogItem
    {
        /// <summary>
        /// The identity and definition of the blog (the header)
        /// </summary>
        BlogHeader Header { get; set; }

        /// <summary>
        /// The content of the blog in whatever string format it needs to be in
        /// </summary>
        String Content { get; set; }

    }
}
