using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Get the Model in the proper formt from the IHtmlHelper context
        /// </summary>
        /// <typeparam name="T">The type to be returned</typeparam>
        /// <param name="helper">The IHtmlHelper in context</param>
        /// <returns>The view model in correct class format</returns>
        public static T GetModel<T>(IHtmlHelper helper) where T : new()
        {
            T returnModel = new T(); // The model that will be returned (Default to new but an error will be raised if needed)

            // Wrap in a try as the helper could be being used in the incorrect context
            try
            {
                returnModel = (T)helper.ViewContext.ViewData.Model; // Cast it
            }
            catch (Exception ex)
            {
                // The helper is probably not being used in the right context so the model is wrong
                throw new CastObjectBlogException(ex);
            }

            return returnModel;
        }

        /// <summary>
        /// Get the string content from the IHtmlContent encoded
        /// </summary>
        /// <param name="content">The IHtmlContent package to be rendered</param>
        /// <returns>The rendered string</returns>
        public static String GetString(this IHtmlContent content)
        {
            /// Create an IO writer
            var writer = new StringWriter();

            // Write to a Html Encoder
            content.WriteTo(writer, HtmlEncoder.Default);

            // Return the string to the caller
            return WebUtility.HtmlDecode(writer.ToString());
        }
    }
}
