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
        /// Get the current user based on the session token
        /// </summary>
        /// <returns>The list of valid permissions</returns>
        public BlogLogin CurrentUser
        {
            get
            {
                return (blog != null && blog.LoginAuths != null && blog.LoginAuths.Logins != null) ?
                  blog.LoginAuths.Logins.Where(login => login.Token == UserToken).FirstOrDefault() : null;
            }
            set 
            {
                if (blog != null && blog.LoginAuths != null && blog.LoginAuths.Logins != null)
                {
                    blog.LoginAuths.Logins.ForEach
                    (
                        login => 
                        {
                            if (login.Username == value.Username)
                            {
                                login.PasswordChange = value.PasswordChange;
                                login.PasswordHash = value.PasswordHash;
                            }
                        }
                    );
                }
            }
        }
        /// <summary>
        /// Handle the tokens (do expiries etc.)
        /// </summary>
        public void HandleTokens()
        {
            // Loop the authenticated logins
            if (blog != null && blog.LoginAuths != null && blog.LoginAuths.Logins != null)
            {
                // Loop the authenticated logins
                blog.LoginAuths.Logins.ForEach(login => 
                {
                });
            }
        }

        /// <summary>
        /// Validate the login credentials
        /// </summary>
        /// <param name="username">The user to validate</param>
        /// <param name="password">The password to check against</param>
        /// <param name="Login">Actually log them in, or just validate?</param>
        /// <returns></returns>
        public Boolean ValidateLogin(String username, String password, Boolean Login)
        {
            // Get the current security token from the session
            Nullable<Guid> currentToken = UserToken;

            // Get the security token if the user is authenticated
            BlogLogin user = blog.Parameters.Provider.AuthenticateUser(username, password);

            // Set the token in the session state if requested to be logged in
            if (user != null)
                return Login ? LoginUser(user) : true;

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
                // No logins yet? Create the array if it is null for some reason
                if (blog.LoginAuths == null)
                    blog.LoginAuths = new BlogUsers();

                // Set the token and the expiry date
                user.Token = Guid.NewGuid();
                user.ExpiryDate = DateTime.Now.AddSeconds(securityTokenExirySeconds);

                // Remove the old logins
                blog.LoginAuths.Logins.RemoveAll(login => 
                    ((login.Username ?? "").Trim() == (user.Username ?? "").Trim()));

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
        /// Logs out a given user
        /// </summary>
        /// <param name="user"></param>
        public Boolean LogOutUser() => LogOutUser(this.CurrentUser); 
        public Boolean LogOutUser(BlogLogin user)
        {
            // If this is the user that is currently logged in
            if (this.CurrentUser.Username == user.Username)
            {
                // No logins yet? Create the array if it is null for some reason
                if (blog.LoginAuths == null)
                    blog.LoginAuths = new BlogUsers();

                // Set the expiry date
                user.ExpiryDate = DateTime.Now.AddSeconds(-1);

                // Remove the old login
                blog.LoginAuths.Logins.RemoveAll(login =>
                    ((login.Username ?? "").Trim() == (user.Username ?? "").Trim()));
                
                // Add the token to the session and return if successful
                return sessionHelper.Remove(context.Session, securityTokenKey);
            }

            return false;
        }

        /// <summary>
        /// Constructors
        /// </summary>
        public BlogLoginManager(IBlog blog) => Setup(blog);
        public BlogLoginManager(IBlog blog, HttpContext context)
        {
            this.context = context; // Assign the context;
            Setup(blog); // Call the base setup
        }

        /// <summary>
        /// Base setup call (from the constructors)
        /// </summary>
        /// <param name="blog"></param>
        private void Setup(IBlog blog)
        {
            // Start a new instance of the session helper pointing at the http context session
            this.blog = blog; // Set the blog context 
            sessionHelper = new SessionHelper();
        }
    }
}
