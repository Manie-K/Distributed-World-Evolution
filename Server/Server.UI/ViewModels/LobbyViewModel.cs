

namespace Server.UI.ViewModels
{
    internal class LobbyViewModel : BaseTabViewModel
    {
        public LobbyViewModel(int id, string name, int maxPlayers, int currentPlayers, int mapID)
            : base(id, $"#{id} {name}", $"ID: {id}, Max players: {maxPlayers}, Current Players: {currentPlayers}, MapID: {mapID}")
        {

        }

    }
}
