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
        /// Route for displaying a blog item with the fake seo url title attached
        /// </summary>
        /// <param name="id">The id of the blog item</param>
        /// <param name="seoUrlTitle">The fake SEO Url Title</param>
        /// <returns></returns>
        [Route("[controller]/Display/{id}/{seoUrlTitle}")]
        public virtual IActionResult DisplayWithTitle(String id, String seoUrlTitle)
            => Display(id); // Send to the standard controller without the fake Url title

        /// <summary>
        /// Route for viewing a blog item
        /// </summary>
        /// <returns>The default view</returns>
        [Route("[controller]/Display/{id}")]
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
                viewModel.Item = blog.Get(new BlogHeader() { Id = id });

                // Pass the view model with the view name so the redirected view is correct
                return View("Display", viewModel);
            }
            else
                return View("Display", new DisplayViewModel()); // Pass with view name so the redirected view is correct
        }
    }
}
