namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// The model to display files attached to a given blog item
    /// </summary>
    public class FileBrowserViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The item that is being viewed for files
        /// </summary>
        public IBlogItem Item { get; set; }

        /// <summary>
        /// The file that has just been uploaded so that the response 
        /// function can render the callback to the editor
        /// </summary>
        public BlogFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileBrowserViewModel() : base()
        {
            // Defaults
            File = new BlogFile();
            Item = new BlogItem();
        }
    }
}