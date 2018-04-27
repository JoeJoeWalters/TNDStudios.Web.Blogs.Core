using System;
using Xunit;

namespace TNDStudios.Blogs.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class ContentTestsFixture : IDisposable
    {
        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public ContentTestsFixture()
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
    public class ContentTests : IClassFixture<ContentTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private ContentTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public ContentTests(ContentTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }
        
        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}
