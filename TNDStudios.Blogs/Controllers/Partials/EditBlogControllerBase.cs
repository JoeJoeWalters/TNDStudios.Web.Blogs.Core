using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
        [HttpGet]
        [Route("[controller]/Edit/{id}")]
        public virtual IActionResult EditBlog(String id)
            => EditBlogCommon(id);

        /// <summary>
        /// Route for saving the data for a blog item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/Edit/{id}")]
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
        /// Upload a file to be attached to a blog item
        /// </summary>
        /// <param name="id">The Id of of the blog item to attach the file to</param>
        /// <param name="file">The file from the form to be uploaded</param>
        /// <returns>The standard edit action result</returns>
        [HttpPost]
        [Route("[controller]/Edit/{id}/Upload")]
        public IActionResult UploadFile(String id, String title, IFormFile file)
        {
            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Make sure there is actually a file 
                if (file != null)
                {
                    // Get the blog item
                    IBlogItem blogItem = blog.Get(new BlogHeader() { Id = id });
                    if (blogItem != null)
                    {
                        // The content of the file ready to pass to the data provider
                        Byte[] fileContent = null; // Empty by default

                        // Create a memory stream to read the file
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream); // Copy the file in to a memory stream
                            fileContent = memoryStream.ToArray(); // convert the steam of data to a byte array
                            memoryStream.Close(); // It's in a using anyway but just incase
                        }

                        // Something to save?
                        if (fileContent != null && fileContent.Length != 0)
                        {
                            // Get the content header
                            ContentDispositionHeaderValue parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                            // Create the new blog file for saving
                            BlogFile blogFile = new BlogFile()
                            {
                                Content = fileContent,
                                Filename = parsedContentDisposition.FileName.Replace("\"", ""),
                                Tags = new List<String>(),
                                Title = title ?? ("File: " + Guid.NewGuid().ToString())
                            };

                            // Add the file to the blog file list
                            blogItem.Files.Add(blogFile);

                            // Save the blog item and with it the new file
                            blog.Save(blogItem);
                        }
                    }
                }
            }

            // Redirect back to the edit action
            return RedirectToAction("Edit", new { id });
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
