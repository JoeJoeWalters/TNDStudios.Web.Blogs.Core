using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace TNDStudios.Blogs
{
    public abstract class BlogBase : IBlogBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogBase()
        {
            // Set up the location of any additional data that might be captured
            AdditionalData = new Dictionary<string, JToken>();
        }

        /// <summary>
        /// Property bag of additional data as stated by Newtonsoft
        /// </summary>
        [XmlIgnore]
        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalData { get; set; }
        
        /// <summary>
        /// Cast the item to an Xml String
        /// </summary>
        /// <returns></returns>
        public String ToXmlString()
        {
            // Create a new XmlSerializer instance with the type of the test class
            XmlSerializer serialiser = new XmlSerializer(this.GetType());
            String renderedItem = "";
            using (StringWriter writer = new StringWriter())
            {
                serialiser.Serialize(writer, this);
                renderedItem = writer.ToString();
            }

            return renderedItem;
        }

        /// <summary>
        /// Convert an incoming string to a given object type of IBlogBase 
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="data">The data string to convert</param>
        /// <returns></returns>
        public Object FromXmlString(String data)
        {
            try
            {
                // Create a new XmlSerializer instance with the type of the existing type
                XmlSerializer serialiser = new XmlSerializer(this.GetType());
                using (StringReader reader = new StringReader(data))
                {
                    // Deserialise and return the object
                    return serialiser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new CastObjectBlogException(ex)); // Failed, explain why
            }
        }
    }
}
