using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TNDStudios.Blogs.ViewModels;

namespace TNDStudios.Blogs.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Wrapper for the underlaying renderer for the file browser for a given blog item
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <param name="item">The item to be rendered</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent FileBrowser(this IHtmlHelper helper, IBlogItem item)
            => BlogAttachments(item, GetModel(helper));

        /// <summary>
        /// None-helper signature of the blog item blog item editor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preview"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent BlogAttachments(IBlogItem item, BlogViewModelBase viewModel)
            => ContentFill(BlogViewTemplatePart.Attachment_View,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_SEOUrlTitle, SEOUrlTitle(item.Header.Name), false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Attachments, EditAttachments(item, viewModel).GetString(), false)

                }, viewModel);

        /// <summary>
        /// Build the attachment editing content
        /// </summary>
        /// <param name="item">The blog entry to get the attachments from</param>
        /// <param name="viewModel">The view model that contains the relevant templates</param>
        /// <returns>The Html Content of the attachment editor</returns>
        private static IHtmlContent EditAttachments(IBlogItem item, BlogViewModelBase viewModel)
        {
            // Create a content builder just to make the looped items content
            HtmlContentBuilder attachmentBuilder = new HtmlContentBuilder();

            // Loop the results and create the row for each result in the itemsBuilder
            item.Files.ForEach(
                file =>
                {
                    attachmentBuilder.AppendHtml(EditAttachment(item, file, viewModel));
                }
                );

            // Call the standard content filler function
            return ContentFill(BlogViewTemplatePart.Attachments,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Attachment_List, attachmentBuilder.GetString(), false)
                },
                viewModel);
        }

        /// <summary>
        /// Build the singular attachment editor
        /// </summary>
        /// <param name="file">The file (attachment) to be displayed</param>
        /// <param name="viewModel">The view model that contains the relevant templates</param>
        /// <returns>The Html Content for the attachment editor</returns>
        private static IHtmlContent EditAttachment(IBlogItem item, BlogFile file, BlogViewModelBase viewModel)
            => ContentFill(BlogViewTemplatePart.Attachment_Item,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Attachment_Title, file.Title, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Attachment_Url, AttachmentUrl(item, file, viewModel), false)
                }, viewModel);
    }
}
