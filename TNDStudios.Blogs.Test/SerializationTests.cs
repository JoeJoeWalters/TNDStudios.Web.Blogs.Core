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
    /// Tests to make sure serialization functionality is working correctly
    /// </summary>
    public class SerializationTests
    {

        [Fact(DisplayName = "Serialization - Deserialize Provides Additional Data")]
        public void Deserialize_Provides_AdditionalData()
        {
            // Arrange

            // What property are we going to add / check is put in the right place?
            KeyValuePair<String, JToken> propertyToCheck =
                new KeyValuePair<String, JToken>
                (
                    "AddedProperty",
                    JToken.FromObject("Added Value")
                );

            // Set up the dictionary of properties we expect to see
            IDictionary<String, JToken> expectedAdditionalData = 
                new Dictionary<String, JToken>()
                {
                    { propertyToCheck.Key, propertyToCheck.Value }
                };

            // Generate the new blog to use to save blog items
            IBlog blog = new Blog(
                new BlogParameters()
                {
                    Id = "TestBlogId",
                    Provider = new BlogMemoryProvider()
                });

            IBlogItem blogItem = blog.Save(new BlogItem() { }); // Save the blog item to make sure it is safe
            String output = JsonConvert.SerializeObject(blogItem, Formatting.Indented); // Serialize it
            JObject jsonObject = JObject.Parse(output); // Parse the output back as a generic JObject
            JObject header = (JObject)jsonObject["Header"]; // Get the header section to work with

            // Act 
            header.Property("State")
                .AddAfterSelf(new JProperty(propertyToCheck.Key, propertyToCheck.Value)); // Add the new property in
            BlogHeader deserializedItem = JsonConvert.DeserializeObject<BlogHeader>(header.ToString()); // Deserialize

            // Assert
            Assert.Equal(expectedAdditionalData, deserializedItem.AdditionalData); // Are the dictionaries equal to what we expect?
        }
    }
}
