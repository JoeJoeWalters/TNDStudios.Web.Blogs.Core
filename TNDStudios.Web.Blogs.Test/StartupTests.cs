using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        /// Setup parameters for this set of tests
        /// </summary>

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public StartupTestsFixture()
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
