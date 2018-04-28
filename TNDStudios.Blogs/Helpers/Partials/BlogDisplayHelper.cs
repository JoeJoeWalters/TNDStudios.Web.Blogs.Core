
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
        /// Html Helper to render a blog item (for general display)
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogDisplay(this IHtmlHelper helper)
        {
            /// Get the model from the helper context
            DisplayViewModel viewModel = GetModel<DisplayViewModel>(helper);

            // Create the tag builders to return to the calling MVC page
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();

            // Stick the content all together in the table
            contentBuilder
                .AppendHtml(BlogDisplayHeader(viewModel))
                .AppendHtml(BlogDisplayBody(viewModel))
                .AppendHtml(BlogDisplayFooter(viewModel));

            // Send the tag back
            return contentBuilder;
        }

        /// <summary>
        /// Build the table header tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogDisplayHeader(DisplayViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Display_Header, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table footer tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogDisplayFooter(DisplayViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Display_Footer, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table body tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogDisplayBody(DisplayViewModel viewModel)
        {
            // Create a content builder just to make the looped items content
            HtmlContentBuilder itemsBuilder = new HtmlContentBuilder();

            // Call the standard content filler function
            return ContentFill(BlogViewTemplatePart.Display_Body,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogHeader_Item,
                        BlogDisplayItem(viewModel.Item, viewModel).GetString(), false)
                },
                viewModel);
        }


        /// <summary>
        /// Convert a blog item to a tag builder row item
        /// </summary>
        /// <param name="item">The blog item to convert</param>
        /// <returns>Tag Builder item for a row</returns>
        private static IHtmlContent BlogDisplayItem(IBlogItem item, DisplayViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Display_Item,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_Author, item.Header.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_Description, item.Header.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_Id, item.Header.Id, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_Name, item.Header.Name, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_PublishedDate,
                        item.Header.PublishedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_State, item.Header.State.GetDescription(), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_UpdatedDate,
                        item.Header.UpdatedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Display_BlogItem_Content, item.Content, false)
                },
                viewModel);

    }
}