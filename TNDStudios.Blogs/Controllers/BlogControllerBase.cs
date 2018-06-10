using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.Providers;
using TNDStudios.Blogs.Attributes;

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
        Display = 3,
        FileBrowser = 4,
        FileBrowserUpload = 5
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
        public IBlog Currrent { get => GetInstanceBlog(); }

        /// <summary>
        /// Get the Base Url for this controller
        /// </summary>
        public String BaseUrl =>
            (new Uri($"{Request.HttpContext.Request.Scheme}://{Request.HttpContext.Request.Host.Value}/{ControllerContext.RouteData.Values["controller"]}")).ToString();

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
        /// Blog startup with the hosting environment pushed in
        /// </summary>
        public BlogControllerBase()
            => BlogStartup();

        /// <summary>
        /// Common blog startup method
        /// </summary>
        private void BlogStartup()
        {
            try
            {
                // Get the instance type (the calling class that inherited the BlogBaseController class)
                Type instanceType = this.GetType();

                // Reset the blog references so we can gather information about them
                if (blogs == null)
                    blogs = new Dictionary<String, IBlog>();

                // If the base controller hasn't been set up yet the go gather the information it
                // needs to connect to the blogs that have been assigned for it to manage
                if (!blogs.ContainsKey(instanceType.Name))
                {
                    try
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

                                // Initialise the data provider
                                blogDataProvider.Initialise();

                                // Get all of the blog setup attributes for this controller / blog combination
                                BlogSEOAttribute[] sEOAttrs = (BlogSEOAttribute[])instanceType.GetCustomAttributes(typeof(BlogSEOAttribute), false);
                                BlogSEOSettings blogSEOSettings = (sEOAttrs.Length > 0) ? sEOAttrs[0].SEOSettings : new BlogSEOSettings() { };

                                // Construct the parameters for setting up the blog
                                IBlogParameters blogParameters = new BlogParameters()
                                {
                                    Id = blogId,
                                    Provider = blogDataProvider,
                                    SEOSettings = blogSEOSettings
                                };

                                // Assign the instantiated blog class to the static array of blogs
                                blogs[instanceType.Name] = new Blog(blogParameters);
                            }
                        }

                        // Call the blog initialised method so custom actions can be applied
                        BlogInitialised();
                    }
                    catch (Exception ex)
                    {
                        // Tell the caller we could not initialise the blog controller
                        throw BlogException.Passthrough(ex, new NotInitialisedBlogException(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new NotInitialisedBlogException(ex));
            }

        }

        /// <summary>
        /// The startup routine for the blog controller
        /// to allow a user to override the method to items only once on startup of this blog
        /// </summary>
        public virtual void BlogInitialised()
        {
            // Load the default templates from the resources file
            Currrent.LoadDefaultTemplates();
        }
    }
}
