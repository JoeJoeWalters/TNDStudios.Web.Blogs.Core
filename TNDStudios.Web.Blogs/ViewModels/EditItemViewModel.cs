using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// Flattened view of the item that is easier to transfer for saving by the controller
    /// </summary>
    public class EditItemViewModel
    {
        /// <summary>
        /// The Identifier for the blog item
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// Who the author of the blog item is
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// The name (title) of the blog item
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The description of the blog item
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// The content of the blog item
        /// </summary>
        public String Content { get; set; }

        /// <summary>
        /// When the blog item was published
        /// </summary>
        public String PublishedDate { get; set; }

        /// <summary>
        /// The Keywords to be associated with this entry
        /// </summary>
        public String Tags { get; set; }

        /// <summary>
        /// The blog SEO tags that are held in the meta data for this item
        /// </summary>
        public String SEOTags { get; set; }

        /// <summary>
        /// List of files to display that are attached to the blog item
        /// </summary>
        public List<BlogFile> Files { get; set; }
    }
}
