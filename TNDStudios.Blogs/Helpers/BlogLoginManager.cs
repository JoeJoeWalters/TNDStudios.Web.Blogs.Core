using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.Web.Blogs.Core.Providers;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    /// <summary>
    /// Handle logins and permissions for the blog
    /// </summary>
    public class BlogLoginManager
    {
        /// <summary>
        /// Set up any constants that are used for authentication etc.
        /// </summary>
        private const String securityTokenKey = "tndstudios.web.blogs.core.token"; // Security token key path
        private const Int32 securityTokenExirySeconds = 60 * 30; // The length of time before the security token expires

        /// <summary>
        /// The session helper connected to the web session (or other)
        /// </summary>
        private SessionHelper sessionHelper;

        /// <summary>
        /// The user's that are currently loggged in (or at least have tokens)
        /// </summary>
        private List<BlogLogin> logins;

        /// <summary>
        /// Validate the login credentials
        /// </summary>
        /// <returns></returns>
        public Boolean ValidateLogin(IBlog blog, String username, String password)
        {
            // Get the current security token from the session
            String currentToken = sessionHelper.GetString(securityTokenKey, "");

            // Get the security token if the user is authenticated
            Nullable<Guid> securityToken = blog.Parameters.Provider.AuthenticateUser(username, password);

            // Set the token in the session state
            if (securityToken.HasValue)
            {
                SetSecurityToken(blog.Parameters.Id, securityToken.Value, username, new List<BlogPermission>());
                sessionHelper.SetString(securityTokenKey, securityToken.ToString());
            }

            return true;
        }

        /// <summary>
        /// Set the security token in the login list
        /// </summary>
        /// <param name="blogId">The blog this is for</param>
        /// <param name="token">The security token for the user</param>
        /// <param name="username">The username for reference</param>
        private void SetSecurityToken(String blogId, Guid token, String username, 
            List<BlogPermission> permissions)
        {
            // Create a new login reference containing the token
            BlogLogin loginRef = new BlogLogin()
            {
                 BlogId = blogId,
                 ExpiryDate = DateTime.Now.AddSeconds(securityTokenExirySeconds),
                 Permissions = permissions,
                 Token = token
            };

            // Remove old logins with the same token should there be one
            logins.RemoveAll(login => (login.Token == loginRef.Token));

            // Add the new token
            logins.Add(loginRef);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogLoginManager(ISession session)
        {
            // Start a new instance of the session helper pointing at the http context session
            sessionHelper = new SessionHelper(session);

            logins = new List<BlogLogin>(); // No logins by default
        }
    }
}
