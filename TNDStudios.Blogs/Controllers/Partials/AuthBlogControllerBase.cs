using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
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

                // Validate the login
                if (loginManager.ValidateLogin(blog, username, password))
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
