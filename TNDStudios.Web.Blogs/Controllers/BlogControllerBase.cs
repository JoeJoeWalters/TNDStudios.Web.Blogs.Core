using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using TNDStudios.Web.Blogs.Core;
using TNDStudios.Web.Blogs.Core.Providers;
using TNDStudios.Web.Blogs.Core.Attributes;
using TNDStudios.Web.Blogs.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TNDStudios.Web.Blogs.Core.Controllers
{
    /// <summary>
    /// Enumeration of the blog controller views so that the view type can be 
    /// referenced by key
    /// </summary>
    public enum BlogControllerView : Int32
    {
        Index = 0,
        Login = 1,
        Edit = 2,
        Display = 3,
        FileBrowser = 4
    }

    /// <summary>
    /// Collection of items and helpers to manage the incoming URL and page display etc. for blogs
    /// </summary>
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// The view engine used to resolve view locations from actions
        /// </summary>
        //private ICompositeViewEngine viewEngine;

        /// <summary>
        /// Login manager for the blog
        /// </summary>
        private BlogLoginManager loginManager;

        /// <summary>
        /// The blog that the handler is managing
        /// </summary>
        internal static Dictionary<String, IBlog> Blogs;

        /// <summary>
        /// Get the current blog
        /// </summary>
        private IBlog currentBlog;
        public IBlog Current
        {
            get
            {
                // No current blog reference?
                if (currentBlog == null)
                    currentBlog = GetInstanceBlog();

                // Return the reference
                return currentBlog;
            }
        }

        /// <summary>
        /// Get the name of the controller by analysing the route data
        /// </summary>
        public String ControllerName =>
            (ControllerContext != null && ControllerContext.RouteData != null) ? ControllerContext.RouteData.Values["controller"].ToString() : "";

        public String ViewLocation(String action)
        {
            // Get the view result from the vuew engine assigned to this controller
            //ViewEngineResult viewResult = viewEngine.FindView(ControllerContext, action, false);

            // Return the physical path to the current view
            //return (viewResult.View == null) ? "" : viewResult.View.Path;
            return action;
        }

        /// <summary>
        /// Get the Base Url for this controller
        /// </summary>
        public String BaseUrl =>
            (new Uri($"{Request.HttpContext.Request.Scheme}://{Request.HttpContext.Request.Host.Value}/{ControllerName}")).ToString();

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
            if (Blogs.ContainsKey(instanceType.Name))
                result = Blogs[instanceType.Name];

            // Return the blog
            return result;
        }

        /// <summary>
        /// Blog startup with the hosting environment pushed in
        /// </summary>
        public BlogControllerBase()//ICompositeViewEngine viewEngine)
            => BlogStartup();// viewEngine);

        /// <summary>
        /// When the action is executed, set up anything needed for any subsequent actions
        /// </summary>
        /// <param name="context">The execution context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Is a redirect?
            Boolean isRedirect = false;
            if (context.HttpContext.Request.Query.ContainsKey("redirect"))
                Boolean.TryParse(context.HttpContext.Request.Query["redirect"], out isRedirect);

            // Is it a password change?
            if (context.HttpContext.Request.Path.Value.Contains($"auth/passwordchange"))
                isRedirect = true;

            try
            {
                // Get a new login manager instance to check against the current blog 
                // with the current session context
                loginManager = new BlogLoginManager(Current, context.HttpContext);
                if (loginManager != null)
                {
                    // Handle the logins (tokens etc.)
                    loginManager.HandleTokens();

                    // Current user? Is there some reason to redirect?
                    if (loginManager.CurrentUser != null)
                    {
                        // Password change required and not already on the auth screen?
                        if (loginManager.CurrentUser.PasswordChange && !isRedirect)
                        {
                            // Redirect ..
                            context.Result = new RedirectResult($"{BaseUrl}/auth/login/?redirect=true");
                            return;
                        }
                    }
                }
                else
                    throw new UserBlogException("Login Manager could not be initialised.");

                // Check to see if we have any security attributes applied to the current method
                ControllerActionDescriptor controllerDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                if (controllerDescriptor != null) // Did we get a controller descriptor?
                {
                    // Get the required security level if there is one
                    BlogSecurityAttribute[] securityAttributes = (BlogSecurityAttribute[])controllerDescriptor.MethodInfo.GetCustomAttributes(typeof(BlogSecurityAttribute), false);
                    if (securityAttributes.Length != 0)
                    {
                        // Loop the items and see if any fail to meet criteria, if so then set the redirect result
                        foreach (BlogSecurityAttribute attrib in securityAttributes)
                        {
                            // Check against the current user for the security level needed
                            if (loginManager.CurrentUser == null ||
                                loginManager.CurrentUser.Permissions == null ||
                                !loginManager.CurrentUser.Permissions.Contains(attrib.Permission))
                                context.Result = new RedirectResult($"{BaseUrl}"); // Redirect to the user to the home page
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Throw the exception wrapped (if needed) in a non-initialised exception
                throw BlogException.Passthrough(ex, new UserBlogException(ex));
            }

            // Call the base method
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Common blog startup method
        /// </summary>
        private void BlogStartup()//ICompositeViewEngine viewEngine)
        {
            // Assign the viewengine used for this environment
            //this.viewEngine = viewEngine;
            try
            {
                // Get the instance type (the calling class that inherited the BlogBaseController class)
                Type instanceType = this.GetType();

                // Reset the blog references so we can gather information about them
                if (Blogs == null)
                    Blogs = new Dictionary<String, IBlog>();

                // Register this instance with the registration helper
                BlogRegistrationHelper regHelper = new BlogRegistrationHelper();
                if (regHelper.Register(instanceType, ref Blogs))
                {
                    // Call the blog initialised method so custom actions can be applied
                    BlogInitialised();
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
            Current.LoadDefaultTemplates();
        }
    }
}
