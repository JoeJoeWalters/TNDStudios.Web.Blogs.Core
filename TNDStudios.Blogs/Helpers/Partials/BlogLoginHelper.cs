
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
        /// Wrapper for the underlaying renderer for the login box for the system
        /// </summary>
        /// <param name="helper">The HtmlHelper reference to extend the function in to</param>
        /// <returns>The Html String output for the helper</returns>
        public static IHtmlContent BlogLoginBox(this IHtmlHelper helper)
            => LoginBoxRender((LoginViewModel)GetModel(helper));

        /// <summary>
        /// Render the login box content (without the need of the helper reference)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IHtmlContent LoginBoxRender(LoginViewModel viewModel)
            => ContentFill(BlogViewTemplatePart.Auth_LoginBox,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Login_Username, viewModel.Username, false)
                }, viewModel);
    }
}