using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Used to index the blog items
    /// </summary>
    public interface IBlogIndex : IBlogBase
    {
        /// <summary>
        /// List of the headers
        /// </summary>
        List<IBlogItem> Headers { get; set; }
    }
}
