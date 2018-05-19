using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Wrapper for the underlaying renderer for the blog item SEO items using the current "view" template
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <param name="item">The item to be rendered</param>
        /// <param name="preview">Display in "prevew" mode for indexes etc.</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogSEOHeader(this IHtmlHelper helper, IBlogItem item)
            => BlogSEOHeader(item, GetModel(helper));

        /// <summary>
        /// None-helper signature of the blog item blog item editor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preview"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent BlogSEOHeader(IBlogItem item, BlogViewModelBase viewModel)
            => ContentFill(BlogViewTemplatePart.Blog_SEO_Header,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Author, item.Header.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Name, item.Header.Name, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Description, item.Header.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Keywords, BuildSEOKeywords(item.Header.Tags), true)
                }, viewModel);

        /// <summary>
        /// Build the keywords string from the list of tags
        /// </summary>
        /// <param name="tags">The list of tags</param>
        /// <returns>The built keywords string</returns>
        private static String BuildSEOKeywords(List<String> tags)
        {
            return "keywords";
        }
    }
}
