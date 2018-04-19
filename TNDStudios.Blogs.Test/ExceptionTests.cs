using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Blogs.Providers;
using Xunit;

namespace TNDStudios.Blogs.Test
{
    /// <summary>
    /// Tests to make sure that custom exception logic is followed correctly
    /// </summary>
    public class ExceptionTests
    {

        [Fact(DisplayName = "Exceptions - Check For Internal Exception (Positive)")]
        public void Exception_Is_Internal_Positive()
        {
            // Arrange

            // Act 
            Boolean result = BlogException.IsInternal(new GeneralBlogException() { });

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Exceptions - Check For Internal Exception (Negative)")]
        public void Exception_Is_Internal_Negative()
        {
            // Arrange

            // Act 
            Boolean result = BlogException.IsInternal(new NotImplementedException() { });

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Exceptions - Provide Internal Passthrough")]
        public void Exception_Internal_Is_Passed_Through()
        {
            // Arrange

            // Act
            Exception result = BlogException.Passthrough(new NoProviderFoundBlogException(), new GeneralBlogException());

            // Assert
            Assert.IsType<NoProviderFoundBlogException>(result);
        }

        [Fact(DisplayName = "Exceptions - Deny External Passthrough")]
        public void Exception_External_Is_Not_Passed_Through()
        {
            // Arrange

            // Act
            Exception result = BlogException.Passthrough(new NotImplementedException(), new GeneralBlogException());

            // Assert
            Assert.IsType<GeneralBlogException>(result);
        }
    }
}
