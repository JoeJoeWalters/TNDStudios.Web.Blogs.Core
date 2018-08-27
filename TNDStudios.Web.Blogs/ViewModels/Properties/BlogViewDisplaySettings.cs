using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// Enumeration to indicate the viewport size
    /// </summary>
    public enum BlogViewSize
    {
        ExtraSmall = 0,
        Small = 1,
        Medium = 2,
        Large = 3
    }

    /// <summary>
    /// Settings dependant on the size of the display
    /// (Roughly analagous to Bootstrap viewport size)
    /// </summary>
    public class BlogViewSizeSettings
    {
        /// <summary>
        /// How many columns are visible in the given port size
        /// </summary>
        public Int16 Columns { get; set; }        
    }

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
        /// Definitions for the viewports based on the size of the screen
        /// e.g. how many columns will be visible for large viewports
        /// </summary>
        public Dictionary<BlogViewSize, BlogViewSizeSettings> ViewPorts { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogViewDisplaySettings()
        {
            DateFormat = "dd MMM yyyy HH:mm"; // The default date format

            // Set the default values for the viewports
            ViewPorts = new Dictionary<BlogViewSize, BlogViewSizeSettings>();
            ViewPorts.Add(BlogViewSize.ExtraSmall, new BlogViewSizeSettings() { Columns = 1 });
            ViewPorts.Add(BlogViewSize.Small, new BlogViewSizeSettings() { Columns = 1 });
            ViewPorts.Add(BlogViewSize.Medium, new BlogViewSizeSettings() { Columns = 2 });
            ViewPorts.Add(BlogViewSize.Large, new BlogViewSizeSettings() { Columns = 3 });
        }
    }
}
