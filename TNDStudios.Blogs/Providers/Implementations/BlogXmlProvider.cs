using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Blogs.RequestResponse;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Provider for the blog using Xml files in the App_Data (Or other) folder
    /// </summary>
    public class BlogXmlProvider : BlogDataProviderBase, IBlogDataProvider
    {
        /// <summary>
        /// Constants for the filenames that need to be saved
        /// </summary>
        private const String indexXmlFilename = "index.xml";
        private const String blogItemXmlFilename = "{0}.xml";
        private const String blogItemFolder = "blogitems";

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
            catch(Exception ex)
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
                {
                    // Load the body from disk and send back to the caller
                    return ReadBlogItem(headerOnly.Header);
                }

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
                    item.Files.ForEach(file =>
                    {
                    // Anything to write?
                    if (file.Content != null && file.Content.Length > 0)
                            file = SaveFile(item.Header.Id, file);
                    });

                    // Write the blog item to disk
                    if (WriteBlogItem(item))
                    {
                        // Try and save the header records to disk so any updates are cached there too
                        if (WriteBlogIndex())
                            return response;
                    }
                }
            }
            catch(Exception ex)
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

            // No item index and not initialised then raise an error
            if (items == null || !items.Initialised)
                throw new NotInitialisedBlogException();
        }
    }
}
