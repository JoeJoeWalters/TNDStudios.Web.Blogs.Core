using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;
using TNDStudios.Blogs.ViewModels;
using TNDStudios.Blogs.Providers;
using TNDStudios.Blogs.Attributes;
using Microsoft.AspNetCore.Html;

namespace TNDStudios.Blogs.Controllers
{
    /// <summary>
    /// Enumeration of the blog controller views so that the view type can be 
    /// referenced by key
    /// </summary>
    public enum BlogControllerView : Int32
    {
        Index = 0,
        Admin = 1,
        Edit = 2,
        Display = 3
    }

    /// <summary>
    /// Collection of items and helpers to manage the incoming URL and page display etc. for blogs
    /// </summary>
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// The blog that the handler is managing
        /// </summary>
        private static Dictionary<String, IBlog> blogs;

        /// <summary>
        /// Get the current blog
        /// </summary>
        public IBlog Currrent { get => GetInstanceBlog();  }

        /// <summary>
        /// List of templates by view type that can then be loaded in to the view as it is rendered
        /// </summary>
        public IDictionary<BlogControllerView, BlogViewTemplates> Templates { get; internal set; }

        /// <summary>
        /// Get the blog that belongs to the instance of the parent controller
        /// </summary>
        /// <returns>The blog requested</returns>
        private IBlog GetInstanceBlog()
        {
            // The returning Blog instance
            IBlog result = null;

            // Get the instance type (the calling class that inherited the BlogBaseController class)
            Type instanceType = this.GetType();

            // Check to see if the controller has been setup and matched with a blog
            if (blogs.ContainsKey(instanceType.Name))
                result = blogs[instanceType.Name];

            // Return the blog
            return result;
        }

        /// <summary>
        /// Default constructor for the blog handler
        /// </summary>
        public BlogControllerBase()
        {
            // Set an empty dictionary for the views
            Templates = new Dictionary<BlogControllerView, BlogViewTemplates>();

            // Reset the blog references so we can gather information about them
            if (blogs == null)
                blogs = new Dictionary<String, IBlog>();

            // Get the instance type (the calling class that inherited the BlogBaseController class)
            Type instanceType = this.GetType();

            // If the base controller hasn't been set up yet the go gather the information it
            // needs to connect to the blogs that have been assigned for it to manage
            if (!blogs.ContainsKey(instanceType.Name))
            {
                // Get all of the blog setup attributes for this controller / blog combination
                BlogSetupAttribute[] attrs = (BlogSetupAttribute[])instanceType.GetCustomAttributes(typeof(BlogSetupAttribute), false);
                if (attrs.Length > 0)
                {
                    // Get the blog Id from the calling class custom parameters
                    String blogId = attrs[0].BlogId;

                    // Work out and create an instance of the correct data provider
                    IBlogDataProvider blogDataProvider = (new BlogDataProviderFactory()).Get(attrs[0].Provider);
                    if (blogDataProvider != null)
                    {
                        blogDataProvider.ConnectionString = new BlogDataProviderConnectionString(attrs[0].ProviderConnectionString);

                        // Construct the parameters for setting up the blog
                        IBlogParameters blogParameters = new BlogParameters()
                        {
                            Id = blogId,
                            Provider = blogDataProvider
                        };

                        // Assign the instantiated blog class to the static array of blogs
                        blogs[instanceType.Name] = new Blog(blogParameters);
                    }
                }
            }
        }
    }
}
