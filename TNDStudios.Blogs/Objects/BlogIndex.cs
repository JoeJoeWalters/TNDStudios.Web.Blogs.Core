using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TNDStudios.Web.Blogs.Core
{
    /// <summary>
    /// Implementation of the blog index
    /// </summary>
    public class BlogIndex : BlogBase
    {
        /// <summary>
        /// List of the headers
        /// </summary>
        [XmlArray]
        public List<BlogItem> Headers { get; set; }

        /// <summary>
        /// Has the index been initialised (from the source)
        /// </summary>
        public Boolean Initialised { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogIndex()
        {
            Initialised = false; // Not initialised by default

            // Empty by default (some list functions assume it is instantiated)
            Headers = new List<BlogItem>();
        }
    }
}
