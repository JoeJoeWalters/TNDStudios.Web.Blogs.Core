using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using TNDStudios.Web.Blogs.Core.Providers;
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
            if (Current != null)
            {
                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = Current.Templates.ContainsKey(BlogControllerView.Login) ?
                        Current.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                };

                // Pass the view model
                return View(viewModel);
            }
            else
                return View(new LoginViewModel());
        }

        /// <summary>
        /// Change the current user's password
        /// </summary>
        /// <param name="password">The existing password</param>
        /// <param name="newpassword">The new password</param>
        /// <param name="newpasswordconfirm">Confirmaton of the new password</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/auth/passwordchange")]
        public virtual IActionResult PasswordChange([FromForm]String password, [FromForm]String newpassword, [FromForm]String newpasswordconfirm)
        {
            // Get the blog that is for this controller instance
            if (Current != null)
            {
                // Get the provider reference (shorthand)
                IBlogDataProvider provider = Current.Parameters.Provider;
                if (provider != null)
                {
                    BlogLogin changedUser = provider.ChangePassword(loginManager.CurrentUser.Username, password, newpassword, newpasswordconfirm);
                    if (changedUser != null)
                        loginManager.CurrentUser = changedUser;
                }

                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = Current.Templates.ContainsKey(BlogControllerView.Login) ?
                        Current.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                    Username = loginManager.CurrentUser.Username
                };

                // Pass the view model
                return View(this.ViewLocation("login"), viewModel);
            }
            else
                return View(this.ViewLocation("login"), new LoginViewModel());
        }

        /// <summary>
        /// Validate a login
        /// </summary>
        /// <returns>The redirection based on the outcome</returns>
        [HttpPost]
        [Route("[controller]/auth/login")]
        public virtual IActionResult AuthenticateLogin([FromForm]String username, [FromForm]String password, [FromQuery]Boolean Redirect)
        {
            // Get the blog that is for this controller instance
            if (Current != null)
            {
                // Generate the view model to pass
                LoginViewModel viewModel = new LoginViewModel()
                {
                    Templates = Current.Templates.ContainsKey(BlogControllerView.Login) ?
                        Current.Templates[BlogControllerView.Login] : new BlogViewTemplates(),
                    Username = username
                };

                // Validate the login
                if (loginManager.ValidateLogin(username, password, true))
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
