﻿using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.IO;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Model passed to the MVC view to be the home page and search blog items
    /// </summary>
    public class IndexViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The search parameters used to find the blog items
        /// </summary>
        public BlogListRequest SearchParameters { get; set; }

        /// <summary>
        /// List of search results when searching for items to show
        /// </summary>
        public List<IBlogItem> Results { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public IndexViewModel() : base()
        {
            SearchParameters = new BlogListRequest() { }; // Default searc items
            Results = new List<IBlogItem>(); // Blank list by default
        }
    }
}