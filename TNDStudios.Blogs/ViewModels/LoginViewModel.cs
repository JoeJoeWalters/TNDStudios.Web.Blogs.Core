using System;

namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// Model passed to the MVC view to log in to the blog
    /// </summary>
    public class LoginViewModel : BlogViewModelBase
    {
        public String Username { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginViewModel() : base()
        {
            Username = "";
        }
    }
}