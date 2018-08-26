using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using TNDStudios.Web.Blogs.Core.Controllers;
using TNDStudios.Web.Blogs.Core.Helpers;

namespace TNDStudios.Web.Blogs.Core
{
    /// <summary>
    /// Class used to set up the configuration of the blog from the app startup,
    /// by attaching an extension to the IApplicationBuilder
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// The hosting environment for this blog package
        /// </summary>
        public static IHostingEnvironment Environment { get; internal set; }

        /// <summary>
        /// Set up the blog from the configuration section in the app startup
        /// </summary>
        /// <param name="value">The application builder that this attaches to</param>
        /// <param name="env">The hosting environment supplied from the startup process </param>
        public static IApplicationBuilder UseBlog(this IApplicationBuilder value, IHostingEnvironment env)
        {
            // Set the environment
            Environment = env;

            // Scan all of the controllers for blog controllers so that they can be registered at start ip
            // that way the blogs can be used from other pages etc.
            BlogRegistrationHelper regHelper = new BlogRegistrationHelper();
            IEnumerable<Type> controllers =
                BaseClassExtensions.GetEnumerableOfType<BlogControllerBase>(Assembly.GetCallingAssembly());

            // Loop the found controllers
            foreach (Type controller in controllers)
            {
                try
                {
                    // Call the registration process
                    regHelper.Register(controller, ref Blogs.Items);

                    // Create a new instance of the blog controller purely to 
                    // fire the customisable initialisation routine
                    BlogControllerBase blogInstance = (BlogControllerBase)Activator.CreateInstance(controller);
                    if (blogInstance == null)
                        throw new NotInitialisedBlogException($"Could not initialise the blog '{controller.Name}'");
                    else
                    {
                        // Call the blog initialised method so custom actions can be applied
                        blogInstance.BlogInitialised();
                    }
                }
                catch(Exception ex)
                {
                    throw BlogException.Passthrough(ex, new NotInitialisedBlogException($"Could not initialise the blog '{controller.Name}'"));
                }
            }

            // Allow stacking so it's consistent with the other extension methods
            return value;
        }
    }
}
