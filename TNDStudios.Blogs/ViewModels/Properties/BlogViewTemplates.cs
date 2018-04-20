using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Html;
using TNDStudios.Blogs.Helpers;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Serialisable class to load the blog display templates from Json
    /// </summary>
    [JsonObject]
    public class BlogViewTemplateLoader : BlogJsonBase
    {
        /// <summary>
        /// Array of the template items
        /// </summary>
        [JsonProperty(PropertyName = "Items", Required = Required.Always)]
        public List<BlogViewTemplateLoaderItem> Items { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewTemplateLoader()
        {
            Items = new List<BlogViewTemplateLoaderItem>();
        }
    }

    /// <summary>
    /// Serialisable class for the individual template items to be loaded from Json
    /// </summary>
    [JsonObject]
    public class BlogViewTemplateLoaderItem : BlogJsonBase
    {
        /// <summary>
        /// Id for the template item
        /// </summary>
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public String Id { get; set; }

        /// <summary>
        /// Content (template) of the template item
        /// </summary>
        [JsonProperty(PropertyName = "Content", Required = Required.Default)]
        public String Content { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewTemplateLoaderItem()
        {
            Id = "";
            Content = "";
        }
    }

    /// <summary>
    /// Handler to retrieve and load the templates from Json and index them
    /// </summary>
    public class BlogViewTemplates
    {
        /// <summary>
        /// Templates used to render the display
        /// </summary>
        private Dictionary<String, IHtmlContent> templates { get; set; }

        /// <summary>
        /// Get a HtmlTemplate from the dictionary with proper error trapping
        /// </summary>
        /// <returns></returns>
        public IHtmlContent Get(String key)
            => (templates.ContainsKey(key)) ? templates[key]
            : throw new HtmlTemplateNotFoundBlogException();

        /// <summary>
        /// Process / fill a template with a set of key value pairs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public String Process(String key, IDictionary<String, String> values)
        {
            // Get the content (will raise an error if it fails)
            try
            {
                IHtmlContent content = Get(key);
                return (content != null) ? HtmlHelpers.GetString(content) : "";
            }
            catch(Exception ex)
            {
                throw BlogException.Passthrough(ex, new CastObjectBlogException(ex));
            }
        }

        /// <summary>
        /// Load the templates from a stream (which needs to be in the appropriate Json format)
        /// </summary>
        /// <param name="data">The loader object</param>
        /// <returns>Success Or Failure</returns>
        public Boolean Load(BlogViewTemplateLoader data)
        {
            // Must have something to work with so loop the items and load their content up
            data.Items.ForEach(item =>
            {
                // Add the content to the dictionary
                templates.Add(item.Id, new HtmlContentBuilder().Append(item.Content));
            });

            // Successful?
            return true;
        }

        /// <summary>
        /// Load the templates from a stream (which needs to be in the appropriate Json format)
        /// </summary>
        /// <param name="stream">A stream object that contains the loader object</param>
        /// <returns>Success Or Failure</returns>
        public Boolean Load(Stream stream)
        {
            BlogViewTemplateLoader templateLoader = null; // Define the template loader outside the try

            try
            {
                templates = new Dictionary<String, IHtmlContent>(); // Empty list of templates by default

                // Convert stream to string
                using (StreamReader reader = new StreamReader(stream))
                {
                    String serialisedObject = reader.ReadToEnd();

                    // Deserialise the string to the template loader format
                    templateLoader = JsonConvert.DeserializeObject<BlogViewTemplateLoader>(serialisedObject);

                    // Nothing to work with? Raise the error
                    if (templateLoader == null || templateLoader.Items == null || templateLoader.Items.Count == 0)
                        throw new CastObjectBlogException();
                }

            }
            catch (Exception ex)
            {
                // Decide whether or not to pass the origional or inner exception to the user
                throw BlogException.Passthrough(ex, new HtmlTemplateLoadFailureBlogException(ex));
            }

            // Call the loader with the object and not the stream
            return Load(templateLoader);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewTemplates()
        {
            templates = new Dictionary<String, IHtmlContent>(); // Empty list of templates by default
        }

    }
}
