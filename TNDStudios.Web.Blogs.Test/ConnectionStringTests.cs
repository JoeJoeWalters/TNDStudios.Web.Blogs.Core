using System;
using System.Collections.Generic;
using System.Web;
using TNDStudios.Web.Blogs.Core.Providers;
using TNDStudios.Web.Blogs.Core.RequestResponse;
using Xunit;

namespace TNDStudios.Web.Blogs.Core.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class ConnectionStringTestsFixture : IDisposable
    {
        /// <summary>
        /// Setup parameters for this set of tests
        /// </summary>
        public BlogDataProviderConnectionString ConnectionString;

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public ConnectionStringTestsFixture()
        {
            ConnectionString = new BlogDataProviderConnectionString(); // Create a new connection string object to run tests against
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
    public class ConnectionStringTests : IClassFixture<ConnectionStringTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private ConnectionStringTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public ConnectionStringTests(ConnectionStringTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }

        [Fact(DisplayName = "Connection String - Split And Count")]
        public void Split_ConnectionString()
        {
            // Arrange
            String testConnectionStringValue = "xxx=yyy;ccc=ddd;fff=ssss";

            // Act
            fixture.ConnectionString = new BlogDataProviderConnectionString(testConnectionStringValue); // Set the connection string

            // Assert
            Assert.Equal(fixture.ConnectionString.Properties.Count, (int)3); // Right amount of properties?
        }

        [Fact(DisplayName = "Connection String - Verify Key Content")]
        public void VerifyContent_ConnectionString()
        {
            // Arrange
            String key = "key%%&&&&"; // Generate a key with potentially dangerous values in it
            String value = "value!\"£$%^"; // Generate a value pair with potentially dangerous values in it
            String testConnectionStringValue = HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value); // Set the connection string value

            // Act
            fixture.ConnectionString = new BlogDataProviderConnectionString(testConnectionStringValue); // Set the connection string

            // Assert
            Assert.Equal(fixture.ConnectionString.Properties[key], value); // Right value and the key also matches
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}
