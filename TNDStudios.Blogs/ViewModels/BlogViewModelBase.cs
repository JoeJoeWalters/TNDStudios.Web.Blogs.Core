using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TNDStudios.Blogs.Helpers;

namespace TNDStudios.Blogs.ViewModels
{    
    /// <summary>
    /// Common items for the view models 
    /// </summary>
    public abstract class BlogViewModelBase
    {
        /// <summary>
        /// The display settings for the view model
        /// Also used for the IHtmlHelper classes
        /// </summary>
        public BlogViewDisplaySettings DisplaySettings { get; set; }

        /// <summary>
        /// Display templates used for rendering the views for the helpers
        /// </summary>
        public BlogViewTemplates Templates { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogViewModelBase()
        {
            DisplaySettings = new BlogViewDisplaySettings();
            Templates = new BlogViewTemplates();
        }
    }
}
