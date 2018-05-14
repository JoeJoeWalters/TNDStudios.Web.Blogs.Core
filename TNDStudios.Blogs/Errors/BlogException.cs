using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs
{
    /// <summary>
    /// Static classes for handling blog exceptions
    /// </summary>
    public class BlogException
    {
        /// <summary>
        /// List of exceptions that are classed as "internal"
        /// </summary>
        private static List<Type> internalExceptions = new List<Type>()
        {
            typeof(GeneralBlogException),
            typeof(ErrorFindingProviderBlogException),
            typeof(NoProviderFoundBlogException),
            typeof(SplittingConnectionStringBlogException),
            typeof(CastObjectBlogException),
            typeof(HtmlTemplateNotFoundBlogException),
            typeof(HtmlTemplateLoadFailureBlogException),
            typeof(CouldNotSaveBlogException),
            typeof(CouldNotLoadBlogException)
        };

        /// <summary>
        /// Pass through the exception rather than stack it in a error trap
        /// if the exception is an internal (blog) exception so that trapped
        /// blog exceptions don't get stacked as inners
        /// </summary>
        /// <param name="ex">The chosen exception to be thrown</param>
        /// <returns></returns>
        public static Exception Passthrough(Exception origional, Exception alternate)
            => IsInternal(origional) ? origional : alternate;

        /// <summary>
        /// Is this an "internal" exception type?
        /// </summary>
        /// <param name="ex">An Exception to check</param>
        /// <returns>If the exception sent in is an internal one</returns>
        public static Boolean IsInternal(Exception ex)
            => internalExceptions.Contains(ex.GetType());
    }

    #region [Exception Definitions]

    public class GeneralBlogException : Exception { }

    public class NoProviderFoundBlogException : Exception { }

    public class ItemNotFoundBlogException : Exception
    {
        public ItemNotFoundBlogException() : base() { }
        public ItemNotFoundBlogException(String message) : base(message) { }
    }

    public class ErrorFindingProviderBlogException : Exception
    {
        public ErrorFindingProviderBlogException(Exception inner) : base(inner.Message, inner) { }
    }

    public class SplittingConnectionStringBlogException : Exception
    {
        public SplittingConnectionStringBlogException(Exception inner) : base(inner.Message, inner) { }
    }

    public class HtmlTemplateNotFoundBlogException : Exception { }

    public class HtmlTemplateLoadFailureBlogException : Exception
    {
        public HtmlTemplateLoadFailureBlogException(Exception inner) : base(inner.Message, inner) { }
    }

    public class CouldNotSaveBlogException : Exception
    {
        public CouldNotSaveBlogException() : base() { }
        public CouldNotSaveBlogException(Exception inner) : base(inner.Message, inner) { }
    }

    public class CouldNotLoadBlogException : Exception
    {
        public CouldNotLoadBlogException() : base() { }
        public CouldNotLoadBlogException(Exception inner) : base(inner.Message, inner) { }
    }
    
    public class CastObjectBlogException : Exception
    {
        public CastObjectBlogException() : base() { }
        public CastObjectBlogException(Exception inner) : base(inner.Message, inner) { }
    }

    #endregion
}
