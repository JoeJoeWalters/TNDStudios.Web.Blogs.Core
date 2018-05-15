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
        private const String blogItemFolder = "blogsitems";
        
        /// <summary>
        /// Save a blog item
        /// </summary>
        /// <param name="item">The item to be saved</param>
        /// <returns>The item once it has been saved</returns>
        public override IBlogItem Save(IBlogItem item)
        {
            // The base class will save the item to the in-memory header
            // so we don't want to pass the content in to this. We only want to save 
            // the content to the file, so copy and update the item without the content
            // to the index
            IBlogItem headerRecord = item.Duplicate();
            headerRecord.Content = ""; // Remove the content from being saved to the header record
            IBlogItem response = base.Save(headerRecord);

            // Successfully saved?
            if (response != null && response.Header != null && response.Header.Id != "")
            {
                // Write the blog item to disk
                if (WriteBlogItem(response))
                {
                    // Try and save the header records to disk so any updates are cached there too
                    if (WriteBlogIndex())
                        return response;
                }
            }

            // Got to here so must have failed
            throw new CouldNotSaveBlogException();
        }

        /// <summary>
        /// Write the index to disk so it can be retrieved later
        /// </summary>
        private Boolean WriteBlogIndex()
            => Write<BlogIndex>(
                Path.Combine(
                    this.ConnectionString.Property("path"), indexXmlFilename
                    ),
                    this.items
                );

        /// <summary>
        /// Write a blog item to disk so it can be retrieved later
        /// </summary>
        /// <param name="blogItem">The Blog Item to be saved</param>
        private Boolean WriteBlogItem(IBlogItem blogItem)
            => Write<IBlogItem>(
                Path.Combine(
                    this.ConnectionString.Property("path"), blogItemFolder, String.Format(blogItemXmlFilename, blogItem.Header.Id)
                    ),
                    blogItem
                );

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
        /// Default constructor
        /// </summary>
        public BlogXmlProvider() : base()
        {
        }
    }
}
