using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using TNDStudios.Web.Blogs.Core.Helpers;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Set up any constants that are used for authentication etc.
        /// </summary>
        private const String securityTokenKey = "tndstudios.web.blogs.core.token"; // Security token key path

        /// <summary>
        /// Route for the login view
        /// </summary>
        /// <returns>The login view</returns>
        [HttpGet]
        [Route("[controller]/auth/login")]
        public virtual IActionResult Login()
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Login) ?
                        blog.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                    CurrentBlog = blog
                };

                // Pass the view model
                return View(viewModel);
            }
            else
                return View(new LoginViewModel());
        }

        /// <summary>
        /// Validate a login
        /// </summary>
        /// <returns>The redirection based on the outcome</returns>
        [HttpPost]
        [Route("[controller]/auth/login")]
        public virtual IActionResult AuthenticateLogin([FromForm]String username, [FromForm]String password)
        {
            // Start a new instance of the session helper pointing at the http context session
            SessionHelper sessionHelper = new SessionHelper(HttpContext.Session);

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Get the current security token from the session
                String currentToken = sessionHelper.GetString(securityTokenKey, "");

                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Login) ?
                        blog.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                    CurrentBlog = blog,
                    Username = username
                };

                // Get the security token if the user is authenticated
                Nullable<Guid> securityToken = blog.Parameters.Provider.AuthenticateUser(username, password);

                // Set the token in the session state
                if (securityToken.HasValue)
                    sessionHelper.SetString(securityTokenKey, securityToken.ToString());

                // Pass the view model
                return View(this.ViewLocation("login"), viewModel);
            }
            else
                return View(this.ViewLocation("login"), new LoginViewModel());
        }
    }
}
