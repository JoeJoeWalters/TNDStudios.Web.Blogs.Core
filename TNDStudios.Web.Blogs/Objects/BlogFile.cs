using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TNDStudios.Web.Blogs.Core
{
    /// <summary>
    /// Files that belong to a blog entry or the blog itself
    /// </summary>
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogFile
    {
        /// <summary>
        /// Mime types that can be handled by the attachment controller
        /// </summary>
        public static String DefaultMimeType = "text/plain";
        public static Dictionary<String, String> MimeTypes =
            new Dictionary<String, String>
            {
                        {".txt", BlogFile.DefaultMimeType},
                        {".pdf", "application/pdf"},
                        {".doc", "application/vnd.ms-word"},
                        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                        {".xls", "application/vnd.ms-excel"},
                        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                        {".png", "image/png"},
                        {".jpg", "image/jpeg"},
                        {".jpeg", "image/jpeg"},
                        {".gif", "image/gif"},
                        {".csv", "text/csv"}
            };

        /// <summary>
        /// List of mime types that would be classed as "images", used for the get operation
        /// </summary>
        public static List<String> ImageMimeTypes = new List<String>() { "image/png", "image/jpeg", "image/jpeg", "image/gif" };

        /// <summary>
        /// The identifier for the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Id", Required = Required.Always)]
        public String Id { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Filename", Required = Required.Always)]
        public String Filename { get; set; }

        /// <summary>
        /// The title of the file
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Title", Required = Required.AllowNull)]
        public String Title { get; set; }

        /// <summary>
        /// When was the blog entry updated
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "UpdatedDate", Required = Required.Always)]
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Tags for the file (so that they can be used for SEO)
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Tags", Required = Required.AllowNull)]
        public List<String> Tags { get; set; }

        /// <summary>
        /// Content of the file if it needs to be passed or uploaded etc.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Byte[] Content { get; set; }

        /// <summary>
        /// Is this attachment an "image" type?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Boolean IsImage
        {
            get => BlogFile.ImageMimeTypes.Contains(BlogFile.GetContentType(this));
        }

        /// <summary>
        /// Get the content type of this file
        /// </summary>
        [XmlIgnore]
        public String ContentType
        {
            get => BlogFile.GetContentType(this);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogFile()
        {
            Filename = "";
            Title = "";
            Tags = new List<String>();
            UpdatedDate = new DateTime();
        }

        /// <summary>
        /// Get the content type base on the file passed to it
        /// </summary>
        /// <param name="file">The BlogFile that is being discovered</param>
        /// <returns>The content type for the filename</returns>
        public static String GetContentType(BlogFile file)
            => BlogFile.MimeTypes.ContainsKey(Path.GetExtension(file.Filename).ToLowerInvariant()) ?
                BlogFile.MimeTypes[Path.GetExtension(file.Filename).ToLowerInvariant()] : BlogFile.DefaultMimeType;

    }
}
