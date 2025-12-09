namespace Server.UI.ViewModels
{
    /// <summary>
    /// Class representing the view model for a lobby tab in the server UI.
    /// </summary>
    internal class LobbyViewModel : BaseTabViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"> The ID of the lobby. </param>
        /// <param name="maxPlayers"> The maximum number of players allowed in the lobby. </param>
        /// <param name="currentPlayers"> The current number of players in the lobby. </param>
        /// <param name="mapID"> The map ID of the lobby. </param>
        public LobbyViewModel(int id, string name, int maxPlayers, int currentPlayers, int mapID)
            : base(id, $"#{id} {name}", $"ID: {id}, Max players: {maxPlayers}, Current Players: {currentPlayers}, MapID: {mapID}")
        {

        }

    }

}