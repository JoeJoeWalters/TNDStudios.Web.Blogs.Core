using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TNDStudios.Web.Blogs.Core
{
    /// <summary>
    /// Collection of authenticated logins
    /// </summary>
    /// 
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogUsers
    {
        /// <summary>
        /// List of authenticated logins
        /// </summary>
        [XmlArray]
        public List<BlogLogin> Logins { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogUsers()
        {
            Logins = new List<BlogLogin>(); // Create a blank array by default
        }
    }
}
