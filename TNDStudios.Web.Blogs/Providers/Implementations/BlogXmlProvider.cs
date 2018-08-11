using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TNDStudios.Web.Blogs.Core.Helpers;

namespace TNDStudios.Web.Blogs.Core.Providers
{
    /// <summary>
    /// Provider for the blog using Xml files in the App_Data (Or other) folder
    /// </summary>
    public class BlogXmlProvider : BlogDataProviderBase, IBlogDataProvider
    {
        /// <summary>
        /// Constants for the blog items that need to be saved
        /// </summary>
        private const String indexXmlFilename = "index.xml";
        private const String blogItemXmlFilename = "{0}.xml";
        private const String blogItemFolder = "blogitems";

        /// <summary>
        /// Constants for the users that need to be saved
        /// </summary>
        private const String blogUsersXmlFilename = "users.xml"; // The index file for the list of users
        private const String blogUsersFolder = "users"; // The folder to put the user details under
        private const String blogUserFilename = "{0}.xml"; // The file name for the user details

        /// <summary>
        /// Override for the base delete functionality
        /// </summary>
        /// <returns></returns>
        public override Boolean Delete(IList<IBlogHeader> items, Boolean permanent)
        {
            try
            {
                // List of filename to remove should it be a hard delete (done before becuase the 
                // items won't exist in the index after the base method call)
                List<String> filesToDelete = permanent ?
                    ((List<IBlogHeader>)items).Select(x => BlogItemFilename(x.Id)).ToList<String>()
                    : null;

                // Any files attached to the blogs that might be removed
                if (permanent)
                    ((List<IBlogHeader>)items).ForEach(
                        header =>
                        {
                            // Get the item to check to see if there are any other files to remove
                            IBlogItem item = base.Load(header);
                            if (item != null)
                                item.Files.ForEach(file => DeleteFile(header.Id, file));
                        }
                    );

                // Call the base implementation to handle the headers etc.
                if (base.Delete(items, permanent))
                {
                    // Hard delete? Remove the files ..
                    if (permanent && filesToDelete != null)
                    {
                        // Remove the blog files itself
                        filesToDelete.ForEach(file =>
                        {
                            try
                            {
                                // Attempt to delete the file from disk ..
                                File.Delete(file);
                            }
                            catch
                            {
                                // Move to the next one if failed to delete, it has no real impact on the system
                            }
                        });
                    }

                    // Save the index regardless on a hard or soft delete
                    return WriteBlogIndex();
                }

                // Failed if it gets to here
                return false;
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new CouldNotRemoveBlogException(ex));
            }
        }

        /// <summary>
        /// Load the item from disk
        /// </summary>
        /// <param name="request">The header of the item that is to be loaded</param>
        /// <returns>The blog item that was found</returns>
        public override IBlogItem Load(IBlogHeader request)
        {
            try
            {
                // Get the header record part from the base load
                IBlogItem headerOnly = base.Load(request);

                // Did it give us a valid header to now go and load the content from disk?
                if (headerOnly.Header.Id != "")
                    return ReadBlogItem(headerOnly.Header);

                // Only should get to here if something has gone wrong
                throw new CouldNotLoadBlogException(); // Failed to load the content so error
            }
            catch (Exception ex)
            {
                // Something went wrong, explain why
                throw BlogException.Passthrough(ex, new CouldNotLoadBlogException(ex));
            }
        }

        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to be saved</param>
        /// <returns>The item once it has been saved</returns>
        public override IBlogItem Save(IBlogItem item)
        {
            try
            {
                // New Item?
                Boolean isNew = (item.Header.Id ?? "") == "";

                // The base class will save the item to the in-memory header
                // so we don't want to pass the content in to this. We only want to save 
                // the content to the file, so copy and update the item without the content
                // to the index
                IBlogItem headerRecord = item.Duplicate();
                headerRecord.Content = ""; // Remove the content from being saved to the header record
                headerRecord.Files = new List<BlogFile>(); // The header record doesn't need the file listing for the header record only
                IBlogItem response = base.Save(headerRecord); // Make sure we have an Id

                // Successfully saved?
                if (response != null && response.Header != null && response.Header.Id != "")
                {
                    // Make sure that the origional record that is about to be writen has an associated Id with it
                    item.Header.Id = response.Header.Id;

                    // If we have any file attachments to save we need to do this now 
                    // If the item.Files is blank then see if we already have files to attach
                    if (item.Files != null)
                    {
                        item.Files.ForEach(file =>
                        {
                            // Anything to write?
                            if (file.Content != null && file.Content.Length > 0)
                                file = SaveFile(item.Header.Id, file);
                        });
                    }
                    else
                    {
                        // Sort out the file array
                        if (isNew && item.Files == null)
                            item.Files = response.Files = new List<BlogFile>();
                        else if (!isNew && item.Files == null)
                        {
                            // Get the old blog back again to get the file listing etc.
                            response = Load(response.Header);
                            item.Files = new List<BlogFile>();
                            if (response != null && response.Header != null && response.Header.Id != "")
                                item.Files.AddRange(response.Files);
                        }
                    }

                    // Write the blog item to disk
                    if (WriteBlogItem(item))
                    {
                        // Try and save the header records to disk so any updates are cached there too
                        if (WriteBlogIndex())
                            return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new CouldNotSaveBlogException(ex));
            }

            // Got to here so must have failed
            throw new CouldNotSaveBlogException();
        }

        /// <summary>
        /// Save a file to a blog item
        /// </summary>
        /// <param name="id">The blog item to save the file against</param>
        /// <param name="file">The file to be saved</param>
        /// <returns>The saved file</returns>
        public override BlogFile SaveFile(String id, BlogFile file)
        {
            try
            {
                // Make sure that there is an identifier
                file.Id = ((file.Id ?? "") == "") ? NewId() : file.Id;

                // Generate the path for the file item
                String fileLocation = BlogFilePath(id, file.Id, Path.GetExtension(file.Filename).Replace(".", ""));

                // Calculate the relative directory based on the path
                String combinedPath = Path.Combine(Configuration.Environment.WebRootPath, fileLocation);

                // Get the directory portion from the combined Path
                String fileDirectory = Path.GetDirectoryName(combinedPath);

                // If the directory doesn't exist then create it
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                // Write the file contents 
                File.WriteAllBytes(combinedPath, file.Content);
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new CouldNotSaveBlogException(ex));
            }
            // Send the file back
            return file;
        }

        /// <summary>
        /// Delete a file in a blog item
        /// </summary>
        /// <param name="id">The blog item to delete the file against</param>
        /// <param name="file">The file to be deleted</param>
        /// <returns>If the file was deleted successfully</returns>
        public override Boolean DeleteFile(String id, BlogFile file)
        {
            return true;
        }

        /// <summary>
        /// Load the content of the file to the object
        /// </summary>
        /// <param name="id">The blog item to get the file from</param>
        /// <param name="file">The file to have the content populated into</param>
        /// <returns>The populated Blog File object</returns>
        public override BlogFile LoadFile(String id, BlogFile file)
        {
            try
            {
                // Generate the path for the file item
                String fileLocation = BlogFilePath(id, file.Id, Path.GetExtension(file.Filename).Replace(".", ""));

                // Calculate the relative directory based on the path
                String combinedPath = Path.Combine(Configuration.Environment.WebRootPath, fileLocation);

                // Get the directory portion from the combined Path
                String fileDirectory = Path.GetDirectoryName(combinedPath);

                // If the directory doesn't exist then create it
                if (Directory.Exists(fileDirectory))
                {
                    // Read the file contents 
                    file.Content = File.ReadAllBytes(combinedPath);
                }
            }
            catch (Exception ex)
            {
                throw BlogException.Passthrough(ex, new CouldNotLoadBlogException(ex));
            }

            // Send the file back
            return file;
        }

        /// <summary>
        /// Write the index to disk so it can be retrieved later
        /// </summary>
        private Boolean WriteBlogIndex()
            => Write<BlogIndex>(Path.Combine(this.ConnectionString.Property("path"), indexXmlFilename), this.items);

        /// <summary>
        /// Read the index from disk
        /// </summary>
        private BlogIndex ReadBlogIndex()
            => Read<BlogIndex>(Path.Combine(this.ConnectionString.Property("path"), indexXmlFilename));

        /// <summary>
        /// Write a blog item to disk so it can be retrieved later
        /// </summary>
        /// <param name="blogItem">The Blog Item to be saved</param>
        private Boolean WriteBlogItem(IBlogItem blogItem) => Write<IBlogItem>(BlogItemFilename(blogItem.Header.Id), blogItem);

        /// <summary>
        /// Load the item from disk
        /// </summary>
        /// <param name="request">The header of the item we want to load from disk</param>
        /// <returns>The cast blog item</returns>
        private IBlogItem ReadBlogItem(IBlogHeader request) => Read<BlogItem>(BlogItemFilename(request.Id));

        /// <summary>
        /// Get the filename for any given blog item
        /// </summary>
        /// <param name="Id">The Id of the blog item</param>
        /// <returns>The filename and path of the blog item</returns>
        private String BlogItemFilename(String Id)
            => Path.Combine(this.ConnectionString.Property("path"), blogItemFolder, String.Format(blogItemXmlFilename, Id));

        /// <summary>
        /// The location of the users file that stores the users hashes etc.
        /// </summary>
        private String UserFileLocation =>
            Path.Combine(this.ConnectionString.Property("path"), blogUsersXmlFilename);

        /// <summary>
        /// Get the file path to the file attached to the blog
        /// </summary>
        /// <param name="Id">The Id of the blog item</param>
        /// <param name="Filename">The filename attached to the blog item</param>
        /// <returns></returns>
        private String BlogFilePath(String blogItemId, String fileId, String extension)
            => Path.Combine(this.ConnectionString.Property("path"), blogItemFolder.Trim(), blogItemId.Trim(), (fileId.Trim() + '.' + extension.Trim()));

        /// <summary>
        /// Write an item to disk with a given path from a given object type
        /// </summary>
        /// <typeparam name="T">The object type to be written to disk</typeparam>
        /// <param name="path">The path to write the item to</param>
        private Boolean Write<T>(String path, T toWrite) where T : IBlogBase
        {
            try
            {
                // Calculate the relative directory based on the path
                String combinedPath = Path.Combine(Configuration.Environment.WebRootPath, path);

                // Get the filename from the combined path
                String fileName = Path.GetFileName(combinedPath);

                // Get the directory alone from the combined path
                String pathAlone = (fileName != "") ? Path.GetDirectoryName(combinedPath) : combinedPath;

                // Check to make sure the directory exists
                if (!Directory.Exists(pathAlone))
                    Directory.CreateDirectory(pathAlone);

                // Write the Xml to disk
                File.WriteAllText(combinedPath, toWrite.ToXmlString());

                // Check if the file exists after the write
                return File.Exists(combinedPath);
            }
            catch (Exception ex)
            {
                // Throw that the file could not be saved
                throw BlogException.Passthrough(ex, new CouldNotSaveBlogException(ex));
            }
        }

        /// <summary>
        /// Read an item from disk with a given path from a given object type
        /// </summary>
        /// <typeparam name="T">The object type to be read from disk</typeparam>
        /// <param name="path">The path to read the item from</param>
        private T Read<T>(String path) where T : IBlogBase, new()
        {
            try
            {
                // Calculate the relative directory based on the path
                String combinedPath = Path.Combine(Configuration.Environment.WebRootPath, path);

                // Get the filename from the combined path
                String fileName = Path.GetFileName(combinedPath);

                // Get the directory alone from the combined path
                String pathAlone = (fileName != "") ? Path.GetDirectoryName(combinedPath) : combinedPath;

                // Write the Xml to disk
                String XmlString = File.ReadAllText(combinedPath);

                // Generate a new object
                T result = new T();

                // Populate the object by calling the extension method to cast the incoming string
                result = (T)result.FromXmlString(XmlString);

                // Send the result back
                return result;
            }
            catch (Exception ex)
            {
                // Throw that the file could not be saved
                throw BlogException.Passthrough(ex, new CouldNotLoadBlogException(ex));
            }
        }

        /// <summary>
        /// Authenticate a user, will return a token (Guid) if the user is authenticated
        /// </summary>
        /// <param name="username">The username as cleartext</param>
        /// <param name="password">The password as cleartext but as a secure string (no clear memory footprint)</param>
        /// <returns>The authentication token</returns>
        public override BlogLogin AuthenticateUser(String username, String password)
            => base.AuthenticateUser(username, password); // Normally not have an override but for future ..

        /// <summary>
        /// Change the user's password
        /// </summary>
        /// <param name="username">The username to change</param>
        /// <param name="password">The current password</param>
        /// <param name="newpassword">the new password</param>
        /// <param name="newpasswordconfirm">The new password confirmation</param>
        /// <returns></returns>
        public override BlogLogin ChangePassword(String username, String password, String newpassword, String newpasswordconfirm)
        { 
            // Call the base password change functionality
            BlogLogin updated = base.ChangePassword(username, password, newpassword, newpasswordconfirm);

            // Changed ok?
            if (updated != null)
                SaveUsers(users); // Save the users

            // Return the tokenised user
            return updated;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogXmlProvider() : base()
        {
        }

        /// <summary>
        /// Initialise call made by the factory
        /// </summary>
        public override void Initialise()
        {
            // Initialise the users file if there is not already one present in the correct location
            Boolean usersInitialised = InitialiseUsers();
            if (usersInitialised)
            {
                // Check to see if there is an a file containing the index to load to initialise the blog
                try
                {
                    BlogIndex foundIndex = ReadBlogIndex();
                    items = foundIndex;
                    items.Initialised = true;
                }
                catch (Exception ex)
                {
                    // Could not load error?
                    if (ex.GetType() == typeof(CouldNotLoadBlogException))
                    {
                        // File was not found so create a blank index file
                        if (ex.InnerException != null && ex.InnerException.GetType() == typeof(FileNotFoundException))
                        {
                            // Try and write the blank index
                            try
                            {
                                items = new BlogIndex(); // Generate the new index to be saved
                                if (WriteBlogIndex())
                                    items.Initialised = true;
                            }
                            catch (Exception ex2)
                            {
                                throw BlogException.Passthrough(ex, new CouldNotSaveBlogException(ex2)); // Could not do save of the index
                            }
                        }
                    }
                    else
                        throw BlogException.Passthrough(ex, new CouldNotLoadBlogException(ex)); // Not a handled issue (such as no index so create one)
                }
            }

            // No item index and not initialised or the users were not initialised then raise an error
            if (!usersInitialised || items == null || !items.Initialised)
                throw new NotInitialisedBlogException();
        }

        /// <summary>
        /// Initialise the users file in the blog location if there is not one there already 
        /// giving the admin user the default password which will require changing first time it is used
        /// </summary>
        private Boolean InitialiseUsers()
        {
            // Try and load the users to memory so they can be used by the provider
            try
            {
                users = LoadUsers();
            }
            catch
            {
                users = new BlogUsers();
            }

            // No users, set up default ones ..
            if (users.Logins.Count == 0)
            {
                // The user's file doesn't exist so create the base credentials that will need changing on first login
                users.Logins.Add(GenerateDefaultUser(BlogPermission.Admin)); // Use the default admin user generator

                // Save the base credentials to the file
                return SaveUsers(users);
            }
            else
                return true;
        }

        /// <summary>
        /// Save a list of users to the XML location
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private Boolean SaveUsers(BlogUsers users)
            => Write<BlogUsers>(UserFileLocation, users);

        /// <summary>
        /// Load a list of users from the XML Location
        /// </summary>
        /// <returns>A list of users</returns>
        private BlogUsers LoadUsers()
        {
            BlogUsers users = new BlogUsers(); // The default return parameter

            try
            {
                // Check to see if the file exists
                if (!File.Exists(UserFileLocation))
                    users = Read<BlogUsers>(UserFileLocation);
            }
            catch (Exception ex)
            {
                // Bounce up the chain the error about loading the user's list (and not because it wasn't there)
                throw BlogException.Passthrough(ex, new UserBlogException("Failed to connect to the users repository"));
            }

            return users; // Return the list of users
        }
    }
}
