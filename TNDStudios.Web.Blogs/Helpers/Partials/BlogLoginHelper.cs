
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
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
        public static IHtmlContent BlogAuthBox(this IHtmlHelper helper)
            => AuthBoxRender((LoginViewModel)GetModel(helper), helper.ViewContext.HttpContext);

        /// <summary>
        /// Render the login box content (without the need of the helper reference)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>The content of the login / auth box </returns>
        public static IHtmlContent AuthBoxRender(LoginViewModel viewModel, HttpContext context)
        {
            BlogLoginManager loginManager = new BlogLoginManager(viewModel.CurrentBlog, context); // Set up a new login manager

            // Get the content of the login box (and sub parts)
            return ContentFill(BlogViewTemplatePart.Auth_LoginBox,
                new List<BlogViewTemplateReplacement>()
                {
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Login_LoginContent, AuthBoxLogin(viewModel, loginManager).GetString(), false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Login_LogoutContent, AuthBoxLogout(viewModel, loginManager).GetString(), false),
                    new BlogViewTemplateReplacement(BlogViewTemplateField.Login_PasswordChangeContent, AuthBoxChangePassword(viewModel, loginManager).GetString(), false)
                }, viewModel);
        }

        /// <summary>
        /// Render the content of the login box (to actually log in)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>The content of the login functionality</returns>
        public static IHtmlContent AuthBoxLogin(LoginViewModel viewModel, BlogLoginManager loginManager)
        {
            // Logged in?
            BlogLogin loggedInUser = loginManager.CurrentUser;
            if (loggedInUser == null)
                return ContentFill(BlogViewTemplatePart.Auth_LoginBox_Login,
                    new List<BlogViewTemplateReplacement>()
                    {
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Login_Username, viewModel.Username, false)
                    }, viewModel);
            else
                return new HtmlContentBuilder();
        }

        /// <summary>
        /// Render the content of the login box logout functionality
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>The content of the logout functionality</returns>
        public static IHtmlContent AuthBoxLogout(LoginViewModel viewModel, BlogLoginManager loginManager)
        {
            // Logged in?
            BlogLogin loggedInUser = loginManager.CurrentUser;
            if (loggedInUser != null)
                return ContentFill(BlogViewTemplatePart.Auth_LoginBox_Logout,
                    new List<BlogViewTemplateReplacement>()
                    {
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Login_Username, viewModel.Username, false)
                    }, viewModel);
            else
                return new HtmlContentBuilder();
        }

        /// <summary>
        /// Render the content of the login box password change functionality
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>The content of the password change functionality</returns>
        public static IHtmlContent AuthBoxChangePassword(LoginViewModel viewModel, BlogLoginManager loginManager)
        {
            // Logged in?
            BlogLogin loggedInUser = loginManager.CurrentUser;
            if (loggedInUser != null)
                return ContentFill(BlogViewTemplatePart.Auth_LoginBox_PasswordChange,
                    new List<BlogViewTemplateReplacement>()
                    {
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Common_Controller_Url, viewModel.ControllerUrl, false),
                        new BlogViewTemplateReplacement(BlogViewTemplateField.Login_Username, viewModel.Username, false)
                    }, viewModel);
            else
                return new HtmlContentBuilder();
        }
    }
}