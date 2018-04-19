using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Provider for the blog using JSon files
    /// </summary>
    public class BlogJsonProvider : BlogDataProviderBase, IBlogDataProvider
    {
        /// <summary>
        /// Location of the JSon files etc.
        /// </summary>
        public BlogDataProviderConnectionString ConnectionString { get; set; }

        /// <summary>
        /// In Memory list of headers (content is stored as JSON)
        /// </summary>
        private IList<IBlogHeader> items;

        /// <summary>
        /// Get a list of the blogs using the request parameters provided
        /// </summary>
        /// <param name="request">Parameters to search / list with</param>
        /// <returns>A list of blog headers</returns>
        public IList<IBlogItem> Get(BlogDataProviderGetRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to be saved</param>
        /// <returns>The item once it has been saved</returns>
        public IBlogItem Save(IBlogItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a set of blog items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>True or false to indicate it worked or not</returns>
        public Boolean Delete(IList<IBlogHeader> items, Boolean permanent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a full listing of all blog items (used mainly for serialising in the blog class)
        /// </summary>
        /// <returns>A full list of blog items</returns>
        public List<IBlogHeader> GetListing()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogJsonProvider()
        {
            // Instantiate the list of headers
            items = new List<IBlogHeader>();
        }
    }
}
