using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// A Blog item which can be serialised
    /// </summary>
    [Serializable()]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogItem : BlogBase, IBlogItem, IEquatable<BlogItem>
    {
        /// <summary>
        /// The blog header (contains the ID etc.)
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Header")]
        public BlogHeader Header { get; set; }

        /// <summary>
        /// The content of the actual blog
        /// </summary>
        [XmlElement]
        [JsonProperty(PropertyName = "Content", Required = Required.Always)]
        public String Content { get; set; }

        /// <summary>
        /// The list of files attached to this blog entry
        /// </summary>
        [XmlArray]
        [JsonProperty(PropertyName = "Files", Required = Required.AllowNull)]
        public List<BlogFile> Files { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogItem()
        {
            Header = new BlogHeader(); // Generate a default header
            Content = ""; // No content by default
            Files = new List<BlogFile>(); // No files by default
        }

        /// <summary>
        /// Copys a given item in to this item (so we preserve the object reference)
        /// Does not copy the file references however
        /// </summary>
        /// <returns>The the current item</returns>
        public IBlogItem Copy(IBlogItem from)
        {
            // Copy the items in
            this.Header.Author = from.Header.Author;
            this.Header.Description = from.Header.Description;
            this.Header.Name = from.Header.Name;
            this.Header.PublishedDate = from.Header.PublishedDate;
            this.Header.State = from.Header.State;
            this.Header.UpdatedDate = from.Header.UpdatedDate;
            this.Content = from.Content;

            // Return itself after the copy in
            return this;
        }

        /// <summary>
        /// Copys a given edit model (flat version for editing) in to this item
        /// Does not copy the file references however
        /// </summary>
        /// <param name="from"></param>
        /// <returns>The the current item</returns>
        public IBlogItem Copy(EditItemViewModel from)
        {
            // Copy the items in
            this.Header.Author = from.Author;
            this.Header.Name = from.Name;
            this.Header.Description = from.Description;
            this.Header.PublishedDate = DateTime.Parse(from.PublishedDate);
            this.Content = from.Content;

            // Return itself after the copy in
            return this;
        }

        /// <summary>
        /// Duplicates the current item to a new item
        /// Does not copy the file references however
        /// </summary>
        /// <returns>The copy of the current item</returns>
        public IBlogItem Duplicate()
        {
            // Return a copy
            return new BlogItem()
            {
                Header = new BlogHeader()
                {
                    Author = this.Header.Author,
                    Description = this.Header.Description,
                    Id = this.Header.Id,
                    Name = this.Header.Name,
                    PublishedDate = this.Header.PublishedDate,
                    State = this.Header.State,
                    Tags = this.Header.Tags,
                    UpdatedDate = this.Header.UpdatedDate
                },
                Content = this.Content
            };
        }

        #region [IEquatable overrides]

        /// <summary>
        /// IEquitable implementation
        /// </summary>
        /// <param name="other">The other blog item to equate this one to</param>
        /// <returns></returns>
        public Boolean Equals(BlogItem other)
            => this.GetHashCode() == other.GetHashCode();

        /// <summary>
        /// Override for the equality to object method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            BlogItem personObj = obj as BlogItem;
            if (personObj == null)
                return false;
            else
                return Equals(personObj);
        }

        /// <summary>
        /// Override to the equality method
        /// </summary>
        public override int GetHashCode()
            => (this.Header.Id +
                this.Header.Author +
                this.Header.Name +
                this.Header.UpdatedDate.ToString() +
                (this.Header.PublishedDate.HasValue ? this.Header.PublishedDate.ToString() : "")
                ).GetHashCode();

        /// <summary>
        /// Override to the "Not Equals" operator
        /// </summary>
        public static Boolean operator != (BlogItem blogItem1, BlogItem blogItem2)
        {
            if (((object)blogItem1) == null || ((object)blogItem2) == null)
                return Object.Equals(blogItem1, blogItem2);

            return blogItem1.Equals(blogItem2);
        }

        /// <summary>
        /// Override to the "Equals" operator
        /// </summary>
        public static Boolean operator == (BlogItem blogItem1, BlogItem blogItem2)
        {
            if (((object)blogItem1) == null || ((object)blogItem2) == null)
                return Object.Equals(blogItem1, blogItem2);

            return blogItem1.Equals(blogItem2);
        }

        #endregion

    }
}