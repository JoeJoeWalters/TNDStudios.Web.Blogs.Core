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
        /// Wrapper for registering a controller around the testable functionality
        /// as the controller discovery cannot be tested within the context of the tests
        /// </summary>
        /// <param name="instanceType">The new blog controller (or type) to be registered</param>
        /// <param name="blogs">The list of blogs to register against (strictly ref isn't needed but added to make clear)</param>
        public Boolean Register(Type instanceType, ref Dictionary<String, IBlog> blogs)
        {
            // The result which is failed by default
            Boolean result = false;

            try
            {
                // Try and scan the attributes from the blog controller instance
                BlogSetupAttribute[] setupAttributes = (BlogSetupAttribute[])instanceType.GetCustomAttributes(typeof(BlogSetupAttribute), false);
                BlogSEOAttribute[] seoAttributes = (BlogSEOAttribute[])instanceType.GetCustomAttributes(typeof(BlogSEOAttribute), false);

                // Call the testable functionality from this wrapper with the derived attributes
                result = Register(instanceType.Name, setupAttributes, seoAttributes, ref blogs);
            }
            catch (Exception ex)
            {
                // Tell the caller we could not initialise the blog controller
                throw BlogException.Passthrough(ex, new NotInitialisedBlogException(ex));
            }

            return result; // Send back the result
        }

        /// <summary>
        /// Overload to take the setup attributes and blog id so that tests can 
        /// be ran against them without needing it scan for the blog controller
        /// </summary>
        /// <returns></returns>
        public Boolean Register(String instanceName,
            BlogSetupAttribute[] setupAttributes, 
            BlogSEOAttribute[] seoAttributes, 
            ref Dictionary<String, IBlog> blogs)
        {
            // Define the result as failed by default
            Boolean result = false;

            // Check to see if the blogs dictionary has been instantiated (is not from the startup routine)
            blogs = blogs ?? new Dictionary<string, IBlog>();
            
            // If the base controller hasn't been set up yet the go gather the information it
            // needs to connect to the blogs that have been assigned for it to manage
            if (!blogs.ContainsKey(instanceName))
            {
                try
                {
                    // Get all of the blog setup attributes for this controller / blog combination
                    if (setupAttributes.Length > 0)
                    {
                        // Get the blog Id from the calling class custom parameters
                        String blogId = setupAttributes[0].BlogId;

                        // Work out and create an instance of the correct data provider
                        IBlogDataProvider blogDataProvider = (new BlogDataProviderFactory()).Get(setupAttributes[0].Provider);
                        if (blogDataProvider != null)
                        {
                            blogDataProvider.ConnectionString = new BlogDataProviderConnectionString(setupAttributes[0].ProviderConnectionString);

                            // Initialise the data provider
                            blogDataProvider.Initialise();

                            // Get all of the blog setup attributes for this controller / blog combination
                            BlogSEOSettings blogSEOSettings = (seoAttributes.Length > 0) ? seoAttributes[0].SEOSettings : new BlogSEOSettings() { };

                            // Construct the parameters for setting up the blog
                            IBlogParameters blogParameters = new BlogParameters()
                            {
                                Id = blogId,
                                Provider = blogDataProvider,
                                SEOSettings = blogSEOSettings
                            };

                            // Assign the instantiated blog class to the static array of blogs
                            blogs[instanceName] = new Blog(blogParameters);

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

            // Return if it was successful
            return result;
        }
    }
}
