using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Web.Blogs.Core.RequestResponse;

namespace TNDStudios.Web.Blogs.Core.Providers
{
    /// <summary>
    /// Provider for the blog using memory only
    /// </summary>
    public class BlogMemoryProvider : BlogDataProviderBase, IBlogDataProvider
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogMemoryProvider() : base()
        {
        }

        /// <summary>
        /// Initialise call made by the factory
        /// </summary>
        public override void Initialise()
        {
            // In-memory provider so always inialised
            items.Initialised = true;

            // Initialise the users file if there is not already one present in the correct location
            Boolean usersInitialised = InitialiseUsers();
            if (usersInitialised)
            { 
            }

            // No item index and not initialised or the users were not initialised then raise an error
            if (!usersInitialised || items == null || !items.Initialised)
                throw new NotInitialisedBlogException();
        }

        /// <summary>
        /// Initialise the memory provider with the default admin user always
        /// as there is no where to store the user it must be created each time
        /// </summary>
        /// <returns>Always return true</returns>
        public override Boolean InitialiseUsers()
        {
            // Try and load the users to memory so they can be used by the provider
            users = new BlogUsers();
            users.Logins.Add(GenerateDefaultUser(BlogPermission.Admin)); // Use the default admin user generator
            
            return true;
        }
    }

}
