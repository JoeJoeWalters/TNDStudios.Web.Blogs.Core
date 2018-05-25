using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Files that belong to a blog entry or the blog itself
    /// </summary>
    public class BlogFile
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public String Filename { get; set; }

        /// <summary>
        /// The title of the file
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogFile()
        {
            Filename = "";
            Title = "";
        }
    }
}
