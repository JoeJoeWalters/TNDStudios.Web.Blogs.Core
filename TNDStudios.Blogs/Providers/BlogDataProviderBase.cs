using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Provides common items for the IBlogDataProvider implementations
    /// </summary>
    public abstract class BlogDataProviderBase
    {
        /// <summary>
        /// Generate a new Id for the blog
        /// </summary>
        /// <returns>A unique identifier</returns>
        public String NewId()
            => (Guid.NewGuid()).ToString(); // Get a a new Guid ID and cast it to string to return
    }
}
