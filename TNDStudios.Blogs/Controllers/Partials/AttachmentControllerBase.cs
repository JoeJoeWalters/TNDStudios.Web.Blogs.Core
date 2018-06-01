using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace TNDStudios.Blogs.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Get the attachment content and stream it back to the caller
        /// </summary>
        /// <param name="id">The Id of the blog item that the attachment is connected to</param>
        /// <param name="fileId">The Id of the file attached to the blog item</param>
        /// <returns>A stream of the file to the caller</returns>
        [HttpGet]
        [Route("[controller]/item/{id}/attachment/{fileId}")]
        public virtual IActionResult GetAttachment(String id, String fileId)
        {
            // Bytes to send back
            Byte[] content = null; // No content by default
            String contentType = BlogFile.DefaultMimeType; // Default content type is plain text
            String fileName = ""; // Default filename

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Get the blog item
                IBlogItem blogItem = blog.Get(new BlogHeader() { Id = blog.Parameters.Provider.DecodeId(id) });

                // Did the "Get" actually work?
                if (blogItem != null && blogItem.Header.Id != "")
                {
                    // Get the blog item related to the request
                    BlogFile blogFile = blogItem.Files.Where(file => file.Id == fileId).FirstOrDefault();

                    // Does this file exist under this blog item?
                    if (blogFile != null)
                    {
                        // Populate the file class with the data from the provider
                        blogFile = blog.Parameters.Provider.LoadFile(blogItem.Header.Id, blogFile);

                        // Content was returned for this request?
                        if (blogFile != null && blogFile.Id != "")
                        {
                            // Assign the varaibles for the return stream
                            contentType = blogFile.ContentType; // Work out the content type for the header
                            fileName = blogFile.Filename; // Get the origional filename
                            content = blogFile.Content; // Get the returned content type

                            // Return the content here
                            return File(content, contentType, fileName);
                        }
                    }
                }
            }

            // Must have failed to have arrived here
            return File((Byte[])null, BlogFile.DefaultMimeType, "");
        }

        /// <summary>
        /// Delete a file from a blog item
        /// </summary>
        /// <param name="id">The Id of the blog item that the attachment is connected to</param>
        /// <param name="fileId">The Id of the file attached to the blog item</param>
        /// <returns>A redirect to the edit page</returns>
        [HttpDelete]
        [Route("[controller]/item/{id}/attachment/{fileId}")]
        public IActionResult DeleteFile(String id, String fileId)
        {
#warning "Delete functionality not implemented yet"

            // Redirect back to the edit action
            return RedirectToAction("Edit", new { id });
        }

        [HttpGet]
        [Route("[controller]/item/{id}/attachment")]
        public IActionResult FileBrowser(String CKEditor, String CKEditorFuncNum, String langCode)
        {
            return RedirectToAction("Edit", new { });
        }

        /// <summary>
        /// Upload a file to be attached to a blog item
        /// </summary>
        /// <param name="id">The Id of of the blog item to attach the file to</param>
        /// <param name="file">The file from the form to be uploaded</param>
        /// <returns>The standard edit action result</returns>
        [HttpPost]
        [Route("[controller]/item/{id}/attachment")]
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
                    IBlogItem blogItem = blog.Get(new BlogHeader() { Id = blog.Parameters.Provider.DecodeId(id) });
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

            // Saved so do the common redirect
            return Redirect(
                Request.Headers.ContainsKey("Referer") ? 
                Request.Headers["Referer"].ToString() : 
                String.Format("{0}", BaseUrl));
        }

    }
}
