
namespace TNDStudios.Blogs.ViewModels
{
    /// <summary>
    /// The model passed to the MVC view for displaying the item
    /// </summary>
    public class DisplayViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The item that is being displayed
        /// </summary>
        public IBlogItem Item { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DisplayViewModel() : base()
        {

        }
    }

}