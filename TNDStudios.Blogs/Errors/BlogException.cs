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
            typeof(HtmlTemplateNotProcessedBlogException),
            typeof(HtmlTemplateLoadFailureBlogException),
            typeof(CouldNotSaveBlogException),
            typeof(CouldNotLoadBlogException),
            typeof(CouldNotRemoveBlogException),
            typeof(NotInitialisedBlogException)
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

    /// <summary>
    /// Isn't it annoying that C# doesn't allow inheritable constructors ...
    /// .. Just sayin'
    /// </summary>
    #region [Exception Definitions]

    public class GeneralBlogException : Exception
    {
        public GeneralBlogException() : base() { }
        public GeneralBlogException(Exception inner) : base(inner.Message, inner) { }
        public GeneralBlogException(String Message) : base(Message) { }
    }

    public class NoProviderFoundBlogException : Exception
    {
        public NoProviderFoundBlogException() : base() { }
        public NoProviderFoundBlogException(Exception inner) : base(inner.Message, inner) { }
        public NoProviderFoundBlogException(String Message) : base(Message) { }
    }

    public class ItemNotFoundBlogException : Exception
    {
        public ItemNotFoundBlogException() : base() { }
        public ItemNotFoundBlogException(Exception inner) : base(inner.Message, inner) { }
        public ItemNotFoundBlogException(String Message) : base(Message) { }
    }

    public class ErrorFindingProviderBlogException : Exception
    {
        public ErrorFindingProviderBlogException() : base() { }
        public ErrorFindingProviderBlogException(Exception inner) : base(inner.Message, inner) { }
        public ErrorFindingProviderBlogException(String Message) : base(Message) { }
    }

    public class SplittingConnectionStringBlogException : Exception
    {
        public SplittingConnectionStringBlogException() : base() { }
        public SplittingConnectionStringBlogException(Exception inner) : base(inner.Message, inner) { }
        public SplittingConnectionStringBlogException(String Message) : base(Message) { }
    }

    public class HtmlTemplateNotFoundBlogException : Exception
    {
        public HtmlTemplateNotFoundBlogException() : base() { }
        public HtmlTemplateNotFoundBlogException(Exception inner) : base(inner.Message, inner) { }
        public HtmlTemplateNotFoundBlogException(String Message) : base(Message) { }
    }

    public class HtmlTemplateNotProcessedBlogException : Exception
    {
        public HtmlTemplateNotProcessedBlogException() : base() { }
        public HtmlTemplateNotProcessedBlogException(Exception inner) : base(inner.Message, inner) { }
        public HtmlTemplateNotProcessedBlogException(String Message) : base(Message) { }
    }

    public class HtmlTemplateLoadFailureBlogException : Exception
    {
        public HtmlTemplateLoadFailureBlogException() : base() { }
        public HtmlTemplateLoadFailureBlogException(Exception inner) : base(inner.Message, inner) { }
        public HtmlTemplateLoadFailureBlogException(String Message) : base(Message) { }
    }

    public class CouldNotSaveBlogException : Exception
    {
        public CouldNotSaveBlogException() : base() { }
        public CouldNotSaveBlogException(Exception inner) : base(inner.Message, inner) { }
        public CouldNotSaveBlogException(String Message) : base(Message) { }
    }

    public class CouldNotLoadBlogException : Exception
    {
        public CouldNotLoadBlogException() : base() { }
        public CouldNotLoadBlogException(Exception inner) : base(inner.Message, inner) { }
        public CouldNotLoadBlogException(String Message) : base(Message) { }
    }

    public class CouldNotRemoveBlogException : Exception
    {
        public CouldNotRemoveBlogException() : base() { }
        public CouldNotRemoveBlogException(Exception inner) : base(inner.Message, inner) { }
        public CouldNotRemoveBlogException(String Message) : base(Message) { }
    }

    public class CastObjectBlogException : Exception
    {
        public CastObjectBlogException() : base() { }
        public CastObjectBlogException(Exception inner) : base(inner.Message, inner) { }
        public CastObjectBlogException(String Message) : base(Message) { }
    }

    public class NotInitialisedBlogException : Exception
    {
        public NotInitialisedBlogException() : base() { }
        public NotInitialisedBlogException(Exception inner) : base(inner.Message, inner) { }
        public NotInitialisedBlogException(String Message) : base(Message) { }
    }

    #endregion
}
