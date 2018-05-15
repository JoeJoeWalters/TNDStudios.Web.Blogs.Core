using System;
using System.Collections.Generic;

namespace TNDStudios.Blogs.RequestResponse
{
    /// <summary>
    /// Search parameters for blog listings
    /// </summary>
    public class BlogListRequest
    {
        /// <summary>
        /// Set of tags to restrict the search to
        /// </summary>
        public IList<String> Tags { get; set; }

        /// <summary>
        /// After a given date
        /// </summary>
        public DateTime? PeriodFrom { get; set; }

        /// <summary>
        /// Before a given date
        /// </summary>
        public DateTime? PeriodTo { get; set; }

        /// <summary>
        /// List of Ids to get
        /// </summary>
        public List<String> Ids { get; set; }

        /// <summary>
        /// List of states that are allowed otherwise set to a default set
        /// </summary>
        public List<BlogHeaderState> States { get; set; } 

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogListRequest()
        {
            Tags = new List<String>(); // No tags to search by default
            PeriodFrom = null; // No from date by default
            PeriodTo = null; // No end date by default
            Ids = new List<String>(); // A list of Ids to search for
            States = new List<BlogHeaderState>(); // List of states that are allowed otherwise set to a default set
        }
    }
}
