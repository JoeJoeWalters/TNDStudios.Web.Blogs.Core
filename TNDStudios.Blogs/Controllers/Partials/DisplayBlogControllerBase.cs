﻿using Microsoft.AspNetCore.Mvc;
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
        public virtual IActionResult Display(String id)
        {
            // Generate the view model to pass
            DisplayViewModel viewModel = new DisplayViewModel()
            {
                Templates = this.Templates.ContainsKey(BlogControllerView.Display) ? 
                    this.Templates[BlogControllerView.Display] : new BlogViewTemplates()
            };

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                viewModel.Item = blog.Get(new BlogHeader() { Id = id });
            }

            // Pass the view model
            return View(viewModel);
        }
    }
}
