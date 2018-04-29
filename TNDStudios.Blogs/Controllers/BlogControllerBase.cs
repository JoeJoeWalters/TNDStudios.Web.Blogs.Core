using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.RequestResponse;
using TNDStudios.Blogs.ViewModels;
using TNDStudios.Blogs.Providers;
using TNDStudios.Blogs.Attributes;
using Microsoft.AspNetCore.Html;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

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
        /// The resource pattern to find the templates in the current assembly
        /// </summary>
        private const String templateResourcePattern = "TNDStudios.Blogs.Resources.ContentTemplates.{0}ViewDefaultContent.json";

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
        /// Load all of the default templates from the embedded resources to the templates dictionary
        /// </summary>
        /// <returns></returns>
        private void LoadDefaultTemplates()
        {
            // Set an empty dictionary for the views
            Templates = new Dictionary<BlogControllerView, BlogViewTemplates>();
            
            // Loop the blog controller view enum to load the content file for each
            foreach (BlogControllerView view in Enum.GetValues(typeof(BlogControllerView)))
            {
                try
                {
                    // Get the target within the assembly by building the namespace with the view name
                    String viewName = Enum.GetName(typeof(BlogControllerView), view);
                    String assemblyTarget = String.Format(templateResourcePattern, viewName);

                    Templates.Add(view, LoadTemplatesFromAssembly(assemblyTarget));
                }
                catch (Exception ex)
                {
                    // Failed, explain why
                    throw BlogException.Passthrough(ex, new CastObjectBlogException(ex));
                }
            }
        }

        private BlogViewTemplates LoadTemplatesFromAssembly(String assemblyTarget)
        {
            // Attempt to get the resource stream from the executing assembly
            Stream assemblyStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyTarget);
            if (assemblyStream != null)
            {
                // Create a new template collection object and pass the stream to the loader within it
                BlogViewTemplates viewTemplates = new BlogViewTemplates();
                if (viewTemplates.Load(assemblyStream))
                    return viewTemplates;
            }

            return null;
        }

        /// <summary>
        /// Default constructor for the blog handler
        /// </summary>
        public BlogControllerBase()
        {
            // Load the default templates from the resources file
            LoadDefaultTemplates();

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
