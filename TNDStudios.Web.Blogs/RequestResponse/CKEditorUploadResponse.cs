using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.Blogs.Core.RequestResponse
{
    /// <summary>
    /// The response object to be encoded as Json when an upload is made back from CKEditor
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class CKEditorUploadResponse : BlogBase
    {
        /// <summary>
        /// Was the file uploaded successfully?
        /// </summary>
        [JsonProperty(PropertyName = "uploaded", Required = Required.Always)]
        public Int16 Uploaded { get; set; }

        /// <summary>
        /// The name of the file that was uploaded
        /// </summary>
        [JsonProperty(PropertyName = "fileName", Required = Required.Always)]
        public String Filename { get; set; }

        /// <summary>
        /// The url to the file that was uploaded
        /// </summary>
        [JsonProperty(PropertyName = "url", Required = Required.Always)]
        public String Url { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CKEditorUploadResponse()
        {
            // Defaults
            Uploaded = 0; // Not successful by default
            Filename = ""; // No filename by default
            Url = ""; // There is no url by default
        }
    }
}
