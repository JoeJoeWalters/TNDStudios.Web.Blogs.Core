using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// A Blog item which can be serialised
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class BlogItem : BlogJsonBase, IBlogItem, IEquatable<BlogItem>
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
        /// Default Constructor
        /// </summary>
        public BlogItem()
        {
            Header = new BlogHeader(); // Generate a default header
            Content = ""; // No content by default
        }

        /// <summary>
        /// Copy the current item
        /// </summary>
        /// <returns>The copy of the current item</returns>
        public IBlogItem Copy()
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