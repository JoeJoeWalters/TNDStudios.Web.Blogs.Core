using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.Blogs.Core.Helpers;
using Xunit;

namespace TNDStudios.Web.Blogs.Core.Test
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class EncryptionTestsFixture : IDisposable
    {
        /// <summary>
        /// Items needed to run the tests
        /// </summary>
        public CryptoHelper Helper { get; set; }

        /// <summary>
        /// Setup the fixture with the needed items
        /// </summary>
        public EncryptionTestsFixture()
        {
            Helper = new CryptoHelper(); // Set up a new crypto helper to test against
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
    public class EncryptionTests : IClassFixture<EncryptionTestsFixture>, IDisposable
    {
        /// <summary>
        /// Holder for the fixture data
        /// </summary>
        private EncryptionTestsFixture fixture;

        /// <summary>
        /// Constructor with the fixture (setup) passed in to it
        /// </summary>
        /// <param name="data"></param>
        public EncryptionTests(EncryptionTestsFixture data)
        {
            fixture = data; // Assign the instantiated fixture data locally
        }
        
        [Fact(DisplayName = "Encryption - Encode Password Hash")]
        public void Encode_Password_Hash()
        {
            // Arrange
            String result = "";

            // Act
            result = fixture.Helper.CalculateHash("password");

            // Assert
            Assert.NotEqual("", (result ?? "")); // Not empty
            Assert.Contains(":", result); // Splitter exists for the hash
        }

        [Fact(DisplayName = "Encryption - Compare Password Hash (Positive)")]
        public void Compare_Password_Hash_Positive()
        {
            // Arrange
            String compareTo = "password";
            String comparingHash = fixture.Helper.CalculateHash(compareTo);
            Boolean result = false;

            // Act
            result = fixture.Helper.CheckMatch(comparingHash, compareTo);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Encryption - Compare Password Hash (Negative)")]
        public void Compare_Password_Hash_Negative()
        {
            // Arrange
            String compareTo = "notthepassword";
            String comparingHash = fixture.Helper.CalculateHash("password");
            Boolean result = true;

            // Act
            result = fixture.Helper.CheckMatch(comparingHash, compareTo);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public void Dispose()
        {
        }
    }
}