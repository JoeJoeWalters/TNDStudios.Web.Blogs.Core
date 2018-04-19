using System;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Provide an instance of a data provider from an input string or type
    /// </summary>
    public interface IBlogDataProviderFactory
    {
        /// <summary>
        /// Get the instance of the data provider from an input string
        /// </summary>
        /// <param name="type">The string representing the data provider (Name of the provider)</param>
        /// <returns>The implementation of the data provider</returns>
        IBlogDataProvider Get(String type);
    }
}