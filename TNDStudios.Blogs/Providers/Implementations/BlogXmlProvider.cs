using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Provider for the blog using Xml files in the App_Data (Or other) folder
    /// </summary>
    public class BlogXmlProvider : BlogDataProviderBase, IBlogDataProvider
    {
        /// <summary>
        /// Connection string for the Xml Provider (Mainly where the files are located)
        /// </summary>
        public BlogDataProviderConnectionString ConnectionString { get; set; }

        /// <summary>
        /// In Memory reference of the items for quicker lookup
        /// </summary>
        private List<IBlogItem> items;

        /// <summary>
        /// Get a list of the blogs using the request parameters provided
        /// </summary>
        /// <param name="request">Parameters to search / list with</param>
        /// <returns>A list of blog headers</returns>
        public IList<IBlogItem> Get(BlogDataProviderGetRequest request)
        {
            // If no list of states is given then only search published articles
            List<BlogHeaderState> checkStates =
                (request.States == null || request.States.Count == 0) ?
                new List<BlogHeaderState>() { BlogHeaderState.Published } :
                request.States;

            // Filter based on the items provided
            IEnumerable<IBlogItem> filtered = items
                .Where(headCheck => checkStates.Contains(headCheck.Header.State))
                .Where(ids => (request.Ids == null || request.Ids.Count == 0 || request.Ids.Contains(ids.Header.Id)))
                .Where(from => (from.Header.PublishedDate >= request.PeriodFrom) || (request.PeriodFrom == null))
                .Where(to => (to.Header.PublishedDate <= request.PeriodTo) || (request.PeriodTo == null))
                .Where(tags => (request.Tags == null || request.Tags.Count == 0 || request.Tags.Any(y => tags.Header.Tags.ToString().Contains(y))))
                .Where(head => (request.HeaderList.Count == 0 || request.HeaderList.Any(req => req.Id == head.Header.Id)));

            // Return all of the headers and success if it didn't die, but as a copy so that returned
            // item isn't a reference to the origional
            return filtered.Select(
                    item => (request.HeaderOnly ?
                    new BlogItem()
                    {
                        Content = "",
                        Header = item.Header
                    } : item).Copy()
                    ).ToList<IBlogItem>();
        }

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to be saved</param>
        /// <returns>The item once it has been saved</returns>
        public IBlogItem Save(IBlogItem item)
        {
            // Define the response
            IBlogItem response = null;

            // Check and see if the blog item exists in the local item list first
            IBlogItem foundItem = items.Where(x => x.Header.Id == item.Header.Id).FirstOrDefault();
            if (foundItem == null)
            {
                item.Header.Id = base.NewId(); // Generate a new Id and assign it
                items.Add(item); // Add the item
                response = item; // Assign the data to the response
            }
            else
            {
                foundItem = item.Copy(); // Copy the data in (don't repoint the reference)
                response = foundItem; // Assign the data to the response
            }

            // Return the item back to the caller
            return response;
        }

        /// <summary>
        /// Delete a set of blog items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>True or false to indicate it worked or not</returns>
        public Boolean Delete(IList<IBlogHeader> items, Boolean permanent)
            => permanent ? DeletePermanent(items) : DeleteSoft(items);

        /// <summary>
        /// Permanent delete of items
        /// </summary>
        /// <param name="items">The list of items to delete</param>
        /// <returns>Success or Failure</returns>
        private Boolean DeletePermanent(IList<IBlogHeader> items)
        {
            // Search for any items that match the incoming headers and mark them as deleted
            List<String> itemIds = items.Select(x => x.Id).ToList<String>();
            Int64 deleteCount = this.items.RemoveAll(x => itemIds.Contains(x.Header.Id));

            // Was anything deleted?
            return (deleteCount > 0);
        }

        /// <summary>
        /// Soft delete of items
        /// </summary>
        /// <param name="items">The list of items to delete</param>
        /// <returns>Success or Failure</returns>
        private Boolean DeleteSoft(IList<IBlogHeader> items)
        {
            Int64 deleteCount = 0; // How many items were deleted

            // Search for any items that match the incoming headers and mark them as deleted
            List<String> itemIds = items.Select(x => x.Id).ToList<String>();
            this.items.ForEach(x =>
            {
                // Is this item in the list of derived Ids?
                if (itemIds.Contains(x.Header.Id))
                {
                    x.Header.State = BlogHeaderState.Deleted; // Set the state
                    deleteCount++; // Increment the deleted counter
                }
            });

            // Was anything deleted?
            return (deleteCount > 0);
        }

        /// <summary>
        /// Get a full listing of all blog items (used mainly for serialising in the blog class)
        /// </summary>
        /// <returns>A full list of blog items</returns>
        public List<IBlogHeader> GetListing()
            => this.items.Select(x => x.Header).ToList<IBlogHeader>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogXmlProvider()
        {
            // Instantiate the items
            items = new List<IBlogItem>();
        }
    }
}
