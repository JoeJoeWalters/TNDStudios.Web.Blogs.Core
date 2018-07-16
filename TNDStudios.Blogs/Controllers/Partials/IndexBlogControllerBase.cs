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
        /// Route for the default view
        /// </summary>
        /// <returns>The default view</returns>
        public virtual IActionResult Index()
        {
            // Get the blog that is for this controller instance
            if (Current != null)
            {
                // Generate the results view to send back
                IndexViewModel viewModel = new IndexViewModel()
                {
                    Templates = Current.Templates.ContainsKey(BlogControllerView.Index) ?
                        Current.Templates[BlogControllerView.Index] : new BlogViewTemplates(),
                    CurrentBlog = Current
                };

                // Generate a list request to send to the blog handler
                BlogListRequest listRequest = new BlogListRequest() { };

                // Generate the view model results to pass
                List<IBlogHeader> listResponse = (List<IBlogHeader>)Current.List(listRequest);
                if (listResponse != null)
                {
                    // Set the data for the view model
                    viewModel.Results = listResponse;
                }

                // Pass the view model
                return View(viewModel);
            }
            else
                return View(new IndexViewModel());
        }
    }
}
