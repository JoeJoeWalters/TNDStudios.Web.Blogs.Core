using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Interface for all blog implementations
    /// </summary>
    public interface IBlog
    {
        /// <summary>
        ///  The Parameters for the blog
        /// </summary>
        IBlogParameters Parameters { get; }

        /// <summary>
        /// Headers
        /// </summary>
        List<IBlogHeader> Items { get; set; }

        /// <summary>
        /// Intiialise the blog implemntation with the starting parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Boolean Initialise(IBlogParameters parameters);

        /// <summary>
        /// Get a list of blogs based a set of parameters
        /// </summary>
        /// <param name="request">The parameters to search on</param>
        /// <returns>A list of blog items</returns>
        IList<IBlogHeader> List(BlogListRequest request);

        /// <summary>
        /// Save an item to the blog
        /// </summary>
        /// <param name="item">The item that you wish to save</param>
        /// <returns>The saved item</returns>
        IBlogItem Save(IBlogItem item);

        /// <summary>
        /// Get a blog item from it's header information
        /// </summary>
        /// <param name="header">The header belonging to the blog you want to get</param>
        /// <returns>The full blog item</returns>
        IBlogItem Get(IBlogHeader header);

        /// <summary>
        /// Delete a set of blog items based on the header
        /// </summary>
        /// <param name="items">The set of item headers to delete</param>
        /// <param name="permanent">Permanent delete?</param>
        /// <returns>Success (true or false)</returns>
        Boolean Delete(IList<IBlogHeader> items, Boolean permanent);
    }
}
