using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Blogs.RequestResponse
{
    /// <summary>
    /// Request to browse files for a blog item
    /// </summary>
    public class FileBrowserRequest
    {
        public String id { get; set; } // The id for the blog item that is being edited
        public String Source { get; set; } // The source editor string (CKEditor by default)

        // CK Editor parameters (if they are passed when using CKEditor)
        public String CKEditor { get; set; } // The calling field to be returned
        public String CKEditorFuncNum { get; set; } // The calling function number
        public String langCode { get; set; } // The language code passed in by the CK Editor

        // Defualt constructor for the file browser request
        public FileBrowserRequest()
        {
            Source = "CKEditor"; // Default for the editor type
        }
    }

}
