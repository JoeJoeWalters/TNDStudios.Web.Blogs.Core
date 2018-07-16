using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TNDStudios.Web.Blogs.Core
{
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogLogin
    {
        /// <summary>
        /// The Blog ID for the login
        /// </summary>
        public String BlogId { get; set; }

        /// <summary>
        /// Login token used to identify the user's login back to the user themselves
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Nullable<Guid> Token { get; set; }

        /// <summary>
        /// When the login should expire (Once they have logged in)
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The user's login name
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// The user's email 
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// The hash for the user's password
        /// </summary>
        public String PasswordHash { get; set; }

        /// <summary>
        /// The permissions that this login token has been granted
        /// </summary>
        [XmlArray]
        public List<BlogPermission> Permissions { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogLogin()
        {
            BlogId = "";
            Token = null; // No token given yet
            ExpiryDate = DateTime.Now; // Exire now by default
            Permissions = new List<BlogPermission>(); // No permissions by default
        }
    }
}
