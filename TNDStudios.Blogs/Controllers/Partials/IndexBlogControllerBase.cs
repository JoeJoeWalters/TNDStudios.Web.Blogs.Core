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
        /// Route for the default view
        /// </summary>
        /// <returns>The default view</returns>
        public virtual IActionResult Index()
        {
            // Generate the results view to send back
            IndexViewModel viewModel = new IndexViewModel()
            {
                Templates = this.Templates.ContainsKey(BlogControllerView.Index) ? 
                    this.Templates[BlogControllerView.Index] : new BlogViewTemplates()
            };

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate a list request to send to the blog handler
                BlogListRequest listRequest = new BlogListRequest() { };

                // Generate the view model results to pass
                List<IBlogItem> listResponse = (List<IBlogItem>)blog.List(listRequest);
                if (listResponse != null)
                {
                    // Set the data for the view model
                    viewModel.Results = listResponse;
                }
            }

            // Pass the view model
            return View(viewModel);
        }

    }
}
