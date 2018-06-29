using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace TNDStudios.Web.Blogs.Core.ViewModels
{    
    /// <summary>
    /// Common items for the view models 
    /// </summary>
    public abstract class BlogViewModelBase
    {
        /// <summary>
        /// The current blog context
        /// </summary>
        public IBlog CurrentBlog { get; set; }

        /// <summary>
        /// Get the base url of the site we are running on
        /// </summary>
        public String BaseUrl { get; set; }

        /// <summary>
        /// Get the url of the controller that the view is in
        /// </summary>
        public String ControllerUrl { get; set; }

        /// <summary>
        /// The relative url to the controller that the view is in
        /// </summary>
        public String RelativeControllerUrl { get; set; }

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

        /// <summary>
        /// Populate common attributes in to the model before returning it if needed
        /// </summary>
        /// <param name="model">The model to be populated</param>
        /// <param name="helper">The helper to use to populate the model</param>
        /// <returns></returns>
        public BlogViewModelBase Populate(IHtmlHelper helper)
        {
            // Generate the base url for this view
            this.BaseUrl = (new Uri($"{helper.ViewContext.HttpContext.Request.Scheme}://{helper.ViewContext.HttpContext.Request.Host.Value}")).ToString();

            // Set any common properties
            //this.BaseUrl = request.Path;
            this.ControllerUrl = $"{this.BaseUrl}{helper.ViewContext.RouteData.Values["Controller"].ToString()}"; // Get the Controller route attribute for the Url replacement
            this.RelativeControllerUrl = $"/{helper.ViewContext.RouteData.Values["Controller"].ToString()}";
            return this; // Some items need to return the value once it's populated for ease of use
        }
    }
}
