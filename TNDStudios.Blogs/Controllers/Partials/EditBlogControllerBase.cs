using Microsoft.AspNetCore.Mvc;
using System;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Route for editing a blog item
        /// </summary>
        /// <returns>The default view</returns>
        [HttpGet]
        [Route("[controller]/edit/{id}")]
        public virtual IActionResult EditBlog(String id)
            => EditBlogCommon(id);

        /// <summary>
        /// Route for saving the data for a blog item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/edit/{id}")]
        public virtual IActionResult SaveBlogEdit(EditItemViewModel model)
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Get the item that needs to be saved
                IBlogItem blogItem = (model.Id == "") ? new BlogItem() : blog.Get(new BlogHeader() { Id = blog.Parameters.Provider.DecodeId(model.Id) });

                // Blog item valid?
                if (blogItem != null)
                {
                    // Update the properties of the blog item from the incoming model
                    blogItem.Copy(model);

                    // (Re)Save the blog item back to the blog handler
                    blogItem = blog.Save(blogItem);
                }
                else
                    throw new ItemNotFoundBlogException("Item with id '{id}' not found");

            }

            // Call the common view handler
            return EditBlogCommon(model.Id);
        }
        
        /// <summary>
        /// Common call between the verbs to return the blog edit model
        /// </summary>
        /// <returns></returns>
        private IActionResult EditBlogCommon(String id)
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Generate the view model to pass
                EditViewModel viewModel = new EditViewModel()
                {
                    Templates = blog.Templates.ContainsKey(BlogControllerView.Edit) ?
                        blog.Templates[BlogControllerView.Edit] : new BlogViewTemplates(),
                    CurrentBlog = blog
                };
                viewModel.Item = blog.Get(new BlogHeader() { Id = blog.Parameters.Provider.DecodeId(id) });

                // Pass the view model
                return View("Edit", viewModel);
            }
            else
                return View(new EditViewModel());
        }
    }
}
