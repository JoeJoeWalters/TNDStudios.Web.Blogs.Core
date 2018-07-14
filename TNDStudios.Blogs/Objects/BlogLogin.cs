using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.Blogs.Core
{
    public class BlogLogin
    {
        /// <summary>
        /// The Blog ID for the login
        /// </summary>
        public String BlogId { get; set; }

        /// <summary>
        /// Login token used to identify the user's login
        /// </summary>
        public Nullable<Guid> Token { get; set; }

        /// <summary>
        /// When the login should expire
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The permissions that this login token has been granted
        /// </summary>
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
