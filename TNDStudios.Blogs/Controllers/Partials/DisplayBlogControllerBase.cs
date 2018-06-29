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
        /// Route for viewing a blog item
        /// </summary>
        /// <returns>The default view</returns>
        [Route("[controller]/item/{id}")]
        public virtual IActionResult Display(String id)
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate the view model to pass
                DisplayViewModel viewModel = new DisplayViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Display) ?
                        blog.Templates[BlogControllerView.Display] : new BlogViewTemplates(),
                    CurrentBlog = blog
                };
                viewModel.Item = blog.Get(new BlogHeader() { Id = blog.Parameters.Provider.DecodeId(id) });

                // Pass the view model back
                return View(viewModel);
            }
            else
                return View(new DisplayViewModel());
        }
    }
}
