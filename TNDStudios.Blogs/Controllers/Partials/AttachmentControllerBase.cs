using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TNDStudios.Blogs.RequestResponse;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Controllers
{
    public abstract partial class BlogControllerBase : Controller
    {
        /// <summary>
        /// Mime types that can be handled by the attachment controller
        /// </summary>
        internal static Dictionary<String, String> mimeTypes =
            new Dictionary<String, String>
            {
                        {".txt", "text/plain"},
                        {".pdf", "application/pdf"},
                        {".doc", "application/vnd.ms-word"},
                        {".docx", "application/vnd.ms-word"},
                        {".xls", "application/vnd.ms-excel"},
                        {".png", "image/png"},
                        {".jpg", "image/jpeg"},
                        {".jpeg", "image/jpeg"},
                        {".gif", "image/gif"},
                        {".csv", "text/csv"}
            };

        /// <summary>
        /// Get the attachment content and stream it back to the caller
        /// </summary>
        /// <param name="id">The Id of the blog item that the attachment is connected to</param>
        /// <param name="fileId">The Id of the file attached to the blog item</param>
        /// <returns>A stream of the file to the caller</returns>
        [HttpGet]
        [Route("[controller]/Attachment/{id}/{fileId}")]
        public virtual IActionResult GetAttachment(String id, String fileId)
        {
            // Bytes to send back
            Byte[] content = null; // No content by default
            String contentType = "text/plain"; // Default content type is plain text
            String fileName = ""; // Default filename

            // Get the blog that is for this controller instance
            IBlog blog = GetInstanceBlog();
            if (blog != null)
            {
                // Get the blog item
                IBlogItem blogItem = blog.Get(new BlogHeader() { Id = id });

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
                            contentType = GetContentType(blogFile); // Work out the content type for the header
                            fileName = blogFile.Filename; // Get the origional filename
                            content = blogFile.Content; // Get the returned content type

                            // Return the content here
                            return File(content, contentType, fileName);
                        }
                    }
                }
            }

            // Must have failed to have arrived here
            return File((Byte[])null, "text/plain", "");
        }

        /// <summary>
        /// Get the content type base on the file passed to it
        /// </summary>
        /// <param name="file">The BlogFile that is being discovered</param>
        /// <returns>The content type for the filename</returns>
        private String GetContentType(BlogFile file)
            => mimeTypes[Path.GetExtension(file.Filename).ToLowerInvariant()] ?? "text/plain";

    }
}
