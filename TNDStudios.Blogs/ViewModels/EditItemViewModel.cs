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
        public String Content { get; set; }
    }
}
