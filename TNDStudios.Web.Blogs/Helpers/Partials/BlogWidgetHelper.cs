using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Render the widget for pages that want to display blog 
        /// entries outside of the controllers
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogWidget(this IHtmlHelper helper)
        {
            return new HtmlContentBuilder();
        }
    }
}