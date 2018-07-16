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
        /// Validate the login credentials
        /// </summary>
        /// <returns></returns>
        public Boolean ValidateLogin(HttpContext context, IBlog blog, String username, String password)
        {
            // Get the current security token from the session
            Nullable<Guid> currentToken = sessionHelper.GetGuid(context.Session, securityTokenKey);

            // Get the security token if the user is authenticated
            BlogLogin user = blog.Parameters.Provider.AuthenticateUser(username, password);

            // Set the token in the session state
            if (user.Token.HasValue)
                return LoginUser(context, blog, user);

            // Failed to return a positive so exit with a fail at this point
            return false;
        }

        /// <summary>
        /// Get the list of permissions for a given token (to seperate username etc. from the process)
        /// </summary>
        /// <returns>The list of valid permissions</returns>
        public List<BlogPermission> GetPermissions(Nullable<Guid> token)
        {
            List<BlogPermission> result = new List<BlogPermission>(); // No permissions by default

            // Got a value?
            if (token.HasValue)
            {

            }

            return result; // Return the list of permissions
        }

        /// <summary>
        /// Set a given user as logged in
        /// </summary>
        /// <param name="user"></param>
        private Boolean LoginUser(HttpContext context, IBlog blog, BlogLogin user)
        {
            // Remove old logins with the same token or username should there be one
            try
            { 
                blog.LoginAuths.Logins.RemoveAll(login =>
                    ((login.Token == user.Token) || (login.Username == user.Username)));

                // Add the new user token
                blog.LoginAuths.Logins.Add(user);

                // Add the token to the session and return if successful
                return sessionHelper.SetGuid(context.Session, securityTokenKey, user.Token.Value);
            }
            catch(Exception ex)
            {
            }

            // Bombed out so return a failure
            return false;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogLoginManager()
        {
            // Start a new instance of the session helper pointing at the http context session
            sessionHelper = new SessionHelper();
        }
    }
}
