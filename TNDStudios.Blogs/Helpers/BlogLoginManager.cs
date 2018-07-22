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
        /// The blog that this manager is handling
        /// </summary>
        private IBlog blog;

        /// <summary>
        /// The context of the login manager
        /// </summary>
        public HttpContext context;

        /// <summary>
        /// The current logged in user's token
        /// </summary>
        public Nullable<Guid> UserToken => sessionHelper.GetGuid(context.Session, securityTokenKey);

        /// <summary>
        /// Get the list of permissions for a given token (to seperate username etc. from the process)
        /// </summary>
        /// <returns>The list of valid permissions</returns>
        public BlogLogin CurrentUser => null; 

        /// <summary>
        /// Validate the login credentials
        /// </summary>
        /// <returns></returns>
        public Boolean ValidateLogin(String username, String password)
        {
            // Get the current security token from the session
            Nullable<Guid> currentToken = UserToken;

            // Get the security token if the user is authenticated
            BlogLogin user = blog.Parameters.Provider.AuthenticateUser(username, password);

            // Set the token in the session state
            if (user != null && user.Token.HasValue)
                return LoginUser(user);

            // Failed to return a positive so exit with a fail at this point
            return false;
        }

        /// <summary>
        /// Set a given user as logged in
        /// </summary>
        /// <param name="user"></param>
        private Boolean LoginUser(BlogLogin user)
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
            catch
            {
                // Failed to log in, it's a generic fail anyway so allow a fail
            }

            // Bombed out so return a failure
            return false;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogLoginManager(IBlog blog)
        {
            // Start a new instance of the session helper pointing at the http context session
            this.blog = blog;
            sessionHelper = new SessionHelper();
        }

        /// <summary>
        /// Constructor where the HttpContext is passed with the constructor
        /// </summary>
        /// <param name="context">The HttpContext to use</param>
        public BlogLoginManager(IBlog blog, HttpContext context)
        {
            this.context = context; // Assign the context;
            sessionHelper = new SessionHelper();
        }
    }
}
