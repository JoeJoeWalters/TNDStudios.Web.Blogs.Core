using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Blogs.Providers;
using Xunit;

namespace TNDStudios.Blogs.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class ProviderFactoryTestsFixture : IDisposable
    {
        /// <summary>
        /// The definition of the factory provider to be tested
        /// </summary>
        public IBlogDataProviderFactory Factory = null;

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public ProviderFactoryTestsFixture()
        {
            Factory = new BlogDataProviderFactory();
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
    public class ProviderFactoryTests : IClassFixture<ProviderFactoryTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private ProviderFactoryTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public ProviderFactoryTests(ProviderFactoryTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }

        [Fact(DisplayName = "Provider Factory - Memory Provider Get")]
        public void Get_Provider_MemoryProvider()
        {
            // Arrange
            IBlogDataProvider dataProvider = null;

            // Act
            try
            {
                // Try and get the provider based on the namespace
                dataProvider = fixture.Factory.Get("TNDStudios.Blogs.Providers.BlogMemoryProvider");
            }
            catch
            {
                // Nothing, provider is still null, so will fail anyway
            }

            // Assert
            Assert.NotNull(dataProvider); // Was a new id created for the object?
        }

        [Fact(DisplayName = "Provider Factory - Json Provider Get")]
        public void Get_Provider_JsonProvider()
        {
            // Arrange
            IBlogDataProvider dataProvider = null;

            // Act
            try
            {
                // Try and get the provider based on the namespace
                dataProvider = fixture.Factory.Get("TNDStudios.Blogs.Providers.BlogJsonProvider");
            }
            catch
            {
                // Nothing, provider is still null, so will fail anyway
            }

            // Assert
            Assert.NotNull(dataProvider); // Was a new id created for the object?
        }

        [Fact(DisplayName = "Provider Factory - Throw Error On Unknown")]
        public void RaiseError_Provider_Unknown()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                // Try and get the provider based on the namespace
                IBlogDataProvider dataProvider = fixture.Factory.Get("TNDStudios.Blogs.Providers.FakeProvider");
            }
            catch (Exception ex)
            {
                // Nothing, provider is still null, so will fail anyway
                exception = ex;
            }

            // Assert
            Assert.IsType<NoProviderFoundBlogException>(exception); // Was a new id created for the object?
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}
