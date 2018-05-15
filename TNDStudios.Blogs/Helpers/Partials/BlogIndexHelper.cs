
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
                .AppendHtml(BlogIndexHeader(viewModel))
                .AppendHtml(BlogIndexBody(viewModel))
                .AppendHtml(BlogIndexFooter(viewModel));

            // Send the tag back
            return contentBuilder;
        }

        /// <summary>
        /// Build the table header tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogIndexHeader(IndexViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Index_Header, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table footer tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogIndexFooter(IndexViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Index_Footer, new List<BlogViewTemplateReplacement>() { }, viewModel);

        /// <summary>
        /// Build the table body tag
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static IHtmlContent BlogIndexBody(IndexViewModel viewModel)
        {
            // Create a content builder just to make the looped items content
            HtmlContentBuilder itemsBuilder = new HtmlContentBuilder();

            // Build the clearfix insert classes from the model templates so we don't have to retrieve them each time
            String clearFixMedium = String.Format(" {0}", viewModel.Templates.Get(BlogViewTemplatePart.Index_Clearfix_Medium).GetString());
            String clearFixLarge = String.Format(" {0}", viewModel.Templates.Get(BlogViewTemplatePart.Index_Clearfix_Large).GetString());

            // Loop the results and create the row for each result in the itemsBuilder
            Int32 itemId = 0; // Counter to count the amount of items there are
            viewModel.Results
                .ForEach(blogHeader =>
                    {
                        // Add the new item Html
                        itemId++;
                        itemsBuilder.AppendHtml(BlogItem(new BlogItem() { Header = (BlogHeader)blogHeader }, (BlogViewModelBase)viewModel));

                        // Built up template content classes to transpose in the clearfix template should it be needed
                        String clearfixHtml = "";
                        clearfixHtml += (itemId % viewModel.DisplaySettings.ViewPorts[BlogViewSize.Medium].Columns == 0) ? clearFixMedium : "";
                        clearfixHtml += (itemId % viewModel.DisplaySettings.ViewPorts[BlogViewSize.Large].Columns == 0) ? clearFixLarge : "";

                        // Do we have a clearfix to append?
                        if (clearfixHtml != "")
                        {
                            // .. yes we do, append the clearfix with the appropriate template and the class string that was built
                            itemsBuilder.AppendHtml(ContentFill(BlogViewTemplatePart.Index_Clearfix,
                                new List<BlogViewTemplateReplacement>()
                                {
                                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_BlogItem_ClearFix, clearfixHtml, false)
                                },
                                viewModel));
                        }
                    }
                );

            // Call the standard content filler function
            return ContentFill(BlogViewTemplatePart.Index_Body,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Index_Body_Items, itemsBuilder.GetString(), false)
                },
                viewModel);
        }
    }
}