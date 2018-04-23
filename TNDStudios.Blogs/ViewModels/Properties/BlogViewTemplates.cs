using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ser = Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Runtime.Serialization;
using TNDStudios.Blogs.Helpers;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Enumeration for the content parts for each of the display templates
    /// </summary>
    [DefaultValue(Unknown)]
    public enum BlogViewTemplatePart : Int32
    {
        Unknown = 0, // When the item cannot be found

        [EnumMember(Value = "indexheader")]
        Index_Header = 101,

        [EnumMember(Value = "indexbody")]
        Index_Body = 102,

        [EnumMember(Value = "indexitem")]
        Index_BlogItem = 103,

        [EnumMember(Value = "indexfooter")]
        Index_Footer = 104
    }

    /// <summary>
    /// Enumeration to identify the fields for each content part (actual field names in the description attribute)
    /// </summary>
    [DefaultValue(Unknown)]
    public enum BlogViewTemplateField : Int32
    {
        Unknown = 0, // When the item cannot be found

        [Description("name")]
        Index_BlogItem_Name = 10301
    }

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
        /// Id for the template item (Uses a custom tolerant enum converter until Newtonsoft supports one)
        /// </summary>
        [JsonConverter(typeof(TolerantEnumConverter))]
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public BlogViewTemplatePart Id { get; set; }

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
        public String Process(BlogViewTemplatePart key, IDictionary<BlogViewTemplateField, String> values)
            => Process(key, GetFieldNames(values));

        /// <summary>
        /// Get the field names from the dictionary of field enums provided
        /// </summary>
        public IDictionary<String, String> GetFieldNames(IDictionary<BlogViewTemplateField, String> values)
        {
            // Return set
            Dictionary<String, String> result = new Dictionary<String, String>();

            // Loop the keys
            foreach (BlogViewTemplateField key in values.Keys)
                result.Add(key.GetDescription(), values[key]);

            // Return the result
            return result;
        }

        /// <summary>
        /// Process / fill a template with a set of key value pairs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public String Process(BlogViewTemplatePart key, IDictionary<String, String> values)
        {
            // Get the content (will raise an error if it fails)
            try
            {
                // Get the content
                IHtmlContent content = Get(key);

                // Something to process?
                String renderedContent = (content != null) ? HtmlHelpers.GetString(content) : "";
                if (renderedContent != "" && values.Keys.Count != 0)
                {
                    foreach (String valueKey in values.Keys)
                    {
                        // AntiXSS measures
                        String replaceValue = WebUtility.HtmlEncode(values[valueKey]);

                        // Replace the value
                        renderedContent = renderedContent.Replace("{" + valueKey + "}", replaceValue);
                    }
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
                    String serialisedObject = reader.ReadToEnd();

                    // Custom event handler for this flavour of deserialisation
                    EventHandler<ser.ErrorEventArgs> internalErrorHandler = delegate (object sender, ser.ErrorEventArgs args)
                    {
                        // Check the member type
                        /*
                        switch (args.ErrorContext.Member)
                        {
                            // Issue converting the Id (as they mistyped the enum value) -- Now replaced with TolerentEnumConverter
                            case "Id":
                                
                                break;
                        }
                        */

                        args.ErrorContext.Handled = false; // true
                    };

                    // Deserialise the string to the template loader format
                    templateLoader = JsonConvert.DeserializeObject<BlogViewTemplateLoader>(serialisedObject, new JsonSerializerSettings()
                    {
                        Error = internalErrorHandler
                    });

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
