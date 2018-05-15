using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Implementation of the IBlog Interface
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Blog : BlogBase, IBlog
    {
        /// <summary>
        /// Headers
        /// </summary>
        public List<IBlogHeader> Items
        {
            get => this.parameters.Provider.GetListing();
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The parameters for this blog
        /// </summary>
        private IBlogParameters parameters;
        [JsonProperty(PropertyName = "Parameters", Required = Required.Default)]
        public IBlogParameters Parameters { get { return parameters; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Blog()
        {
        }

        /// <summary>
        /// Parameter based Constructor (for shorthand)
        /// </summary>
        /// <param name="parameters">The new parameters for the blog</param>
        public Blog(IBlogParameters parameters)
            => Initialise(parameters); // Initialise the blog engine

        /// <summary>
        /// Initialise the blog engine
        /// </summary>
        /// <param name="parameters">The new parameters for the blog</param>
        public Boolean Initialise(IBlogParameters parameters)
        {
            // Set the parameters
            this.parameters = parameters;

            // Success?
            return true;
        }

        /// <summary>
        /// Get a list of blog items
        /// </summary>
        /// <param name="request">The parameters to search on</param>
        /// <returns>A list of blog items</returns>
        public IList<IBlogHeader> List(BlogListRequest request)
        {
            // Pass the request down to the data provider and get the result
            // Making sure that the list of restricted headers is empty
            return parameters.Provider.Search(
                new BlogDataProviderGetRequest()
                {
                    HeaderList = new List<IBlogHeader>(),
                    PeriodFrom = request.PeriodFrom,
                    PeriodTo = request.PeriodTo,
                    Tags = request.Tags,
                    Ids = request.Ids,
                    States = request.States
                }
                );
        }

        /// <summary>
        /// Save an item to the blog
        /// </summary>
        /// <param name="item">The item that you wish to save</param>
        /// <returns>The saved item</returns>
        public IBlogItem Save(IBlogItem item)
            => parameters.Provider.Save(item); // Pass the request on to the provider and get the response

        /// <summary>
        /// Get a blog item from it's header information
        /// </summary>
        /// <param name="header">The header belonging to the blog you want to get</param>
        /// <returns>The full blog item</returns>
        public IBlogItem Get(IBlogHeader header)
            => parameters.Provider.Load(header);

        /// <summary>
        /// Delete a set of blog items based on the header
        /// </summary>
        /// <param name="items">The set of item headers to delete</param>
        /// <returns>Success (true or false)</returns>
        public Boolean Delete(IList<IBlogHeader> items, Boolean permanent)
            => parameters.Provider.Delete(items, permanent); // Pass the command through to the provider
    }
}
