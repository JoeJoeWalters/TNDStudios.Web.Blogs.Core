using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Model passed to the MVC view to search and administer blog items
    /// </summary>
    public class AdminViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The search parameters used to find the blog items
        /// </summary>
        public BlogListRequest SearchParameters { get; set; }

        /// <summary>
        /// List of search results when searching for items to administer
        /// </summary>
        public List<IBlogItem> Results { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AdminViewModel() : base()
        {
            SearchParameters = new BlogListRequest() { }; // Default searc items
            Results = new List<IBlogItem>(); // Blank list by default
        }
    }
}