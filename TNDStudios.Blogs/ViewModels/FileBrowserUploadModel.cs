namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// The model to display files attached to a given blog item
    /// </summary>
    public class FileBrowserUploadViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The item that was just uploaded
        /// </summary>
        public BlogFile BlogFile { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileBrowserUploadViewModel() : base()
        {
            BlogFile = new BlogFile();
        }
    }
}