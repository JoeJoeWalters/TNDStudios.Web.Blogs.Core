using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// The options for how the templates should render the data
    /// </summary>
    public class BlogViewDisplaySettings
    {
        /// <summary>
        /// How dates should be displayed
        /// </summary>
        public String DateFormat { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewDisplaySettings()
        {
            DateFormat = "dd MMM yyyy HH:mm"; // The default date format
        }
    }
}
