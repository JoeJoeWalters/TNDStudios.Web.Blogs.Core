
namespace TNDStudios.Web.Blogs.Core.ViewModels
{
    /// <summary>
    /// The model passed to the MVC view for editing the item
    /// </summary>
    public class EditViewModel : BlogViewModelBase
    {
        /// <summary>
        /// The item that is being edited
        /// </summary>
        public IBlogItem Item { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditViewModel() : base()
        {

        }
    }
}