using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TNDStudios.Web.Blogs.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class AuthorisationTestsFixture : IDisposable
    {
        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public AuthorisationTestsFixture()
        {
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

            // Act
        
            // Assert
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}