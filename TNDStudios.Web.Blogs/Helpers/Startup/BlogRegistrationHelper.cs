using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.Blogs.Core.Attributes;
using TNDStudios.Web.Blogs.Core.Providers;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    /// <summary>
    /// Helper to handle deciphering and presenting the properties
    /// needed for a blog to be registered (can be called by hitting 
    /// the controller itself or from the startup of the website)
    /// </summary>
    public class BlogRegistrationHelper
    {
        /// <summary>
        /// Register a controller based on the instance type
        /// </summary>
        /// <param name="instanceType">The new blog controller (or type) to be registered</param>
        /// <param name="Blogs">The list of blogs to register against (strictly ref isn't needed but added to make clear)</param>
        public Boolean Register(Type instanceType, ref Dictionary<String, IBlog> blogs)
        {
            // The result which is failed by default
            Boolean result = false;

            // Check to see if the blogs dictionary has been instantiated (is not from the startup routine)
            blogs = blogs ?? new Dictionary<string, IBlog>();

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

                            // Success
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tell the caller we could not initialise the blog controller
                    throw BlogException.Passthrough(ex, new NotInitialisedBlogException(ex));
                }
            }

            return result; // Send back the result
        }
    }
}
