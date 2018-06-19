
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
        /// Wrapper for the underlaying renderer for the blog item editor
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <param name="item">The item to be rendered</param>
        /// <param name="preview">Display in "prevew" mode for indexes etc.</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogEditItem(this IHtmlHelper helper, IBlogItem item)
            => BlogEditItem(item, GetModel(helper));
        
        /// <summary>
        /// None-helper signature of the blog item blog item editor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preview"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent BlogEditItem(IBlogItem item, BlogViewModelBase viewModel)
            => ContentFill(BlogViewTemplatePart.Blog_EditItem,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Author, item.Header.Author, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Description, item.Header.Description, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Id, item.Header.Id, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Name, item.Header.Name, true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_PublishedDate,
                        item.Header.PublishedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_State, item.Header.State.GetDescription(), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_UpdatedDate,
                        item.Header.UpdatedDate.ToCustomDate(viewModel.DisplaySettings.DateFormat), true),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Content, item.Content, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_SEOUrlTitle, SEOUrlTitle(item.Header.Name), false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_SEOTags, item.Header.SEOTags.ToCSV(), false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.BlogItem_Tags, item.Header.Tags.ToCSV(), false)
                }, viewModel);

    }
}