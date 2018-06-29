using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Helpers
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
        public static IHtmlContent BlogSEOHeader(this IHtmlHelper helper)
            => BlogSEOHeader(GetModel(helper));

        /// <summary>
        /// Wrapper for the underlaying renderer for the blog item SEO items using the current "view" template
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <param name="item">The item to be rendered</param>
        /// <param name="preview">Display in "prevew" mode for indexes etc.</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogSEOHeader(this IHtmlHelper helper, IBlogItem item)
            => BlogSEOHeader(GetModel(helper), item);

        /// <summary>
        /// None-helper signature of the blog item blog item editor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preview"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent BlogSEOHeader(BlogViewModelBase viewModel)
            => ContentFill(BlogViewTemplatePart.Blog_SEO_Header,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Author, viewModel.CurrentBlog.Parameters.SEOSettings.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Name, viewModel.CurrentBlog.Parameters.SEOSettings.Title, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Description, viewModel.CurrentBlog.Parameters.SEOSettings.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_SEOTags, SEOKeywords(viewModel.CurrentBlog.Parameters.SEOSettings.Tags), true)
                }, viewModel);

        /// <summary>
        /// None-helper signature of the blog item blog item editor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preview"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent BlogSEOHeader(BlogViewModelBase viewModel, IBlogItem item)
            => ContentFill(BlogViewTemplatePart.Blog_SEO_Header,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Author, item.Header.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Name, item.Header.Name, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Description, item.Header.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_SEOTags, SEOKeywords(viewModel.CurrentBlog.Parameters.SEOSettings.Tags, item.Header.Tags), true)
                }, viewModel);

        /// <summary>
        /// Output the default SEO keywords
        /// </summary>
        /// <returns></returns>
        private static String SEOKeywords(String keywords)
            => keywords ?? "";

        /// <summary>
        /// Build the keywords string from the list of tags
        /// </summary>
        /// <param name="tags">The list of tags</param>
        /// <returns>The built keywords string</returns>
        private static String SEOKeywords(String keywords, List<String> tags)
            => (tags == null || tags.Count == 0) ? "" : String.Join(",", tags.ToArray());

        /// <summary>
        /// Converts the title of the blog from the header in to a Url acceptable string so
        /// that SEO mechanics uses the Url as an extra keyword
        /// (Only alpha numerics with spaces replaced with dash and in lower case)
        /// </summary>
        /// <param name="header">The header for the blog to be referenced</param>
        /// <returns>The title of the blog as a Url acceptable String</returns>
        private static String SEOUrlTitle(String title)
            => (title == null) ? "" : (new Regex("[^a-zA-Z0-9 -]")).Replace(title, "").Replace(' ', '-').ToLower();
    }
}
