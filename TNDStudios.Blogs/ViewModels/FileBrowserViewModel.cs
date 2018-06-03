namespace TNDStudios.Blogs.ViewModels
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
        /// Default constructor
        /// </summary>
        public FileBrowserViewModel() : base()
        {

        }
    }
}