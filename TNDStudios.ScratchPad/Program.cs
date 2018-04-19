using System;
using TNDStudios.Blogs;
using Newtonsoft.Json;
using System.Collections.Generic;
using TNDStudios.Blogs.Providers;
using Newtonsoft.Json.Linq;
using TNDStudios.Blogs.RequestResponse;

namespace TNDStudios.ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {

            IBlog blog = new Blog(new BlogParameters() { Id = "xxx", Provider = new BlogMemoryProvider() });

            // Arrange
            IBlogItem savedItem = blog.Save(new BlogItem() { });
            String newId = savedItem.Header.Id; // Get the Id
            Boolean deleteResponse = blog.Delete(new List<IBlogHeader>() { savedItem.Header }, false); // Delete

            // Act
            IList<IBlogItem> searchResults =
                blog.List(
                    new BlogListRequest()
                    {
                        Ids = new List<String>() { newId }
                    });
            
        }
    }
}
