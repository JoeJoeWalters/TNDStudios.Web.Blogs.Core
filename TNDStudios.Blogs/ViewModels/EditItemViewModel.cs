using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// Flattened view of the item that is easier to transfer for saving by the controller
    /// </summary>
    public class EditItemViewModel
    {
        public String Id { get; set; }
        public String Author { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Content { get; set; }
        public String PublishedDate { get; set; }
    }
}
