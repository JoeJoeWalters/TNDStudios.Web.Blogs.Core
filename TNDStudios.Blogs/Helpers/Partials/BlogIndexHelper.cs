
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
            => ContentFill(BlogViewTemplatePart.Index_Header, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table footer tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent HtmlBlogFooter(IndexViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Index_Footer, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table body tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent HtmlBlogBody(IndexViewModel viewModel)
        {
            // Create a content builder just to make the looped items content
            HtmlContentBuilder itemsBuilder = new HtmlContentBuilder();

            // Loop the results and create the row for each result in the itemsBuilder
            Int32 itemId = 0;
            viewModel.Results
                .ForEach(blogItem =>
                    {
#warning [Clearfix prototype, add the items to the template so it can be upgraded to bootstrap 4 by the user and apply the modulus items to the settings class]
                        String clearfixTemplate = "<div class=\"clearfix {clearfix}\"></div>";
                        String clearfixHtml = "";

                        itemId++;
                        itemsBuilder.AppendHtml(HtmlBlogItem(blogItem, viewModel));

                        clearfixHtml += (itemId % 2 == 0) ? "visible-md" : "";
                        clearfixHtml += (itemId % 3 == 0) ? "visible-lg" : "";

                        if (clearfixHtml != "")
                        {
                            clearfixTemplate = clearfixTemplate.Replace("{clearfix}", clearfixHtml);
                            itemsBuilder.AppendHtml(clearfixTemplate);
                        }
                    }
                );

            // Generate the list of replacements
            List<BlogViewTemplateReplacement> contentValues = new List<BlogViewTemplateReplacement>()
            {
                new BlogViewTemplateReplacement(BlogViewTemplateField.Index_Body_Items, itemsBuilder.GetString(), false)
            };

            // Call the standard content filler function
            return ContentFill(BlogViewTemplatePart.Index_Body, contentValues, viewModel);
        }

        /// <summary>
        /// Convert a blog item to a tag builder row item
        /// </summary>
        /// <param name="item">The blog item to convert</param>
        /// <returns>Tag Builder item for a row</returns>
        private static IHtmlContent HtmlBlogItem(IBlogItem item, IndexViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Index_BlogItem, 
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_Author, item.Header.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_Description, item.Header.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_Id, item.Header.Id, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_Name, item.Header.Name, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_PublishedDate, 
                        item.Header.PublishedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_State, item.Header.State.GetDescription(), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_UpdatedDate, 
                        item.Header.UpdatedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true)
                }, 
                viewModel);
    }
}