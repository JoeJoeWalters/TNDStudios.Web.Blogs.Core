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
                // Set the state
                Byte[] securityTokenValue = new byte[0]; 
                HttpContext.Session.TryGetValue("tndstudios.web.blogs.core.token", out securityTokenValue);
                String pulledToken = Encoding.UTF8.GetString(securityTokenValue ?? new byte[0]);

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
                {
                    // Set the state
                    HttpContext.Session.Set(
                        "tndstudios.web.blogs.core.token",
                        Encoding.UTF8.GetBytes(securityToken.ToString())
                        );
                }

                // Pass the view model
                return View(this.ViewLocation("login"), viewModel);
            }
            else
                return View(this.ViewLocation("login"), new LoginViewModel());
        }
    }
}
