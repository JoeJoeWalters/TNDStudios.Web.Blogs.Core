using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TNDStudios.Web.Blogs.Core.Helpers;
using TNDStudios.Web.Blogs.Core.RequestResponse;

namespace TNDStudios.Web.Blogs.Core.Providers
{
    /// <summary>
    /// Provides common items for the IBlogDataProvider implementations
    /// </summary>
    public abstract class BlogDataProviderBase : IBlogDataProvider
    {
        /// <summary>
        /// No needed here as the memory provider does not need a connection
        /// </summary>
        public BlogDataProviderConnectionString ConnectionString { get; set; }

        /// <summary>
        /// In Memory list of items
        /// </summary>
        internal BlogIndex items;

        /// <summary>
        /// In Memory list of users (not credentials)
        /// </summary>
        internal BlogUsers users;
        internal const String defaultAdminUsername = "admin";
        internal const String defaultAdminPassword = "password";
        internal const String defaultAdminEmail = "anon@anon.com";

        /// <summary>
        /// Delete a set of blog items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>True or false to indicate it worked or not</returns>
        public virtual Boolean Delete(IList<IBlogHeader> items, Boolean permanent)
            => permanent ? DeletePermanent(items) : DeleteSoft(items);

        /// <summary>
        /// Permanent delete of items
        /// </summary>
        /// <param name="items">The list of items to delete</param>
        /// <returns>Success or Failure</returns>
        private Boolean DeletePermanent(IList<IBlogHeader> items)
        {
            // Search for any items that match the incoming headers and mark them as deleted
            List<String> itemIds = items.Select(x => x.Id).ToList<String>();
            Int64 deleteCount = this.items.Headers.RemoveAll(x => itemIds.Contains(x.Header.Id));

            // Was anything deleted?
            return (deleteCount > 0);
        }

        /// <summary>
        /// Soft delete of items
        /// </summary>
        /// <param name="items">The list of items to delete</param>
        /// <returns>Success or Failure</returns>
        private Boolean DeleteSoft(IList<IBlogHeader> items)
        {
            Int64 deleteCount = 0; // How many items were deleted

            // Search for any items that match the incoming headers and mark them as deleted
            List<String> itemIds = items.Select(x => x.Id).ToList<String>();
            this.items.Headers.ForEach(x =>
            {
                // Is this item in the list of derived Ids?
                if (itemIds.Contains(x.Header.Id))
                {
                    x.Header.State = BlogHeaderState.Deleted; // Set the state
                    deleteCount++; // Increment the deleted counter
                }
            });

            // Was anything deleted?
            return (deleteCount > 0);
        }

        /// <summary>
        /// Search the blogs using the request parameters provided
        /// </summary>
        /// <param name="request">Parameters to search / list with</param>
        /// <returns>A list of blog headers</returns>
        public virtual IList<IBlogHeader> Search(BlogDataProviderGetRequest request)
        {
            // If no list of states is given then only search published articles
            List<BlogHeaderState> checkStates =
                (request.States == null || request.States.Count == 0) ?
                new List<BlogHeaderState>() { BlogHeaderState.Published } :
                request.States;

            // Filter based on the items provided
            IEnumerable<IBlogHeader> filtered = items.Headers
                .Where(headCheck => checkStates.Contains(headCheck.Header.State))
                .Where(ids => (request.Ids == null || request.Ids.Count == 0 || request.Ids.Contains(ids.Header.Id)))
                .Where(from => (from.Header.PublishedDate >= request.PeriodFrom) || (request.PeriodFrom == null))
                .Where(to => (to.Header.PublishedDate <= request.PeriodTo) || (request.PeriodTo == null))
                .Where(tags => (request.Tags == null || request.Tags.Count == 0 || request.Tags.Any(y => tags.Header.Tags.ToString().Contains(y))))
                .Where(head => (request.HeaderList.Count == 0 || request.HeaderList.Any(req => req.Id == head.Header.Id)))
                .Select(x => x.Header);

            // Return all of the headers and success if it didn't die, but as a copy so that returned
            // item isn't a reference to the origional
            return filtered.Select(x => x).ToList<IBlogHeader>();
        }

        /// <summary>
        /// Get a full listing of all blog items (used mainly for serialising in the blog class)
        /// </summary>
        /// <returns>A full list of blog items</returns>
        public virtual List<IBlogHeader> GetListing()
            => this.items.Headers.Select(x => x.Header).ToList<IBlogHeader>();

        /// <summary>
        /// Generate a new Id for the blog
        /// </summary>
        /// <returns>A unique identifier</returns>
        public virtual String NewId()
            => (Guid.NewGuid()).ToString(); // Get a a new Guid ID and cast it to string to return

        /// <summary>
        /// Decode an incoming Id so any extra parts to the id are stripped away
        /// such as for SEO there might be an additional part added to the end of the id
        /// </summary>
        /// <param name="id">The id to decipher</param>
        /// <returns>The cleaned id</returns>
        public virtual String DecodeId(String id)
            => (id.IndexOf('_') != -1) ? id.Split('_')[0] : id;

        /// <summary>
        /// Load the individual blog item
        /// </summary>
        /// <param name="request">The header for the blog item to be loaded</param>
        /// <returns>The blog item</returns>
        public virtual IBlogItem Load(IBlogHeader request)
            => items.Headers.Where(x => x.Header.Id == request.Id).FirstOrDefault();

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to be saved</param>
        /// <returns>The item once it has been saved</returns>
        public virtual IBlogItem Save(IBlogItem item)
        {
            // Define the response
            IBlogItem response = null;

            // Check and see if the blog item exists in the local item list first
            IBlogItem foundItem = items.Headers.Where(x => x.Header.Id == item.Header.Id).FirstOrDefault();
            if (foundItem == null)
            {
                item.Header.Id = NewId(); // Generate a new Id and assign it
                items.Headers.Add((BlogItem)item); // Add the item
                response = item; // Assign the data to the response
            }
            else
            {
                foundItem.Copy(item); // Copy the data in (don't repoint the reference)
                response = foundItem; // Assign the data to the response
            }

            // Return the item back to the caller
            return response;
        }

        /// <summary>
        /// Save a file to a blog item
        /// </summary>
        /// <param name="id">The blog item to save the file against</param>
        /// <param name="file">The file to be saved</param>
        /// <returns>The saved file</returns>
        public virtual BlogFile SaveFile(String id, BlogFile file)
            => throw new NotImplementedException(); // Not implemented in the base class

        /// <summary>
        /// Delete a file in a blog item
        /// </summary>
        /// <param name="id">The blog item to delete the file against</param>
        /// <param name="file">The file to be deleted</param>
        /// <returns>If the file was deleted successfully</returns>
        public virtual Boolean DeleteFile(String id, BlogFile file)
            => throw new NotImplementedException(); // Not implemented in the base class

        /// <summary>
        /// Load the content of the file to the object
        /// </summary>
        /// <param name="id">The blog item to get the file from</param>
        /// <param name="file">The file to have the content populated into</param>
        /// <returns>The populated Blog File object</returns>
        public virtual BlogFile LoadFile(String id, BlogFile file)
            => throw new NotImplementedException(); // Not implemented in the base class

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogDataProviderBase()
        {
            // Instantiate the items
            items = new BlogIndex();
            users = new BlogUsers();
        }

        /// <summary>
        /// Authenticate a user, will return a token (Guid) if the user is authenticated
        /// </summary>
        /// <param name="username">The username as cleartext</param>
        /// <param name="password">The password as cleartext but as a secure string (no clear memory footprint)</param>
        /// <returns>The authentication token</returns>
        public virtual BlogLogin AuthenticateUser(String username, String password)
        {
            // Define a false response by default
            BlogLogin result = null;

            // Start the crypto helper to check the password
            CryptoHelper cryptoHelper = new CryptoHelper();

            // Get the hash for the username
            username = username ?? "";
            BlogLogin login = users.Logins.Where(user => user.Username.Trim().ToLower() == username.Trim().ToLower()).FirstOrDefault();
            if (login != null)
            {
                // Check the match from the password to the admin hash
                if (cryptoHelper.CheckMatch((login.PasswordHash ?? ""), (password ?? "").Trim()))
                    result = login;
            }

            // Send the tokenised user back to the caller
            return result;
        }

        /// <summary>
        /// Change the user's password
        /// </summary>
        /// <param name="username">The username to change</param>
        /// <param name="password">The current password</param>
        /// <param name="newpassword">the new password</param>
        /// <param name="newpasswordconfirm">The new password confirmation</param>
        /// <returns></returns>
        public virtual BlogLogin ChangePassword(String username, String password, String newpassword, String newpasswordconfirm)
        {
            // Check that the new password meets the correct criteria
            if (((newpassword ?? "") == "") ||
                ((newpassword ?? "").Trim().ToLower() != (newpasswordconfirm ?? "").Trim().ToLower()))
                return null;

            // Get the current login that matches the credentials
            BlogLogin result = AuthenticateUser(username, password);

            // Did we find a match?
            if (result != null)
            {
                // Start the crypto helper to check the password
                CryptoHelper cryptoHelper = new CryptoHelper();

                // Encode the new password and mark the login as not needing changing anymore
                result.PasswordChange = false;
                result.PasswordHash = cryptoHelper.CalculateHash(newpassword);

                // Save the result to the users lookup (so that the overriden method can save the users)
                users.Logins.ForEach(login => 
                {
                    // Update this user if the username matches
                    if (login.Username == username)
                    {
                        // Set the properties
                        login.PasswordChange = result.PasswordChange;
                        login.PasswordHash = result.PasswordHash;
                    }
                });
            }

            // Send the tokenised user back to the caller
            return result;
        }

        /// <summary>
        /// Generate a new admin user (for when doesn't exist) independent of the implementation type
        /// </summary>
        /// <returns>The new login</returns>
        public virtual BlogLogin GenerateDefaultUser(BlogPermission permissionLevel, String password = "")
            => new BlogLogin()
            {
                Id = Guid.NewGuid().ToString(), // Generate a new ID for this user
                BlogId = "",
                Username = defaultAdminUsername,
                PasswordHash = (new CryptoHelper()).CalculateHash(
                    (permissionLevel == BlogPermission.Admin) ? defaultAdminPassword : password),
                Email = defaultAdminEmail, // Default email (not a real one)
                PasswordChange = true, // Requires a password change the first login
                Permissions = (permissionLevel == BlogPermission.Admin) ?
                        new List<BlogPermission>()
                        {
                            BlogPermission.Admin,
                            BlogPermission.User
                        } :
                        new List<BlogPermission>()
                        {
                            BlogPermission.User
                        }
            };

        /// <summary>
        /// Initialisation method called from the factory class
        /// </summary>
        public virtual void Initialise()
        {

        }
    }
}
