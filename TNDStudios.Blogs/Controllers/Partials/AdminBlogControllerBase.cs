using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.Blogs.Core.RequestResponse;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Route for the admin view
        /// </summary>
        /// <returns>The admin view</returns>
        public virtual IActionResult Admin()
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate the view model to pass
                AdminViewModel viewModel = new AdminViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Admin) ?
                        blog.Templates[BlogControllerView.Admin] : new BlogViewTemplates(),
                    CurrentBlog = blog
                };

                // Pass the view model
                return View(viewModel);
            }
            else
                return View(new AdminViewModel());
        }
    }
}
