using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Blogs.RequestResponse;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Route for the admin view
        /// </summary>
        /// <returns>The admin view</returns>
        public virtual IActionResult Admin()
        {
            // Generate the view model to pass
            AdminViewModel viewModel = new AdminViewModel()
            {
                Templates = this.Templates.ContainsKey(BlogControllerView.Admin) ? 
                    this.Templates[BlogControllerView.Index] : new BlogViewTemplates()
            };

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {

            }

            // Pass the view model
            return View(viewModel);
        }
    }
}
