using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Web.Blogs.Core.Attributes;
using TNDStudios.Web.Blogs.Core.Helpers;
using TNDStudios.Web.Blogs.Core.Providers;
using TNDStudios.Web.Blogs.Core.RequestResponse;
using Xunit;

namespace TNDStudios.Web.Blogs.Core.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class StartupTestsFixture : IDisposable
    {
        /// <summary>
        /// Test blog setup parameters
        /// </summary>
        public readonly String TestSetupBlogControllerName = "TestBlogController";
        public readonly String TestSetupBlogId = "TestBlogID";
        public readonly String TestSetupProvider = "TNDStudios.Web.Blogs.Core.Providers.BlogMemoryProvider";
        public readonly String TestSetupProviderConnectionString = "path=..\\AppData";

        /// <summary>
        /// Test blog SEO parameters
        /// </summary>
        public readonly String TestSEOAuthor = "Test Author";
        public readonly String TestSEOTitle = "Test SEO Title";
        public readonly String TestSEODescription = "Test SEO Description";
        public readonly String[] TestSEOTags = new String[] { "Test", "SEO", "Tags" };

        /// <summary>
        /// Setup parameters for this set of tests
        /// </summary>
        public Dictionary<String, IBlog> Blogs;

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public StartupTestsFixture()
            => Initialise();

        /// <summary>
        /// Function to re-initialise the fixture for each test 
        /// that needs a fresh start
        /// </summary>
        public void Initialise()
        {
            Blogs = new Dictionary<String, IBlog>(); // Set up a new dictionary to hold the blogs
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
    public class StartupTests : IClassFixture<StartupTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private StartupTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public StartupTests(StartupTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }

        [Fact(DisplayName = "Blog Startup - Initialise Blog Controller")]
        public void Initialise_New_BlogController()
        {
            // Arrange
            Boolean result = false;
            String instanceName = fixture.TestSetupBlogControllerName;
            BlogSetupAttribute[] setupAttributes = new BlogSetupAttribute[1];
            BlogSEOAttribute[] seoAttributes = new BlogSEOAttribute[1];

            fixture.Initialise(); // Reset the fixture

            setupAttributes[0] = new BlogSetupAttribute(
                blogId: fixture.TestSetupBlogId,
                provider: fixture.TestSetupProvider,
                providerConnectionString: fixture.TestSetupProviderConnectionString);

            String tags = String.Join(",", fixture.TestSEOTags);
            seoAttributes[0] = new BlogSEOAttribute(
                author: fixture.TestSEOAuthor, 
                title: fixture.TestSEOTitle, 
                description: fixture.TestSEODescription, 
                tags: tags);
              
            // Act
            BlogRegistrationHelper regHelper = new BlogRegistrationHelper();
            result = regHelper.Register(instanceName, setupAttributes, seoAttributes, ref fixture.Blogs);

            // Assert
            Assert.Contains<String, IBlog>(
                fixture.TestSetupBlogControllerName, 
                (IReadOnlyDictionary<String, IBlog>)fixture.Blogs
                );
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}
