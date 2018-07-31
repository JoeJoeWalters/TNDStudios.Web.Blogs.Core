using System;
using System.Collections.Generic;
using TNDStudios.Web.Blogs.Core.RequestResponse;

namespace TNDStudios.Web.Blogs.Core.Providers
{
    /// <summary>
    /// Define how a blog class should be able to save it's data
    /// </summary>
    public interface IBlogDataProvider
    {
        /// <summary>
        /// Some providers will need a connection string property that can be parsed
        /// </summary>
        BlogDataProviderConnectionString ConnectionString { get; set; }

        /// <summary>
        /// // Get a set of blog data (or one)
        /// </summary>
        /// <param name="request">The parameters used to request a set of data</param>
        /// <returns>A list of blog items</returns>
        IList<IBlogHeader> Search(BlogDataProviderGetRequest request);

        /// <summary>
        /// // Get a singular blog item
        /// </summary>
        /// <param name="request">The header for the request</param>
        /// <returns>The blog item</returns>
        IBlogItem Load(IBlogHeader request);

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to save</param>
        /// <returns>The saved item</returns>
        IBlogItem Save(IBlogItem item); // Save a blog

        /// <summary>
        /// Delete a set of blog items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>True or false to indicate it worked or not</returns>
        Boolean Delete(IList<IBlogHeader> items, Boolean permanent);

        /// <summary>
        /// Get a full listing of all blog headers (used mainly for serialising in the blog class)
        /// </summary>
        /// <returns>A full list of blog items</returns>
        List<IBlogHeader> GetListing();

        /// <summary>
        /// Generate a New Id for a blog item
        /// </summary>
        /// <returns></returns>
        String NewId();
        
        /// <summary>
        /// Decode an incoming Id so any extra parts to the id are stripped away
        /// such as for SEO there might be an additional part added to the end of the id
        /// </summary>
        /// <param name="id">The id to decipher</param>
        /// <returns>The cleaned id</returns>
        String DecodeId(String id);

        /// <summary>
        /// Save a file to a blog item
        /// </summary>
        /// <param name="id">The blog item to save the file against</param>
        /// <param name="file">The file to be saved</param>
        /// <returns>The saved file</returns>
        BlogFile SaveFile(String id, BlogFile file);

        /// <summary>
        /// Delete a file in a blog item
        /// </summary>
        /// <param name="id">The blog item to delete the file against</param>
        /// <param name="file">The file to be deleted</param>
        /// <returns>If the file was deleted successfully</returns>
        Boolean DeleteFile(String id, BlogFile file);
        
        /// <summary>
        /// Load the content of the file to the object
        /// </summary>
        /// <param name="id">The blog item to get the file from</param>
        /// <param name="file">The file to have the content populated into</param>
        /// <returns>The populated Blog File object</returns>
        BlogFile LoadFile(String id, BlogFile file);

        /// <summary>
        /// Authenticate a user, will return a token (Guid) if the user is authenticated
        /// </summary>
        /// <param name="username">The username as cleartext</param>
        /// <param name="password">The password as cleartext but as a secure string (no clear memory footprint)</param>
        /// <returns>The authentication token</returns>
        BlogLogin AuthenticateUser(String username, String password);

        /// <summary>
        /// Change the user's password
        /// </summary>
        /// <param name="username">The username to change</param>
        /// <param name="password">The current password</param>
        /// <param name="newpassword">the new password</param>
        /// <param name="newpasswordconfirm">The new password confirmation</param>
        /// <returns></returns>
        BlogLogin ChangePassword(String username, String password, String newpassword, String newpasswordconfirm);

        /// <summary>
        /// Initialisation routine called by default
        /// </summary>
        void Initialise();
    }
}
