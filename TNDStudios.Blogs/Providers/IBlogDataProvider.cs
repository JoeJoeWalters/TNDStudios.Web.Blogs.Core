using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Define how a blog class should be able to save it's data
    /// </summary>
    public interface IBlogDataProvider
    {
        /// <summary>
        /// Some providers will need a connection string property that can be parsed
        /// </summary>
        BlogDataProviderConnectionString ConnectionString { get; set; }

        /// <summary>
        /// // Get a set of blog data (or one)
        /// </summary>
        /// <param name="request">The parameters used to request a set of data</param>
        /// <returns>A list of blog items</returns>
        IList<IBlogHeader> Search(BlogDataProviderGetRequest request);

        /// <summary>
        /// // Get a singular blog item
        /// </summary>
        /// <param name="request">The header for the request</param>
        /// <returns>The blog item</returns>
        IBlogItem Load(IBlogHeader request);

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to save</param>
        /// <returns>The saved item</returns>
        IBlogItem Save(IBlogItem item); // Save a blog

        /// <summary>
        /// Delete a set of blog items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>True or false to indicate it worked or not</returns>
        Boolean Delete(IList<IBlogHeader> items, Boolean permanent);

        /// <summary>
        /// Get a full listing of all blog headers (used mainly for serialising in the blog class)
        /// </summary>
        /// <returns>A full list of blog items</returns>
        List<IBlogHeader> GetListing();

        /// <summary>
        /// Generate a New Id for a blog item
        /// </summary>
        /// <returns></returns>
        String NewId();

        /// <summary>
        /// Initialisation routine called by default
        /// </summary>
        void Initialise();
    }
}
