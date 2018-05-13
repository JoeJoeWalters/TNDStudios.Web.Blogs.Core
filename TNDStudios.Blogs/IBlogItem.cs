using System;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Interface for a blog item type
    /// </summary>
    public interface IBlogItem : IBlogBase
    {
        /// <summary>
        /// The identity and definition of the blog (the header)
        /// </summary>
        BlogHeader Header { get; set; }

        /// <summary>
        /// The content of the blog in whatever string format it needs to be in
        /// </summary>
        String Content { get; set; }

        /// <summary>
        /// Copy a blog item in to the current item
        /// </summary>
        /// <returns>The copy of the blog item</returns>
        IBlogItem Copy(IBlogItem from);

        /// <summary>
        /// Copys a given edit model (flat version for editing) in to this item
        /// </summary>
        /// <returns>The copy of the blog item</returns>
        IBlogItem Copy(EditItemViewModel from);

        /// <summary>
        /// Duplicaes the current item
        /// </summary>
        /// <returns>The copy of the blog item</returns>
        IBlogItem Duplicate();

    }
}
