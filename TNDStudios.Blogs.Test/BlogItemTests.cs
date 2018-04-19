using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.Providers;
using TNDStudios.Blogs.RequestResponse;
using Xunit;

namespace TNDStudios.Blogs.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class BlogItemTestsFixture : IDisposable
    {
        /// <summary>
        /// Setup parameters for this set of tests
        /// </summary>
        public IBlogParameters Parameters;
        public IBlogDataProvider DataProvider;
        public IBlog Blog;

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public BlogItemTestsFixture()
        {
            Parameters = new BlogParameters(); // Create a new set of blog parameters
            Parameters.Provider = new BlogMemoryProvider(); // provider.Object; // Assign the provider to the blog parameters
            Blog = new Blog(Parameters); // Create the blog based on the parameters given
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// Test runner implementing the appropriate fixture
    /// </summary>
    public class BlogItemTests : IClassFixture<BlogItemTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private BlogItemTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public BlogItemTests(BlogItemTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }

        /// <summary>
        /// Create a random entry that can be used for the test cases
        /// </summary>
        /// <returns>A blog entry that can be used for the test cases</returns>
        private IBlogItem RandomBlogItem(Boolean withId)
        {
            return new BlogItem()
            {
                Content = "Test Content",
                Header = new BlogHeader()
                {
                    Id = withId ? (new BlogMemoryProvider()).NewId() : null,
                    Author = "Joe Walters",
                    Description = "Some Description",
                    Name = "This is a blog entry",
                    PublishedDate = DateTime.Now,
                    State = BlogHeaderState.Published,
                    Tags = new List<String>() { "Test" },
                    UpdatedDate = DateTime.Now
                }
            };
        }

        [Fact(DisplayName = "Blog Item - Save")]
        public void Save_New_BlogItem()
        {
            // Arrange

            // Act
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));

            // Assert
            Assert.NotNull(savedItem.Header.Id); // Was a new id created for the object?
        }

        [Fact(DisplayName = "Blog Item - Resave Existing Blog Item")]
        public void Save_Existing_BlogItem()
        {
            // Arrange
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));
            String newId = savedItem.Header.Id; // Get the Id

            // Act
            IBlogItem resavedItem = fixture.Blog.Save(savedItem); // Resave
            String resaveId = resavedItem.Header.Id; // Get the resaved Id

            // Assert
            Assert.Equal(newId, resaveId); // Is the origional Id the same as the resaved Id?
        }

        [Fact(DisplayName = "Blog Item - Delete Blog Item (Permanent)")]
        public void Delete_Existing_BlogItem_Permanent()
        {
            // Arrange
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));
            String newId = savedItem.Header.Id; // Get the Id

            // Act
            Boolean deleteResponse = fixture.Blog.Delete(new List<IBlogHeader>() { savedItem.Header }, true); // Delete

            // Assert
            Assert.True(deleteResponse);
        }

        [Fact(DisplayName = "Blog Item - Delete Blog Item (Soft)")]
        public void Delete_Existing_BlogItem_Soft()
        {
            // Arrange
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));
            String newId = savedItem.Header.Id; // Get the Id

            // Act
            Boolean deleteResponse = fixture.Blog.Delete(new List<IBlogHeader>() { savedItem.Header }, false); // Delete

            // Assert
            Assert.True(deleteResponse);
        }

        [Fact(DisplayName = "Blog Item - Soft Deleted Blog Not In Default Search")]
        public void Soft_Deleted_BlogItem_Not_Searchable()
        {
            // Arrange
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));
            String newId = savedItem.Header.Id; // Get the Id
            Boolean deleteResponse = fixture.Blog.Delete(new List<IBlogHeader>() { savedItem.Header }, false); // Delete

            // Act
            IList<IBlogItem> searchResults = 
                fixture.Blog.List(
                    new BlogListRequest()
                    {
                        Ids = new List<String>() { newId },
                        States = new List<BlogHeaderState>() {  } // No state list passed so should default to published only
                    });

            // Assert
            Assert.Equal(0, searchResults.Count);
        }

        [Fact(DisplayName = "Blog Item - Soft Deleted Blog Showing In Specific Search")]
        public void Soft_Deleted_BlogItem_Searchable()
        {
            // Arrange
            IBlogItem savedItem = fixture.Blog.Save(RandomBlogItem(false));
            String newId = savedItem.Header.Id; // Get the Id
            Boolean deleteResponse = fixture.Blog.Delete(new List<IBlogHeader>() { savedItem.Header }, false); // Delete

            // Act
            IList<IBlogItem> searchResults =
                fixture.Blog.List(
                    new BlogListRequest()
                    {
                        Ids = new List<String>() { newId },
                        States = new List<BlogHeaderState>() { BlogHeaderState.Deleted } // Search specifically for deleted items
                    });

            // Assert
            Assert.Equal(1, searchResults.Count);
        }

        [Fact(DisplayName = "Blog Item - Equals Operator (Positive)")]
        public void Equals_BlogItem_Positive()
        {
            // Arrange
            BlogItem item1 = (BlogItem)RandomBlogItem(true);

            // Act
            String jsonCast = JsonConvert.SerializeObject(item1, Formatting.Indented);
            BlogItem item2 = JsonConvert.DeserializeObject<BlogItem>(jsonCast);

            // Assert
            Assert.True(item1 == item2);
        }

        [Fact(DisplayName = "Blog Item - Equals Operator (Negative)")]
        public void Equals_BlogItem_Negative()
        {
            // Arrange
            BlogItem item1 = (BlogItem)RandomBlogItem(true);
            BlogItem item2 = (BlogItem)RandomBlogItem(true);

            // Act

            // Assert
            Assert.False(item1 == item2);
        }

        [Fact(DisplayName = "Blog Item - Not Equals Operator (Positive)")]
        public void NotEquals_BlogItem_Positive()
        {
            // Arrange
            BlogItem item1 = (BlogItem)RandomBlogItem(true);

            // Act
            String jsonCast = JsonConvert.SerializeObject(item1, Formatting.Indented);
            BlogItem item2 = JsonConvert.DeserializeObject<BlogItem>(jsonCast);

            // Assert
            Assert.True(item1 != item2);
        }

        [Fact(DisplayName = "Blog Item - Not Equals Operator (Negative)")]
        public void NotEquals_BlogItem_Negative()
        {
            // Arrange
            BlogItem item1 = (BlogItem)RandomBlogItem(true);
            BlogItem item2 = (BlogItem)RandomBlogItem(true);

            // Act

            // Assert
            Assert.False(item1 != item2);
        }
        
        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}
