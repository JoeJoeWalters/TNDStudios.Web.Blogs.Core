using Microsoft.AspNetCore.Mvc;
using System;
using TNDStudios.Blogs;
using TNDStudios.Blogs.Attributes;
using TNDStudios.Blogs.Controllers;

namespace TNDStudios.Web.Controllers
{
    [BlogSetup(blogId: "testBlog", provider: "TNDStudios.Blogs.Providers.BlogMemoryProvider", providerConnectionString: "")]
    public class BlogController : BlogControllerBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogController() : base()
        {   
#warning [Temporary Code, Remove once tested]
            for (var blogID = 0; blogID < 100; blogID++)
                base.Currrent.Save(
                    new BlogItem()
                    {
                        Header = new BlogHeader()
                        {
                            Author = "Joe Walters",
                            Name = "New Blog (" + blogID.ToString() + ")",
                            State = BlogHeaderState.Published,
                            PublishedDate = DateTime.Now
                        }
                    });

        }
    }
}