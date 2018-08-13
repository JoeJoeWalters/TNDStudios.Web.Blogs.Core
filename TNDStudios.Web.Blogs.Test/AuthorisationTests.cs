using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.Blogs.Core.Helpers;
using TNDStudios.Web.Blogs.Core.Providers;
using Xunit;

namespace TNDStudios.Web.Blogs.Core.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class AuthorisationTestsFixture : IDisposable
    {
        /// <summary>
        /// Data provider to test the authorisation against
        /// </summary>
        public IBlog TestBlog { get; set; }
        public IBlogDataProvider DataProvider { get; set; }
        public BlogUsers Users { get; set; }
        public BlogLoginManager LoginManager { get; set; }

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public AuthorisationTestsFixture()
        {
            // Create a data provider to test against
            DataProvider = new BlogMemoryProvider() { };
            DataProvider.Initialise();

            // Create a new blog to test against
            TestBlog = new Blog(new BlogParameters()
            {
                Provider = DataProvider,
                Id = "TestBlog"                 
            });

            // Create a new login manager against the test blog
            LoginManager = new BlogLoginManager(TestBlog);
            LoginManager.context = new MockedContext();
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
    public class AuthorisationTests : IClassFixture<AuthorisationTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private AuthorisationTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public AuthorisationTests(AuthorisationTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }

        [Fact(DisplayName = "Authorisation - Log In User")]
        public void Log_In_User()
        {
            // Arrange
            Boolean result = false;

            // Act
            result = fixture.LoginManager.ValidateLogin("admin", "password", false);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}