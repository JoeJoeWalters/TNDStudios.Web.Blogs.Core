using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using ser = Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TNDStudios.Blogs.Helpers;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;

namespace TNDStudios.Blogs.ViewModels
{

    /// <summary>
    /// Class to define the replacement text items
    /// </summary>
    public class BlogViewTemplateReplacement
    {
        public BlogViewTemplateField Id { get; set; }
        public String SearchString { get; set; }
        public String Content { get; set; }
        public Boolean Encode { get; set; }

        public BlogViewTemplateReplacement(BlogViewTemplateField id, String content, Boolean encode = true)
        {
            Id = id;
            SearchString = id.GetDescription();
            Content = content;
            Encode = encode;
        }

        public BlogViewTemplateReplacement(String searchString, String content, Boolean encode = true)
        {
            Id = BlogViewTemplateField.Unknown;
            SearchString = searchString;
            Content = content;
            Encode = encode;
        }
    }

    /// <summary>
    /// Serialisable class to load the blog display templates from Json
    /// </summary>
    [JsonObject]
    [XmlRoot]
    public class BlogViewTemplateLoader : BlogJsonBase
    {
        /// <summary>
        /// Array of the template items
        /// </summary>
        [XmlArray]
        [JsonProperty(PropertyName = "items")]
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
        /// Id for the template item (Uses a custom tolerant enum converter until Newtonsoft supports one)
        /// </summary>
        [XmlAttribute]
        [JsonConverter(typeof(TolerantEnumConverter))]
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public BlogViewTemplatePart Id { get; set; }

        /// <summary>
        /// Content (template) of the template item
        /// </summary>
        [XmlAttribute]
        [JsonProperty(PropertyName = "content", Required = Required.Default)]
        public String Content { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewTemplateLoaderItem()
        {
            Id = BlogViewTemplatePart.Unknown;
            Content = "";
        }
    }

    /// <summary>
    /// Handler to retrieve and load the templates from Json and index them
    /// </summary>
    public class BlogViewTemplates
    {
        /// <summary>
        /// The marker used to open a tag for replacement
        /// </summary>
        public String ReplacementStartMarker { get => "{"; }

        /// <summary>
        /// The marker used to close a tag for replacement
        /// </summary>
        public String ReplacementEndMarker { get => "}"; }

        /// <summary>
        /// Templates used to render the display
        /// </summary>
        private Dictionary<BlogViewTemplatePart, IHtmlContent> templates { get; set; }

        /// <summary>
        /// Get a HtmlTemplate from the dictionary with proper error trapping
        /// </summary>
        /// <returns></returns>
        public IHtmlContent Get(BlogViewTemplatePart key)
            => (templates.ContainsKey(key)) ? templates[key]
            : throw new HtmlTemplateNotFoundBlogException();

        /// <summary>
        /// Process / fill a template with a set of key value pairs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public String Process(BlogViewTemplatePart key, List<BlogViewTemplateReplacement> values)
        {
            // Get the content (will raise an error if it fails)
            try
            {
                // Get the content
                IHtmlContent content = Get(key);

                // Something to process?
                String renderedContent = (content != null) ? HtmlHelpers.GetString(content) : "";
                if (renderedContent != "")
                {
                    // For each replacement text
                    values.ForEach(replacement =>
                    {
                        // Replace the value and check if it needs encoding for anti XSS or not
                        renderedContent = renderedContent.Replace(ReplacementStartMarker + replacement.SearchString + ReplacementEndMarker,
                            replacement.Encode ? WebUtility.HtmlEncode(replacement.Content) : replacement.Content);
                    });
                }

                // Send the rendered content back
                return renderedContent;
            }
            catch (Exception ex)
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
                templates = new Dictionary<BlogViewTemplatePart, IHtmlContent>(); // Empty list of templates by default

                // Convert stream to string
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Parse the XML stream as an XDocument to query
                    XDocument doc = XDocument.Parse(reader.ReadToEnd());

                    // Query the XDocument and pull out the new template loader item 
                    // for each node of type "Item" in the XDocument
                    templateLoader = new BlogViewTemplateLoader()
                    {
                        Items = doc.Descendants("item").Select(item =>
                        new BlogViewTemplateLoaderItem()
                        {
                            // Get the Id by analysing the attributes portion
                            Id = (BlogViewTemplatePart)item.Attribute("id").Value.
                                GetValueFromDescription<BlogViewTemplatePart>(),

                            // Get the content by selecting all sub-nodes and pulling the CData value out for the first item it finds
                            Content = item.Elements("content").Select(
                                node => node.Value
                                ).First() ?? ""                        
                        }).ToList()
                    };

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
            templates = new Dictionary<BlogViewTemplatePart, IHtmlContent>(); // Empty list of templates by default
        }

    }
}
