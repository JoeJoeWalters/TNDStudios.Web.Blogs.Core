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
        /// Route for editing a blog item
        /// </summary>
        /// <returns>The default view</returns>
        public virtual IActionResult Edit()
        {
            // Generate the view model to pass
            EditViewModel viewModel = new EditViewModel()
            {
                Templates = this.Templates.ContainsKey(BlogControllerView.Edit) ? 
                    this.Templates[BlogControllerView.Edit] : new BlogViewTemplates()
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
