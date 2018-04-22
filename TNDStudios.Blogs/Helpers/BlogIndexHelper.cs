
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Html Helper to render the blog index
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogIndex(this IHtmlHelper helper)
        {
            /// Get the model from the helper context
            IndexViewModel viewModel = GetModel<IndexViewModel>(helper);

            // Create the tag builders to return to the calling MVC page
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();
            IDictionary<BlogViewTemplateField, String> contentValues = new Dictionary<BlogViewTemplateField, String>()
            {
                { BlogViewTemplateField.Index_BlogItem_Name, viewModel.SearchParameters.PeriodFrom.HasValue ? viewModel.SearchParameters.PeriodFrom.Value.ToString(viewModel.DisplaySettings.DateFormat) : ""}
            };
            contentBuilder.AppendHtml(viewModel.Templates.Process(BlogViewTemplatePart.Index_Body, contentValues));

            // Stick the content all together in the table
            contentBuilder
                .AppendHtml(HtmlBlogHeader(viewModel))
                .AppendHtml(HtmlBlogBody(viewModel))
                .AppendHtml(HtmlBlogFooter(viewModel));

            // Send the tag back
            return contentBuilder;
        }

        /// <summary>
        /// Build the table header tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent HtmlBlogHeader(IndexViewModel viewModel)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();

            // Return the builder
            return contentBuilder;
        }

        /// <summary>
        /// Build the table footer tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent HtmlBlogFooter(IndexViewModel viewModel)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();

            // Return the builder
            return contentBuilder;
        }

        /// <summary>
        /// Build the table body tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent HtmlBlogBody(IndexViewModel viewModel)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();

            // Loop the results and create the row for each result
            viewModel.Results
                .ForEach(blogItem =>
                    contentBuilder.AppendHtml(HtmlBlogItem(blogItem, viewModel))
                );

            // Return the builder
            return contentBuilder;
        }

        /// <summary>
        /// Convert a blog item to a tag builder row item
        /// </summary>
        /// <param name="item">The blog item to convert</param>
        /// <returns>Tag Builder item for a row</returns>
        private static IHtmlContent HtmlBlogItem(IBlogItem item, IndexViewModel viewModel)
        {
            // Create the tag builders to return to the calling MVC page
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();
            IDictionary<String, String> contentValues = new Dictionary<String, String>()
            {
                  { "name", item.Header.Name}
            };
            contentBuilder.AppendHtml(viewModel.Templates.Process(BlogViewTemplatePart.Index_BlogItem, contentValues));

            // Return the builder
            return contentBuilder;
        }
    }
}