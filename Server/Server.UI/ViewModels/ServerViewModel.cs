using Server.UI.ViewModels;

namespace Server.UI.Models
{
    /// <summary>
    /// Class representing the view model for the server tab in the server UI.
    /// </summary>
    internal class ServerViewModel : BaseTabViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"> The ID of the server tab. </param>
        public ServerViewModel(int id) : base(id, "Server", "Global server logs.")
        {

        }

    }

}