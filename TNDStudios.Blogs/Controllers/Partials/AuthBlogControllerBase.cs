using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.Blogs.Core.Helpers;
using TNDStudios.Web.Blogs.Core.RequestResponse;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
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
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Login) ?
                        blog.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                    CurrentBlog = blog,
                    Username = username
                };

                // Generate a new instance of the local cryptography helper
                CryptoHelper cryptoHelper = new CryptoHelper();

                // Check if the user password matches the hashed one
                String adminHash = ""; // Get from storage
                if (cryptoHelper.CheckMatch(adminHash, password))
                {

                }

                // Pass the view model
                return View(this.ViewLocation("login"), viewModel);
            }
            else
                return View(this.ViewLocation("login"), new LoginViewModel());
        }
    }
}
