using System;
using System.Collections.Generic;

namespace TNDStudios.Blogs
{
    public interface IBlogHeader 
    {
        /// <summary>
        /// Id For the blog header / item
        /// </summary>
        String Id { get; set; }

        /// <summary>
        /// The state of the blog item
        /// </summary>
        BlogHeaderState State { get; set; }

        /// <summary>
        /// Name of the blog entry
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Short description of the blog entry
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Associated tags of the blog entry
        /// </summary>
        IList<String> Tags { get; set; }

        /// <summary>
        /// Who authored the blog entry
        /// </summary>
        String Author { get; set; }

        /// <summary>
        /// When was the blog entry published
        /// </summary>
        DateTime? PublishedDate { get; set; }

        /// <summary>
        /// When was the blog entry updated
        /// </summary>
        DateTime UpdatedDate { get; set; }

    }
}
