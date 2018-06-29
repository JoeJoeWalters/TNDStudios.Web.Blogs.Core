﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

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
            
            // Allow stacking so it's consistent with the other extension methods
            return value;
        }
    }
}
