using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Web.Blogs.Core.Controllers;
using TNDStudios.Web.Blogs.Core.Helpers;

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
        public IBlog CurrentBlog
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current user that is logged in for this view
        /// </summary>
        public BlogLogin CurrentUser
        {
            get;
            internal set;
        }

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
        /// Populate the common attributes in the model before returning it if needed
        /// </summary>
        /// <param name="helper">The helper to use to populate the model</param>
        /// <param name="blogId">The known blog Id (For when being called from outside the controller itself)</param>
        /// <returns></returns>
        public BlogViewModelBase Populate(IHtmlHelper helper, String blogId)
        {
            // Get the blog pairing
            KeyValuePair<String, IBlog> blogPair = Blogs.Items.Where(
                pair => pair.Value.Parameters.Id == blogId 
                ).FirstOrDefault();

            // Work out what the controller url prefix would be based on the name
            String controllerPrefix = blogPair.Key.Replace("Controller", "");

            // Has some value?
            this.CurrentBlog = blogPair.Value;

            // Do common population things
            PopulateBase(helper);

            // Handle estimated URL's based on the controller name
            this.ControllerUrl = $"{this.BaseUrl}{controllerPrefix}"; 
            this.RelativeControllerUrl = $"/{controllerPrefix}";

            // Return itself
            return this;
        }

        /// <summary>
        /// Populate common attributes in to the model before returning it if needed
        /// </summary>
        /// <param name="helper">The helper to use to populate the model</param>
        /// <returns></returns>
        public BlogViewModelBase Populate(IHtmlHelper helper)
        {
            // Get the current blog based on the controller (as the blog Id was not passed in)
            ControllerActionDescriptor controllerDescriptor = (ControllerActionDescriptor)helper.ViewContext.ActionDescriptor;
            if (controllerDescriptor != null) // Did we get a controller descriptor?
            {
                // Does the type info for the controller exist already as an index for the blog listing?
                if (Blogs.Items.ContainsKey(controllerDescriptor.ControllerTypeInfo.Name))
                    this.CurrentBlog = Blogs.Items[controllerDescriptor.ControllerTypeInfo.Name];
            }

            // Do common population things
            PopulateBase(helper);

            this.ControllerUrl = $"{this.BaseUrl}{helper.ViewContext.RouteData.Values["Controller"].ToString()}"; // Get the Controller route attribute for the Url replacement
            this.RelativeControllerUrl = $"/{helper.ViewContext.RouteData.Values["Controller"].ToString()}";

            // Return itself
            return this;
        }

        /// <summary>
        /// Common items that are populated as part of the blog view population
        /// </summary>
        /// <param name="helper"></param>
        private void PopulateBase(IHtmlHelper helper)
        {
            // Generate the base url for this view
            this.BaseUrl = (new Uri($"{helper.ViewContext.HttpContext.Request.Scheme}://{helper.ViewContext.HttpContext.Request.Host.Value}")).ToString();
            
            // Do we have a current blog set? If so, do we have a current user?
            if (this.CurrentBlog != null)
            {
                // Instantiate a new login manager
                BlogLoginManager loginManager = new BlogLoginManager(this.CurrentBlog, helper.ViewContext.HttpContext);

                // Get the current user (if we can) and assign it to the viewmodel
                this.CurrentUser = loginManager.CurrentUser;
            }  
        }
    }
}
