using System;
using System.Reflection;
using TNDStudios.Blogs;

namespace TNDStudios.Blogs.Providers
{
    /// <summary>
    /// Get the instance of a provider
    /// </summary>
    public class BlogDataProviderFactory : IBlogDataProviderFactory
    {
        /// <summary>
        /// Get the instance of the data provider from an input string
        /// </summary>
        /// <param name="type">The string representing the data provider (Name of the provider)</param>
        /// <returns>The implementation of the data provider</returns>
        public IBlogDataProvider Get(String type)
        {
            // Create an instance of the implementation from containing assembly
            IBlogDataProvider provider = null;

            try
            {
                // Loop the available assemblies (not in current usually, might even be a custom provider
                // so don't make assumptions about where it is)
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    provider = (IBlogDataProvider)assembly.CreateInstance(type);
                    if (provider != null)
                        break;
                }

                // Raise an error if no provider could be found
                if (provider == null)
                    throw new NoProviderFoundBlogException();
            }
            catch (Exception ex)
            {
                // Wrap the exception correctly if it is not a blog exception type
                throw BlogException.Passthrough(ex, new ErrorFindingProviderBlogException(ex));
            }

            // Send the provider back
            return provider;
        }
    }
}
