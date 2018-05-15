using System;
using System.Collections.Generic;

namespace TNDStudios.Blogs.RequestResponse
{
    /// <summary>
    /// Get a set of data within the data provider (not the same as the blog class level requests)
    /// Inherits the standard blog list request used by the blog engine itself as it is passed down
    /// in certain circumstances
    /// </summary>
    public class BlogDataProviderGetRequest : BlogListRequest
    {
        /// <summary>
        /// Set of headers to retrieve (when not doing a general search)
        /// </summary>
        public IList<IBlogHeader> HeaderList { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogDataProviderGetRequest()
        {
            HeaderList = new List<IBlogHeader>(); // Default to an empty list of headers
            Tags = new List<String>(); // Default to an empty list of strings
            PeriodFrom = null; // No from date by default
            PeriodTo = null; // No end date by default
            Ids = new List<String>(); // No Ids by default
            States = new List<BlogHeaderState>(); // List of states that are allowed otherwise set to a default set
        }
    }
}
