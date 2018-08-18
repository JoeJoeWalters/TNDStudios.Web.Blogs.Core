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
        public AuthorisationTestsFixture() => Initialise();

        /// <summary>
        /// Reset the fixture for the next test
        /// </summary>
        public void Initialise()
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
            fixture.Initialise();
            Boolean result = false;

            // Act
            result = fixture.LoginManager.ValidateLogin("admin", "password", false);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Authorisation - Logged In User Marked As Current User")]
        public void Logged_In_User_Showing_As_Current()
        {
            // Arrange
            fixture.Initialise();
            Boolean loggedInResult = false;

            // Act
            loggedInResult = fixture.LoginManager.ValidateLogin("admin", "password", true);

            // Assert
            Assert.True(fixture.LoginManager.CurrentUser != null &&
                        fixture.LoginManager.CurrentUser.Username == "admin");
        }

        [Fact(DisplayName = "Authorisation - Log Out User")]
        public void Log_Out_User()
        {
            // Arrange
            fixture.Initialise();
            Boolean logInResult = fixture.LoginManager.ValidateLogin("admin", "password", true);
            Boolean result = false; // Failed by default

            // Act
            if (logInResult)
                result = fixture.LoginManager.LogOutUser(fixture.LoginManager.CurrentUser);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Authorisation - Check Admin Permission Via Property")]
        public void Check_Admin_Permission_Via_Property()
        {
            // Arrange
            fixture.Initialise();
            fixture.LoginManager.ValidateLogin("admin", "password", true);

            // Act

            // Assert
            Assert.True(fixture.LoginManager.CurrentUser.IsAdmin);
        }

        [Fact(DisplayName = "Authorisation - Check Admin Permission Via Collection")]
        public void Check_Admin_Permission_Via_Collection()
        {
            // Arrange
            fixture.Initialise();
            fixture.LoginManager.ValidateLogin("admin", "password", true);

            // Act

            // Assert
            Assert.True(
                fixture.LoginManager.CurrentUser.Permissions.Exists(
                    permission => permission == BlogPermission.Admin
                    ));
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}