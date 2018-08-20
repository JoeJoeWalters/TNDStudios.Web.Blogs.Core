using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using TNDStudios.Web.Blogs.Core.ViewModels;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Get the model that is needed not from the controller name but from a given blog Id 
        /// </summary>
        /// <typeparam name="T">The type to be returned</typeparam>
        /// <param name="helper">The IHtmlHelper in context</param>
        /// <param name="blogId">The blog Id that was set up</param>
        /// <returns></returns>
        public static T GetModel<T>(IHtmlHelper helper, String blogId) where T : BlogViewModelBase, new()
        {
            T returnModel = new T(); // The model that will be returned
#warning [The blog may not be initialised so this should fail at first]
            // Return the model
            return returnModel;
        }

        /// <summary>
        /// Get the Model in the proper formt from the IHtmlHelper context
        /// </summary>
        /// <typeparam name="T">The type to be returned</typeparam>
        /// <param name="helper">The IHtmlHelper in context</param>
        /// <returns>The view model in correct class format</returns>
        public static T GetModel<T>(IHtmlHelper helper) where T : BlogViewModelBase, new()
        {
            T returnModel = new T(); // The model that will be returned (Default to new but an error will be raised if needed)

            // Wrap in a try as the helper could be being used in the incorrect context
            try
            {
                returnModel = (T)helper.ViewContext.ViewData.Model; // Cast it

                // Populate any common items required in it
                returnModel.Populate(helper);
            }
            catch (Exception ex)
            {
                // The helper is probably not being used in the right context so the model is wrong
                throw new CastObjectBlogException(ex);
            }

            return returnModel;
        }

        /// <summary>
        /// Get the Model in the proper format from the IHtmlHelper context
        /// alternative signature as BlogViewModelBase is an abstract so can't fall under type T
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static BlogViewModelBase GetModel(IHtmlHelper helper)
            => ((BlogViewModelBase)helper.ViewContext.ViewData.Model).Populate(helper);

        /// <summary>
        /// Standardised content fill function to provide IHtmlContent based on a 
        /// template and a set of replacement values
        /// </summary>
        /// <param name="part">The id of the template part</param>
        /// <param name="contentValues">A list of replacement values to process in the template</param>
        /// <param name="viewModel">The viewmodel to provide a link to the templates being used</param>
        /// <returns></returns>
        private static IHtmlContent ContentFill(
            BlogViewTemplatePart part,
            List<BlogViewTemplateReplacement> contentValues,
            BlogViewModelBase viewModel)
        {
            // Create the tag builders to return to the calling MVC page
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();

            // Append the processed Html to the content builder
            contentBuilder.AppendHtml(viewModel.Templates.Process(part, contentValues));

            // Return the builder
            return contentBuilder;
        }

        /// <summary>
        /// Get the string content from the IHtmlContent encoded
        /// </summary>
        /// <param name="content">The IHtmlContent package to be rendered</param>
        /// <returns>The rendered string</returns>
        public static String GetString(this IHtmlContent content)
        {
            /// Create an IO writer
            var writer = new StringWriter();

            // Write to a Html Encoder
            content.WriteTo(writer, HtmlEncoder.Default);

            // Get the string content before decoding
            String stringContent = writer.ToString();

            // Return the string to the caller
            return WebUtility.HtmlDecode(stringContent);
        }

        /// <summary>
        /// Split a CSV String in to an array (with associated checks)
        /// </summary>
        /// <param name="csvString">The string to split</param>
        /// <returns>An array of strings</returns>
        public static List<String> SplitCSV(this String csvString, Char seperator = ',')
        {
            // Default to an empty list
            List<String> response = new List<string>();

            // Try and split (will default to an empty list otherwise)
            try
            {
                // Add in the split, do a check it's not null first and have a default split char
                response.AddRange((csvString ?? "").Split(seperator).Select(item => (item == null) ? "": item.Trim()));
            }
            catch
            {

            }

            // Return the response or a blank array assuming something went wrong
            return response ?? new List<string>();
        }

        /// <summary>
        /// Cast a list of strings to a CSV string
        /// </summary>
        /// <param name="listOfItems">The list of items to stick together</param>
        /// <param name="seperator">The character to use to seperate the items</param>
        /// <returns>The CSV String</returns>
        public static String ToCSV(this List<String> listOfItems, char seperator = ',')
            => String.Join(seperator.ToString(), 
                (listOfItems ?? new List<string>())
                .Select(item => $"{item.Replace(",", "").Trim()}").ToArray())
            .Trim();

        /// <summary>
        /// The translated attachment url (Will not be direct but 
        /// through a controller to relay the data)
        /// </summary>
        /// <param name="item">The blog item the file is attached to</param>
        /// <param name="file">The file to provide the url for</param>
        /// <param name="viewModel">The view model that containers the controller base url (incase there are multiple blogs)</param>
        /// <returns>The url for the file attachment</returns>
        public static String AttachmentUrl(IBlogItem item, BlogFile file, BlogViewModelBase viewModel)
            => AttachmentUrl(item, file, viewModel.RelativeControllerUrl);

        /// <summary>
        /// Base implementation of the attachment url for putting together the relative path
        /// </summary>
        /// <param name="item">The blog item</param>
        /// <param name="file">The file in the blog item to link to</param>
        /// <param name="ControllerName">The name of the controller to be linked to</param>
        /// <returns></returns>
        public static String AttachmentUrl(IBlogItem item, BlogFile file, String ControllerName)
            => $"{FormatControllerName(ControllerName)}/item/{item.Header.Id}/attachment/{file.Id}";

        /// <summary>
        /// Check that the controller name follows certain principles
        /// </summary>
        /// <param name="controllerName">The incoming controller name</param>
        /// <returns>The formatted controller name</returns>
        private static String FormatControllerName(String controllerName)
        {
            // Something to work with?
            if (controllerName.Length != 0)
            {
                // Add a leading seperator if one does not exist
                if (controllerName.Substring(0, 1) != "/")
                    controllerName = $"/{controllerName}";
            }

            return controllerName; // Return the formatted data
        }
    }
}
