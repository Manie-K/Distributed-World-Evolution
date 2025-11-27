

namespace Server.UI.ViewModels
{
    internal class LobbyViewModel : BaseTabViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private int _maxPlayers;
        public int MaxPlayers
        {
            get => _maxPlayers;
            set { _maxPlayers = value; OnPropertyChanged(); }
        }

        private int _currentPlayers;
        public int CurrentPlayers
        {
            get => _currentPlayers;
            set { _currentPlayers = value; OnPropertyChanged(); }
        }

        private int _mapID;
        public int MapID
        {
            get => _mapID;
            set { _mapID = value; OnPropertyChanged(); }
        }

        public LobbyViewModel(int id, string name, int maxPlayers, int currentPlayer, int mapID)
            : base(id, name, $"ID: {id}, Max players: {maxPlayers}, Current Players: {currentPlayer}, MapID: {mapID}")
        {
            _name = name;
            _maxPlayers = maxPlayers;
            _currentPlayers = currentPlayer;
            _mapID = mapID;
        }

    }
}
